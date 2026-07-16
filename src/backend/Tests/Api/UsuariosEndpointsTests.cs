using System.Net;
using System.Net.Http.Json;
using AplicacionTareas.Application.DTOs;

namespace AplicacionTareas.Tests.Api;

public sealed class UsuariosEndpointsTests : IClassFixture<TareasApiFactory>
{
    private readonly HttpClient cliente;

    public UsuariosEndpointsTests(TareasApiFactory factory)
    {
        cliente = factory.CreateClient();
    }

    [Fact]
    public async Task Crear_ConPayloadValido_DevuelveCreatedConUsuario()
    {
        var dto = new CrearUsuarioDto
        {
            Nombre = "Ana Martinez",
        };

        var respuesta = await cliente.PostAsJsonAsync("/api/usuarios", dto);

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);

        var creado = await respuesta.Content.ReadFromJsonAsync<UsuarioDto>();
        Assert.NotNull(creado);
        Assert.True(creado!.Id > 0);
        Assert.Equal(dto.Nombre, creado.Nombre);
    }

    [Fact]
    public async Task Crear_ConNombreSoloEspacios_DevuelveBadRequest()
    {
        var dto = new CrearUsuarioDto
        {
            Nombre = "   ",
        };

        var respuesta = await cliente.PostAsJsonAsync("/api/usuarios", dto);

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [Fact]
    public async Task Crear_ConNombreMaximoPermitido_DevuelveCreated()
    {
        var dto = new CrearUsuarioDto
        {
            Nombre = new string('a', 200),
        };

        var respuesta = await cliente.PostAsJsonAsync("/api/usuarios", dto);

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoHayUsuarios_DevuelveOkConLista()
    {
        await cliente.PostAsJsonAsync("/api/usuarios", new CrearUsuarioDto { Nombre = "Ana Martinez" });
        await cliente.PostAsJsonAsync("/api/usuarios", new CrearUsuarioDto { Nombre = "Luis Perez" });

        var respuesta = await cliente.GetAsync("/api/usuarios");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        var usuarios = await respuesta.Content.ReadFromJsonAsync<List<UsuarioDto>>();
        Assert.NotNull(usuarios);
        Assert.True(usuarios!.Count >= 2);
    }
}