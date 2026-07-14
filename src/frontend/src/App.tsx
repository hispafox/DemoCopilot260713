import { useEffect, useMemo, useState } from 'react'
import type { FormEvent } from 'react'
import { tareasServicio } from './services/tareasServicio'
import type { Tarea } from './types/tarea'
import './App.css'

function App() {
  const [tareas, setTareas] = useState<Tarea[]>([])
  const [titulo, setTitulo] = useState('')
  const [descripcion, setDescripcion] = useState('')
  const [venceEnLocal, setVenceEnLocal] = useState('')
  const [cargando, setCargando] = useState(true)
  const [creando, setCreando] = useState(false)
  const [error, setError] = useState('')

  const tareasOrdenadas = useMemo(
    () => [...tareas].sort((a, b) => a.estaCompletada === b.estaCompletada ? 0 : a.estaCompletada ? 1 : -1),
    [tareas],
  )

  useEffect(() => {
    void cargarTareas()
  }, [])

  async function cargarTareas() {
    try {
      setError('')
      setCargando(true)
      const resultado = await tareasServicio.obtenerTodas()
      setTareas(resultado)
    } catch (errorCargar) {
      setError(errorCargar instanceof Error ? errorCargar.message : 'No se pudo cargar la lista de tareas.')
    } finally {
      setCargando(false)
    }
  }

  async function crearTarea(evento: FormEvent<HTMLFormElement>) {
    evento.preventDefault()

    if (!titulo.trim()) {
      setError('El titulo es obligatorio.')
      return
    }

    try {
      setError('')
      setCreando(true)
      const tareaCreada = await tareasServicio.crear({
        titulo,
        descripcion: descripcion.trim() || undefined,
        venceEnUtc: convertirFechaLocalAIsoUtc(venceEnLocal),
      })

      setTareas((previo) => [tareaCreada, ...previo])
      setTitulo('')
      setDescripcion('')
      setVenceEnLocal('')
    } catch (errorCrear) {
      setError(errorCrear instanceof Error ? errorCrear.message : 'No se pudo crear la tarea.')
    } finally {
      setCreando(false)
    }
  }

  async function completarTarea(id: number) {
    try {
      setError('')
      await tareasServicio.completar(id)
      await cargarTareas()
    } catch (errorCompletar) {
      setError(errorCompletar instanceof Error ? errorCompletar.message : 'No se pudo completar la tarea.')
    }
  }

  async function eliminarTarea(id: number) {
    try {
      setError('')
      await tareasServicio.eliminar(id)
      setTareas((previo) => previo.filter((tarea) => tarea.id !== id))
    } catch (errorEliminar) {
      setError(errorEliminar instanceof Error ? errorEliminar.message : 'No se pudo eliminar la tarea.')
    }
  }

  function formatearFecha(isoUtc: string) {
    return new Date(isoUtc).toLocaleString()
  }

  function convertirFechaLocalAIsoUtc(fechaLocal: string) {
    if (!fechaLocal.trim()) {
      return undefined
    }

    return new Date(fechaLocal).toISOString()
  }

  return (
    <main className="contenedor">
      <header className="cabecera">
        <h1>Lista de tareas</h1>
        <p>Gestiona tus tareas con backend ASP.NET y frontend React.</p>
      </header>

      <section className="panel panel-formulario">
        <h2>Nueva tarea</h2>
        <form onSubmit={crearTarea}>
          <label htmlFor="titulo">Titulo</label>
          <input
            id="titulo"
            value={titulo}
            maxLength={200}
            onChange={(evento) => setTitulo(evento.target.value)}
            placeholder="Ejemplo: preparar demo"
            required
          />

          <label htmlFor="descripcion">Descripcion (opcional)</label>
          <textarea
            id="descripcion"
            value={descripcion}
            maxLength={1000}
            onChange={(evento) => setDescripcion(evento.target.value)}
            placeholder="Detalles de la tarea"
          />

          <label htmlFor="venceEnLocal">Vence el (opcional)</label>
          <input
            id="venceEnLocal"
            type="datetime-local"
            value={venceEnLocal}
            onChange={(evento) => setVenceEnLocal(evento.target.value)}
          />

          <button type="submit" disabled={creando}>
            {creando ? 'Creando...' : 'Crear tarea'}
          </button>
        </form>
      </section>

      <section className="panel panel-lista">
        <div className="encabezado-lista">
          <h2>Tareas</h2>
          <button type="button" onClick={() => void cargarTareas()} disabled={cargando}>
            {cargando ? 'Cargando...' : 'Recargar'}
          </button>
        </div>

        {error ? <p className="error">{error}</p> : null}

        {!cargando && tareasOrdenadas.length === 0 ? (
          <p className="vacio">Todavia no hay tareas.</p>
        ) : (
          <ul className="lista-tareas" aria-live="polite">
            {tareasOrdenadas.map((tarea) => (
              <li key={tarea.id} className={tarea.estaCompletada ? 'completada' : ''}>
                <div>
                  <h3>{tarea.titulo}</h3>
                  {tarea.descripcion ? <p>{tarea.descripcion}</p> : null}
                  <small>
                    Creada: {formatearFecha(tarea.creadoEnUtc)} | Actualizada: {formatearFecha(tarea.actualizadoEnUtc)}
                  </small>
                  {tarea.venceEnUtc ? <small>Vence: {formatearFecha(tarea.venceEnUtc)}</small> : null}
                </div>
                <div className="acciones">
                  {!tarea.estaCompletada ? (
                    <button type="button" onClick={() => void completarTarea(tarea.id)}>
                      Completar
                    </button>
                  ) : null}
                  <button type="button" className="eliminar" onClick={() => void eliminarTarea(tarea.id)}>
                    Eliminar
                  </button>
                </div>
              </li>
            ))}
          </ul>
        )}
      </section>
    </main>
  )
}

export default App
