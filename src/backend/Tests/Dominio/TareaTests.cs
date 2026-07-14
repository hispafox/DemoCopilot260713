using AplicacionTareas.Domain;

namespace AplicacionTareas.Tests.Dominio;

public sealed class TareaTests
{
    [Fact]
    public void Crear_ConTituloValido_DevuelveTareaPendiente()
    {
        var tarea = Tarea.Crear("Preparar entrega", "Revisar puntos finales");

        Assert.Equal("Preparar entrega", tarea.Titulo);
        Assert.False(tarea.EstaCompletada);
        Assert.Equal(DateTimeKind.Utc, tarea.CreadoEnUtc.Kind);
        Assert.Equal(DateTimeKind.Utc, tarea.ActualizadoEnUtc.Kind);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Crear_ConTituloInvalido_LanzaArgumentException(string titulo)
    {
        Assert.Throws<ArgumentException>(() => Tarea.Crear(titulo, null));
    }

    [Fact]
    public void Crear_ConTituloMayorAMaximo_LanzaArgumentException()
    {
        var titulo = new string('a', 201);

        Assert.Throws<ArgumentException>(() => Tarea.Crear(titulo, null));
    }

    [Fact]
    public void Completar_CuandoYaEstaCompletada_LanzaInvalidOperationException()
    {
        var tarea = Tarea.Crear("Enviar informe", null);
        tarea.Completar();

        Assert.Throws<InvalidOperationException>(() => tarea.Completar());
    }

    [Fact]
    public void Actualizar_ConTituloInvalido_LanzaArgumentException()
    {
        var tarea = Tarea.Crear("Titulo inicial", null);

        Assert.Throws<ArgumentException>(() => tarea.Actualizar("", null));
    }
}
