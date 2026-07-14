namespace AplicacionTareas.Application.DTOs;

public sealed class TareaDto
{
    public int Id { get; init; }

    public string Titulo { get; init; } = string.Empty;

    public string? Descripcion { get; init; }

    public bool EstaCompletada { get; init; } = false;

    public DateTime CreadoEnUtc { get; init; }

    public DateTime ActualizadoEnUtc { get; init; }
}
