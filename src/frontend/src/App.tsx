import { useEffect, useMemo, useState } from 'react'
import type { FormEvent } from 'react'
import { ContenidoDescripcion } from './components/ContenidoDescripcion'
import { EditorDescripcion } from './components/EditorDescripcion'
import { tareasServicio } from './services/tareasServicio'
import { usuariosServicio } from './services/usuariosServicio'
import type { Tarea } from './types/tarea'
import type { Usuario } from './types/usuario'
import './App.css'

function App() {
  const [tareas, setTareas] = useState<Tarea[]>([])
  const [usuarios, setUsuarios] = useState<Usuario[]>([])
  const [titulo, setTitulo] = useState('')
  const [categoria, setCategoria] = useState('')
  const [usuarioAsignadoId, setUsuarioAsignadoId] = useState('')
  const [descripcion, setDescripcion] = useState('')
  const [venceEnLocal, setVenceEnLocal] = useState('')
  const [nombreUsuario, setNombreUsuario] = useState('')
  const [cargando, setCargando] = useState(true)
  const [creando, setCreando] = useState(false)
  const [creandoUsuario, setCreandoUsuario] = useState(false)
  const [error, setError] = useState('')
  const [mensajeUsuario, setMensajeUsuario] = useState('')

  const tareasOrdenadas = useMemo(
    () => [...tareas].sort((a, b) => a.estaCompletada === b.estaCompletada ? 0 : a.estaCompletada ? 1 : -1),
    [tareas],
  )

  useEffect(() => {
    void Promise.all([cargarTareas(), cargarUsuarios()])
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

  async function cargarUsuarios() {
    try {
      const resultado = await usuariosServicio.obtenerTodos()
      setUsuarios(resultado)
    } catch (errorCargarUsuarios) {
      setError(errorCargarUsuarios instanceof Error ? errorCargarUsuarios.message : 'No se pudo cargar la lista de usuarios.')
    }
  }

  async function crearTarea(evento: FormEvent<HTMLFormElement>) {
    evento.preventDefault()

    if (!titulo.trim()) {
      setError('El titulo es obligatorio.')
      return
    }

    if (!categoria.trim()) {
      setError('La categoria es obligatoria.')
      return
    }

    if (!usuarioAsignadoId.trim()) {
      setError('El usuario asignado es obligatorio.')
      return
    }

    try {
      setError('')
      setCreando(true)
      const tareaCreada = await tareasServicio.crear({
        titulo,
        categoria,
        usuarioAsignadoId: Number(usuarioAsignadoId),
        descripcion: descripcion.trim() || undefined,
        venceEnUtc: convertirFechaLocalAIsoUtc(venceEnLocal),
      })

      setTareas((previo) => [tareaCreada, ...previo])
      setTitulo('')
      setCategoria('')
      setUsuarioAsignadoId('')
      setDescripcion('')
      setVenceEnLocal('')
    } catch (errorCrear) {
      setError(errorCrear instanceof Error ? errorCrear.message : 'No se pudo crear la tarea.')
    } finally {
      setCreando(false)
    }
  }

  async function crearUsuario(evento: FormEvent<HTMLFormElement>) {
    evento.preventDefault()

    if (!nombreUsuario.trim()) {
      setError('El nombre del usuario es obligatorio.')
      setMensajeUsuario('')
      return
    }

    try {
      setError('')
      setMensajeUsuario('')
      setCreandoUsuario(true)

      const usuarioCreado = await usuariosServicio.crear({
        nombre: nombreUsuario,
      })

      setNombreUsuario('')
      setMensajeUsuario(`Usuario creado: ${usuarioCreado.nombre}`)
    } catch (errorCrearUsuario) {
      setError(errorCrearUsuario instanceof Error ? errorCrearUsuario.message : 'No se pudo crear el usuario.')
      setMensajeUsuario('')
    } finally {
      setCreandoUsuario(false)
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

  function obtenerNombreUsuario(usuarioId: number) {
    return usuarios.find((usuario) => usuario.id === usuarioId)?.nombre ?? `Usuario ${usuarioId}`
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

          <label htmlFor="categoria">Categoria</label>
          <input
            id="categoria"
            value={categoria}
            maxLength={100}
            onChange={(evento) => setCategoria(evento.target.value)}
            placeholder="Ejemplo: trabajo"
            required
          />

          <label htmlFor="usuarioAsignadoId">Usuario asignado</label>
          <select
            id="usuarioAsignadoId"
            value={usuarioAsignadoId}
            onChange={(evento) => setUsuarioAsignadoId(evento.target.value)}
            required
          >
            <option value="">Selecciona un usuario</option>
            {usuarios.map((usuario) => (
              <option key={usuario.id} value={usuario.id}>
                {usuario.nombre}
              </option>
            ))}
          </select>

          <label htmlFor="descripcion">Descripcion (opcional)</label>
          <EditorDescripcion
            id="descripcion"
            valor={descripcion}
            maxLength={1000}
            onChange={setDescripcion}
            placeholder="Detalles de la tarea. Admite negrita, cursiva, listas, enlaces y bloques de codigo."
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

      <section className="panel panel-formulario">
        <h2>Alta de usuario</h2>
        <form onSubmit={crearUsuario}>
          <label htmlFor="nombreUsuario">Nombre</label>
          <input
            id="nombreUsuario"
            value={nombreUsuario}
            maxLength={200}
            onChange={(evento) => setNombreUsuario(evento.target.value)}
            placeholder="Ejemplo: Ana Martinez"
            required
          />

          <button type="submit" disabled={creandoUsuario}>
            {creandoUsuario ? 'Creando usuario...' : 'Crear usuario'}
          </button>
        </form>
        {mensajeUsuario ? <p>{mensajeUsuario}</p> : null}
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
                  <small>Categoria: {tarea.categoria}</small>
                  <small>Usuario: {obtenerNombreUsuario(tarea.usuarioAsignadoId)}</small>
                  {tarea.descripcion ? <ContenidoDescripcion html={tarea.descripcion} className="contenido-enriquecido" /> : null}
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
