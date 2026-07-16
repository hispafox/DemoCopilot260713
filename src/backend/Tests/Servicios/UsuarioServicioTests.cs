using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Application.Repositorios;
using AplicacionTareas.Application.Servicios;
using AplicacionTareas.Domain;
using NSubstitute;

namespace AplicacionTareas.Tests.Servicios;

public sealed class UsuarioServicioTests
{
    private readonly IUsuarioRepositorio usuarioRepositorio;
    private readonly IDepartamentoRepositorio departamentoRepositorio;
    private readonly UsuarioServicio usuarioServicio;

    public UsuarioServicioTests()
    {
        usuarioRepositorio = Substitute.For<IUsuarioRepositorio>();
        departamentoRepositorio = Substitute.For<IDepartamentoRepositorio>();
        usuarioServicio = new UsuarioServicio(usuarioRepositorio, departamentoRepositorio);
    }

    [Fact]
    public async Task CrearAsync_ConDtoValido_CreaYDevuelveDto()
    {
        var dto = new CrearUsuarioDto
        {
            Nombre = "Ana Martinez",
            DepartamentoId = 1,
        };

        departamentoRepositorio.ObtenerPorIdAsync(dto.DepartamentoId, Arg.Any<CancellationToken>())
            .Returns(AplicacionTareas.Domain.Departamento.Crear("Operaciones"));

        usuarioRepositorio.AgregarAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult<Usuario>(x.Arg<Usuario>()!));

        var resultado = await usuarioServicio.CrearAsync(dto, CancellationToken.None);

        await usuarioRepositorio.Received(1).AgregarAsync(Arg.Any<Usuario>(), Arg.Any<CancellationToken>());
        Assert.Equal(dto.Nombre, resultado.Nombre);
        Assert.Equal(dto.DepartamentoId, resultado.DepartamentoId);
    }

    [Fact]
    public async Task CrearAsync_ConNombreInvalido_LanzaArgumentException()
    {
        var dto = new CrearUsuarioDto
        {
            Nombre = "   ",
            DepartamentoId = 1,
        };

        departamentoRepositorio.ObtenerPorIdAsync(dto.DepartamentoId, Arg.Any<CancellationToken>())
            .Returns(AplicacionTareas.Domain.Departamento.Crear("Operaciones"));

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await usuarioServicio.CrearAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task CrearAsync_ConDepartamentoInexistente_LanzaArgumentException()
    {
        var dto = new CrearUsuarioDto
        {
            Nombre = "Ana Martinez",
            DepartamentoId = 999,
        };

        departamentoRepositorio.ObtenerPorIdAsync(dto.DepartamentoId, Arg.Any<CancellationToken>())
            .Returns((AplicacionTareas.Domain.Departamento?)null);

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await usuarioServicio.CrearAsync(dto, CancellationToken.None));
    }
}