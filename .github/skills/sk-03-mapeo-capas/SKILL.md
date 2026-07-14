---
name: sk-03-mapeo-capas
description: "Transforma dominio, DTOs y entidades de persistencia de forma estandar mediante mapeadores reutilizables y faciles de probar. Aplica a cualquier entidad del proyecto."
---

# SK-03 - Mapeo entre capas

Define como traducir objetos entre las tres capas del backend: dominio → DTO (respuesta), DTO (entrada) → dominio, y dominio → entidad EF (y viceversa). Mantiene cada capa aislada sin que ninguna conozca los detalles de la otra.

## Prerequisito: el proyecto debe existir

Antes de aplicar este skill, verificar que la estructura del proyecto esta creada:

```powershell
Test-Path src/backend -PathType Container
Test-Path src/frontend -PathType Container
```

Si alguno devuelve `False`, **detener y aplicar SK-00 primero** (`sk-00-scaffolding-proyecto`).

Ademas, SK-03 requiere que existan tanto el modelo de dominio (SK-01) como los DTOs (SK-02). Si alguna de estas capas esta vacia, aplicar primero el skill correspondiente.

## Cuando usar este skill

- Al necesitar convertir una entidad de dominio en un DTO de respuesta.
- Al recibir un DTO de entrada y construir o actualizar una entidad de dominio.
- Al tener una entidad EF separada del dominio y necesitar traducir entre ellas.
- Siempre que exista la tentacion de poner logica de transformacion dentro del controlador o del servicio.

## Objetivo

Producir mapeadores donde:
- La logica de transformacion esta centralizada y es testeable en aislamiento.
- Ningun controlador ni servicio contiene `new TareaDto { ... }` inline.
- Las capas no dependen entre si mas alla del contrato de mapeo.

## Estrategia de mapeo recomendada: metodos estaticos o clases de extension

Para este proyecto se prefieren **metodos de extension estaticos** sobre AutoMapper u otras librerias, por simplicidad y trazabilidad. Solo introducir una libreria si el volumen de entidades lo justifica claramente.

## Estructura esperada de mapeadores

### Dominio → DTO de respuesta

```csharp
// src/backend/Application/Mapeadores/TareaMapeador.cs
public static class TareaMapeador
{
    public static TareaDto ADto(this EntidadTarea tarea) =>
        new()
        {
            Id              = tarea.Id,
            Titulo          = tarea.Titulo,
            Descripcion     = tarea.Descripcion,
            EstaCompletada  = tarea.EstaCompletada,
            CreadoEnUtc     = tarea.CreadoEnUtc,
            ActualizadoEnUtc = tarea.ActualizadoEnUtc
        };

    public static IEnumerable<TareaDto> AListaDto(this IEnumerable<EntidadTarea> tareas) =>
        tareas.Select(t => t.ADto());
}
```

### DTO de creacion → Dominio (via metodo factoria)

El dominio es quien construye la entidad; el mapeador solo coordina:

```csharp
public static EntidadTarea ADominio(this CrearTareaDto dto) =>
    EntidadTarea.Crear(dto.Titulo, dto.Descripcion);
```

### Dominio ↔ Entidad EF (cuando se separan)

Si el proyecto usa una entidad EF distinta de la entidad de dominio:

```csharp
public static TareaEntidadEf AEntidadEf(this EntidadTarea tarea) =>
    new()
    {
        Id               = tarea.Id,
        Titulo           = tarea.Titulo,
        Descripcion      = tarea.Descripcion,
        EstaCompletada   = tarea.EstaCompletada,
        CreadoEnUtc      = tarea.CreadoEnUtc,
        ActualizadoEnUtc = tarea.ActualizadoEnUtc
    };

public static EntidadTarea ADominio(this TareaEntidadEf ef) =>
    EntidadTarea.Reconstituir(ef.Id, ef.Titulo, ef.Descripcion, ef.EstaCompletada, ef.CreadoEnUtc, ef.ActualizadoEnUtc);
```

> Nota: `Reconstituir` es un segundo metodo factoria en la entidad de dominio que permite rehidratar desde persistencia sin pasar por las validaciones de creacion (el objeto ya fue validado cuando se creo por primera vez).

## Convencion de nombres

- Clase mapeadora: `{Entidad}Mapeador` (por ejemplo, `TareaMapeador`).
- Metodo dominio → DTO: `ADto()`.
- Metodo coleccion → lista de DTOs: `AListaDto()`.
- Metodo DTO → dominio: `ADominio()`.
- Ubicacion: `src/backend/Application/Mapeadores/`.

## Checklist de calidad

- [ ] No hay logica de transformacion inline en controladores ni en servicios.
- [ ] Los mapeadores son metodos de extension estaticos (sin estado, sin inyeccion de dependencias).
- [ ] El metodo `ADominio()` para creacion delega en el metodo factoria de la entidad (no construye la entidad directamente).
- [ ] El mapeador de coleccion usa `Select`, no bucles manuales.
- [ ] Existe un test unitario por cada metodo mapeador no trivial.

## Errores comunes a evitar

- **Poner `new TareaDto { ... }` en el servicio o en el controlador**: centralizar en el mapeador.
- **El mapeador conoce el DbContext**: el mapeador no debe tener acceso a datos, solo transforma objetos.
- **Usar AutoMapper sin justificacion**: para pocas entidades, los metodos estaticos son mas claros y debugueables.
- **Olvidar el metodo `Reconstituir`**: sin el, la entidad de dominio no puede rehidratarse desde la base de datos sin violar sus invariantes.

## Relacion con otros skills

- **SK-01 (Dominio)**: provee las entidades que el mapeador traduce.
- **SK-02 (DTOs)**: provee los tipos de destino del mapeo.
- **SK-04 (Casos de uso)**: los servicios de aplicacion invocan los mapeadores.
- **SK-06 (Persistencia)**: si la entidad EF es distinta del dominio, el repositorio usa los mapeadores de SK-03.
