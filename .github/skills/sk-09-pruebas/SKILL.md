---
name: sk-09-pruebas
description: "Define la estrategia de pruebas por capa: unitarias para dominio y servicios, integracion para endpoints criticos, y componente para UI. Criterios de cobertura minima y convencion de tests."
---

# SK-09 - Estrategia de pruebas por capa

Define como probar cada capa del proyecto sin duplicidades: que probar con tests unitarios, que con tests de integracion y que con tests de componente en el frontend. Incluye criterios de cobertura minima y convencion de nomenclatura.

## Prerequisito: el proyecto debe existir

Antes de aplicar este skill, verificar que la estructura del proyecto esta creada:

```powershell
Test-Path src/backend -PathType Container
Test-Path src/frontend -PathType Container
```

Si alguno devuelve `False`, **detener y aplicar SK-00 primero** (`sk-00-scaffolding-proyecto`).

Ademas, el proyecto `Tests` debe existir en la solucion .NET (creado por SK-00). Si no existe, ejecutar:
```powershell
dotnet new xunit -n NombreProyecto.Tests -o src/backend/Tests
dotnet sln src/backend/NombreProyecto.sln add src/backend/Tests/NombreProyecto.Tests.csproj
```

## Cuando usar este skill

- Al implementar cualquier feature nueva (los tests van con la feature, no en un issue separado).
- Al revisar si la cobertura de una capa es suficiente.
- Al decidir que tipo de test escribir para un caso concreto.

## Objetivo

Producir una suite de pruebas donde:
- El dominio se prueba en aislamiento total (sin mocks, sin DB).
- Los servicios se prueban con mocks del repositorio (sin DB real).
- Los endpoints criticos se prueban con `WebApplicationFactory` (con DB en memoria o SQLite de test).
- Los componentes del frontend se prueban con Vitest + Testing Library.

## Matriz de pruebas por capa

| Capa | Tipo de test | Herramienta | Necesita DB | Necesita HTTP |
|------|-------------|-------------|-------------|---------------|
| Dominio (entidades, reglas) | Unitario | xUnit | No | No |
| Servicios de aplicacion | Unitario | xUnit + NSubstitute | No (mock) | No |
| Controladores / endpoints | Integracion | xUnit + WebApplicationFactory | SQLite test | Si (HTTP en memoria) |
| Repositorios | Integracion | xUnit + EF InMemory o SQLite | Si | No |
| Componentes React | Componente | Vitest + Testing Library | No | No (mock fetch) |
| Servicios frontend | Unitario | Vitest | No | No (mock fetch) |

## Backend: tests unitarios de dominio

```csharp
// src/backend/Tests/Dominio/EntidadTareaTests.cs
public sealed class EntidadTareaTests
{
    [Fact]
    public void Crear_ConTituloValido_DevuelveEntidadConEstaCompletadaFalso()
    {
        var tarea = EntidadTarea.Crear("Mi tarea");

        Assert.Equal("Mi tarea", tarea.Titulo);
        Assert.False(tarea.EstaCompletada);
        Assert.True(tarea.CreadoEnUtc <= DateTime.UtcNow);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Crear_ConTituloVacio_LanzaArgumentException(string? titulo)
    {
        Assert.Throws<ArgumentException>(() => EntidadTarea.Crear(titulo!));
    }

    [Fact]
    public void Completar_TareaNoCompletada_CambiaEstaCompletadaAVerdadero()
    {
        var tarea = EntidadTarea.Crear("Pendiente");

        tarea.Completar();

        Assert.True(tarea.EstaCompletada);
    }

    [Fact]
    public void Completar_TareaYaCompletada_LanzaInvalidOperationException()
    {
        var tarea = EntidadTarea.Crear("Ya hecha");
        tarea.Completar();

        Assert.Throws<InvalidOperationException>(() => tarea.Completar());
    }
}
```

## Backend: tests unitarios de servicios

```csharp
// src/backend/Tests/Servicios/TareasServicioTests.cs
public sealed class TareasServicioTests
{
    private readonly ITareasRepositorio _repositorio = Substitute.For<ITareasRepositorio>();
    private readonly TareasServicio _servicio;

    public TareasServicioTests()
    {
        _servicio = new TareasServicio(_repositorio);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_IdInexistente_DevuelveNull()
    {
        _repositorio.ObtenerPorIdAsync(99).Returns((EntidadTarea?)null);

        var resultado = await _servicio.ObtenerPorIdAsync(99);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task CrearAsync_DtoValido_LlamaAgregarYDevuelveDto()
    {
        var dto = new CrearTareaDto { Titulo = "Nueva tarea" };

        var resultado = await _servicio.CrearAsync(dto);

        await _repositorio.Received(1).AgregarAsync(Arg.Any<EntidadTarea>());
        Assert.Equal("Nueva tarea", resultado.Titulo);
    }
}
```

## Backend: tests de integracion de endpoints

```csharp
// src/backend/Tests/Api/TareasEndpointsTests.cs
public sealed class TareasEndpointsTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _cliente;

    public TareasEndpointsTests(WebApplicationFactory<Program> factory)
    {
        _cliente = factory.WithWebHostBuilder(builder =>
            builder.ConfigureServices(services =>
            {
                // Reemplazar DbContext por SQLite en memoria para tests
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AplicacionDbContext>));
                if (descriptor != null) services.Remove(descriptor);
                services.AddDbContext<AplicacionDbContext>(o => o.UseSqlite("Data Source=:memory:"));
            })
        ).CreateClient();
    }

    [Fact]
    public async Task Get_Tareas_DevuelveOkConListaVacia()
    {
        var respuesta = await _cliente.GetAsync("/api/tareas");

        respuesta.EnsureSuccessStatusCode();
        var contenido = await respuesta.Content.ReadFromJsonAsync<List<TareaDto>>();
        Assert.NotNull(contenido);
    }

    [Fact]
    public async Task Post_TareaValida_Devuelve201ConTareaCreada()
    {
        var dto = new CrearTareaDto { Titulo = "Test integracion" };

        var respuesta = await _cliente.PostAsJsonAsync("/api/tareas", dto);

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);
    }
}
```

## Frontend: tests de componentes con Vitest

```typescript
// src/frontend/src/tests/ListaTareas.test.tsx
import { render, screen } from '@testing-library/react';
import { describe, it, expect, vi } from 'vitest';
import { ListaTareas } from '../components/ListaTareas';
import type { Tarea } from '../types/tarea';

const tareasEjemplo: Tarea[] = [
  { id: 1, titulo: 'Tarea uno', estaCompletada: false, creadoEnUtc: '2026-07-14T10:00:00Z', actualizadoEnUtc: '2026-07-14T10:00:00Z' }
];

describe('ListaTareas', () => {
  it('muestra el titulo de cada tarea', () => {
    render(<ListaTareas tareas={tareasEjemplo} onCompletar={vi.fn()} onEliminar={vi.fn()} />);

    expect(screen.getByText('Tarea uno')).toBeInTheDocument();
  });

  it('muestra mensaje cuando la lista esta vacia', () => {
    render(<ListaTareas tareas={[]} onCompletar={vi.fn()} onEliminar={vi.fn()} />);

    expect(screen.getByText(/no hay tareas/i)).toBeInTheDocument();
  });
});
```

## Cobertura minima esperada

| Capa | Cobertura minima |
|------|-----------------|
| Dominio (entidades) | 100% de reglas de negocio y caminos de error |
| Servicios de aplicacion | Todos los casos de uso y casos de error (recurso no encontrado) |
| Endpoints API | GET lista, POST crear, PATCH completar, DELETE eliminar |
| Componentes frontend | Render correcto, estados vacio/con datos, interacciones principales |

## Convencion de nombres de tests

- Formato: `{Metodo}_{Escenario}_{ResultadoEsperado}`.
- Ejemplos:
  - `Crear_ConTituloVacio_LanzaArgumentException`
  - `ObtenerPorId_IdInexistente_DevuelveNull`
  - `Post_TareaValida_Devuelve201ConTareaCreada`

## Checklist de calidad

- [ ] Cada feature nueva incluye sus tests (no en issue separado).
- [ ] Los tests de dominio no tienen mocks ni base de datos.
- [ ] Los tests de servicio usan NSubstitute para mockear el repositorio.
- [ ] Los tests de endpoint usan `WebApplicationFactory` con SQLite en memoria.
- [ ] Los tests de frontend usan Vitest + Testing Library.
- [ ] Los nombres de tests siguen el patron `Metodo_Escenario_Resultado`.
- [ ] No hay tests rotos en `main`; se corrigen antes de agregar features.

## Errores comunes a evitar

- **Tests que prueban el framework, no el codigo**: no verificar que ASP.NET Core serializa correctamente; probar el comportamiento de la aplicacion.
- **Tests acoplados a detalles de implementacion**: probar comportamiento observable, no metodos privados.
- **Tests sin aserciones**: un test sin `Assert` no verifica nada.
- **Base de datos compartida entre tests**: cada test debe ser independiente; usar DB en memoria o limpiar entre tests.

## Relacion con otros skills

- **SK-01 (Dominio)**: la capa mas facil y mas importante de probar en aislamiento.
- **SK-04 (Casos de uso)**: los servicios son la segunda prioridad de cobertura.
- **SK-05 (API REST)**: los endpoints criticos tienen tests de integracion.
- **SK-08 (Cliente frontend)**: los servicios frontend se prueban con mock de `fetch`.
