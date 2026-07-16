using AplicacionTareas.Application.Repositorios;
using AplicacionTareas.Domain;
using AplicacionTareas.Infrastructure.Datos;
using Microsoft.EntityFrameworkCore;

namespace AplicacionTareas.Infrastructure.Repositorios;

public sealed class UsuarioRepositorio : IUsuarioRepositorio
{
    private readonly AplicacionDbContext contexto;

    public UsuarioRepositorio(AplicacionDbContext contexto)
    {
        this.contexto = contexto;
    }

    public async Task<IReadOnlyList<Usuario>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await contexto.Usuarios
            .Include(usuario => usuario.Departamento)
            .AsNoTracking()
            .OrderBy(usuario => usuario.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<Usuario?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await contexto.Usuarios
            .Include(usuario => usuario.Departamento)
            .AsNoTracking()
            .FirstOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);
    }

    public async Task<Usuario?> ObtenerPorIdParaActualizacionAsync(int id, CancellationToken cancellationToken = default)
    {
        return await contexto.Usuarios
            .Include(usuario => usuario.Departamento)
            .FirstOrDefaultAsync(usuario => usuario.Id == id, cancellationToken);
    }

    public async Task<Usuario> AgregarAsync(Usuario usuario, CancellationToken cancellationToken = default)
    {
        await contexto.Usuarios.AddAsync(usuario, cancellationToken);
        await contexto.SaveChangesAsync(cancellationToken);
        return usuario;
    }

    public async Task GuardarCambiosAsync(CancellationToken cancellationToken = default)
    {
        await contexto.SaveChangesAsync(cancellationToken);
    }
}