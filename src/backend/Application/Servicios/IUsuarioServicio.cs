using AplicacionTareas.Application.DTOs;

namespace AplicacionTareas.Application.Servicios;

public interface IUsuarioServicio
{
    Task<IReadOnlyList<UsuarioDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<UsuarioDto> CrearAsync(CrearUsuarioDto dto, CancellationToken cancellationToken = default);
}