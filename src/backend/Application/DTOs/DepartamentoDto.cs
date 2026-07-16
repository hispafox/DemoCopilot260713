namespace AplicacionTareas.Application.DTOs;

public sealed class DepartamentoDto
{
    public int Id { get; init; }

    public string Nombre { get; init; } = string.Empty;

    public DateTime CreadoEnUtc { get; init; }

    public DateTime ActualizadoEnUtc { get; init; }
}