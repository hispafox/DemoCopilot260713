import { useEffect, useMemo, useState } from 'react'
import type { FormEvent, MouseEvent } from 'react'
import { ContenidoDescripcion } from './components/ContenidoDescripcion'
import { EditorDescripcion } from './components/EditorDescripcion'
import { departamentosServicio } from './services/departamentosServicio'
import { tareasServicio } from './services/tareasServicio'
import { usuariosServicio } from './services/usuariosServicio'
import type { Departamento } from './types/departamento'
import type { Tarea } from './types/tarea'
import type { Usuario } from './types/usuario'
import './App.css'

type Seccion = 'usuarios' | 'tareas' | 'departamentos'

const rutaPorSeccion: Record<Seccion, string> = {
  departamentos: '/departamentos',
  usuarios: '/usuarios',
  tareas: '/tareas',
}

function resolverSeccionDesdeRuta(ruta: string): Seccion | null {
  if (ruta === rutaPorSeccion.usuarios) {
    return 'usuarios'
  }

  if (ruta === rutaPorSeccion.departamentos) {
    return 'departamentos'
  }

  if (ruta === rutaPorSeccion.tareas || ruta === '/') {
    return 'tareas'
  }

  return null
}

function resolverSeccionInicial(): Seccion {
  const seccion = resolverSeccionDesdeRuta(window.location.pathname)

  if (!seccion) {
    window.history.replaceState(null, '', rutaPorSeccion.tareas)
    return 'tareas'
  }

  if (window.location.pathname === '/') {
    window.history.replaceState(null, '', rutaPorSeccion.tareas)
  }

  return seccion
}

function App() {
  const [seccionActiva, setSeccionActiva] = useState<Seccion>(() => resolverSeccionInicial())
  const [tareas, setTareas] = useState<Tarea[]>([])
  const [usuarios, setUsuarios] = useState<Usuario[]>([])
  const [departamentos, setDepartamentos] = useState<Departamento[]>([])
  const [titulo, setTitulo] = useState('')
  const [categoria, setCategoria] = useState('')
  const [usuarioAsignadoId, setUsuarioAsignadoId] = useState('')
  const [descripcion, setDescripcion] = useState('')
  const [venceEnLocal, setVenceEnLocal] = useState('')
  const [nombreUsuario, setNombreUsuario] = useState('')
  const [departamentoUsuarioId, setDepartamentoUsuarioId] = useState('')
  const [usuarioEnEdicionId, setUsuarioEnEdicionId] = useState<number | null>(null)
  const [nombreDepartamento, setNombreDepartamento] = useState('')
  const [departamentoEnEdicionId, setDepartamentoEnEdicionId] = useState<number | null>(null)
  const [cargando, setCargando] = useState(true)
  const [creando, setCreando] = useState(false)
  const [creandoUsuario, setCreandoUsuario] = useState(false)
  const [guardandoDepartamento, setGuardandoDepartamento] = useState(false)
  const [error, setError] = useState('')
  const [mensajeUsuario, setMensajeUsuario] = useState('')
  const [mensajeDepartamento, setMensajeDepartamento] = useState('')

  const tareasOrdenadas = useMemo(
    () => [...tareas].sort((a, b) => a.estaCompletada === b.estaCompletada ? 0 : a.estaCompletada ? 1 : -1),
    [tareas],
  )

  useEffect(() => {
    void Promise.all([cargarTareas(), cargarUsuarios(), cargarDepartamentos()])
  }, [])

  useEffect(() => {
    function manejarCambioHistorial() {
      const seccion = resolverSeccionDesdeRuta(window.location.pathname)

      if (!seccion) {
        window.history.replaceState(null, '', rutaPorSeccion.tareas)
        setSeccionActiva('tareas')
        return
      }

      setSeccionActiva(seccion)
    }

    window.addEventListener('popstate', manejarCambioHistorial)
    return () => window.removeEventListener('popstate', manejarCambioHistorial)
  }, [])

  function navegarASeccion(seccion: Seccion) {
    const rutaObjetivo = rutaPorSeccion[seccion]

    if (window.location.pathname !== rutaObjetivo) {
      window.history.pushState(null, '', rutaObjetivo)
    }

    setSeccionActiva(seccion)
  }

  function manejarClickMenu(evento: MouseEvent<HTMLAnchorElement>, seccion: Seccion) {
    evento.preventDefault()
    navegarASeccion(seccion)
  }

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

  async function cargarDepartamentos() {
    try {
      const resultado = await departamentosServicio.obtenerTodos()
      setDepartamentos(resultado)
    } catch (errorCargarDepartamentos) {
      setError(errorCargarDepartamentos instanceof Error ? errorCargarDepartamentos.message : 'No se pudo cargar la lista de departamentos.')
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

    if (!departamentoUsuarioId.trim()) {
      setError('El departamento del usuario es obligatorio.')
      setMensajeUsuario('')
      return
    }

    try {
      setError('')
      setMensajeUsuario('')
      setCreandoUsuario(true)

      const esEdicion = usuarioEnEdicionId !== null

      const usuarioGuardado = esEdicion
        ? await usuariosServicio.actualizar(usuarioEnEdicionId, {
          nombre: nombreUsuario,
          departamentoId: Number(departamentoUsuarioId),
        })
        : await usuariosServicio.crear({
          nombre: nombreUsuario,
          departamentoId: Number(departamentoUsuarioId),
        })

      await cargarUsuarios()
      setNombreUsuario('')
      setDepartamentoUsuarioId('')
      setUsuarioEnEdicionId(null)
      setMensajeUsuario(esEdicion
        ? `Usuario actualizado: ${usuarioGuardado.nombre} (${usuarioGuardado.departamentoNombre})`
        : `Usuario creado: ${usuarioGuardado.nombre} (${usuarioGuardado.departamentoNombre})`)
    } catch (errorCrearUsuario) {
      setError(errorCrearUsuario instanceof Error ? errorCrearUsuario.message : 'No se pudo crear el usuario.')
      setMensajeUsuario('')
    } finally {
      setCreandoUsuario(false)
    }
  }

  async function editarUsuario(id: number) {
    try {
      setError('')
      setMensajeUsuario('')
      const usuario = await usuariosServicio.obtenerPorId(id)
      setUsuarioEnEdicionId(usuario.id)
      setNombreUsuario(usuario.nombre)
      setDepartamentoUsuarioId(String(usuario.departamentoId))
    } catch (errorEditarUsuario) {
      setError(errorEditarUsuario instanceof Error ? errorEditarUsuario.message : 'No se pudo cargar el usuario para edicion.')
    }
  }

  function cancelarEdicionUsuario() {
    setUsuarioEnEdicionId(null)
    setNombreUsuario('')
    setDepartamentoUsuarioId('')
    setMensajeUsuario('')
  }

  async function guardarDepartamento(evento: FormEvent<HTMLFormElement>) {
    evento.preventDefault()

    if (!nombreDepartamento.trim()) {
      setError('El nombre del departamento es obligatorio.')
      setMensajeDepartamento('')
      return
    }

    try {
      setError('')
      setMensajeDepartamento('')
      setGuardandoDepartamento(true)

      if (departamentoEnEdicionId) {
        const actualizado = await departamentosServicio.actualizar(departamentoEnEdicionId, { nombre: nombreDepartamento })
        setDepartamentos((previo) => previo.map((departamento) => departamento.id === actualizado.id ? actualizado : departamento))
        setMensajeDepartamento(`Departamento actualizado: ${actualizado.nombre}`)
      } else {
        const creado = await departamentosServicio.crear({ nombre: nombreDepartamento })
        setDepartamentos((previo) => [...previo, creado].sort((a, b) => a.nombre.localeCompare(b.nombre)))
        setMensajeDepartamento(`Departamento creado: ${creado.nombre}`)
      }

      await cargarUsuarios()
      setNombreDepartamento('')
      setDepartamentoEnEdicionId(null)
    } catch (errorGuardarDepartamento) {
      setError(errorGuardarDepartamento instanceof Error ? errorGuardarDepartamento.message : 'No se pudo guardar el departamento.')
      setMensajeDepartamento('')
    } finally {
      setGuardandoDepartamento(false)
    }
  }

  async function eliminarDepartamento(id: number) {
    try {
      setError('')
      setMensajeDepartamento('')
      await departamentosServicio.eliminar(id)
      setDepartamentos((previo) => previo.filter((departamento) => departamento.id !== id))
      await cargarUsuarios()
    } catch (errorEliminarDepartamento) {
      setError(errorEliminarDepartamento instanceof Error ? errorEliminarDepartamento.message : 'No se pudo eliminar el departamento.')
    }
  }

  function editarDepartamento(departamento: Departamento) {
    setDepartamentoEnEdicionId(departamento.id)
    setNombreDepartamento(departamento.nombre)
    setMensajeDepartamento('')
    setError('')
  }

  function cancelarEdicionDepartamento() {
    setDepartamentoEnEdicionId(null)
    setNombreDepartamento('')
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

      <nav className="menu-secciones" aria-label="Navegacion principal">
        <a
          href={rutaPorSeccion.departamentos}
          className={seccionActiva === 'departamentos' ? 'menu-seccion menu-seccion--activa' : 'menu-seccion'}
          aria-current={seccionActiva === 'departamentos' ? 'page' : undefined}
          onClick={(evento) => manejarClickMenu(evento, 'departamentos')}
        >
          Departamentos
        </a>
        <a
          href={rutaPorSeccion.usuarios}
          className={seccionActiva === 'usuarios' ? 'menu-seccion menu-seccion--activa' : 'menu-seccion'}
          aria-current={seccionActiva === 'usuarios' ? 'page' : undefined}
          onClick={(evento) => manejarClickMenu(evento, 'usuarios')}
        >
          Usuarios
        </a>
        <a
          href={rutaPorSeccion.tareas}
          className={seccionActiva === 'tareas' ? 'menu-seccion menu-seccion--activa' : 'menu-seccion'}
          aria-current={seccionActiva === 'tareas' ? 'page' : undefined}
          onClick={(evento) => manejarClickMenu(evento, 'tareas')}
        >
          Tareas
        </a>
      </nav>

      {seccionActiva === 'usuarios' ? (
        <>
          <section className="panel panel-formulario">
            <h2>Mantenimiento de usuarios</h2>
            {error ? <p className="error">{error}</p> : null}
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

              <label htmlFor="departamentoUsuarioId">Departamento</label>
              <select
                id="departamentoUsuarioId"
                value={departamentoUsuarioId}
                onChange={(evento) => setDepartamentoUsuarioId(evento.target.value)}
                required
              >
                <option value="">Selecciona un departamento</option>
                {departamentos.map((departamento) => (
                  <option key={departamento.id} value={departamento.id}>
                    {departamento.nombre}
                  </option>
                ))}
              </select>

              <button type="submit" disabled={creandoUsuario}>
                {creandoUsuario ? 'Guardando usuario...' : usuarioEnEdicionId ? 'Guardar cambios' : 'Crear usuario'}
              </button>

              {usuarioEnEdicionId ? (
                <button type="button" onClick={cancelarEdicionUsuario}>
                  Cancelar edicion
                </button>
              ) : null}
            </form>
            {mensajeUsuario ? <p>{mensajeUsuario}</p> : null}
          </section>

          <section className="panel panel-lista">
            <h2>Usuarios registrados</h2>
            {usuarios.length === 0 ? (
              <p className="vacio">Todavia no hay usuarios.</p>
            ) : (
              <ul className="lista-usuarios" aria-live="polite">
                {usuarios.map((usuario) => (
                  <li key={usuario.id}>
                    {usuario.nombre} - {usuario.departamentoNombre}
                    <button type="button" onClick={() => void editarUsuario(usuario.id)}>
                      Editar
                    </button>
                  </li>
                ))}
              </ul>
            )}
          </section>
        </>
      ) : null}

      {seccionActiva === 'departamentos' ? (
        <>
          <section className="panel panel-formulario">
            <h2>Mantenimiento de departamentos</h2>
            {error ? <p className="error">{error}</p> : null}
            <form onSubmit={guardarDepartamento}>
              <label htmlFor="nombreDepartamento">Nombre</label>
              <input
                id="nombreDepartamento"
                value={nombreDepartamento}
                maxLength={200}
                onChange={(evento) => setNombreDepartamento(evento.target.value)}
                placeholder="Ejemplo: Operaciones"
                required
              />

              <button type="submit" disabled={guardandoDepartamento}>
                {guardandoDepartamento ? 'Guardando departamento...' : departamentoEnEdicionId ? 'Guardar cambios' : 'Crear departamento'}
              </button>

              {departamentoEnEdicionId ? (
                <button type="button" onClick={cancelarEdicionDepartamento}>
                  Cancelar edicion
                </button>
              ) : null}
            </form>
            {mensajeDepartamento ? <p>{mensajeDepartamento}</p> : null}
          </section>

          <section className="panel panel-lista">
            <h2>Departamentos registrados</h2>
            {departamentos.length === 0 ? (
              <p className="vacio">Todavia no hay departamentos.</p>
            ) : (
              <ul className="lista-tareas" aria-live="polite">
                {[...departamentos].sort((a, b) => a.nombre.localeCompare(b.nombre)).map((departamento) => (
                  <li key={departamento.id}>
                    <div>
                      <h3>{departamento.nombre}</h3>
                      <small>Creado: {formatearFecha(departamento.creadoEnUtc)}</small>
                    </div>
                    <div className="acciones">
                      <button type="button" onClick={() => editarDepartamento(departamento)}>
                        Editar
                      </button>
                      <button type="button" className="eliminar" onClick={() => void eliminarDepartamento(departamento.id)}>
                        Eliminar
                      </button>
                    </div>
                  </li>
                ))}
              </ul>
            )}
          </section>
        </>
      ) : null}

      {seccionActiva === 'tareas' ? (
        <>
          <section className="panel panel-formulario">
            <h2>Mantenimiento de tareas</h2>
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
        </>
      ) : null}
    </main>
  )
}

export default App
