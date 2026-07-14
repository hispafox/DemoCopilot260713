---
name: sk-07-auditoria-utc
description: "Estandariza el manejo de fechas y auditoria en UTC e ISO 8601 en backend y frontend. Aplica a cualquier entidad que registre marcas de tiempo."
---

# SK-07 - Trazabilidad temporal y auditoria UTC

Define las reglas uniformes para manejar fechas y horas en todo el proyecto: almacenamiento en UTC, serializacion en ISO 8601, campos de auditoria (`CreadoEnUtc`, `ActualizadoEnUtc`) y presentacion en frontend.

## Prerequisito: el proyecto debe existir

Antes de aplicar este skill, verificar que la estructura del proyecto esta creada:

```powershell
Test-Path src/backend -PathType Container
Test-Path src/frontend -PathType Container
```

Si alguno devuelve `False`, **detener y aplicar SK-00 primero** (`sk-00-scaffolding-proyecto`).

## Cuando usar este skill

- Al agregar campos de fecha a cualquier entidad.
- Al revisar si las fechas se estan serializando correctamente.
- Al necesitar mostrar fechas en el frontend con zona horaria del usuario.
- Al detectar inconsistencias de zona horaria entre backend y frontend.

## Objetivo

Garantizar que:
- Todas las fechas se almacenan y transmiten en UTC.
- La serializacion JSON incluye el sufijo `Z` (indicador de UTC).
- El frontend solo convierte a hora local en el momento de mostrar al usuario.
- Los campos de auditoria siguen una convencion de nombres consistente.

## Reglas de backend

### 1. Siempre `DateTime.UtcNow`, nunca `DateTime.Now`

```csharp
// Correcto
var ahora = DateTime.UtcNow;

// Incorrecto: captura la hora local del servidor
var ahora = DateTime.Now;
```

### 2. Campos de auditoria en todas las entidades con marca de tiempo

```csharp
public DateTime CreadoEnUtc { get; private set; }
public DateTime ActualizadoEnUtc { get; private set; }
```

Convencion de nombre obligatoria: sufijo `Utc` en cualquier campo `DateTime` que represente un instante en el tiempo.

### 3. Configuracion de serializacion JSON

Configurar `System.Text.Json` para serializar fechas en formato ISO 8601 con `Z`:

```csharp
// src/backend/Api/Program.cs
builder.Services.AddControllers()
    .AddJsonOptions(opciones =>
    {
        opciones.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        // DateTime se serializa automaticamente con sufijo Z si es DateTimeKind.Utc
    });
```

Asegurarse de que las instancias de `DateTime` creadas con `DateTime.UtcNow` tienen `Kind = DateTimeKind.Utc`. Si se leen desde SQLite, convertir explicitamente:

```csharp
// Al leer de SQLite via EF, el Kind puede quedar Unspecified
// Forzar UTC al rehidratar la entidad:
public static EntidadTarea Reconstituir(..., DateTime creadoEnUtc, DateTime actualizadoEnUtc) =>
    new()
    {
        CreadoEnUtc      = DateTime.SpecifyKind(creadoEnUtc, DateTimeKind.Utc),
        ActualizadoEnUtc = DateTime.SpecifyKind(actualizadoEnUtc, DateTimeKind.Utc)
    };
```

### 4. DTOs: fechas siempre como `DateTime` con nombre `*Utc`

```csharp
public sealed class TareaDto
{
    public DateTime CreadoEnUtc { get; init; }
    public DateTime ActualizadoEnUtc { get; init; }
}
```

Nunca usar `DateTimeOffset` si no es necesario; para este proyecto, `DateTime` en UTC es suficiente.

## Reglas de frontend

### 1. Tipos TypeScript para fechas

Las fechas llegan del backend como strings ISO 8601 (por ejemplo, `"2026-07-14T10:30:00Z"`). Parsearlas con `new Date()`:

```typescript
// src/frontend/src/types/tarea.ts
export interface Tarea {
  id: number;
  titulo: string;
  descripcion?: string;
  estaCompletada: boolean;
  creadoEnUtc: string;       // string ISO 8601, se convierte al mostrar
  actualizadoEnUtc: string;
}
```

### 2. Conversion a hora local solo al mostrar

```typescript
// src/frontend/src/utils/fechas.ts
export function formatearFechaLocal(isoUtc: string): string {
  return new Date(isoUtc).toLocaleString();
}

export function formatearFechaCorta(isoUtc: string): string {
  return new Date(isoUtc).toLocaleDateString();
}
```

Nunca almacenar ni comparar fechas como strings; usar `Date` para operaciones y strings solo para mostrar.

### 3. No mutar fechas en el frontend

Las fechas de auditoria (`creadoEnUtc`, `actualizadoEnUtc`) son de solo lectura. El frontend no las modifica; el backend las actualiza en cada operacion de escritura.

## Checklist de calidad

- [ ] Todos los campos `DateTime` en el backend usan `DateTime.UtcNow`, no `DateTime.Now`.
- [ ] Los campos de fecha en entidades y DTOs tienen sufijo `Utc`.
- [ ] La serializacion JSON produce fechas con sufijo `Z` (UTC).
- [ ] Al rehidratar desde SQLite se fuerza `DateTimeKind.Utc` con `DateTime.SpecifyKind`.
- [ ] El frontend almacena fechas como `string` en tipos TypeScript y las convierte solo al mostrar.
- [ ] No hay fechas hardcodeadas en tests; usar `DateTime.UtcNow` o fechas fijas documentadas.

## Errores comunes a evitar

- **`DateTime.Now` en el servidor**: captura la zona horaria del servidor, no UTC.
- **Serializar sin sufijo `Z`**: el cliente no puede saber si la fecha es UTC o local.
- **Comparar fechas como strings en frontend**: siempre convertir a `Date` primero.
- **Almacenar `DateTimeOffset` en SQLite**: SQLite no tiene soporte nativo; puede causar perdida de la informacion de zona si no se gestiona correctamente.
- **Mostrar UTC directamente al usuario sin convertir**: confuso para usuarios en zonas horarias distintas.

## Relacion con otros skills

- **SK-01 (Dominio)**: las entidades tienen `CreadoEnUtc` y `ActualizadoEnUtc` con `private set`.
- **SK-02 (DTOs)**: los DTOs de respuesta incluyen los campos UTC serializados correctamente.
- **SK-06 (Persistencia)**: SQLite almacena `DateTime` como texto; asegurarse de que EF los lee como UTC.
- **SK-08 (Cliente frontend)**: el cliente recibe strings ISO 8601 y los convierte con las utilidades de SK-07.
