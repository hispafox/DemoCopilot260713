using AplicacionTareas.Domain;

namespace AplicacionTareas.Application.Repositorios;

public interface ITareaRepositorio
{
    Task<IReadOnlyList<Tarea>> ObtenerTodasAsync(CancellationToken cancellationToken = default);

    Task<Tarea?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Tarea> AgregarAsync(Tarea tarea, CancellationToken cancellationToken = default);

    Task EliminarAsync(Tarea tarea, CancellationToken cancellationToken = default);

    Task GuardarCambiosAsync(CancellationToken cancellationToken = default);
}
