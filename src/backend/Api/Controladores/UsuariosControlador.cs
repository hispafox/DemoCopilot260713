using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Application.Servicios;
using Microsoft.AspNetCore.Mvc;

namespace AplicacionTareas.Api.Controladores;

[ApiController]
[Route("api/usuarios")]
public sealed class UsuariosControlador : ControllerBase
{
    private readonly IUsuarioServicio usuarioServicio;

    public UsuariosControlador(IUsuarioServicio usuarioServicio)
    {
        this.usuarioServicio = usuarioServicio;
    }

    [HttpGet]
    public async Task<ActionResult<IReadOnlyList<UsuarioDto>>> ObtenerTodos(CancellationToken cancellationToken)
    {
        var usuarios = await usuarioServicio.ObtenerTodosAsync(cancellationToken);
        return Ok(usuarios);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioDto>> ObtenerPorId(int id, CancellationToken cancellationToken)
    {
        var usuario = await usuarioServicio.ObtenerPorIdAsync(id, cancellationToken);
        if (usuario is null)
        {
            return NotFound();
        }

        return Ok(usuario);
    }

    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> Crear([FromBody] CrearUsuarioDto dto, CancellationToken cancellationToken)
    {
        var usuarioCreado = await usuarioServicio.CrearAsync(dto, cancellationToken);
        return CreatedAtAction(nameof(ObtenerPorId), new { id = usuarioCreado.Id }, usuarioCreado);
    }

    [HttpPut("{id:int}")]
    public async Task<ActionResult<UsuarioDto>> Actualizar(int id, [FromBody] ActualizarUsuarioDto dto, CancellationToken cancellationToken)
    {
        var usuarioActualizado = await usuarioServicio.ActualizarAsync(id, dto, cancellationToken);
        return Ok(usuarioActualizado);
    }
}