# Tickets GitHub - Alta de usuarios y asignacion de tareas

Fecha: 2026-07-15

## Orden de creacion recomendado

1. E1-F1.1-HU1 - Alta de usuario por administrador
2. E1-F1.2-HU2 - Tarea con usuario obligatorio
3. E1-F1.3-HU4 - Conteo de tareas por usuario
4. E1-F1.2-HU3 - Reasignacion de tarea
5. E1-F1.3-HU5 - Listado de tareas por usuario

## Ticket 1

Titulo: feat(usuarios): alta de usuario por administrador con nombre obligatorio

Labels sugeridas:
- tipo:feature
- area:backend
- area:frontend
- prioridad:must

Descripcion:
Como administrador, quiero dar de alta un usuario con nombre obligatorio, para poder asignarle tareas.

Criterios de aceptacion:
- Dado que soy administrador autenticado, cuando registro un usuario con nombre valido, entonces el sistema crea el usuario.
- Dado que envio nombre vacio o con espacios, cuando intento crear el usuario, entonces el sistema responde error de validacion.
- Dado que ingreso un nombre en longitud maxima permitida, cuando envio el alta, entonces el sistema lo acepta correctamente.

Subtareas sugeridas:
- Backend: crear DTO de alta de usuario con validacion de nombre obligatorio.
- Backend: crear endpoint de alta de usuario por administrador.
- Backend: agregar pruebas unitarias y de integracion para validaciones y alta correcta.
- Frontend: formulario de alta de usuario con validacion de nombre.
- Frontend: pruebas de componente para validacion y envio exitoso.

Dependencias:
- Ninguna.

Estimacion sugerida:
- S

## Ticket 2

Titulo: feat(tareas): obligar usuario asignado al crear tarea

Labels sugeridas:
- tipo:feature
- area:backend
- area:frontend
- prioridad:must

Descripcion:
Como administrador, quiero crear tareas solo con usuario asignado, para garantizar que ninguna tarea quede sin responsable.

Criterios de aceptacion:
- Dado que existe un usuario valido, cuando creo una tarea con usuario asignado, entonces la tarea se guarda con esa asignacion.
- Dado que creo una tarea sin usuario asignado, cuando envio la solicitud, entonces el sistema responde error de validacion.
- Dado que la tarea se crea correctamente, cuando recibo la respuesta, entonces incluye el identificador de usuario asignado.

Subtareas sugeridas:
- Backend: actualizar entidad y DTOs de tarea para incluir usuarioAsignadoId obligatorio.
- Backend: actualizar endpoint de creacion de tarea con validacion de usuario existente.
- Backend: pruebas unitarias y de integracion para caso feliz y validaciones.
- Frontend: actualizar formulario de tarea para seleccionar usuario.
- Frontend: pruebas de comportamiento para obligar seleccion de usuario.

Dependencias:
- Ticket 1.

Estimacion sugerida:
- M

## Ticket 3

Titulo: feat(tareas): consultar cantidad de tareas por usuario

Labels sugeridas:
- tipo:feature
- area:backend
- area:frontend
- prioridad:must

Descripcion:
Como administrador, quiero ver cuantas tareas tiene un usuario, para monitorear carga y distribucion de trabajo.

Criterios de aceptacion:
- Dado que el usuario tiene tareas asignadas, cuando consulto su carga, entonces obtengo la cantidad total.
- Dado que el usuario existe y no tiene tareas, cuando consulto su carga, entonces obtengo cero.
- Dado que el usuario no existe, cuando consulto su carga, entonces obtengo no encontrado.

Subtareas sugeridas:
- Backend: crear endpoint de conteo por usuario.
- Backend: optimizar consulta con indice por usuario asignado si aplica.
- Backend: pruebas unitarias e integracion para 0, n y no encontrado.
- Frontend: mostrar indicador de cantidad de tareas por usuario.
- Frontend: pruebas para visualizacion de conteo.

Dependencias:
- Ticket 2.

Estimacion sugerida:
- S

## Ticket 4

Titulo: feat(tareas): reasignar tarea entre usuarios

Labels sugeridas:
- tipo:feature
- area:backend
- area:frontend
- prioridad:should

Descripcion:
Como administrador, quiero reasignar una tarea de un usuario a otro, para mantener la trazabilidad cuando cambia la responsabilidad.

Criterios de aceptacion:
- Dado que una tarea esta asignada al Usuario A, cuando la reasigno al Usuario B, entonces queda asignada al Usuario B.
- Dado que intento reasignar al mismo usuario actual, cuando confirmo, entonces el sistema no aplica cambios.
- Dado que el usuario destino no existe, cuando intento reasignar, entonces el sistema responde error.

Subtareas sugeridas:
- Backend: endpoint de reasignacion de tarea.
- Backend: validar existencia de usuario destino.
- Backend: pruebas de integracion para reasignacion y errores.
- Frontend: accion de reasignar desde detalle o lista de tarea.
- Frontend: pruebas de flujo de reasignacion.

Dependencias:
- Ticket 2.

Estimacion sugerida:
- M

## Ticket 5

Titulo: feat(tareas): listar tareas filtradas por usuario

Labels sugeridas:
- tipo:feature
- area:backend
- area:frontend
- prioridad:should

Descripcion:
Como administrador, quiero listar las tareas de un usuario especifico, para auditar su carga de trabajo concreta.

Criterios de aceptacion:
- Dado que el usuario tiene tareas, cuando solicito el listado, entonces obtengo unicamente sus tareas.
- Dado que el usuario no tiene tareas, cuando solicito el listado, entonces obtengo lista vacia.
- Dado que el identificador es invalido, cuando solicito el listado, entonces obtengo error de validacion.

Subtareas sugeridas:
- Backend: endpoint de listado por usuario con paginacion.
- Backend: pruebas de integracion para filtros, lista vacia y validacion.
- Frontend: vista o filtro por usuario para tareas.
- Frontend: pruebas de render de lista filtrada.

Dependencias:
- Ticket 2 y Ticket 3.

Estimacion sugerida:
- M

## Notas para carga en GitHub

- Crear primero los tickets Must (1, 2, 3).
- Crear despues los tickets Should (4, 5).
- En cada ticket, enlazar dependencias usando referencias tipo #numero una vez creados.
- Mantener el mismo prefijo de titulo para trazabilidad con backlog.