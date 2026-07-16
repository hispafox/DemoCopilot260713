import { beforeEach, describe, expect, it, vi } from 'vitest'
import { departamentosServicio } from '../services/departamentosServicio'

describe('departamentosServicio', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })

  it('obtenerTodos consume /api/departamentos y devuelve la lista', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(
      new Response(
        JSON.stringify([
          {
            id: 1,
            nombre: 'Operaciones',
            creadoEnUtc: '2026-07-16T08:00:00Z',
            actualizadoEnUtc: '2026-07-16T08:00:00Z',
          },
        ]),
        {
          status: 200,
          headers: { 'Content-Type': 'application/json' },
        },
      ),
    )

    const resultado = await departamentosServicio.obtenerTodos()

    expect(fetchMock).toHaveBeenCalledWith('/api/departamentos', expect.any(Object))
    expect(resultado[0]?.nombre).toBe('Operaciones')
  })

  it('crear consume /api/departamentos y devuelve el departamento creado', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(
      new Response(
        JSON.stringify({
          id: 2,
          nombre: 'Finanzas',
          creadoEnUtc: '2026-07-16T08:00:00Z',
          actualizadoEnUtc: '2026-07-16T08:00:00Z',
        }),
        {
          status: 201,
          headers: { 'Content-Type': 'application/json' },
        },
      ),
    )

    const resultado = await departamentosServicio.crear({ nombre: 'Finanzas' })

    expect(fetchMock).toHaveBeenCalledWith('/api/departamentos', expect.any(Object))
    expect(resultado.nombre).toBe('Finanzas')
  })

  it('actualizar consume /api/departamentos/{id} y devuelve el departamento actualizado', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(
      new Response(
        JSON.stringify({
          id: 2,
          nombre: 'Finanzas y Control',
          creadoEnUtc: '2026-07-16T08:00:00Z',
          actualizadoEnUtc: '2026-07-16T09:00:00Z',
        }),
        {
          status: 200,
          headers: { 'Content-Type': 'application/json' },
        },
      ),
    )

    const resultado = await departamentosServicio.actualizar(2, { nombre: 'Finanzas y Control' })

    expect(fetchMock).toHaveBeenCalledWith('/api/departamentos/2', expect.any(Object))
    expect(resultado.nombre).toBe('Finanzas y Control')
  })

  it('eliminar consume /api/departamentos/{id} y devuelve void', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(
      new Response(null, {
        status: 204,
      }),
    )

    const resultado = await departamentosServicio.eliminar(2)

    expect(fetchMock).toHaveBeenCalledWith('/api/departamentos/2', expect.any(Object))
    expect(resultado).toBeUndefined()
  })

  it('actualizar propaga error de backend con detalle', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue(
      new Response(
        JSON.stringify({
          error: 'No se puede eliminar el departamento porque tiene usuarios asociados.',
        }),
        {
          status: 400,
          headers: { 'Content-Type': 'application/json' },
        },
      ),
    )

    await expect(departamentosServicio.actualizar(2, { nombre: 'Operaciones' }))
      .rejects
      .toThrow('Error 400: No se puede eliminar el departamento porque tiene usuarios asociados.')
  })

  it('eliminar propaga error de backend con detalle', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue(
      new Response(
        JSON.stringify({
          error: 'No se puede eliminar el departamento porque tiene usuarios asociados.',
        }),
        {
          status: 400,
          headers: { 'Content-Type': 'application/json' },
        },
      ),
    )

    await expect(departamentosServicio.eliminar(2))
      .rejects
      .toThrow('Error 400: No se puede eliminar el departamento porque tiene usuarios asociados.')
  })
})