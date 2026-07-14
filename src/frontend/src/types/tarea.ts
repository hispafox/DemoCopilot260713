export interface Tarea {
  id: number
  titulo: string
  descripcion?: string
  estaCompletada: boolean
  creadoEnUtc: string
  actualizadoEnUtc: string
}

export interface CrearTareaDto {
  titulo: string
  descripcion?: string
}

export interface ActualizarTareaDto {
  titulo: string
  descripcion?: string
}
