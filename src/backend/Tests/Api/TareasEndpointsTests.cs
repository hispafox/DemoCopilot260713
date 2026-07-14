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
        var dto = new CrearTareaDto
        {
            Titulo = "Test integracion",
            Descripcion = "Validar endpoint post",
        };

        var respuesta = await cliente.PostAsJsonAsync("/api/tareas", dto);

        Assert.Equal(HttpStatusCode.Created, respuesta.StatusCode);

        var creada = await respuesta.Content.ReadFromJsonAsync<TareaDto>();
        Assert.NotNull(creada);
        Assert.True(creada!.Id > 0);
    }

    [Fact]
    public async Task ObtenerPorId_CuandoExiste_DevuelveOkConTarea()
    {
        var crearDto = new CrearTareaDto
        {
            Titulo = "Consultar tarea",
            Descripcion = "Caso de get por id",
        };

        var creacion = await cliente.PostAsJsonAsync("/api/tareas", crearDto);
        var creada = await creacion.Content.ReadFromJsonAsync<TareaDto>();

        var respuesta = await cliente.GetAsync($"/api/tareas/{creada!.Id}");

        Assert.Equal(HttpStatusCode.OK, respuesta.StatusCode);

        var tarea = await respuesta.Content.ReadFromJsonAsync<TareaDto>();
        Assert.NotNull(tarea);
        Assert.Equal(creada.Id, tarea!.Id);
    }

    [Fact]
    public async Task Completar_CuandoExiste_DevuelveNoContent()
    {
        var crearDto = new CrearTareaDto
        {
            Titulo = "Completar tarea",
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
        var crearDto = new CrearTareaDto
        {
            Titulo = "Eliminar tarea",
            Descripcion = "Caso de delete",
        };

        var creacion = await cliente.PostAsJsonAsync("/api/tareas", crearDto);
        var creada = await creacion.Content.ReadFromJsonAsync<TareaDto>();

        var respuesta = await cliente.DeleteAsync($"/api/tareas/{creada!.Id}");

        Assert.Equal(HttpStatusCode.NoContent, respuesta.StatusCode);
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
