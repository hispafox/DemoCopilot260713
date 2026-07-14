---
name: sk-05-api-rest
description: "Expone endpoints REST con convenciones uniformes: rutas, codigos HTTP semanticos, validacion de entrada y manejo centralizado de errores. Aplica a cualquier recurso del proyecto."
---

# SK-05 - API REST consistente

Define como construir los controladores y endpoints de la API siguiendo convenciones uniformes de rutas, codigos HTTP, validacion y manejo de errores. El controlador es solo un adaptador HTTP: recibe, delega al servicio y responde.

## Prerequisito: el proyecto debe existir

Antes de aplicar este skill, verificar que la estructura del proyecto esta creada:

```powershell
Test-Path src/backend -PathType Container
Test-Path src/frontend -PathType Container
```

Si alguno devuelve `False`, **detener y aplicar SK-00 primero** (`sk-00-scaffolding-proyecto`).

SK-05 requiere que existan los DTOs (SK-02) y el servicio de aplicacion (SK-04). Aplicarlos antes si no existen.

## Cuando usar este skill

- Al crear un controlador nuevo para un recurso.
- Al revisar si los codigos HTTP de respuesta son semanticamente correctos.
- Al necesitar estandarizar el manejo de errores entre endpoints.

## Objetivo

Producir controladores donde:
- El controlador no contiene logica de negocio.
- Los codigos HTTP son semanticos y consistentes en toda la API.
- Los errores se manejan de forma centralizada (no `try/catch` en cada endpoint).
- La validacion de entrada es automatica via `ModelState` y las anotaciones del DTO.

## Convencion de rutas

```
GET    /api/{recurso}           → listar todos
GET    /api/{recurso}/{id}      → obtener por id
POST   /api/{recurso}           → crear
PUT    /api/{recurso}/{id}      → actualizar completo
PATCH  /api/{recurso}/{id}/{accion} → accion parcial (completar, archivar, etc.)
DELETE /api/{recurso}/{id}      → eliminar
```

Ejemplos para Tareas:
```
GET    /api/tareas
GET    /api/tareas/{id}
POST   /api/tareas
PUT    /api/tareas/{id}
PATCH  /api/tareas/{id}/completar
DELETE /api/tareas/{id}
```

## Codigos HTTP semanticos

| Situacion | Codigo |
|-----------|--------|
| Lectura exitosa | 200 OK |
| Creacion exitosa | 201 Created + Location header |
| Accion sin body de respuesta (completar, eliminar) | 204 No Content |
| Validacion fallida | 400 Bad Request |
| Recurso no encontrado | 404 Not Found |
| Error interno del servidor | 500 Internal Server Error (via middleware) |

## Estructura esperada del controlador

```csharp
// src/backend/Api/Controladores/TareasControlador.cs
[ApiController]
[Route("api/tareas")]
public sealed class TareasControlador : ControllerBase
{
    private readonly ITareasServicio _servicio;

    public TareasControlador(ITareasServicio servicio)
    {
        _servicio = servicio;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TareaDto>>> ObtenerTodas() =>
        Ok(await _servicio.ObtenerTodasAsync());

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TareaDto>> ObtenerPorId(int id)
    {
        var tarea = await _servicio.ObtenerPorIdAsync(id);
        return tarea is null ? NotFound() : Ok(tarea);
    }

    [HttpPost]
    public async Task<ActionResult<TareaDto>> Crear([FromBody] CrearTareaDto dto)
    {
        var creada = await _servicio.CrearAsync(dto);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creada.Id }, creada);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TareaDto>> Actualizar(int id, [FromBody] ActualizarTareaDto dto)
    {
        var actualizada = await _servicio.ActualizarAsync(id, dto);
        return Ok(actualizada);
    }

    [HttpPatch("{id:int}/completar")]
    public async Task<IActionResult> Completar(int id)
    {
        await _servicio.CompletarAsync(id);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        await _servicio.EliminarAsync(id);
        return NoContent();
    }
}
```

## Manejo centralizado de errores

Usar un middleware o un filtro de excepciones en lugar de `try/catch` en cada endpoint. Esto evita duplicar logica de mapeo de errores.

```csharp
// src/backend/Api/Middleware/ManejadorExcepcionesMiddleware.cs
public sealed class ManejadorExcepcionesMiddleware
{
    private readonly RequestDelegate _siguiente;

    public ManejadorExcepcionesMiddleware(RequestDelegate siguiente)
    {
        _siguiente = siguiente;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await _siguiente(contexto);
        }
        catch (KeyNotFoundException ex)
        {
            contexto.Response.StatusCode = StatusCodes.Status404NotFound;
            await contexto.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            contexto.Response.StatusCode = StatusCodes.Status400BadRequest;
            await contexto.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
        catch (ArgumentException ex)
        {
            contexto.Response.StatusCode = StatusCodes.Status400BadRequest;
            await contexto.Response.WriteAsJsonAsync(new { error = ex.Message });
        }
    }
}
```

Registro en `Program.cs`:
```csharp
app.UseMiddleware<ManejadorExcepcionesMiddleware>();
```

## Validacion automatica de entrada

Con `[ApiController]`, ASP.NET Core valida el `ModelState` automaticamente antes de entrar al metodo del controlador. Si la validacion falla, devuelve 400 con el detalle de los errores sin necesidad de codigo adicional.

Para que funcione, las anotaciones deben estar en el DTO (SK-02), no en el controlador.

## Checklist de calidad

- [ ] El controlador no contiene logica de negocio ni acceso a datos.
- [ ] Las rutas siguen la convencion `api/{recurso}/{id?}/{accion?}`.
- [ ] Los codigos HTTP son semanticos (201 para creacion, 204 para acciones sin body).
- [ ] `POST` devuelve `CreatedAtAction` con el header `Location`.
- [ ] El manejo de `KeyNotFoundException` esta en el middleware, no en cada endpoint.
- [ ] No hay `try/catch` en los metodos del controlador.
- [ ] La validacion de entrada es automatica via `[ApiController]` + anotaciones en el DTO.

## Errores comunes a evitar

- **Logica de negocio en el controlador**: debe estar en el servicio o en el dominio.
- **Devolver siempre 200**: usar el codigo semantico correcto para cada situacion.
- **`try/catch` por endpoint**: centralizar en middleware.
- **Validar manualmente el DTO en el controlador**: usar las anotaciones del DTO y dejar que `[ApiController]` las evalúe.
- **Rutas inconsistentes entre controladores**: seguir siempre la misma convencion.

## Relacion con otros skills

- **SK-02 (DTOs)**: el controlador recibe y devuelve DTOs definidos en SK-02.
- **SK-04 (Casos de uso)**: el controlador delega toda la logica al servicio.
- **SK-07 (UTC)**: las fechas en los DTOs de respuesta deben serializar en ISO 8601 con Z.
- **SK-08 (Cliente frontend)**: el cliente frontend consume estos endpoints.
- **SK-09 (Pruebas)**: los endpoints criticos deben tener tests de integracion.
