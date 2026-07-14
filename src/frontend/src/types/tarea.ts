export interface Tarea {
  id: number
  titulo: string
  descripcion?: string
  estaCompletada: boolean
  creadoEnUtc: string
  actualizadoEnUtc: string
  venceEnUtc?: string
}

export interface CrearTareaDto {
  titulo: string
  descripcion?: string
  venceEnUtc?: string
}

export interface ActualizarTareaDto {
  titulo: string
  descripcion?: string
  venceEnUtc?: string
}
