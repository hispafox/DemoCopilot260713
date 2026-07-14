using AplicacionTareas.Api.Middleware;
using AplicacionTareas.Application.Repositorios;
using AplicacionTareas.Application.Servicios;
using AplicacionTareas.Infrastructure.Datos;
using AplicacionTareas.Infrastructure.Repositorios;
using Microsoft.EntityFrameworkCore;
using System.Text.Json;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services
    .AddControllers()
    .AddJsonOptions(opciones =>
    {
        opciones.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
    });
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddDbContext<AplicacionDbContext>(opciones =>
    opciones.UseSqlite(builder.Configuration.GetConnectionString("BaseDatos") ?? "Data Source=tareas.db"));
builder.Services.AddScoped<ITareaRepositorio, TareaRepositorio>();
builder.Services.AddScoped<ITareaServicio, TareaServicio>();

var app = builder.Build();

using (var alcance = app.Services.CreateScope())
{
    var contexto = alcance.ServiceProvider.GetRequiredService<AplicacionDbContext>();
    contexto.Database.Migrate();
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseMiddleware<ManejadorExcepcionesMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();

public partial class Program;
