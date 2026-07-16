using AplicacionTareas.Application.Repositorios;
using AplicacionTareas.Domain;
using AplicacionTareas.Infrastructure.Datos;
using Microsoft.EntityFrameworkCore;

namespace AplicacionTareas.Infrastructure.Repositorios;

public sealed class DepartamentoRepositorio : IDepartamentoRepositorio
{
    private readonly AplicacionDbContext contexto;

    public DepartamentoRepositorio(AplicacionDbContext contexto)
    {
        this.contexto = contexto;
    }

    public async Task<IReadOnlyList<Departamento>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        return await contexto.Departamentos
            .AsNoTracking()
            .OrderBy(departamento => departamento.Nombre)
            .ToListAsync(cancellationToken);
    }

    public async Task<Departamento?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        return await contexto.Departamentos
            .FirstOrDefaultAsync(departamento => departamento.Id == id, cancellationToken);
    }

    public async Task<Departamento> AgregarAsync(Departamento departamento, CancellationToken cancellationToken = default)
    {
        await contexto.Departamentos.AddAsync(departamento, cancellationToken);
        await contexto.SaveChangesAsync(cancellationToken);
        return departamento;
    }

    public async Task GuardarCambiosAsync(CancellationToken cancellationToken = default)
    {
        await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task EliminarAsync(Departamento departamento, CancellationToken cancellationToken = default)
    {
        contexto.Departamentos.Remove(departamento);
        await contexto.SaveChangesAsync(cancellationToken);
    }

    public async Task<bool> TieneUsuariosAsociadosAsync(int departamentoId, CancellationToken cancellationToken = default)
    {
        return await contexto.Usuarios
            .AsNoTracking()
            .AnyAsync(usuario => usuario.DepartamentoId == departamentoId, cancellationToken);
    }
}