using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Application.Repositorios;
using AplicacionTareas.Application.Servicios;
using AplicacionTareas.Domain;
using NSubstitute;

namespace AplicacionTareas.Tests.Servicios;

public sealed class DepartamentoServicioTests
{
    private readonly IDepartamentoRepositorio departamentoRepositorio;
    private readonly DepartamentoServicio departamentoServicio;

    public DepartamentoServicioTests()
    {
        departamentoRepositorio = Substitute.For<IDepartamentoRepositorio>();
        departamentoServicio = new DepartamentoServicio(departamentoRepositorio);
    }

    [Fact]
    public async Task CrearAsync_ConDtoValido_CreaDepartamento()
    {
        var dto = new CrearDepartamentoDto { Nombre = "Operaciones" };

        departamentoRepositorio.AgregarAsync(Arg.Any<Departamento>(), Arg.Any<CancellationToken>())
            .Returns(x => Task.FromResult(x.Arg<Departamento>()!));

        var resultado = await departamentoServicio.CrearAsync(dto, CancellationToken.None);

        await departamentoRepositorio.Received(1).AgregarAsync(Arg.Any<Departamento>(), Arg.Any<CancellationToken>());
        Assert.Equal("Operaciones", resultado.Nombre);
    }

    [Fact]
    public async Task CrearAsync_ConNombreInvalido_LanzaArgumentException()
    {
        var dto = new CrearDepartamentoDto { Nombre = "   " };

        await Assert.ThrowsAsync<ArgumentException>(
            async () => await departamentoServicio.CrearAsync(dto, CancellationToken.None));
    }

    [Fact]
    public async Task ActualizarAsync_CuandoNoExiste_LanzaKeyNotFoundException()
    {
        departamentoRepositorio.ObtenerPorIdAsync(10, Arg.Any<CancellationToken>())
            .Returns((Departamento?)null);

        await Assert.ThrowsAsync<KeyNotFoundException>(
            async () => await departamentoServicio.ActualizarAsync(10, new ActualizarDepartamentoDto { Nombre = "Nuevo" }, CancellationToken.None));
    }

    [Fact]
    public async Task EliminarAsync_ConUsuariosAsociados_LanzaInvalidOperationException()
    {
        departamentoRepositorio.ObtenerPorIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(Departamento.Crear("Operaciones"));
        departamentoRepositorio.TieneUsuariosAsociadosAsync(1, Arg.Any<CancellationToken>())
            .Returns(true);

        await Assert.ThrowsAsync<InvalidOperationException>(
            async () => await departamentoServicio.EliminarAsync(1, CancellationToken.None));
    }

    [Fact]
    public async Task EliminarAsync_SinUsuariosAsociados_EliminaDepartamento()
    {
        var departamento = Departamento.Crear("Operaciones");
        departamentoRepositorio.ObtenerPorIdAsync(1, Arg.Any<CancellationToken>())
            .Returns(departamento);
        departamentoRepositorio.TieneUsuariosAsociadosAsync(1, Arg.Any<CancellationToken>())
            .Returns(false);

        await departamentoServicio.EliminarAsync(1, CancellationToken.None);

        await departamentoRepositorio.Received(1).EliminarAsync(departamento, Arg.Any<CancellationToken>());
    }
}