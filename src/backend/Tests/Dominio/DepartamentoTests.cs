using AplicacionTareas.Domain;

namespace AplicacionTareas.Tests.Dominio;

public sealed class DepartamentoTests
{
    [Fact]
    public void Crear_ConNombreValido_DevuelveDepartamentoConAuditoriaUtc()
    {
        var departamento = Departamento.Crear("Operaciones");

        Assert.Equal("Operaciones", departamento.Nombre);
        Assert.Equal(DateTimeKind.Utc, departamento.CreadoEnUtc.Kind);
        Assert.Equal(DateTimeKind.Utc, departamento.ActualizadoEnUtc.Kind);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Crear_ConNombreInvalido_LanzaArgumentException(string nombre)
    {
        Assert.Throws<ArgumentException>(() => Departamento.Crear(nombre));
    }

    [Fact]
    public void ActualizarNombre_ConValorValido_ActualizaDepartamento()
    {
        var departamento = Departamento.Crear("Operaciones");

        departamento.ActualizarNombre("Finanzas");

        Assert.Equal("Finanzas", departamento.Nombre);
    }
}