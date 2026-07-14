using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace AplicacionTareas.Api.Controladores;

[ApiController]
[Route("api/tareas")]
public sealed class TareasControlador : ControllerBase
{
    private readonly ITareaServicio tareaServicio;

    public TareasControlador(ITareaServicio tareaServicio)
    {
        this.tareaServicio = tareaServicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<TareaDto>>> ObtenerTodas(CancellationToken cancellationToken)
    {
        var tareas = await tareaServicio.ObtenerTodasAsync(cancellationToken);
        return Ok(tareas);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TareaDto>> ObtenerPorId(int id, CancellationToken cancellationToken)
    {
        var tarea = await tareaServicio.ObtenerPorIdAsync(id, cancellationToken);

        if (tarea is null)
        {
            return NotFound();
        }

        return Ok(tarea);
    }

    [HttpPost]
    public async Task<ActionResult<TareaDto>> Crear([FromBody] CrearTareaDto dto, CancellationToken cancellationToken)
    {
        var tareaCreada = await tareaServicio.CrearAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = tareaCreada.Id }, tareaCreada);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<TareaDto>> Actualizar(int id, [FromBody] ActualizarTareaDto dto, CancellationToken cancellationToken)
    {
        var tareaActualizada = await tareaServicio.ActualizarAsync(id, dto, cancellationToken);
        return Ok(tareaActualizada);
    }

    [HttpPatch("{id:int}/completar")]
    public async Task<IActionResult> Completar(int id, CancellationToken cancellationToken)
    {
        await tareaServicio.CompletarAsync(id, cancellationToken);
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        await tareaServicio.EliminarAsync(id, cancellationToken);
        return NoContent();
    }
}
