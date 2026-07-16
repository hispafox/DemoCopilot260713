using AplicacionTareas.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace AplicacionTareas.Infrastructure.Datos.Configuraciones;

public sealed class UsuarioConfiguracion : IEntityTypeConfiguration<Usuario>
{
    public void Configure(EntityTypeBuilder<Usuario> builder)
    {
        var convertidorUtc = new ValueConverter<DateTime, DateTime>(
            valor => valor,
            valor => DateTime.SpecifyKind(valor, DateTimeKind.Utc));

        builder.ToTable("Usuarios");

        builder.HasKey(usuario => usuario.Id);

        builder.Property(usuario => usuario.Id)
            .ValueGeneratedOnAdd();

        builder.Property(usuario => usuario.Nombre)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(usuario => usuario.DepartamentoId)
            .IsRequired();

        builder.Property(usuario => usuario.CreadoEnUtc)
            .IsRequired()
            .HasConversion(convertidorUtc);

        builder.Property(usuario => usuario.ActualizadoEnUtc)
            .IsRequired()
            .HasConversion(convertidorUtc);

        builder.HasIndex(usuario => usuario.Nombre)
            .HasDatabaseName("IX_Usuarios_Nombre");

        builder.HasIndex(usuario => usuario.DepartamentoId)
            .HasDatabaseName("IX_Usuarios_DepartamentoId");

        builder.HasOne(usuario => usuario.Departamento)
            .WithMany(departamento => departamento.Usuarios)
            .HasForeignKey(usuario => usuario.DepartamentoId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}