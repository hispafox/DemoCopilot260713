using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Application.Repositorios;
using AplicacionTareas.Application.Servicios;
using AplicacionTareas.Domain;
using NSubstitute;

namespace AplicacionTareas.Tests.Servicios;

public sealed class TareaServicioTests
{
    private readonly ITareaRepositorio tareaRepositorio;
    private readonly TareaServicio tareaServicio;

    public TareaServicioTests()
    {
        tareaRepositorio = Substitute.For<ITareaRepositorio>();
        tareaServicio = new TareaServicio(tareaRepositorio);
    }

    [Fact]
    public async Task ObtenerPorIdAsync_CuandoNoExiste_DevuelveNull()
    {
        tareaRepositorio.ObtenerPorIdAsync(42, Arg.Any<CancellationToken>())
            .Returns((Tarea?)null);

        var resultado = await tareaServicio.ObtenerPorIdAsync(42, CancellationToken.None);

        Assert.Null(resultado);
    }

    [Fact]
    public async Task CrearAsync_ConDtoValido_CreaYDevuelveDto()
    {
        var dto = new CrearTareaDto
        {
            Titulo = "Documentar release",
            Descripcion = "Actualizar changelog",
            VenceEnUtc = DateTime.UtcNow.AddDays(1),
        };

        tareaRepositorio.AgregarAsync(Arg.Any<Tarea>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult<Tarea>(x.Arg<Tarea>()!));

        var resultado = await tareaServicio.CrearAsync(dto, CancellationToken.None);

        await tareaRepositorio.Received(1).AgregarAsync(Arg.Any<Tarea>(), Arg.Any<CancellationToken>());
        Assert.Equal(dto.Titulo, resultado.Titulo);
        Assert.False(resultado.EstaCompletada);
        Assert.Equal(dto.VenceEnUtc, resultado.VenceEnUtc);
    }

    [Fact]
    public async Task CompletarAsync_CuandoNoExiste_LanzaKeyNotFoundException()
    {
        tareaRepositorio.ObtenerPorIdAsync(10, Arg.Any<CancellationToken>())
            .Returns((Tarea?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await tareaServicio.CompletarAsync(10, CancellationToken.None));
    }

    [Fact]
    public async Task CrearAsync_ConVencimientoPasado_LanzaArgumentException()
    {
        var dto = new CrearTareaDto
        {
            Titulo = "Tarea invalida",
            VenceEnUtc = DateTime.UtcNow.AddMinutes(-10),
        };

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await tareaServicio.CrearAsync(dto, CancellationToken.None));
    }
}
