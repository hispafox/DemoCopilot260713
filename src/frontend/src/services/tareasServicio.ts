import { clienteHttp } from './clienteHttp'
import type { ActualizarTareaDto, CrearTareaDto, Tarea } from '../types/tarea'

export const tareasServicio = {
  obtenerTodas: () => clienteHttp.get<Tarea[]>('/tareas'),
  obtenerPorId: (id: number) => clienteHttp.get<Tarea>(`/tareas/${id}`),
  crear: (dto: CrearTareaDto) => clienteHttp.post<Tarea>('/tareas', dto),
  actualizar: (id: number, dto: ActualizarTareaDto) =>
    clienteHttp.put<Tarea>(`/tareas/${id}`, dto),
  completar: (id: number) => clienteHttp.patch<void>(`/tareas/${id}/completar`),
  eliminar: (id: number) => clienteHttp.delete<void>(`/tareas/${id}`),
}
