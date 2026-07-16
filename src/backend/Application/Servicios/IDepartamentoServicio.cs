using AplicacionTareas.Application.DTOs;

namespace AplicacionTareas.Application.Servicios;

public interface IDepartamentoServicio
{
    Task<IReadOnlyList<DepartamentoDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<DepartamentoDto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);

    Task<DepartamentoDto> CrearAsync(CrearDepartamentoDto dto, CancellationToken cancellationToken = default);

    Task<DepartamentoDto> ActualizarAsync(int id, ActualizarDepartamentoDto dto, CancellationToken cancellationToken = default);

    Task EliminarAsync(int id, CancellationToken cancellationToken = default);
}