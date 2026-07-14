using AplicacionTareas.Application.DTOs;

namespace AplicacionTareas.Application.Servicios;

public interface ITareaServicio
{
    Task<IReadOnlyList<TareaDto>> ObtenerTodasAsync(CancellationToken cancellationToken = default);

    Task<TareaDto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);

    Task<TareaDto> CrearAsync(CrearTareaDto dto, CancellationToken cancellationToken = default);

    Task<TareaDto> ActualizarAsync(int id, ActualizarTareaDto dto, CancellationToken cancellationToken = default);

    Task<TareaDto> CompletarAsync(int id, CancellationToken cancellationToken = default);

    Task EliminarAsync(int id, CancellationToken cancellationToken = default);
}
