using System.ComponentModel.DataAnnotations;

namespace AplicacionTareas.Application.DTOs;

public sealed class CrearUsuarioDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(200, ErrorMessage = "El nombre no puede superar 200 caracteres.")]
    public string Nombre { get; init; } = string.Empty;
}