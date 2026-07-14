using AplicacionTareas.Domain;

namespace AplicacionTareas.Tests.Dominio;

public sealed class TareaTests
{
    [Fact]
    public void Crear_ConTituloValido_DevuelveTareaPendiente()
    {
        var tarea = Tarea.Crear("Preparar entrega", "Revisar puntos finales", null);

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
        Assert.Throws<ArgumentException>(() => Tarea.Crear(titulo, null, null));
    }

    [Fact]
    public void Crear_ConTituloMayorAMaximo_LanzaArgumentException()
    {
        var titulo = new string('a', 201);

        Assert.Throws<ArgumentException>(() => Tarea.Crear(titulo, null, null));
    }

    [Fact]
    public void Completar_CuandoYaEstaCompletada_LanzaInvalidOperationException()
    {
        var tarea = Tarea.Crear("Enviar informe", null, null);
        tarea.Completar();

        Assert.Throws<InvalidOperationException>(() => tarea.Completar());
    }

    [Fact]
    public void Actualizar_ConTituloInvalido_LanzaArgumentException()
    {
        var tarea = Tarea.Crear("Titulo inicial", null, null);

        Assert.Throws<ArgumentException>(() => tarea.Actualizar("", null, null));
    }

    [Fact]
    public void Crear_ConVencimiento_ConservaFechaEnUtc()
    {
        var vencimientoUtc = DateTime.UtcNow.AddDays(2);

        var tarea = Tarea.Crear("Revisar contrato", null, vencimientoUtc);

        Assert.Equal(vencimientoUtc, tarea.VenceEnUtc);
        Assert.Equal(DateTimeKind.Utc, tarea.VenceEnUtc!.Value.Kind);
    }

    [Fact]
    public void Crear_ConVencimientoPasado_LanzaArgumentException()
    {
        var vencimientoPasadoUtc = DateTime.UtcNow.AddMinutes(-5);

        Assert.Throws<ArgumentException>(() => Tarea.Crear("Tarea vencida", null, vencimientoPasadoUtc));
    }
}
