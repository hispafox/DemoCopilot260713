import { describe, expect, it, vi, beforeEach } from 'vitest'
import { tareasServicio } from '../services/tareasServicio'

describe('tareasServicio', () => {
  beforeEach(() => {
    vi.restoreAllMocks()
  })

  it('obtenerTodas consume /api/tareas y devuelve la lista', async () => {
    const fetchMock = vi.spyOn(globalThis, 'fetch').mockResolvedValue(
      new Response(
        JSON.stringify([{ id: 1, titulo: 'Tarea demo', estaCompletada: false, creadoEnUtc: '2026-07-14T08:00:00Z', actualizadoEnUtc: '2026-07-14T08:00:00Z' }]),
        {
          status: 200,
          headers: { 'Content-Type': 'application/json' },
        },
      ),
    )

    const resultado = await tareasServicio.obtenerTodas()

    expect(fetchMock).toHaveBeenCalledWith('/api/tareas', expect.any(Object))
    expect(resultado).toHaveLength(1)
    expect(resultado[0]?.titulo).toBe('Tarea demo')
  })

  it('lanza error cuando el backend responde con estado no exitoso', async () => {
    vi.spyOn(globalThis, 'fetch').mockResolvedValue(
      new Response(JSON.stringify({ error: 'Fallo de validacion' }), {
        status: 400,
        headers: { 'Content-Type': 'application/json' },
      }),
    )

    await expect(tareasServicio.obtenerTodas()).rejects.toThrow('Error 400')
  })
})
