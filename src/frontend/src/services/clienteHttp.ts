const urlBase = '/api'

async function peticion<T>(ruta: string, opciones?: RequestInit): Promise<T> {
  const respuesta = await fetch(`${urlBase}${ruta}`, {
    headers: {
      'Content-Type': 'application/json',
      ...(opciones?.headers ?? {}),
    },
    ...opciones,
  })

  if (!respuesta.ok) {
    const detalleError = await respuesta
      .json()
      .then((cuerpo) => (typeof cuerpo?.error === 'string' ? cuerpo.error : JSON.stringify(cuerpo)))
      .catch(async () => await respuesta.text())

    throw new Error(`Error ${respuesta.status}: ${detalleError || respuesta.statusText}`)
  }

  if (respuesta.status === 204) {
    return undefined as T
  }

  return (await respuesta.json()) as T
}

export const clienteHttp = {
  get: <T>(ruta: string) => peticion<T>(ruta),
  post: <T>(ruta: string, cuerpo: unknown) =>
    peticion<T>(ruta, {
      method: 'POST',
      body: JSON.stringify(cuerpo),
    }),
  put: <T>(ruta: string, cuerpo: unknown) =>
    peticion<T>(ruta, {
      method: 'PUT',
      body: JSON.stringify(cuerpo),
    }),
  patch: <T>(ruta: string, cuerpo?: unknown) =>
    peticion<T>(ruta, {
      method: 'PATCH',
      body: cuerpo ? JSON.stringify(cuerpo) : undefined,
    }),
  delete: <T>(ruta: string) =>
    peticion<T>(ruta, {
      method: 'DELETE',
    }),
}
