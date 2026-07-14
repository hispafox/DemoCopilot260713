import { render, screen, waitFor } from '@testing-library/react'
import { describe, expect, it, vi } from 'vitest'
import App from '../App'

vi.mock('../services/tareasServicio', () => {
  return {
    tareasServicio: {
      obtenerTodas: vi.fn().mockResolvedValue([
        {
          id: 1,
          titulo: 'Tarea render',
          descripcion: 'Desde test',
          estaCompletada: false,
          creadoEnUtc: '2026-07-14T09:00:00Z',
          actualizadoEnUtc: '2026-07-14T09:00:00Z',
        },
      ]),
      crear: vi.fn(),
      completar: vi.fn(),
      eliminar: vi.fn(),
      obtenerPorId: vi.fn(),
      actualizar: vi.fn(),
    },
  }
})

describe('App', () => {
  it('muestra la cabecera y carga una tarea en pantalla', async () => {
    render(<App />)

    expect(screen.getByRole('heading', { name: 'Lista de tareas' })).toBeInTheDocument()

    await waitFor(() => {
      expect(screen.getByText('Tarea render')).toBeInTheDocument()
    })
  })
})
