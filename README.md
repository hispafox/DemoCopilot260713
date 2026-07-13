# DemoCopilot260713

Aplicacion de lista de tareas con arquitectura separada por capas:

- Backend: ASP.NET Core 10 Web API
- Frontend: React + Vite + TypeScript
- Datos: SQLite + Entity Framework Core

## Objetivo

Construir una app simple y mantenible para gestionar tareas, con una API clara, un modelo de datos explicito y una interfaz web ligera.

## Alcance funcional

- Crear, consultar, editar y eliminar tareas.
- Marcar tareas como completadas.
- Validar los datos de entrada en el backend.
- Mantener los contratos API separados de las entidades EF.

## Modelo de tarea

La entidad de dominio prevista incluye estos campos minimos:

- Id
- Titulo
- Descripcion
- EstaCompletada
- CreadoEnUtc
- ActualizadoEnUtc

## Convenciones del proyecto

- Todo el codigo, nombres y comentarios debe escribirse en castellano.
- Las fechas se manejan siempre en UTC y se serializan en ISO 8601.
- No exponer entidades EF directamente al frontend.
- Separar responsabilidades entre modelo, logica de negocio, acceso a datos y UI.
- Usar DTOs para crear, actualizar y consultar tareas.

## API prevista

- GET /api/tareas
- GET /api/tareas/{id}
- POST /api/tareas
- PUT /api/tareas/{id}
- PATCH /api/tareas/{id}/completar
- DELETE /api/tareas/{id}

## Estructura esperada

- src/backend/
	- Api/
	- Application/
	- Domain/
	- Infrastructure/
	- Tests/
- src/frontend/
	- src/components/
	- src/pages/
	- src/services/
	- src/types/
	- src/tests/

## Estado actual

Este repositorio esta en fase inicial y el README funciona como guia base para la solucion que se va a construir.
