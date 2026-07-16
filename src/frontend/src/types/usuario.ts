export interface Usuario {
  id: number
  nombre: string
  creadoEnUtc: string
  actualizadoEnUtc: string
}

export interface CrearUsuarioDto {
  nombre: string
}