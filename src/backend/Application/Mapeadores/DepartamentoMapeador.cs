using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Domain;

namespace AplicacionTareas.Application.Mapeadores;

public static class DepartamentoMapeador
{
    public static DepartamentoDto ADto(this Departamento departamento)
    {
        return new DepartamentoDto
        {
            Id = departamento.Id,
            Nombre = departamento.Nombre,
            CreadoEnUtc = departamento.CreadoEnUtc,
            ActualizadoEnUtc = departamento.ActualizadoEnUtc,
        };
    }

    public static IReadOnlyList<DepartamentoDto> AListaDto(this IEnumerable<Departamento> departamentos)
    {
        return departamentos.Select(departamento => departamento.ADto()).ToList();
    }

    public static Departamento ADominio(this CrearDepartamentoDto dto)
    {
        return Departamento.Crear(dto.Nombre);
    }
}