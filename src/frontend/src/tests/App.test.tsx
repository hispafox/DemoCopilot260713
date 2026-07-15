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
          descripcion: '<p>Descripcion con <strong>formato</strong> y <a href="https://ejemplo.com">enlace</a></p>',
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

    expect(screen.getByText('formato', { selector: 'strong' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'enlace' })).toHaveAttribute('href', 'https://ejemplo.com')
  })
})
