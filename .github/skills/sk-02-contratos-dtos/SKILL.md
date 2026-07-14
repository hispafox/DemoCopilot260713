---
name: sk-02-contratos-dtos
description: "Crea contratos de entrada/salida estables y desacoplados del modelo de persistencia. DTOs de comando/consulta para cualquier entidad del proyecto."
---

# SK-02 - Diseno de contratos y DTOs

Define los contratos de entrada y salida de la API: DTOs de comando (crear, actualizar) y DTOs de consulta (respuesta), completamente desacoplados de las entidades de dominio y de EF Core.

## Prerequisito: el proyecto debe existir

Antes de aplicar este skill, verificar que la estructura del proyecto esta creada:

```powershell
Test-Path src/backend -PathType Container
Test-Path src/frontend -PathType Container
```

Si alguno de los dos devuelve `False`, **detener y aplicar SK-00 primero** (`sk-00-scaffolding-proyecto`) para crear la estructura base.

Ademas, SK-02 depende de que el modelo de dominio este definido. Si la carpeta `src/backend/Domain/` esta vacia, aplicar **SK-01 primero** (`sk-01-modelado-dominio`).

## Cuando usar este skill

- Al disenar los contratos HTTP de un modulo nuevo (antes de crear el controlador).
- Al necesitar separar lo que recibe la API de lo que almacena la base de datos.
- Al revisar si algun contrato esta exponiendo propiedades internas de la entidad o de EF.
- Al preparar los tipos que consumira el frontend (alineado con SK-08).

## Objetivo

Producir DTOs estables donde:
- El cliente nunca ve la entidad EF directamente.
- Los campos de entrada estan validados con anotaciones o FluentValidation.
- Los campos de salida son proyecciones explicitas del dominio, no referencias directas.
- Los nombres siguen las convenciones del proyecto (castellano, PascalCase).

## Tipos de DTO

| Tipo | Proposito | Ejemplo de nombre |
|------|-----------|-------------------|
| DTO de creacion | Datos que el cliente envia para crear un recurso | `CrearTareaDto` |
| DTO de actualizacion | Datos que el cliente envia para modificar un recurso | `ActualizarTareaDto` |
| DTO de respuesta | Datos que la API devuelve al cliente | `TareaDto` |
| DTO de accion puntual | Operaciones parciales (patch, toggle, etc.) | No suele necesitar DTO propio; usar el endpoint de accion |

## Convencion de nomenclatura

- Prefijo de accion + nombre de entidad + sufijo `Dto`.
- Ejemplos: `CrearTareaDto`, `ActualizarTareaDto`, `TareaDto`.
- No usar nombres genericos como `RequestDto` o `ResponseModel`.
- Nombres en castellano, PascalCase.
- Ubicacion sugerida: `src/backend/Application/DTOs/` o `src/backend/Api/Contratos/`.

## Estructura esperada de DTOs

### DTO de creacion

```csharp
// src/backend/Application/DTOs/CrearTareaDto.cs
public sealed class CrearTareaDto
{
    [Required(ErrorMessage = "El titulo es obligatorio.")]
    [MaxLength(200, ErrorMessage = "El titulo no puede superar 200 caracteres.")]
    public string Titulo { get; init; } = string.Empty;

    public string? Descripcion { get; init; }
}
```

### DTO de actualizacion

```csharp
// src/backend/Application/DTOs/ActualizarTareaDto.cs
public sealed class ActualizarTareaDto
{
    [Required(ErrorMessage = "El titulo es obligatorio.")]
    [MaxLength(200, ErrorMessage = "El titulo no puede superar 200 caracteres.")]
    public string Titulo { get; init; } = string.Empty;

    public string? Descripcion { get; init; }
}
```

### DTO de respuesta

```csharp
// src/backend/Application/DTOs/TareaDto.cs
public sealed class TareaDto
{
    public int Id { get; init; }
    public string Titulo { get; init; } = string.Empty;
    public string? Descripcion { get; init; }
    public bool EstaCompletada { get; init; } = false;
    public DateTime CreadoEnUtc { get; init; }
    public DateTime ActualizadoEnUtc { get; init; }
}
```

## Reglas de calidad de los DTOs

1. **DTOs de entrada son inmutables**: usar `init` en lugar de `set` para evitar mutaciones accidentales.
2. **Validaciones en el DTO de entrada, no en el controlador**: las anotaciones `[Required]`, `[MaxLength]`, etc. van en el DTO.
3. **DTOs de respuesta no exponen ids internos no relevantes ni propiedades de navegacion EF**: solo campos que el cliente necesita.
4. **Booleanas con valor inicial**: `public bool EstaCompletada { get; init; } = false;`
5. **Fechas en UTC**: los campos de tipo `DateTime` representan siempre UTC. Nombrar con sufijo `Utc`.
6. **No heredar de la entidad de dominio**: el DTO es una proyeccion, no una extension.

## Proceso recomendado

1. Identificar los casos de uso del modulo (crear, leer, actualizar, eliminar, acciones puntuales).
2. Por cada caso de uso de escritura → definir un DTO de entrada con sus validaciones.
3. Por cada caso de uso de lectura → definir un DTO de respuesta con solo los campos necesarios.
4. Verificar que ningun campo del DTO de respuesta exponga detalles de implementacion (claves foraneas internas, propiedades de navegacion, etc.).
5. Documentar con comentario XML los campos cuyo proposito no sea obvio por el nombre.

## Checklist de calidad

- [ ] Cada operacion de escritura tiene su propio DTO de entrada (`Crear...Dto`, `Actualizar...Dto`).
- [ ] El DTO de respuesta no es la entidad EF ni el modelo de dominio.
- [ ] Los campos de entrada tienen anotaciones de validacion (`[Required]`, `[MaxLength]`, etc.).
- [ ] Las propiedades de DTOs de entrada usan `init` (inmutables).
- [ ] Las booleanas tienen valor inicial explicito.
- [ ] Las fechas se llaman con sufijo `Utc` y son `DateTime` en UTC.
- [ ] Los nombres siguen PascalCase en castellano.
- [ ] No hay logica de negocio dentro del DTO (ni metodos, ni calculos).

## Errores comunes a evitar

- **Exponer la entidad EF como respuesta**: el cliente no debe conocer el esquema de persistencia.
- **Un DTO unico para crear y actualizar**: suelen tener validaciones distintas; mejor separarlos.
- **Validaciones en el controlador**: deben estar en el DTO para que el framework las evalue automaticamente via `ModelState`.
- **Propiedades con `set` publico en DTOs de entrada**: facilita mutaciones no controladas; usar `init`.
- **Fechas sin UTC**: serializar como `DateTime` sin zona puede causar interpretaciones incorrectas en el cliente.

## Relacion con otros skills

- **SK-01 (Modelado de dominio)**: el dominio es la fuente de verdad; el DTO es una proyeccion de el.
- **SK-03 (Mapeo entre capas)**: los mapeadores traducen entre entidad de dominio y DTO.
- **SK-05 (API REST)**: los controladores reciben DTOs de entrada y devuelven DTOs de respuesta.
- **SK-08 (Cliente frontend)**: los tipos TypeScript del frontend deben estar alineados con estos DTOs.
