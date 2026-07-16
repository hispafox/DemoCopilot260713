using AplicacionTareas.Domain;
using Microsoft.EntityFrameworkCore;

namespace AplicacionTareas.Infrastructure.Datos;

public sealed class AplicacionDbContext : DbContext
{
    public AplicacionDbContext(DbContextOptions<AplicacionDbContext> opciones)
        : base(opciones)
    {
    }

    public DbSet<Tarea> Tareas => Set<Tarea>();

    public DbSet<Usuario> Usuarios => Set<Usuario>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AplicacionDbContext).Assembly);
    }
}
