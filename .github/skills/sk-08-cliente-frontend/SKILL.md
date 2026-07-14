---
name: sk-08-cliente-frontend
description: "Centraliza el cliente HTTP del frontend y mantiene los tipos TypeScript alineados con los contratos del backend. Manejo de errores comun y capa de servicios tipada."
---

# SK-08 - Cliente frontend y sincronizacion de tipos

Define como organizar la capa de servicios del frontend: cliente HTTP centralizado, tipos TypeScript alineados con los DTOs del backend, manejo de errores uniforme y separacion entre logica de datos y componentes visuales.

## Prerequisito: el proyecto debe existir

Antes de aplicar este skill, verificar que la estructura del proyecto esta creada:

```powershell
Test-Path src/backend -PathType Container
Test-Path src/frontend -PathType Container
```

Si alguno devuelve `False`, **detener y aplicar SK-00 primero** (`sk-00-scaffolding-proyecto`).

SK-08 requiere que la API este definida (SK-05) para conocer los endpoints y contratos. Los tipos TypeScript deben estar alineados con los DTOs de SK-02.

## Cuando usar este skill

- Al crear la capa de servicios de un modulo frontend nuevo.
- Al necesitar centralizar el cliente HTTP y el manejo de errores.
- Al detectar que los tipos TypeScript se han desalineado del backend.
- Al agregar un nuevo endpoint al backend que el frontend debe consumir.

## Objetivo

Producir una capa de servicios donde:
- Existe un unico cliente HTTP configurado (base URL, headers comunes).
- Los tipos TypeScript espejo de los DTOs del backend estan en `src/types/`.
- Cada modulo tiene su propio archivo de servicio en `src/services/`.
- Los componentes visuales no llaman a `fetch` ni a `axios` directamente.
- Los errores HTTP se manejan en un unico lugar.

## Estructura esperada

### Tipos TypeScript (espejo de los DTOs del backend)

```typescript
// src/frontend/src/types/tarea.ts
export interface Tarea {
  id: number;
  titulo: string;
  descripcion?: string;
  estaCompletada: boolean;
  creadoEnUtc: string;
  actualizadoEnUtc: string;
}

export interface CrearTareaDto {
  titulo: string;
  descripcion?: string;
}

export interface ActualizarTareaDto {
  titulo: string;
  descripcion?: string;
}
```

> Convencion: los nombres de los tipos TypeScript replican los nombres de los DTOs del backend, pero en camelCase para campos (alineado con la serializacion JSON del backend).

### Cliente HTTP centralizado

```typescript
// src/frontend/src/services/clienteHttp.ts
const URL_BASE = '/api';  // El proxy de Vite redirige a https://localhost:5001

async function peticion<T>(ruta: string, opciones?: RequestInit): Promise<T> {
  const respuesta = await fetch(`${URL_BASE}${ruta}`, {
    headers: { 'Content-Type': 'application/json', ...opciones?.headers },
    ...opciones
  });

  if (!respuesta.ok) {
    const error = await respuesta.text().catch(() => respuesta.statusText);
    throw new Error(`Error ${respuesta.status}: ${error}`);
  }

  if (respuesta.status === 204) return undefined as unknown as T;

  return respuesta.json() as Promise<T>;
}

export const clienteHttp = {
  get:    <T>(ruta: string)                        => peticion<T>(ruta),
  post:   <T>(ruta: string, cuerpo: unknown)       => peticion<T>(ruta, { method: 'POST',   body: JSON.stringify(cuerpo) }),
  put:    <T>(ruta: string, cuerpo: unknown)       => peticion<T>(ruta, { method: 'PUT',    body: JSON.stringify(cuerpo) }),
  patch:  <T>(ruta: string, cuerpo?: unknown)      => peticion<T>(ruta, { method: 'PATCH',  body: cuerpo ? JSON.stringify(cuerpo) : undefined }),
  delete: <T>(ruta: string)                        => peticion<T>(ruta, { method: 'DELETE' })
};
```

### Servicio de modulo

```typescript
// src/frontend/src/services/tareasServicio.ts
import { clienteHttp } from './clienteHttp';
import type { Tarea, CrearTareaDto, ActualizarTareaDto } from '../types/tarea';

export const tareasServicio = {
  obtenerTodas: ()                              => clienteHttp.get<Tarea[]>('/tareas'),
  obtenerPorId: (id: number)                   => clienteHttp.get<Tarea>(`/tareas/${id}`),
  crear:        (dto: CrearTareaDto)            => clienteHttp.post<Tarea>('/tareas', dto),
  actualizar:   (id: number, dto: ActualizarTareaDto) => clienteHttp.put<Tarea>(`/tareas/${id}`, dto),
  completar:    (id: number)                   => clienteHttp.patch<void>(`/tareas/${id}/completar`),
  eliminar:     (id: number)                   => clienteHttp.delete<void>(`/tareas/${id}`)
};
```

### Uso en componentes (con hook personalizado o directo)

```typescript
// Los componentes consumen el servicio, nunca fetch directamente
import { tareasServicio } from '../services/tareasServicio';

// Ejemplo en un componente funcional con useEffect
const [tareas, setTareas] = useState<Tarea[]>([]);

useEffect(() => {
  tareasServicio.obtenerTodas()
    .then(setTareas)
    .catch(err => console.error('Error al cargar tareas:', err));
}, []);
```

## Convencion de nombres

- Tipos: `src/frontend/src/types/{modulo}.ts` → `Tarea`, `CrearTareaDto`, `ActualizarTareaDto`.
- Servicios: `src/frontend/src/services/{modulo}Servicio.ts` → `tareasServicio`.
- Cliente HTTP: `src/frontend/src/services/clienteHttp.ts` (unico, compartido).
- Campos en tipos TypeScript: camelCase (alineado con la serializacion JSON del backend).

## Checklist de calidad

- [ ] Existe un unico `clienteHttp.ts` compartido por todos los servicios.
- [ ] Los tipos TypeScript estan en `src/types/` y replican los DTOs del backend.
- [ ] Los componentes no llaman a `fetch` directamente; usan el servicio correspondiente.
- [ ] Los errores HTTP se capturan en `clienteHttp.ts`, no en cada servicio ni componente.
- [ ] El cliente HTTP usa la ruta relativa `/api` (el proxy de Vite la redirige al backend).
- [ ] Los campos de fecha son `string` en los tipos TypeScript (se convierten al mostrar, SK-07).
- [ ] Existe al menos un test del servicio mockeando `fetch`.

## Errores comunes a evitar

- **`fetch` directo en componentes**: acoplamiento fuerte; centralizar en el servicio.
- **URL absoluta del backend en el servicio**: usar ruta relativa y dejar que el proxy de Vite la resuelva.
- **Tipos `any` para la respuesta del servidor**: tipar siempre con la interfaz correspondiente.
- **No manejar errores HTTP**: si el servidor devuelve 400 o 404, `fetch` no lanza por defecto; verificar `respuesta.ok`.
- **Campos de fecha como `Date` en los tipos**: JSON no transmite objetos `Date`; almacenar como `string` y convertir al mostrar.

## Relacion con otros skills

- **SK-02 (DTOs)**: los tipos TypeScript son el espejo de los DTOs del backend.
- **SK-05 (API REST)**: los servicios del frontend consumen los endpoints definidos en SK-05.
- **SK-07 (UTC)**: las fechas llegan como strings ISO 8601; convertir con las utilidades de SK-07 al mostrar.
- **SK-09 (Pruebas)**: los servicios del frontend se prueban mockeando `fetch` o el `clienteHttp`.
