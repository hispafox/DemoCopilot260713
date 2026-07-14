using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Domain;

namespace AplicacionTareas.Application.Mapeadores;

public static class TareaMapeador
{
    public static TareaDto ADto(this Tarea tarea)
    {
        return new TareaDto
        {
            Id = tarea.Id,
            Titulo = tarea.Titulo,
            Descripcion = tarea.Descripcion,
            EstaCompletada = tarea.EstaCompletada,
            CreadoEnUtc = tarea.CreadoEnUtc,
            ActualizadoEnUtc = tarea.ActualizadoEnUtc,
        };
    }

    public static IReadOnlyList<TareaDto> AListaDto(this IEnumerable<Tarea> tareas)
    {
        return tareas.Select(t => t.ADto()).ToList();
    }

    public static Tarea ADominio(this CrearTareaDto dto)
    {
        return Tarea.Crear(dto.Titulo, dto.Descripcion);
    }
}
