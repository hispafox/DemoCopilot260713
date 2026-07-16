export interface Usuario {
  id: number
  nombre: string
  departamentoId: number
  departamentoNombre: string
  creadoEnUtc: string
  actualizadoEnUtc: string
}

export interface CrearUsuarioDto {
  nombre: string
  departamentoId: number
}

export interface ActualizarUsuarioDto {
  nombre: string
  departamentoId: number
}