using System.ComponentModel.DataAnnotations;

namespace AplicacionTareas.Application.DTOs;

public sealed class ActualizarUsuarioDto
{
    [Required(ErrorMessage = "El nombre es obligatorio.")]
    [MaxLength(200, ErrorMessage = "El nombre no puede superar 200 caracteres.")]
    public string Nombre { get; init; } = string.Empty;

    [Range(1, int.MaxValue, ErrorMessage = "El departamento es obligatorio.")]
    public int DepartamentoId { get; init; }
}
