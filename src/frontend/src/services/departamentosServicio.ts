import { clienteHttp } from './clienteHttp'
import type { ActualizarDepartamentoDto, CrearDepartamentoDto, Departamento } from '../types/departamento'

export const departamentosServicio = {
  obtenerTodos: () => clienteHttp.get<Departamento[]>('/departamentos'),
  crear: (dto: CrearDepartamentoDto) => clienteHttp.post<Departamento>('/departamentos', dto),
  actualizar: (id: number, dto: ActualizarDepartamentoDto) => clienteHttp.put<Departamento>(`/departamentos/${id}`, dto),
  eliminar: (id: number) => clienteHttp.delete<void>(`/departamentos/${id}`),
}