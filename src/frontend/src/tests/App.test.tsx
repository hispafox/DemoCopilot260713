import { fireEvent, render, screen, waitFor } from '@testing-library/react'
import { beforeEach, describe, expect, it, vi } from 'vitest'
import App from '../App'
import { departamentosServicio } from '../services/departamentosServicio'
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
          departamentoId: 3,
          departamentoNombre: 'Operaciones',
          creadoEnUtc: '2026-07-15T09:00:00Z',
          actualizadoEnUtc: '2026-07-15T09:00:00Z',
        },
      ]),
      obtenerPorId: vi.fn(),
      crear: vi.fn(),
      actualizar: vi.fn(),
    },
  }
})

vi.mock('../services/departamentosServicio', () => {
  return {
    departamentosServicio: {
      obtenerTodos: vi.fn().mockResolvedValue([
        {
          id: 3,
          nombre: 'Operaciones',
          creadoEnUtc: '2026-07-15T09:00:00Z',
          actualizadoEnUtc: '2026-07-15T09:00:00Z',
        },
      ]),
      crear: vi.fn(),
      actualizar: vi.fn(),
      eliminar: vi.fn(),
    },
  }
})

describe('App', () => {
  beforeEach(() => {
    vi.clearAllMocks()
    window.history.replaceState(null, '', '/')
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
    expect(screen.getByRole('link', { name: 'Tareas' })).toHaveAttribute('aria-current', 'page')
  })

  it('permite navegar por menu entre usuarios y tareas con estado activo', async () => {
    render(<App />)

    const enlaceUsuarios = screen.getByRole('link', { name: 'Usuarios' })
    const enlaceTareas = screen.getByRole('link', { name: 'Tareas' })

    fireEvent.click(enlaceUsuarios)

    expect(await screen.findByRole('heading', { name: 'Mantenimiento de usuarios' })).toBeInTheDocument()
    expect(enlaceUsuarios).toHaveAttribute('aria-current', 'page')
    expect(enlaceTareas).not.toHaveAttribute('aria-current')
    expect(window.location.pathname).toBe('/usuarios')

    fireEvent.click(enlaceTareas)

    expect(await screen.findByRole('heading', { name: 'Mantenimiento de tareas' })).toBeInTheDocument()
    expect(enlaceTareas).toHaveAttribute('aria-current', 'page')
    expect(enlaceUsuarios).not.toHaveAttribute('aria-current')
    expect(window.location.pathname).toBe('/tareas')
  })

  it('permite navegar por menu a departamentos con estado activo', async () => {
    render(<App />)

    const enlaceDepartamentos = screen.getByRole('link', { name: 'Departamentos' })

    fireEvent.click(enlaceDepartamentos)

    expect(await screen.findByRole('heading', { name: 'Mantenimiento de departamentos' })).toBeInTheDocument()
    expect(enlaceDepartamentos).toHaveAttribute('aria-current', 'page')
    expect(window.location.pathname).toBe('/departamentos')
  })

  it('mantiene activa la seccion de tareas al recargar en su ruta', async () => {
    window.history.replaceState(null, '', '/tareas')

    render(<App />)

    expect(await screen.findByRole('heading', { name: 'Mantenimiento de tareas' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Tareas' })).toHaveAttribute('aria-current', 'page')
    expect(screen.getByRole('link', { name: 'Usuarios' })).not.toHaveAttribute('aria-current')
  })

  it('redirecciona ruta invalida a tareas y mantiene menu visible', async () => {
    window.history.replaceState(null, '', '/ruta-invalida')

    render(<App />)

    expect(await screen.findByRole('heading', { name: 'Mantenimiento de tareas' })).toBeInTheDocument()
    expect(screen.getByRole('navigation', { name: 'Navegacion principal' })).toBeInTheDocument()
    expect(screen.getByRole('link', { name: 'Tareas' })).toHaveAttribute('aria-current', 'page')
    expect(window.location.pathname).toBe('/tareas')
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
      departamentoId: 3,
      departamentoNombre: 'Operaciones',
      creadoEnUtc: '2026-07-15T09:00:00Z',
      actualizadoEnUtc: '2026-07-15T09:00:00Z',
    })

    render(<App />)

    fireEvent.click(screen.getByRole('link', { name: 'Usuarios' }))

    await screen.findByRole('heading', { name: 'Mantenimiento de usuarios' })

    fireEvent.change(screen.getByLabelText('Nombre'), {
      target: { value: 'Ana Martinez' },
    })
    fireEvent.change(screen.getByLabelText('Departamento'), {
      target: { value: '3' },
    })
    fireEvent.submit(screen.getByRole('button', { name: 'Crear usuario' }).closest('form')!)

    await waitFor(() => {
      expect(usuariosServicio.crear).toHaveBeenCalledWith({
        nombre: 'Ana Martinez',
        departamentoId: 3,
      })
    })

    expect(await screen.findByText('Usuario creado: Ana Martinez (Operaciones)')).toBeInTheDocument()
  })

  it('crea un departamento desde la seccion de departamentos', async () => {
    vi.mocked(departamentosServicio.crear).mockResolvedValueOnce({
      id: 9,
      nombre: 'Finanzas',
      creadoEnUtc: '2026-07-16T09:00:00Z',
      actualizadoEnUtc: '2026-07-16T09:00:00Z',
    })

    render(<App />)

    fireEvent.click(screen.getByRole('link', { name: 'Departamentos' }))
    await screen.findByRole('heading', { name: 'Mantenimiento de departamentos' })

    fireEvent.change(screen.getByLabelText('Nombre'), {
      target: { value: 'Finanzas' },
    })
    fireEvent.submit(screen.getByRole('button', { name: 'Crear departamento' }).closest('form')!)

    await waitFor(() => {
      expect(departamentosServicio.crear).toHaveBeenCalledWith({
        nombre: 'Finanzas',
      })
    })

    expect(await screen.findByText('Departamento creado: Finanzas')).toBeInTheDocument()
  })

  it('actualiza un departamento desde la seccion de departamentos', async () => {
    vi.mocked(departamentosServicio.actualizar).mockResolvedValueOnce({
      id: 3,
      nombre: 'Operaciones Globales',
      creadoEnUtc: '2026-07-15T09:00:00Z',
      actualizadoEnUtc: '2026-07-16T10:00:00Z',
    })

    render(<App />)

    fireEvent.click(screen.getByRole('link', { name: 'Departamentos' }))
    await screen.findByRole('heading', { name: 'Mantenimiento de departamentos' })

    fireEvent.click(screen.getByRole('button', { name: 'Editar' }))
    fireEvent.change(screen.getByLabelText('Nombre'), {
      target: { value: 'Operaciones Globales' },
    })
    fireEvent.submit(screen.getByRole('button', { name: 'Guardar cambios' }).closest('form')!)

    await waitFor(() => {
      expect(departamentosServicio.actualizar).toHaveBeenCalledWith(3, {
        nombre: 'Operaciones Globales',
      })
    })

    expect(await screen.findByText('Departamento actualizado: Operaciones Globales')).toBeInTheDocument()
  })

  it('elimina un departamento desde la seccion de departamentos', async () => {
    vi.mocked(departamentosServicio.eliminar).mockResolvedValueOnce(undefined)

    render(<App />)

    fireEvent.click(screen.getByRole('link', { name: 'Departamentos' }))
    await screen.findByRole('heading', { name: 'Mantenimiento de departamentos' })

    fireEvent.click(screen.getByRole('button', { name: 'Eliminar' }))

    await waitFor(() => {
      expect(departamentosServicio.eliminar).toHaveBeenCalledWith(3)
    })
  })

  it('muestra error de backend al eliminar un departamento', async () => {
    vi.mocked(departamentosServicio.eliminar).mockRejectedValueOnce(
      new Error('Error 400: No se puede eliminar el departamento porque tiene usuarios asociados.'),
    )

    render(<App />)

    fireEvent.click(screen.getByRole('link', { name: 'Departamentos' }))
    await screen.findByRole('heading', { name: 'Mantenimiento de departamentos' })

    fireEvent.click(screen.getByRole('button', { name: 'Eliminar' }))

    expect(await screen.findByText('Error 400: No se puede eliminar el departamento porque tiene usuarios asociados.')).toBeInTheDocument()
  })
})
