using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Application.Repositorios;
using AplicacionTareas.Application.Servicios;
using AplicacionTareas.Domain;
using NSubstitute;

namespace AplicacionTareas.Tests.Servicios;

public sealed class UsuarioServicioTests
{
    private readonly IUsuarioRepositorio usuarioRepositorio;
    private readonly UsuarioServicio usuarioServicio;

    public UsuarioServicioTests()
    {
        usuarioRepositorio = Substitute.For<IUsuarioRepositorio>();
        usuarioServicio = new UsuarioServicio(usuarioRepositorio);
    }

    [Fact]
    public async Task CrearAsync_ConDtoValido_CreaYDevuelveDto()
    {
        var dto = new CrearUsuarioDto
        {
            Nombre = "Ana Martinez",
        };

        usuarioRepositorio.AgregarAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult<Usuario>(x.Arg<Usuario>()!));

        var resultado = await usuarioServicio.CrearAsync(dto, CancellationToken.None);

        await usuarioRepositorio.Received(1).AgregarAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>());
        Assert.Equal(dto.Nombre, resultado.Nombre);
    }

    [Fact]
    public async Task CrearAsync_ConNombreInvalido_LanzaArgumentException()
    {
        var dto = new CrearUsuarioDto
        {
            Nombre = "   ",
        };

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await usuarioServicio.CrearAsync(dto, CancellationToken.None));
    }
}