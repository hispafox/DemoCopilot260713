using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Application.Mapeadores;
using AplicacionTareas.Application.Repositorios;

namespace AplicacionTareas.Application.Servicios;

public sealed class UsuarioServicio : IUsuarioServicio
{
    private readonly IUsuarioRepositorio usuarioRepositorio;
    private readonly IDepartamentoRepositorio departamentoRepositorio;

    public UsuarioServicio(IUsuarioRepositorio usuarioRepositorio, IDepartamentoRepositorio departamentoRepositorio)
    {
        this.usuarioRepositorio = usuarioRepositorio;
        this.departamentoRepositorio = departamentoRepositorio;
    }

    public async Task<IReadOnlyList<UsuarioDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var usuarios = await usuarioRepositorio.ObtenerTodosAsync(cancellationToken);
        return usuarios.Select(usuario => usuario.ADto()).ToList();
    }

    public async Task<UsuarioDto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var usuario = await usuarioRepositorio.ObtenerPorIdAsync(id, cancellationToken);
        return usuario?.ADto();
    }

    public async Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto, CancellationToken cancellationToken = default)
    {
        var departamento = await departamentoRepositorio.ObtenerPorIdAsync(dto.DepartamentoId, cancellationToken);
        if (departamento is null)
        {
            throw new ArgumentException("El departamento del usuario no existe.", nameof(dto.DepartamentoId));
        }

        var usuario = dto.ADominio();
        var usuarioCreado = await usuarioRepositorio.AgregarAsync(usuario, cancellationToken);
        return usuarioCreado.ADto();
    }

    public async Task<UsuarioDto> ActualizarAsync(int id, ActualizarUsuarioDto dto, CancellationToken cancellationToken = default)
    {
        var usuario = await usuarioRepositorio.ObtenerPorIdParaActualizacionAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"El usuario con id {id} no existe.");

        var departamento = await departamentoRepositorio.ObtenerPorIdAsync(dto.DepartamentoId, cancellationToken);
        if (departamento is null)
        {
            throw new ArgumentException("El departamento del usuario no existe.", nameof(dto.DepartamentoId));
        }

        usuario.CambiarNombre(dto.Nombre);
        usuario.CambiarDepartamento(dto.DepartamentoId);

        await usuarioRepositorio.GuardarCambiosAsync(cancellationToken);

        var actualizado = await usuarioRepositorio.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"El usuario con id {id} no existe.");

        return actualizado.ADto();
    }
}