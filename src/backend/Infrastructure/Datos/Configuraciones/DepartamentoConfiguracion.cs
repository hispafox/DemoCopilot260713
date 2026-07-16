using AplicacionTareas.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AplicacionTareas.Infrastructure.Datos.Configuraciones;

public sealed class DepartamentoConfiguracion : IEntityTypeConfiguration<Departamento>
{
    public void Configure(EntityTypeBuilder<Departamento> builder)
    {
        var convertidorUtc = new ValueConverter<DateTime, DateTime>(
            valor => valor,
            valor => DateTime.SpecifyKind(valor, DateTimeKind.Utc));

        builder.ToTable("Departamentos");

        builder.HasKey(departamento => departamento.Id);

        builder.Property(departamento => departamento.Id)
            .ValueGeneratedOnAdd();

        builder.Property(departamento => departamento.Nombre)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(departamento => departamento.CreadoEnUtc)
            .IsRequired()
            .HasConversion(convertidorUtc);

        builder.Property(departamento => departamento.ActualizadoEnUtc)
            .IsRequired()
            .HasConversion(convertidorUtc);

        builder.HasIndex(departamento => departamento.Nombre)
            .IsUnique()
            .HasDatabaseName("IX_Departamentos_Nombre");
    }
}