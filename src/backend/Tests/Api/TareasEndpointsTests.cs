using System.Net;
using System.Net.Http.Json;
using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Infrastructure.Datos;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AplicacionTareas.Tests.Api;

public sealed class TareasEndpointsTests : IClassFixture<TareasApiFactory>
{
    private readonly HttpClient cliente;

    public TareasEndpointsTests(TareasApiFactory factory)
    {
        cliente = factory.CreateClient();
    }

    [Fact]
    public async Task ObtenerTodas_CuandoNoHayDatos_DevuelveOkConLista()
    {
        var respuesta = await cliente.GetAsync("/api/tareas");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        var contenido = await respuesta.Content.ReadFromJsonAsync<List<TareaDto>>();
        Assert.NotNull(contenido);
    }

    [Fact]
    public async Task Crear_ConPayloadValido_DevuelveCreated()
    {
        var vencimientoUtc = DateTime.UtcNow.AddDays(3);
        var usuarioId = await CrearUsuarioAsync("Ana Martinez");
        var dto = new CrearTareaDto
        {
            Titulo = "Test integracion",
            Categoria = "Trabajo",
            UsuarioAsignadoId = usuarioId,
            Descripcion = "Validar endpoint post",
            VenceEnUtc = vencimientoUtc,
        };

        var respuesta = await cliente.PostAsJsonAsync("/api/tareas", dto);

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);

        var creada = await respuesta.Content.ReadFromJsonAsync<TareaDto>();
        Assert.NotNull(creada);
        Assert.True(creada!.Id > 0);
        Assert.Equal(dto.Categoria, creada.Categoria);
        Assert.Equal(usuarioId, creada.UsuarioAsignadoId);
        Assert.Equal(vencimientoUtc, creada.VenceEnUtc);
    }

    [Fact]
    public async Task ObtenerPorId_CuandoExiste_DevuelveOkConTarea()
    {
        var usuarioId = await CrearUsuarioAsync("Ana Martinez");
        var crearDto = new CrearTareaDto
        {
            Titulo = "Consultar tarea",
            Categoria = "Trabajo",
            UsuarioAsignadoId = usuarioId,
            Descripcion = "Caso de get por id",
        };

        var creacion = await cliente.PostAsJsonAsync("/api/tareas", crearDto);
        var creada = await creacion.Content.ReadFromJsonAsync<TareaDto>();

        var respuesta = await cliente.GetAsync($"/api/tareas/{creada!.Id}");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        var tarea = await respuesta.Content.ReadFromJsonAsync<TareaDto>();
        Assert.NotNull(tarea);
        Assert.Equal(creada.Id, tarea!.Id);
        Assert.Equal(creada.Categoria, tarea.Categoria);
        Assert.Equal(usuarioId, tarea.UsuarioAsignadoId);
    }

    [Fact]
    public async Task Completar_CuandoExiste_DevuelveNoContent()
    {
        var usuarioId = await CrearUsuarioAsync("Ana Martinez");
        var crearDto = new CrearTareaDto
        {
            Titulo = "Completar tarea",
            Categoria = "Trabajo",
            UsuarioAsignadoId = usuarioId,
            Descripcion = "Caso de patch",
        };

        var creacion = await cliente.PostAsJsonAsync("/api/tareas", crearDto);
        var creada = await creacion.Content.ReadFromJsonAsync<TareaDto>();

        var respuesta = await cliente.PatchAsync($"/api/tareas/{creada!.Id}/completar", null);

        Assert.Equal(HttpStatusCode.NoContent, respuesta.StatusCode);
    }

    [Fact]
    public async Task Eliminar_CuandoExiste_DevuelveNoContent()
    {
        var usuarioId = await CrearUsuarioAsync("Ana Martinez");
        var crearDto = new CrearTareaDto
        {
            Titulo = "Eliminar tarea",
            Categoria = "Trabajo",
            UsuarioAsignadoId = usuarioId,
            Descripcion = "Caso de delete",
        };

        var creacion = await cliente.PostAsJsonAsync("/api/tareas", crearDto);
        var creada = await creacion.Content.ReadFromJsonAsync<TareaDto>();

        var respuesta = await cliente.DeleteAsync($"/api/tareas/{creada!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, respuesta.StatusCode);
    }

    [Fact]
    public async Task Crear_ConVencimientoPasado_DevuelveBadRequest()
    {
        var usuarioId = await CrearUsuarioAsync("Ana Martinez");
        var dto = new CrearTareaDto
        {
            Titulo = "Tarea invalida",
            Categoria = "Trabajo",
            UsuarioAsignadoId = usuarioId,
            VenceEnUtc = DateTime.UtcNow.AddMinutes(-15),
        };

        var respuesta = await cliente.PostAsJsonAsync("/api/tareas", dto);

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [Fact]
    public async Task Crear_SinCategoria_DevuelveBadRequest()
    {
        var usuarioId = await CrearUsuarioAsync("Ana Martinez");
        var dto = new CrearTareaDto
        {
            Titulo = "Tarea sin categoria",
            UsuarioAsignadoId = usuarioId,
        };

        var respuesta = await cliente.PostAsJsonAsync("/api/tareas", dto);

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [Fact]
    public async Task Actualizar_ConNuevaCategoria_DevuelveOkConCategoriaActualizada()
    {
        var usuarioId = await CrearUsuarioAsync("Ana Martinez");
        var creacionDto = new CrearTareaDto
        {
            Titulo = "Ajustar roadmap",
            Categoria = "Trabajo",
            UsuarioAsignadoId = usuarioId,
            Descripcion = "Version inicial",
        };

        var creacion = await cliente.PostAsJsonAsync("/api/tareas", creacionDto);
        var creada = await creacion.Content.ReadFromJsonAsync<TareaDto>();

        var actualizacionDto = new ActualizarTareaDto
        {
            Titulo = "Ajustar roadmap",
            Categoria = "Planificacion",
            Descripcion = "Version refinada",
            VenceEnUtc = DateTime.UtcNow.AddDays(4),
        };

        var respuesta = await cliente.PutAsJsonAsync($"/api/tareas/{creada!.Id}", actualizacionDto);

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        var actualizada = await respuesta.Content.ReadFromJsonAsync<TareaDto>();
        Assert.NotNull(actualizada);
        Assert.Equal("Planificacion", actualizada!.Categoria);
        Assert.Equal(usuarioId, actualizada.UsuarioAsignadoId);
    }

    [Fact]
    public async Task Crear_SinUsuarioAsignado_DevuelveBadRequest()
    {
        var dto = new CrearTareaDto
        {
            Titulo = "Tarea sin responsable",
            Categoria = "Trabajo",
        };

        var respuesta = await cliente.PostAsJsonAsync("/api/tareas", dto);

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    [Fact]
    public async Task Crear_ConUsuarioAsignadoInexistente_DevuelveBadRequest()
    {
        var dto = new CrearTareaDto
        {
            Titulo = "Tarea con responsable inexistente",
            Categoria = "Trabajo",
            UsuarioAsignadoId = 999,
        };

        var respuesta = await cliente.PostAsJsonAsync("/api/tareas", dto);

        Assert.Equal(HttpStatusCode.BadRequest, respuesta.StatusCode);
    }

    private async Task<int> CrearUsuarioAsync(string nombre)
    {
        var respuesta = await cliente.PostAsJsonAsync("/api/usuarios", new CrearUsuarioDto { Nombre = nombre });
        var usuario = await respuesta.Content.ReadFromJsonAsync<UsuarioDto>();
        return usuario!.Id;
    }
}

public sealed class TareasApiFactory : WebApplicationFactory<Program>
{
    private readonly string rutaBaseDatosTemporal = Path.Combine(Path.GetTempPath(), $"tareas-tests-{Guid.NewGuid():N}.db");

    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.ConfigureTestServices(servicios =>
        {
            var descriptor = servicios.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<AplicacionDbContext>));
            if (descriptor is not null)
            {
                servicios.Remove(descriptor);
            }

            servicios.AddDbContext<AplicacionDbContext>(opciones =>
                opciones.UseSqlite($"Data Source={rutaBaseDatosTemporal}"));

            var proveedor = servicios.BuildServiceProvider();
            using var alcance = proveedor.CreateScope();
            var contexto = alcance.ServiceProvider.GetRequiredService<AplicacionDbContext>();
            contexto.Database.EnsureDeleted();
            contexto.Database.Migrate();
        });
    }
}
