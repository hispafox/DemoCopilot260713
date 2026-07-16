using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Application.Repositorios;
using AplicacionTareas.Application.Servicios;
using AplicacionTareas.Domain;
using NSubstitute;

namespace AplicacionTareas.Tests.Servicios;

public sealed class TareaServicioTests
{
    private readonly ITareaRepositorio tareaRepositorio;
    private readonly IUsuarioRepositorio usuarioRepositorio;
    private readonly TareaServicio tareaServicio;

    public TareaServicioTests()
    {
        tareaRepositorio = Substitute.For<ITareaRepositorio>();
        usuarioRepositorio = Substitute.For<IUsuarioRepositorio>();
        tareaServicio = new TareaServicio(tareaRepositorio, usuarioRepositorio);
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
            Categoria = "Trabajo",
            UsuarioAsignadoId = 3,
            Descripcion = "Actualizar changelog",
            VenceEnUtc = DateTime.UtcNow.AddDays(1),
        };

        usuarioRepositorio.ObtenerPorIdAsync(dto.UsuarioAsignadoId, Arg.Any<CancellationToken>())
            .Returns(Usuario.Crear("Ana Martinez"));

        tareaRepositorio.AgregarAsync(Arg.Any<Tarea>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult<Tarea>(x.Arg<Tarea>()!));

        var resultado = await tareaServicio.CrearAsync(dto, CancellationToken.None);

        await tareaRepositorio.Received(1).AgregarAsync(Arg.Any<Tarea>(), Arg.Any<CancellationToken>());
        Assert.Equal(dto.Titulo, resultado.Titulo);
        Assert.Equal(dto.Categoria, resultado.Categoria);
        Assert.Equal(dto.UsuarioAsignadoId, resultado.UsuarioAsignadoId);
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
            Categoria = "Trabajo",
            UsuarioAsignadoId = 3,
            VenceEnUtc = DateTime.UtcNow.AddMinutes(-10),
        };

        usuarioRepositorio.ObtenerPorIdAsync(dto.UsuarioAsignadoId, Arg.Any<CancellationToken>())
            .Returns(Usuario.Crear("Ana Martinez"));

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await tareaServicio.CrearAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CrearAsync_ConUsuarioInexistente_LanzaArgumentException()
    {
        var dto = new CrearTareaDto
        {
            Titulo = "Tarea sin responsable valido",
            Categoria = "Trabajo",
            UsuarioAsignadoId = 99,
        };

        usuarioRepositorio.ObtenerPorIdAsync(dto.UsuarioAsignadoId, Arg.Any<CancellationToken>())
            .Returns((Usuario?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await tareaServicio.CrearAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task ActualizarAsync_ConNuevaCategoria_DevuelveDtoActualizado()
    {
        var tarea = Tarea.Crear("Refinar backlog", null, "Trabajo", 1, DateTime.UtcNow.AddDays(2));
        var dto = new ActualizarTareaDto
        {
            Titulo = "Refinar backlog",
            Categoria = "Planificacion",
            Descripcion = "Cambiar categoria",
            VenceEnUtc = DateTime.UtcNow.AddDays(3),
        };

        tareaRepositorio.ObtenerPorIdAsync(5, Arg.Any<CancellationToken>())
            .Returns(tarea);

        var resultado = await tareaServicio.ActualizarAsync(5, dto, CancellationToken.None);

        await tareaRepositorio.Received(1).GuardarCambiosAsync(Arg.Any<CancellationToken>());
        Assert.Equal("Planificacion", resultado.Categoria);
    }
}
