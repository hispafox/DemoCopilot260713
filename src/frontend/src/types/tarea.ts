export interface Tarea {
  id: number
  titulo: string
  descripcion?: string
  categoria: string
  usuarioAsignadoId: number
  estaCompletada: boolean
  creadoEnUtc: string
  actualizadoEnUtc: string
  venceEnUtc?: string
}

export interface CrearTareaDto {
  titulo: string
  categoria: string
  usuarioAsignadoId: number
  descripcion?: string
  venceEnUtc?: string
}

export interface ActualizarTareaDto {
  titulo: string
  categoria: string
  descripcion?: string
  venceEnUtc?: string
}
