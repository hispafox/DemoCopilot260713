using System.ComponentModel.DataAnnotations;

namespace AplicacionTareas.Application.DTOs;

public sealed class CrearTareaDto
{
    [Required(ErrorMessage = "El titulo es obligatorio.")]
    [MaxLength(200, ErrorMessage = "El titulo no puede superar 200 caracteres.")]
    public string Titulo { get; init; } = string.Empty;

    [MaxLength(1000, ErrorMessage = "La descripcion no puede superar 1000 caracteres.")]
    public string? Descripcion { get; init; }
}
