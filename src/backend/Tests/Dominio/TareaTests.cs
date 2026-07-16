using AplicacionTareas.Domain;

namespace AplicacionTareas.Tests.Dominio;

public sealed class TareaTests
{
    [Fact]
    public void Crear_ConTituloValido_DevuelveTareaPendiente()
    {
        var tarea = Tarea.Crear("Preparar entrega", "Revisar puntos finales", "Trabajo", 1, null);

        Assert.Equal("Preparar entrega", tarea.Titulo);
        Assert.Equal("Trabajo", tarea.Categoria);
        Assert.Equal(1, tarea.UsuarioAsignadoId);
        Assert.False(tarea.EstaCompletada);
        Assert.Equal(DateTimeKind.Utc, tarea.CreadoEnUtc.Kind);
        Assert.Equal(DateTimeKind.Utc, tarea.ActualizadoEnUtc.Kind);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Crear_ConTituloInvalido_LanzaArgumentException(string titulo)
    {
        Assert.Throws<ArgumentException>(() => Tarea.Crear(titulo, null, "Trabajo", 1, null));
    }

    [Fact]
    public void Crear_ConTituloMayorAMaximo_LanzaArgumentException()
    {
        var titulo = new string('a', 201);

        Assert.Throws<ArgumentException>(() => Tarea.Crear(titulo, null, "Trabajo", 1, null));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void Crear_ConCategoriaInvalida_LanzaArgumentException(string categoria)
    {
        Assert.Throws<ArgumentException>(() => Tarea.Crear("Preparar entrega", null, categoria, 1, null));
    }

    [Fact]
    public void Crear_ConUsuarioAsignadoInvalido_LanzaArgumentException()
    {
        Assert.Throws<ArgumentException>(() => Tarea.Crear("Preparar entrega", null, "Trabajo", 0, null));
    }

    [Fact]
    public void Completar_CuandoYaEstaCompletada_LanzaInvalidOperationException()
    {
        var tarea = Tarea.Crear("Enviar informe", null, "Trabajo", 1, null);
        tarea.Completar();

        Assert.Throws<InvalidOperationException>(() => tarea.Completar());
    }

    [Fact]
    public void Actualizar_ConTituloInvalido_LanzaArgumentException()
    {
        var tarea = Tarea.Crear("Titulo inicial", null, "Trabajo", 1, null);

        Assert.Throws<ArgumentException>(() => tarea.Actualizar("", null, "Trabajo", null));
    }

    [Fact]
    public void Actualizar_ConCategoriaValida_ActualizaCategoriaYMarcaAuditoria()
    {
        var tarea = Tarea.Crear("Titulo inicial", null, "Trabajo", 1, null);
        var actualizadoEnUtcInicial = tarea.ActualizadoEnUtc;

        tarea.Actualizar("Titulo actualizado", "Descripcion", "Personal", DateTime.UtcNow.AddDays(1));

        Assert.Equal("Personal", tarea.Categoria);
        Assert.True(tarea.ActualizadoEnUtc >= actualizadoEnUtcInicial);
    }

    [Fact]
    public void Crear_ConVencimiento_ConservaFechaEnUtc()
    {
        var vencimientoUtc = DateTime.UtcNow.AddDays(2);

        var tarea = Tarea.Crear("Revisar contrato", null, "Trabajo", 1, vencimientoUtc);

        Assert.Equal(vencimientoUtc, tarea.VenceEnUtc);
        Assert.Equal(DateTimeKind.Utc, tarea.VenceEnUtc!.Value.Kind);
    }

    [Fact]
    public void Crear_ConVencimientoPasado_LanzaArgumentException()
    {
        var vencimientoPasadoUtc = DateTime.UtcNow.AddMinutes(-5);

        Assert.Throws<ArgumentException>(() => Tarea.Crear("Tarea vencida", null, "Trabajo", 1, vencimientoPasadoUtc));
    }
}
