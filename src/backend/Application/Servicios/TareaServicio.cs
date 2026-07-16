using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Application.Mapeadores;
using AplicacionTareas.Application.Repositorios;

namespace AplicacionTareas.Application.Servicios;

public sealed class TareaServicio : ITareaServicio
{
    private readonly ITareaRepositorio tareaRepositorio;
    private readonly IUsuarioRepositorio usuarioRepositorio;

    public TareaServicio(ITareaRepositorio tareaRepositorio, IUsuarioRepositorio usuarioRepositorio)
    {
        this.tareaRepositorio = tareaRepositorio;
        this.usuarioRepositorio = usuarioRepositorio;
    }

    public async Task<IReadOnlyList<TareaDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        var tareas = await tareaRepositorio.ObtenerTodasAsync(cancellationToken);
        return tareas.AListaDto();
    }

    public async Task<TareaDto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var tarea = await tareaRepositorio.ObtenerPorIdAsync(id, cancellationToken);
        return tarea?.ADto();
    }

    public async Task<TareaDto> CrearAsync(CrearTareaDto dto, CancellationToken cancellationToken = default)
    {
        var usuarioAsignado = await usuarioRepositorio.ObtenerPorIdAsync(dto.UsuarioAsignadoId, cancellationToken);
        if (usuarioAsignado is null)
        {
            throw new ArgumentException("El usuario asignado no existe.", nameof(dto.UsuarioAsignadoId));
        }

        var tarea = dto.ADominio();
        var tareaCreada = await tareaRepositorio.AgregarAsync(tarea, cancellationToken);
        return tareaCreada.ADto();
    }

    public async Task<TareaDto> ActualizarAsync(int id, ActualizarTareaDto dto, CancellationToken cancellationToken = default)
    {
        var tarea = await tareaRepositorio.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"La tarea con id {id} no existe.");

        tarea.Actualizar(dto.Titulo, dto.Descripcion, dto.Categoria, dto.VenceEnUtc);
        await tareaRepositorio.GuardarCambiosAsync(cancellationToken);

        return tarea.ADto();
    }

    public async Task<TareaDto> CompletarAsync(int id, CancellationToken cancellationToken = default)
    {
        var tarea = await tareaRepositorio.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"La tarea con id {id} no existe.");

        tarea.Completar();
        await tareaRepositorio.GuardarCambiosAsync(cancellationToken);

        return tarea.ADto();
    }

    public async Task EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var tarea = await tareaRepositorio.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"La tarea con id {id} no existe.");

        await tareaRepositorio.EliminarAsync(tarea, cancellationToken);
    }
}
