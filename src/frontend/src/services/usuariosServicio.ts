import { clienteHttp } from './clienteHttp'
import type { CrearUsuarioDto, Usuario } from '../types/usuario'

export const usuariosServicio = {
  obtenerTodos: () => clienteHttp.get<Usuario[]>('/usuarios'),
  crear: (dto: CrearUsuarioDto) => clienteHttp.post<Usuario>('/usuarios', dto),
}