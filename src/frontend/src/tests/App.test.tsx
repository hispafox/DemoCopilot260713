import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import App from '../App'
import { tareasServicio } from '../services/tareasServicio'
import { usuariosServicio } from '../services/usuariosServicio'

vi.mock('../services/tareasServicio', () => {
  return {
    tareasServicio: {
      obtenerTodas: vi.fn().mockResolvedValue([
        {
          id: 1,
          titulo: 'Tarea render',
          categoria: 'Trabajo',
          usuarioAsignadoId: 7,
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

vi.mock('../services/usuariosServicio', () => {
  return {
    usuariosServicio: {
      obtenerTodos: vi.fn().mockResolvedValue([
        {
          id: 7,
          nombre: 'Ana Martinez',
          creadoEnUtc: '2026-07-15T09:00:00Z',
          actualizadoEnUtc: '2026-07-15T09:00:00Z',
        },
      ]),
      crear: vi.fn(),
    },
  }
})

describe('App', () => {
  beforeEach(() => {
    vi.clearAllMocks()
  })

  it('muestra la cabecera y carga una tarea en pantalla', async () => {
    render(<App />)

    expect(screen.getByRole('heading', { name: 'Lista de tareas' })).toBeInTheDocument()

    await waitFor(() => {
      expect(screen.getByText('Tarea render')).toBeInTheDocument()
    })

    expect(screen.getByText('formato', { selector: 'strong' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'enlace' })).toHaveAttribute('href', 'https://ejemplo.com')
    expect(screen.getByText('Categoria: Trabajo')).toBeInTheDocument()
    expect(screen.getByText('Usuario: Ana Martinez')).toBeInTheDocument()
  })

  it('envia la categoria al crear una tarea', async () => {
    vi.mocked(tareasServicio.crear).mockResolvedValueOnce({
      id: 2,
      titulo: 'Nueva tarea',
      categoria: 'Planificacion',
      usuarioAsignadoId: 7,
      descripcion: undefined,
      estaCompletada: false,
      creadoEnUtc: '2026-07-15T09:00:00Z',
      actualizadoEnUtc: '2026-07-15T09:00:00Z',
      venceEnUtc: undefined,
    })

    render(<App />)

    await screen.findByRole('option', { name: 'Ana Martinez' })

    fireEvent.change(screen.getByLabelText('Titulo'), {
      target: { value: 'Nueva tarea' },
    })
    fireEvent.change(screen.getByLabelText('Categoria'), {
      target: { value: 'Planificacion' },
    })
    fireEvent.change(screen.getByLabelText('Usuario asignado'), {
      target: { value: '7' },
    })
    fireEvent.submit(screen.getByRole('button', { name: 'Crear tarea' }).closest('form')!)

    await waitFor(() => {
      expect(tareasServicio.crear).toHaveBeenCalledWith({
        titulo: 'Nueva tarea',
        categoria: 'Planificacion',
        usuarioAsignadoId: 7,
        descripcion: undefined,
        venceEnUtc: undefined,
      })
    })
  })

  it('muestra validacion visual cuando falta usuario asignado', async () => {
    render(<App />)

    await screen.findByRole('option', { name: 'Ana Martinez' })

    fireEvent.change(screen.getByLabelText('Titulo'), {
      target: { value: 'Nueva tarea' },
    })
    fireEvent.change(screen.getByLabelText('Categoria'), {
      target: { value: 'Planificacion' },
    })
    fireEvent.submit(screen.getByRole('button', { name: 'Crear tarea' }).closest('form')!)

    expect(await screen.findByText('El usuario asignado es obligatorio.')).toBeInTheDocument()
    expect(tareasServicio.crear).not.toHaveBeenCalled()
  })

  it('envia el nombre al crear un usuario y muestra confirmacion', async () => {
    vi.mocked(usuariosServicio.crear).mockResolvedValueOnce({
      id: 4,
      nombre: 'Ana Martinez',
      creadoEnUtc: '2026-07-15T09:00:00Z',
      actualizadoEnUtc: '2026-07-15T09:00:00Z',
    })

    render(<App />)

    fireEvent.change(screen.getByLabelText('Nombre'), {
      target: { value: 'Ana Martinez' },
    })
    fireEvent.submit(screen.getByRole('button', { name: 'Crear usuario' }).closest('form')!)

    await waitFor(() => {
      expect(usuariosServicio.crear).toHaveBeenCalledWith({
        nombre: 'Ana Martinez',
      })
    })

    expect(await screen.findByText('Usuario creado: Ana Martinez')).toBeInTheDocument()
  })
})
