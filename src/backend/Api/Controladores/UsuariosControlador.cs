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

    [HttpPost]
    public async Task<ActionResult<UsuarioDto>> Crear([FromBody] CrearUsuarioDto dto, CancellationToken cancellationToken)
    {
        var usuarioCreado = await usuarioServicio.CrearAsync(dto, cancellationToken);
        return Created($"/api/usuarios/{usuarioCreado.Id}", usuarioCreado);
    }
}