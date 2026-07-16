# Evolutivos Agrupados

Fecha de corte: 2026-07-16
Objetivo: vista unica y corta de evolutivos activos, sin duplicidad de documentos.

## 1. Resumen por grupo

### Grupo A: Usuarios y asignacion de tareas

- Fuente principal: docs/planificacion/backlog-alta-usuarios-asignacion-tareas.md
- Incluye: HU1, HU2, HU3, HU4, HU5.
- Estado: parcial.
- Implementado: HU1 y HU2.
- Pendiente: HU3 (reasignacion), HU4 (conteo), HU5 (listado por usuario).
- Evidencia:
	- src/backend/Api/Controladores/UsuariosControlador.cs:8
	- src/backend/Api/Controladores/UsuariosControlador.cs:25
	- src/backend/Application/DTOs/CrearTareaDto.cs:16
	- src/frontend/src/App.tsx:124

### Grupo B: Categorizacion de tareas

- Fuentes principales:
	- docs/planificacion/planificacion-categorias-tareas-2026-07-15.md
	- docs/planificacion/planificacion-categorizar-tareas-2026-07-15.md
- Incluye: HU-1, HU-2, HU-3.
- Estado: parcial.
- Implementado: categoria en dominio, DTOs y flujo UI.
- Pendiente: definir y aplicar catalogo cerrado de categorias.
- Evidencia:
	- src/backend/Domain/Tarea.cs:11
	- src/backend/Application/DTOs/CrearTareaDto.cs:11
	- src/backend/Application/DTOs/ActualizarTareaDto.cs:11
	- src/frontend/src/App.tsx:118

### Grupo C: Separacion visual usuarios/tareas

- Fuente consolidada: implementacion en frontend y pruebas.
- Incluye: menu por seccion + contexto activo.
- Estado: implementado.
- Evidencia:
	- src/frontend/src/App.tsx:10
	- src/frontend/src/App.tsx:79
	- src/frontend/src/App.tsx:210

## 2. Pendientes clave

- Reasignacion de tarea.
- Conteo de tareas por usuario.
- Listado de tareas por usuario.
- Politica de catalogo cerrado de categorias.

## 3. Regla de uso

Este documento queda como referencia unica de evolutivos agrupados.
