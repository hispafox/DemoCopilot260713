using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Application.Mapeadores;
using AplicacionTareas.Application.Repositorios;

namespace AplicacionTareas.Application.Servicios;

public sealed class UsuarioServicio : IUsuarioServicio
{
    private readonly IUsuarioRepositorio usuarioRepositorio;

    public UsuarioServicio(IUsuarioRepositorio usuarioRepositorio)
    {
        this.usuarioRepositorio = usuarioRepositorio;
    }

    public async Task<IReadOnlyList<UsuarioDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var usuarios = await usuarioRepositorio.ObtenerTodosAsync(cancellationToken);
        return usuarios.Select(usuario => usuario.ADto()).ToList();
    }

    public async Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto, CancellationToken cancellationToken = default)
    {
        var usuario = dto.ADominio();
        var usuarioCreado = await usuarioRepositorio.AgregarAsync(usuario, cancellationToken);
        return usuarioCreado.ADto();
    }
}