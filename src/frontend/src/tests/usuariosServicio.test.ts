import { beforeEach, describe, expect, it, vi } from 'vitest'
import { usuariosServicio } from '../services/usuariosServicio'

describe('usuariosServicio', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })

  it('crear consume /api/usuarios y devuelve el usuario creado', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(
      new Response(
        JSON.stringify({
          id: 1,
          nombre: 'Ana Martinez',
          departamentoId: 3,
          departamentoNombre: 'Operaciones',
          creadoEnUtc: '2026-07-15T08:00:00Z',
          actualizadoEnUtc: '2026-07-15T08:00:00Z',
        }),
        {
          status: 201,
          headers: { 'Content-Type': 'application/json' },
        },
      ),
    )

    const resultado = await usuariosServicio.crear({ nombre: 'Ana Martinez', departamentoId: 3 })

    expect(fetchMock).toHaveBeenCalledWith('/api/usuarios', expect.any(Object))
    expect(resultado.nombre).toBe('Ana Martinez')
    expect(resultado.departamentoNombre).toBe('Operaciones')
  })

  it('obtenerTodos consume /api/usuarios y devuelve la lista', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(
      new Response(
        JSON.stringify([
          {
            id: 1,
            nombre: 'Ana Martinez',
            departamentoId: 3,
            departamentoNombre: 'Operaciones',
            creadoEnUtc: '2026-07-15T08:00:00Z',
            actualizadoEnUtc: '2026-07-15T08:00:00Z',
          },
        ]),
        {
          status: 200,
          headers: { 'Content-Type': 'application/json' },
        },
      ),
    )

    const resultado = await usuariosServicio.obtenerTodos()

    expect(fetchMock).toHaveBeenCalledWith('/api/usuarios', expect.any(Object))
    expect(resultado[0]?.nombre).toBe('Ana Martinez')
    expect(resultado[0]?.departamentoNombre).toBe('Operaciones')
  })
})