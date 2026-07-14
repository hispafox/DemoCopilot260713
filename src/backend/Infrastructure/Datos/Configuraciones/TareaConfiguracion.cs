using AplicacionTareas.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AplicacionTareas.Infrastructure.Datos.Configuraciones;

public sealed class TareaConfiguracion : IEntityTypeConfiguration<Tarea>
{
    public void Configure(EntityTypeBuilder<Tarea> builder)
    {
        var convertidorUtc = new ValueConverter<DateTime, DateTime>(
            valor => valor,
            valor => DateTime.SpecifyKind(valor, DateTimeKind.Utc));

        var convertidorUtcNullable = new ValueConverter<DateTime?, DateTime?>(
            valor => valor,
            valor => valor.HasValue
                ? DateTime.SpecifyKind(valor.Value, DateTimeKind.Utc)
                : valor);

        builder.ToTable("Tareas");

        builder.HasKey(tarea => tarea.Id);

        builder.Property(tarea => tarea.Id)
            .ValueGeneratedOnAdd();

        builder.Property(tarea => tarea.Titulo)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(tarea => tarea.Descripcion)
            .HasMaxLength(1000);

        builder.Property(tarea => tarea.EstaCompletada)
            .IsRequired()
            .HasDefaultValue(false);

        builder.Property(tarea => tarea.CreadoEnUtc)
            .IsRequired()
            .HasConversion(convertidorUtc);

        builder.Property(tarea => tarea.ActualizadoEnUtc)
            .IsRequired()
            .HasConversion(convertidorUtc);

        builder.Property(tarea => tarea.VenceEnUtc)
            .HasConversion(convertidorUtcNullable);

        builder.HasIndex(tarea => tarea.EstaCompletada)
            .HasDatabaseName("IX_Tareas_EstaCompletada");

        builder.HasIndex(tarea => tarea.Titulo)
            .HasDatabaseName("IX_Tareas_Titulo");

        builder.HasIndex(tarea => tarea.VenceEnUtc)
            .HasDatabaseName("IX_Tareas_VenceEnUtc");
    }
}
