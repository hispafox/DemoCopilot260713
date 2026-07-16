using AplicacionTareas.Domain;

namespace AplicacionTareas.Tests.Dominio;

public sealed class UsuarioTests
{
    [Fact]
    public void Crear_ConNombreValido_DevuelveUsuarioConAuditoriaUtc()
    {
        var usuario = Usuario.Crear("Ana Martinez");

        Assert.Equal("Ana Martinez", usuario.Nombre);
        Assert.Equal(DateTimeKind.Utc, usuario.CreadoEnUtc.Kind);
        Assert.Equal(DateTimeKind.Utc, usuario.ActualizadoEnUtc.Kind);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Crear_ConNombreInvalido_LanzaArgumentException(string nombre)
    {
        Assert.Throws<ArgumentException>(() => Usuario.Crear(nombre));
    }

    [Fact]
    public void Crear_ConNombreMayorAMaximo_LanzaArgumentException()
    {
        var nombre = new string('a', 201);

        Assert.Throws<ArgumentException>(() => Usuario.Crear(nombre));
    }
}