export interface Departamento {
  id: number
  nombre: string
  creadoEnUtc: string
  actualizadoEnUtc: string
}

export interface CrearDepartamentoDto {
  nombre: string
}

export interface ActualizarDepartamentoDto {
  nombre: string
}