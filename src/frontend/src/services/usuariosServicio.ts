import { clienteHttp } from './clienteHttp'
import type { ActualizarUsuarioDto, CrearUsuarioDto, Usuario } from '../types/usuario'

export const usuariosServicio = {
  obtenerTodos: () => clienteHttp.get<Usuario[]>('/usuarios'),
  obtenerPorId: (id: number) => clienteHttp.get<Usuario>(`/usuarios/${id}`),
  crear: (dto: CrearUsuarioDto) => clienteHttp.post<Usuario>('/usuarios', dto),
  actualizar: (id: number, dto: ActualizarUsuarioDto) => clienteHttp.put<Usuario>(`/usuarios/${id}`, dto),
}