using AplicacionTareas.Domain;

namespace AplicacionTareas.Application.Repositorios;

public interface IDepartamentoRepositorio
{
    Task<IReadOnlyList<Departamento>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<Departamento?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Departamento> AgregarAsync(Departamento departamento, CancellationToken cancellationToken = default);

    Task GuardarCambiosAsync(CancellationToken cancellationToken = default);

    Task EliminarAsync(Departamento departamento, CancellationToken cancellationToken = default);

    Task<bool> TieneUsuariosAsociadosAsync(int departamentoId, CancellationToken cancellationToken = default);
}