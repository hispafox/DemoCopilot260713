---
name: sk-04-casos-de-uso
description: "Estructura la logica de aplicacion en servicios/casos de uso sin mezclar responsabilidades. Flujo claro, testeable e independiente del framework HTTP."
---

# SK-04 - Implementacion de casos de uso

Define como organizar la logica de aplicacion en servicios de aplicacion (casos de uso). Cada caso de uso orquesta dominio, repositorio y mapeadores sin depender del framework HTTP ni del DbContext directamente.

## Prerequisito: el proyecto debe existir

Antes de aplicar este skill, verificar que la estructura del proyecto esta creada:

```powershell
Test-Path src/backend -PathType Container
Test-Path src/frontend -PathType Container
```

Si alguno devuelve `False`, **detener y aplicar SK-00 primero** (`sk-00-scaffolding-proyecto`).

SK-04 requiere que existan el dominio (SK-01), los DTOs (SK-02) y los mapeadores (SK-03). Aplicarlos antes si alguna capa esta vacia.

## Cuando usar este skill

- Al implementar cualquier operacion CRUD o accion de negocio.
- Al necesitar separar la logica de negocio del controlador.
- Al preparar codigo que sea testeable sin levantar el servidor HTTP.

## Objetivo

Producir servicios de aplicacion donde:
- Cada metodo representa un caso de uso completo (crear, obtener, actualizar, eliminar, completar).
- El servicio no sabe nada de HTTP (sin `HttpContext`, sin `IActionResult`).
- El servicio recibe y devuelve DTOs, nunca entidades EF ni de dominio directamente al exterior.
- La logica de negocio esta en el dominio; el servicio solo orquesta.

## Estructura esperada

### Interfaz del servicio

```csharp
// src/backend/Application/Servicios/ITareasServicio.cs
public interface ITareasServicio
{
    Task<IEnumerable<TareaDto>> ObtenerTodasAsync();
    Task<TareaDto?> ObtenerPorIdAsync(int id);
    Task<TareaDto> CrearAsync(CrearTareaDto dto);
    Task<TareaDto> ActualizarAsync(int id, ActualizarTareaDto dto);
    Task CompletarAsync(int id);
    Task EliminarAsync(int id);
}
```

### Implementacion del servicio

```csharp
// src/backend/Application/Servicios/TareasServicio.cs
public sealed class TareasServicio : ITareasServicio
{
    private readonly ITareasRepositorio _repositorio;

    public TareasServicio(ITareasRepositorio repositorio)
    {
        _repositorio = repositorio;
    }

    public async Task<IEnumerable<TareaDto>> ObtenerTodasAsync() =>
        (await _repositorio.ObtenerTodasAsync()).AListaDto();

    public async Task<TareaDto?> ObtenerPorIdAsync(int id)
    {
        var tarea = await _repositorio.ObtenerPorIdAsync(id);
        return tarea?.ADto();
    }

    public async Task<TareaDto> CrearAsync(CrearTareaDto dto)
    {
        var tarea = dto.ADominio();           // SK-03: DTO → dominio via metodo factoria
        await _repositorio.AgregarAsync(tarea);
        return tarea.ADto();                  // SK-03: dominio → DTO
    }

    public async Task<TareaDto> ActualizarAsync(int id, ActualizarTareaDto dto)
    {
        var tarea = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Tarea {id} no encontrada.");

        tarea.ActualizarTitulo(dto.Titulo);   // SK-01: regla de negocio en el dominio
        tarea.ActualizarDescripcion(dto.Descripcion);
        await _repositorio.GuardarCambiosAsync();
        return tarea.ADto();
    }

    public async Task CompletarAsync(int id)
    {
        var tarea = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Tarea {id} no encontrada.");

        tarea.Completar();                    // SK-01: regla de negocio en el dominio
        await _repositorio.GuardarCambiosAsync();
    }

    public async Task EliminarAsync(int id)
    {
        var tarea = await _repositorio.ObtenerPorIdAsync(id)
            ?? throw new KeyNotFoundException($"Tarea {id} no encontrada.");

        await _repositorio.EliminarAsync(tarea);
    }
}
```

## Interfaz del repositorio (contrato que define Application)

```csharp
// src/backend/Application/Repositorios/ITareasRepositorio.cs
public interface ITareasRepositorio
{
    Task<IEnumerable<EntidadTarea>> ObtenerTodasAsync();
    Task<EntidadTarea?> ObtenerPorIdAsync(int id);
    Task AgregarAsync(EntidadTarea tarea);
    Task EliminarAsync(EntidadTarea tarea);
    Task GuardarCambiosAsync();
}
```

> La interfaz vive en `Application`, no en `Infrastructure`. Infrastructure implementa el contrato; Application lo define.

## Registro en el contenedor de dependencias

```csharp
// src/backend/Api/Program.cs (o en un metodo de extension)
builder.Services.AddScoped<ITareasServicio, TareasServicio>();
builder.Services.AddScoped<ITareasRepositorio, TareasRepositorio>();
```

## Convencion de nombres

- Interfaz: `I{Entidad}Servicio` → `ITareasServicio`.
- Implementacion: `{Entidad}Servicio` → `TareasServicio`.
- Interfaz de repositorio: `I{Entidad}Repositorio` → `ITareasRepositorio`.
- Ubicacion servicio: `src/backend/Application/Servicios/`.
- Ubicacion interfaz repositorio: `src/backend/Application/Repositorios/`.

## Checklist de calidad

- [ ] El servicio no importa ni referencia `Microsoft.AspNetCore.*`.
- [ ] El servicio recibe y devuelve DTOs, no entidades de dominio ni EF.
- [ ] La logica de negocio esta en el dominio, no en el servicio.
- [ ] El servicio lanza `KeyNotFoundException` cuando no encuentra un recurso (el controlador lo mapea a 404).
- [ ] Todos los metodos son `async` y usan `await`.
- [ ] La interfaz del repositorio esta en `Application`, no en `Infrastructure`.
- [ ] Existe al menos un test unitario por caso de uso usando mocks del repositorio.

## Errores comunes a evitar

- **Logica de negocio en el servicio**: validar invariantes en el dominio, no en `TareasServicio`.
- **El servicio accede al DbContext directamente**: siempre a traves del repositorio.
- **Devolver entidades EF o de dominio al controlador**: el servicio devuelve DTOs.
- **Metodos sincronos para I/O**: todas las operaciones de base de datos deben ser `async`.
- **`throw new Exception()`**: usar excepciones semanticas (`KeyNotFoundException`, `InvalidOperationException`) para que el controlador pueda mapearlas a codigos HTTP correctos.

## Relacion con otros skills

- **SK-01 (Dominio)**: el servicio invoca metodos de comportamiento de la entidad.
- **SK-02 (DTOs)**: el servicio recibe DTOs de entrada y devuelve DTOs de respuesta.
- **SK-03 (Mapeadores)**: el servicio usa los mapeadores para traducir entre capas.
- **SK-05 (API REST)**: el controlador llama al servicio y mapea sus excepciones a codigos HTTP.
- **SK-06 (Persistencia)**: el repositorio implementa la interfaz definida en Application.
- **SK-09 (Pruebas)**: los servicios son la capa mas importante de probar con tests unitarios.
