using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Domain;

namespace AplicacionTareas.Application.Mapeadores;

public static class UsuarioMapeador
{
    public static UsuarioDto ADto(this Usuario usuario)
    {
        return new UsuarioDto
        {
            Id = usuario.Id,
            Nombre = usuario.Nombre,
            CreadoEnUtc = usuario.CreadoEnUtc,
            ActualizadoEnUtc = usuario.ActualizadoEnUtc,
        };
    }

    public static Usuario ADominio(this CrearUsuarioDto dto)
    {
        return Usuario.Crear(dto.Nombre);
    }
}