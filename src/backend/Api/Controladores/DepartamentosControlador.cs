using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace AplicacionTareas.Api.Controladores;

[ApiController]
[Route("api/departamentos")]
public sealed class DepartamentosControlador : ControllerBase
{
    private readonly IDepartamentoServicio departamentoServicio;

    public DepartamentosControlador(IDepartamentoServicio departamentoServicio)
    {
        this.departamentoServicio = departamentoServicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<DepartamentoDto>>> ObtenerTodos(CancellationToken cancellationToken)
    {
        var departamentos = await departamentoServicio.ObtenerTodosAsync(cancellationToken);
        return Ok(departamentos);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<DepartamentoDto>> ObtenerPorId(int id, CancellationToken cancellationToken)
    {
        var departamento = await departamentoServicio.ObtenerPorIdAsync(id, cancellationToken);
        if (departamento is null)
        {
            return NotFound();
        }

        return Ok(departamento);
    }

    [HttpPost]
    public async Task<ActionResult<DepartamentoDto>> Crear([FromBody] CrearDepartamentoDto dto, CancellationToken cancellationToken)
    {
        var creado = await departamentoServicio.CrearAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = creado.Id }, creado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<DepartamentoDto>> Actualizar(int id, [FromBody] ActualizarDepartamentoDto dto, CancellationToken cancellationToken)
    {
        var actualizado = await departamentoServicio.ActualizarAsync(id, dto, cancellationToken);
        return Ok(actualizado);
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id, CancellationToken cancellationToken)
    {
        await departamentoServicio.EliminarAsync(id, cancellationToken);
        return NoContent();
    }
}