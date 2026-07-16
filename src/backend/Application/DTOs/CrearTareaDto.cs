using System.ComponentModel.DataAnnotations;

namespace AplicacionTareas.Application.DTOs;

public sealed class CrearTareaDto
{
    [Required(ErrorMessage = "El titulo es obligatorio.")]
    [MaxLength(200, ErrorMessage = "El titulo no puede superar 200 caracteres.")]
    public string Titulo { get; init; } = string.Empty;

    [Required(ErrorMessage = "La categoria es obligatoria.")]
    [MaxLength(100, ErrorMessage = "La categoria no puede superar 100 caracteres.")]
    public string Categoria { get; init; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "El usuario asignado es obligatorio.")]
    public int UsuarioAsignadoId { get; init; }

    [MaxLength(1000, ErrorMessage = "La descripcion no puede superar 1000 caracteres.")]
    public string? Descripcion { get; init; }

    public DateTime? VenceEnUtc { get; init; }
}
