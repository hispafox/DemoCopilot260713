using System.Net;
using System.Text.Json;

namespace AplicacionTareas.Api.Middleware;

public sealed class ManejadorExcepcionesMiddleware
{
    private readonly RequestDelegate siguiente;

    public ManejadorExcepcionesMiddleware(RequestDelegate siguiente)
    {
        this.siguiente = siguiente;
    }

    public async Task InvokeAsync(HttpContext contexto)
    {
        try
        {
            await siguiente(contexto);
        }
        catch (Exception ex)
        {
            await ManejarExcepcionAsync(contexto, ex);
        }
    }

    private static Task ManejarExcepcionAsync(HttpContext contexto, Exception excepcion)
    {
        var codigoEstado = excepcion switch
        {
            KeyNotFoundException => HttpStatusCode.NotFound,
            ArgumentException => HttpStatusCode.BadRequest,
            InvalidOperationException => HttpStatusCode.BadRequest,
            _ => HttpStatusCode.InternalServerError,
        };

        contexto.Response.ContentType = "application/json";
        contexto.Response.StatusCode = (int)codigoEstado;

        var error = new
        {
            error = excepcion.Message,
        };

        return contexto.Response.WriteAsync(JsonSerializer.Serialize(error));
    }
}
