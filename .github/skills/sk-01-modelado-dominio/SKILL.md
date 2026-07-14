---
name: sk-01-modelado-dominio
description: "Define entidades de dominio, invariantes y reglas de negocio desacopladas de la API y de EF Core. Aplica a cualquier entidad del proyecto."
---

# SK-01 - Modelado de dominio y reglas

Define el modelo de dominio de un modulo: entidades, invariantes, reglas de negocio y restricciones, sin acoplarlos a la capa de API ni a Entity Framework.

## Prerequisito: el proyecto debe existir

Antes de aplicar este skill, verificar que la estructura del proyecto esta creada:

```powershell
Test-Path src/backend -PathType Container
Test-Path src/frontend -PathType Container
```

Si alguno de los dos devuelve `False`, **detener y aplicar SK-00 primero** (`sk-00-scaffolding-proyecto`) para crear la estructura base. No tiene sentido modelar el dominio si no hay un proyecto donde colocarlo.

## Cuando usar este skill

- Al disenar una entidad nueva desde cero.
- Al revisar si las reglas de negocio estan correctamente ubicadas (no en controladores ni en DbContext).
- Al necesitar extraer invariantes implicitas y hacerlas explicitas y verificables.
- Al preparar el modelo previo a disenar los DTOs (SK-02) o la persistencia (SK-06).

## Objetivo

Producir un modelo de dominio consistente donde:
- Las reglas de negocio se verifican en el propio modelo.
- La entidad no depende de EF Core, HTTP ni ningun framework externo.
- Las invariantes son verificables sin necesidad de la base de datos.

## Que es el dominio en este proyecto

El dominio contiene:
- **Entidades**: objetos con identidad propia (por ejemplo, `EntidadTarea`).
- **Invariantes**: condiciones que deben ser verdaderas siempre (por ejemplo, titulo no vacio).
- **Reglas de negocio**: logica que gobierna el comportamiento de la entidad (por ejemplo, solo se puede completar si no esta ya completada).
- **Value Objects** (opcional): tipos inmutables sin identidad propia que encapsulan un concepto (por ejemplo, un rango de fechas).

El dominio **no** contiene:
- Anotaciones de EF Core (`[Column]`, `[Table]`, navigations, etc.).
- Anotaciones de validacion de ASP.NET (`[Required]`, `[MaxLength]`, etc.).
- Logica de presentacion ni mapeo a DTOs.

## Convencion de nomenclatura obligatoria

- Nombres en castellano (clases, propiedades, metodos, variables).
- `PascalCase` para clases, entidades, enums.
- `camelCase` para variables locales y parametros.
- Booleanas con prefijo semantico: `Es`, `Esta`, `Tiene`, `Puede`, `Debe`.
- Booleanas siempre con valor inicial explicito.
- Fechas y horas siempre en UTC. Nombrar el campo con sufijo `Utc` cuando sea una fecha de dominio (por ejemplo, `CreadoEnUtc`).

## Estructura esperada de una entidad de dominio

```csharp
// Ubicacion sugerida: src/backend/Domain/
public sealed class EntidadEjemplo
{
    // Propiedades: private set para proteger invariantes desde fuera
    public int Id { get; private set; }
    public string Titulo { get; private set; } = string.Empty;
    public bool EstaCompletada { get; private set; } = false;
    public DateTime CreadoEnUtc { get; private set; }
    public DateTime ActualizadoEnUtc { get; private set; }

    // Constructor privado para forzar uso de factoria
    private EntidadEjemplo() { }

    // Metodo factoria: punto de entrada controlado
    public static EntidadEjemplo Crear(string titulo)
    {
        // Verificar invariantes antes de construir
        if (string.IsNullOrWhiteSpace(titulo))
            throw new ArgumentException("El titulo no puede estar vacio.", nameof(titulo));

        var ahora = DateTime.UtcNow;
        return new EntidadEjemplo
        {
            Titulo = titulo,
            EstaCompletada = false,
            CreadoEnUtc = ahora,
            ActualizadoEnUtc = ahora
        };
    }

    // Metodos de comportamiento: cada uno verifica sus propias precondiciones
    public void ActualizarTitulo(string nuevoTitulo)
    {
        if (string.IsNullOrWhiteSpace(nuevoTitulo))
            throw new ArgumentException("El titulo no puede estar vacio.", nameof(nuevoTitulo));

        Titulo = nuevoTitulo;
        ActualizadoEnUtc = DateTime.UtcNow;
    }

    public void Completar()
    {
        if (EstaCompletada)
            throw new InvalidOperationException("La tarea ya esta completada.");

        EstaCompletada = true;
        ActualizadoEnUtc = DateTime.UtcNow;
    }
}
```

## Proceso recomendado

1. **Identificar la entidad**: ¿tiene identidad propia? ¿vive de forma independiente?
2. **Listar propiedades**: separar datos de estado de datos de auditoria.
3. **Extraer invariantes**: ¿que siempre debe ser verdad sobre esta entidad? Hacerlas explicitas como precondiciones en el constructor o en cada metodo.
4. **Identificar comportamientos**: ¿que puede hacer la entidad? Cada accion = un metodo con nombre de dominio (no `Set...`).
5. **Verificar aislamiento**: la clase no debe tener referencias a EF, ASP.NET ni capas externas.
6. **Documentar reglas no obvias**: con comentarios XML en metodos publicos o complejos.

## Checklist de calidad del modelo

- [ ] La entidad no importa ninguna libreria de EF, ASP.NET ni infraestructura.
- [ ] Las propiedades tienen `private set` o son `init` para proteger el estado interno.
- [ ] Existe un metodo factoria estatico (`Crear`, `Nueva`, etc.) en lugar de constructor publico con parametros.
- [ ] Cada invariante esta verificada con una excepcion descriptiva (no con booleanos silenciosos).
- [ ] Las booleanas tienen valor inicial explicito.
- [ ] Las fechas son UTC y el nombre del campo refleja eso (sufijo `Utc`).
- [ ] Los metodos de comportamiento tienen nombres de dominio, no de infraestructura.
- [ ] No hay logica de mapeo a DTOs dentro de la entidad.
- [ ] No hay logica de acceso a datos dentro de la entidad.

## Reglas de negocio para la entidad Tarea (referencia)

Extraidas del documento de requisitos:

| Regla | Descripcion | Verificacion |
|-------|-------------|--------------|
| RB-01 | El titulo es obligatorio y no puede estar vacio. | En `Crear` y en `ActualizarTitulo`. |
| RB-02 | `EstaCompletada` inicia en `false` al crear. | En `Crear`. |
| RB-03 | `CreadoEnUtc` se asigna solo en el alta. | En `Crear`, nunca se modifica despues. |
| RB-04 | `ActualizadoEnUtc` se actualiza en cada modificacion. | En todos los metodos de comportamiento. |

## Errores comunes a evitar

- **Anemic domain model**: entidad con solo getters/setters y logica de negocio en el servicio. La entidad debe proteger sus invariantes.
- **Validacion duplicada**: no verificar la misma regla en el controlador, en el servicio y en la entidad. El dominio es la fuente de verdad.
- **Fechas sin UTC**: usar `DateTime.UtcNow`, no `DateTime.Now`.
- **Constructor publico sin validacion**: usar metodo factoria para garantizar invariantes desde la creacion.
- **Dependencias de infraestructura**: si la entidad necesita un `DbContext` o un `IHttpContextAccessor`, algo esta mal.

## Relacion con otros skills

- **SK-02 (Contratos y DTOs)**: el modelo de dominio es la entrada. Los DTOs son proyecciones del dominio hacia afuera.
- **SK-03 (Mapeo entre capas)**: los mapeadores traducen entre dominio, DTOs y entidades EF.
- **SK-06 (Persistencia)**: EF Core puede usar una entidad de dominio con `private set` mediante configuracion de columnas en `OnModelCreating`, sin contaminar el dominio.
- **SK-09 (Pruebas)**: el dominio es la capa mas facil de probar en aislamiento total, sin mocks ni base de datos.
