using System.Net;
using System.Net.Http.Json;
using AplicacionTareas.Application.DTOs;

namespace AplicacionTareas.Tests.Api;

public sealed class DepartamentosEndpointsTests : IClassFixture<TareasApiFactory>
{
    private readonly HttpClient cliente;

    public DepartamentosEndpointsTests(TareasApiFactory factory)
    {
        cliente = factory.CreateClient();
    }

    [Fact]
    public async Task Crear_ConPayloadValido_DevuelveCreated()
    {
        var dto = new CrearDepartamentoDto { Nombre = $"Operaciones-{Guid.NewGuid():N}" };

        var respuesta = await cliente.PostAsJsonAsync("/api/departamentos", dto);

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);

        var creado = await respuesta.Content.ReadFromJsonAsync<DepartamentoDto>();
        Assert.NotNull(creado);
        Assert.True(creado!.Id > 0);
        Assert.Equal(dto.Nombre, creado.Nombre);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoHayDatos_DevuelveOk()
    {
        await cliente.PostAsJsonAsync("/api/departamentos", new CrearDepartamentoDto { Nombre = $"Operaciones-{Guid.NewGuid():N}" });

        var respuesta = await cliente.GetAsync("/api/departamentos");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var departamentos = await respuesta.Content.ReadFromJsonAsync<List<DepartamentoDto>>();
        Assert.NotNull(departamentos);
        Assert.NotEmpty(departamentos!);
    }

    [Fact]
    public async Task Actualizar_CuandoExiste_DevuelveOkConNombreActualizado()
    {
        var creacion = await cliente.PostAsJsonAsync("/api/departamentos", new CrearDepartamentoDto { Nombre = $"Operaciones-{Guid.NewGuid():N}" });
        var creado = await creacion.Content.ReadFromJsonAsync<DepartamentoDto>();

        var respuesta = await cliente.PutAsJsonAsync($"/api/departamentos/{creado!.Id}", new ActualizarDepartamentoDto { Nombre = "Finanzas" });

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);
        var actualizado = await respuesta.Content.ReadFromJsonAsync<DepartamentoDto>();
        Assert.NotNull(actualizado);
        Assert.Equal("Finanzas", actualizado!.Nombre);
    }

    [Fact]
    public async Task Eliminar_CuandoNoTieneUsuariosAsociados_DevuelveNoContent()
    {
        var creacion = await cliente.PostAsJsonAsync("/api/departamentos", new CrearDepartamentoDto { Nombre = "Temporal" });
        var creado = await creacion.Content.ReadFromJsonAsync<DepartamentoDto>();

        var respuesta = await cliente.DeleteAsync($"/api/departamentos/{creado!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, respuesta.StatusCode);
    }

    [Fact]
    public async Task Eliminar_CuandoTieneUsuariosAsociados_DevuelveBadRequest()
    {
        var creacion = await cliente.PostAsJsonAsync("/api/departamentos", new CrearDepartamentoDto { Nombre = $"Operaciones-{Guid.NewGuid():N}" });
        var departamento = await creacion.Content.ReadFromJsonAsync<DepartamentoDto>();

        await cliente.PostAsJsonAsync("/api/usuarios", new CrearUsuarioDto
        {
            Nombre = "Ana Martinez",
            DepartamentoId = departamento!.Id,
        });

        var respuesta = await cliente.DeleteAsync($"/api/departamentos/{departamento.Id}");

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }
}