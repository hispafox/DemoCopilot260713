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
        var departamentoId = await CrearDepartamentoAsync("Operaciones");
        var dto = new CrearUsuarioDto
        {
            Nombre = "Ana Martinez",
            DepartamentoId = departamentoId,
        };

        var respuesta = await cliente.PostAsJsonAsync("/api/usuarios", dto);

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);

        var creado = await respuesta.Content.ReadFromJsonAsync<UsuarioDto>();
        Assert.NotNull(creado);
        Assert.True(creado!.Id > 0);
        Assert.Equal(dto.Nombre, creado.Nombre);
        Assert.Equal(departamentoId, creado.DepartamentoId);
        Assert.StartsWith("Operaciones", creado.DepartamentoNombre, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Crear_ConNombreSoloEspacios_DevuelveBadRequest()
    {
        var departamentoId = await CrearDepartamentoAsync("Operaciones");
        var dto = new CrearUsuarioDto
        {
            Nombre = "   ",
            DepartamentoId = departamentoId,
        };

        var respuesta = await cliente.PostAsJsonAsync("/api/usuarios", dto);

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [Fact]
    public async Task Crear_ConNombreMaximoPermitido_DevuelveCreated()
    {
        var departamentoId = await CrearDepartamentoAsync("Operaciones");
        var dto = new CrearUsuarioDto
        {
            Nombre = new string('a', 200),
            DepartamentoId = departamentoId,
        };

        var respuesta = await cliente.PostAsJsonAsync("/api/usuarios", dto);

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);
    }

    [Fact]
    public async Task ObtenerTodos_CuandoHayUsuarios_DevuelveOkConLista()
    {
        var departamentoId = await CrearDepartamentoAsync("Operaciones");
        await cliente.PostAsJsonAsync("/api/usuarios", new CrearUsuarioDto { Nombre = "Ana Martinez", DepartamentoId = departamentoId });
        await cliente.PostAsJsonAsync("/api/usuarios", new CrearUsuarioDto { Nombre = "Luis Perez", DepartamentoId = departamentoId });

        var respuesta = await cliente.GetAsync("/api/usuarios");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        var usuarios = await respuesta.Content.ReadFromJsonAsync<List<UsuarioDto>>();
        Assert.NotNull(usuarios);
        Assert.True(usuarios!.Count >= 2);
        Assert.All(usuarios, usuario => Assert.True(usuario.DepartamentoId > 0));
    }

    [Fact]
    public async Task ObtenerPorId_CuandoExiste_DevuelveOkConDepartamentoAsociado()
    {
        var departamentoId = await CrearDepartamentoAsync("Operaciones");
        var creacion = await cliente.PostAsJsonAsync("/api/usuarios", new CrearUsuarioDto
        {
            Nombre = "Ana Martinez",
            DepartamentoId = departamentoId,
        });
        var creado = await creacion.Content.ReadFromJsonAsync<UsuarioDto>();

        var respuesta = await cliente.GetAsync($"/api/usuarios/{creado!.Id}");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        var usuario = await respuesta.Content.ReadFromJsonAsync<UsuarioDto>();
        Assert.NotNull(usuario);
        Assert.Equal(creado.Id, usuario!.Id);
        Assert.Equal(departamentoId, usuario.DepartamentoId);
        Assert.StartsWith("Operaciones", usuario.DepartamentoNombre, StringComparison.Ordinal);
    }

    [Fact]
    public async Task ObtenerPorId_CuandoNoExiste_DevuelveNotFound()
    {
        var respuesta = await cliente.GetAsync("/api/usuarios/999999");

        Assert.Equal(HttpStatusCode.NotFound, respuesta.StatusCode);
    }

    [Fact]
    public async Task Actualizar_CuandoExiste_DevuelveOkConUsuarioActualizado()
    {
        var departamentoOrigenId = await CrearDepartamentoAsync("Operaciones");
        var departamentoDestinoId = await CrearDepartamentoAsync("Finanzas");

        var creacion = await cliente.PostAsJsonAsync("/api/usuarios", new CrearUsuarioDto
        {
            Nombre = "Ana Martinez",
            DepartamentoId = departamentoOrigenId,
        });
        var creado = await creacion.Content.ReadFromJsonAsync<UsuarioDto>();

        var respuesta = await cliente.PutAsJsonAsync($"/api/usuarios/{creado!.Id}", new ActualizarUsuarioDto
        {
            Nombre = "Ana Actualizada",
            DepartamentoId = departamentoDestinoId,
        });

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        var actualizado = await respuesta.Content.ReadFromJsonAsync<UsuarioDto>();
        Assert.NotNull(actualizado);
        Assert.Equal("Ana Actualizada", actualizado!.Nombre);
        Assert.Equal(departamentoDestinoId, actualizado.DepartamentoId);
        Assert.StartsWith("Finanzas", actualizado.DepartamentoNombre, StringComparison.Ordinal);
    }

    [Fact]
    public async Task Actualizar_ConDepartamentoInexistente_DevuelveBadRequest()
    {
        var departamentoId = await CrearDepartamentoAsync("Operaciones");
        var creacion = await cliente.PostAsJsonAsync("/api/usuarios", new CrearUsuarioDto
        {
            Nombre = "Ana Martinez",
            DepartamentoId = departamentoId,
        });
        var creado = await creacion.Content.ReadFromJsonAsync<UsuarioDto>();

        var respuesta = await cliente.PutAsJsonAsync($"/api/usuarios/{creado!.Id}", new ActualizarUsuarioDto
        {
            Nombre = "Ana Martinez",
            DepartamentoId = 99999,
        });

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [Fact]
    public async Task Actualizar_CuandoNoExiste_DevuelveNotFound()
    {
        var departamentoId = await CrearDepartamentoAsync("Operaciones");

        var respuesta = await cliente.PutAsJsonAsync("/api/usuarios/999999", new ActualizarUsuarioDto
        {
            Nombre = "Ana Martinez",
            DepartamentoId = departamentoId,
        });

        Assert.Equal(HttpStatusCode.NotFound, respuesta.StatusCode);
    }

    private async Task<int> CrearDepartamentoAsync(string nombre)
    {
        var respuesta = await cliente.PostAsJsonAsync("/api/departamentos", new CrearDepartamentoDto { Nombre = $"{nombre}-{Guid.NewGuid():N}" });
        var departamento = await respuesta.Content.ReadFromJsonAsync<DepartamentoDto>();
        return departamento!.Id;
    }
}