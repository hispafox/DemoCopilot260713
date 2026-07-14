using AplicacionTareas.Application.Repositorios;
using AplicacionTareas.Domain;
using AplicacionTareas.Infrastructure.Datos;
using Microsoft.EntityFrameworkCore;

namespace AplicacionTareas.Infrastructure.Repositorios;

public sealed class TareaRepositorio : ITareaRepositorio
{
    private readonly AplicacionDbContext contexto;

    public TareaRepositorio(AplicacionDbContext contexto)
    {
        this.contexto = contexto;
    }

    public async Task<IReadOnlyList<Tarea>> ObtenerTodasAsync(CancellationToken cancellationToken = default)
    {
        return await contexto.Tareas
            .AsNoTracking()
            .OrderByDescending(t => t.CreadoEnUtc)
            .ToListAsync(cancellationToken);
    }

    public async Task<Tarea?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await contexto.Tareas
            .FirstOrDefaultAsync(t => t.Id == id, cancellationToken);
    }

    public async Task<Tarea> AgregarAsync(Tarea tarea, CancellationToken cancellationToken = default)
    {
        await contexto.Tareas.AddAsync(tarea, cancellationToken);
        await contexto.SaveChangesAsync(cancellationToken);
        return tarea;
    }

    public async Task EliminarAsync(Tarea tarea, CancellationToken cancellationToken = default)
    {
        contexto.Tareas.Remove(tarea);
        await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task GuardarCambiosAsync(CancellationToken cancellationToken = default)
    {
        await contexto.SaveChangesAsync(cancellationToken);
    }
}
