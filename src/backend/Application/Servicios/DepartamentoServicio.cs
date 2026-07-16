using AplicacionTareas.Application.DTOs;
using AplicacionTareas.Application.Mapeadores;
using AplicacionTareas.Application.Repositorios;

namespace AplicacionTareas.Application.Servicios;

public sealed class DepartamentoServicio : IDepartamentoServicio
{
    private readonly IDepartamentoRepositorio departamentoRepositorio;

    public DepartamentoServicio(IDepartamentoRepositorio departamentoRepositorio)
    {
        this.departamentoRepositorio = departamentoRepositorio;
    }

    public async Task<IReadOnlyList<DepartamentoDto>> ObtenerTodosAsync(CancellationToken cancellationToken = default)
    {
        var departamentos = await departamentoRepositorio.ObtenerTodosAsync(cancellationToken);
        return departamentos.AListaDto();
    }

    public async Task<DepartamentoDto?> ObtenerPorIdAsync(int id, CancellationToken cancellationToken = default)
    {
        var departamento = await departamentoRepositorio.ObtenerPorIdAsync(id, cancellationToken);
        return departamento?.ADto();
    }

    public async Task<DepartamentoDto> CrearAsync(CrearDepartamentoDto dto, CancellationToken cancellationToken = default)
    {
        var departamento = dto.ADominio();
        var creado = await departamentoRepositorio.AgregarAsync(departamento, cancellationToken);
        return creado.ADto();
    }

    public async Task<DepartamentoDto> ActualizarAsync(int id, ActualizarDepartamentoDto dto, CancellationToken cancellationToken = default)
    {
        var departamento = await departamentoRepositorio.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"El departamento con id {id} no existe.");

        departamento.ActualizarNombre(dto.Nombre);
        await departamentoRepositorio.GuardarCambiosAsync(cancellationToken);

        return departamento.ADto();
    }

    public async Task EliminarAsync(int id, CancellationToken cancellationToken = default)
    {
        var departamento = await departamentoRepositorio.ObtenerPorIdAsync(id, cancellationToken)
            ?? throw new KeyNotFoundException($"El departamento con id {id} no existe.");

        var tieneUsuariosAsociados = await departamentoRepositorio.TieneUsuariosAsociadosAsync(id, cancellationToken);
        if (tieneUsuariosAsociados)
        {
            throw new InvalidOperationException("No se puede eliminar el departamento porque tiene usuarios asociados.");
        }

        await departamentoRepositorio.EliminarAsync(departamento, cancellationToken);
    }
}