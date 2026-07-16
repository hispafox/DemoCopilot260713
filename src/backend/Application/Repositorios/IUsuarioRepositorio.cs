using AplicacionTareas.Domain;

namespace AplicacionTareas.Application.Repositorios;

public interface IUsuarioRepositorio
{
    Task<IReadOnlyList<Usuario>> ObtenerTodosAsync(CancellationToken cancellationToken = default);

    Task<Usuario?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default);

    Task<Usuario> AgregarAsync(Usuario usuario, CancellationToken cancellationToken = default);
}