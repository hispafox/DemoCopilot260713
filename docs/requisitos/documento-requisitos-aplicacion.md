# Documento de Requisitos de la Aplicacion

## 1. Informacion general

- Proyecto: DemoCopilot260713
- Fecha: 2026-07-13
- Version: 1.0 (borrador inicial)
- Estado: Base para refinamiento y generacion de backlog

## 2. Objetivo del documento

Definir de forma clara y verificable los requisitos de la aplicacion de gestion de tareas, para facilitar:

- Generacion de issues en el repositorio.
- Redaccion de historias de usuario.
- Priorizacion por iteraciones.
- Definicion de criterios de aceptacion y pruebas.

## 3. Vision del producto

La aplicacion permite gestionar tareas personales de forma simple y mantenible, con:

- Backend: ASP.NET Core 10 Web API.
- Frontend: React + Vite + TypeScript.
- Persistencia: SQLite + Entity Framework Core.

## 4. Alcance

### 4.1 Incluido en el alcance inicial

- Alta, consulta, actualizacion y eliminacion de tareas.
- Marcado explicito de tarea completada.
- Validacion de entrada en backend.
- Uso de DTOs y separacion de capas.
- Manejo de fechas en UTC (ISO 8601).

### 4.2 Fuera de alcance (por ahora)

- Multiusuario y roles.
- Adjuntos en tareas.
- Integraciones externas (correo, calendarios, mensajeria).
- Version movil nativa.

## 5. Actores y perfiles

- Usuario final: crea y gestiona sus tareas.
- Equipo tecnico: implementa, prueba y mantiene la aplicacion.
- Product Owner / Stakeholder: prioriza backlog y valida entregas.

## 6. Historias de usuario iniciales

### HU-01 - Crear tarea

Como usuario, quiero crear una tarea con titulo y descripcion opcional para registrar trabajo pendiente.

Criterios de aceptacion:

- Se permite crear tarea con titulo no vacio.
- Si el titulo esta vacio, se devuelve error de validacion.
- La tarea se guarda con EstaCompletada = false.
- CreadoEnUtc y ActualizadoEnUtc se asignan en UTC.

### HU-02 - Listar tareas

Como usuario, quiero ver la lista de tareas para conocer mi estado de trabajo.

Criterios de aceptacion:

- Se devuelven todas las tareas existentes.
- La respuesta incluye Id, Titulo, Descripcion, EstaCompletada, CreadoEnUtc y ActualizadoEnUtc.
- La consulta de lectura no modifica datos.

### HU-03 - Consultar tarea por Id

Como usuario, quiero ver una tarea especifica para revisar su detalle.

Criterios de aceptacion:

- Si existe, devuelve la tarea con codigo 200.
- Si no existe, devuelve 404.

### HU-04 - Editar tarea

Como usuario, quiero actualizar titulo y descripcion de una tarea para mantener la informacion al dia.

Criterios de aceptacion:

- Se valida que el titulo no quede vacio.
- Si la tarea existe, se actualiza y cambia ActualizadoEnUtc.
- Si no existe, devuelve 404.

### HU-05 - Marcar tarea como completada

Como usuario, quiero marcar una tarea como completada para reflejar avance.

Criterios de aceptacion:

- Cambia EstaCompletada a true.
- Actualiza ActualizadoEnUtc en UTC.
- Si no existe la tarea, devuelve 404.

### HU-06 - Eliminar tarea

Como usuario, quiero eliminar tareas que ya no necesito para mantener la lista limpia.

Criterios de aceptacion:

- Si la tarea existe, se elimina correctamente.
- La API responde con 204 cuando la eliminacion es exitosa.
- Si no existe, devuelve 404.

## 7. Requisitos funcionales (RF)

- RF-01: El sistema debe permitir crear tareas.
- RF-02: El sistema debe permitir listar tareas.
- RF-03: El sistema debe permitir consultar tarea por identificador.
- RF-04: El sistema debe permitir actualizar tareas existentes.
- RF-05: El sistema debe permitir marcar una tarea como completada.
- RF-06: El sistema debe permitir eliminar tareas.
- RF-07: El backend debe validar entradas obligatorias (ej. Titulo).
- RF-08: El sistema debe exponer contratos API mediante DTOs, sin exponer entidades EF directamente.
- RF-09: El sistema debe registrar fechas de creacion y actualizacion en UTC.
- RF-10: El frontend debe consumir la API mediante una capa de servicios dedicada.

## 8. Requisitos no funcionales (RNF)

- RNF-01 (Arquitectura): Separacion clara de capas (API, negocio, datos, UI).
- RNF-02 (Mantenibilidad): Nomenclatura y codigo en castellano, consistente entre backend y frontend.
- RNF-03 (Calidad): Cada funcionalidad debe incluir pruebas (unitarias y/o integracion segun corresponda).
- RNF-04 (Rendimiento): Operaciones CRUD deben responder de forma fluida en entorno local para volumen pequeno y mediano de tareas.
- RNF-05 (Confiabilidad): Manejo semantico de codigos HTTP (200/201/204/400/404).
- RNF-06 (Seguridad basica): Validacion de entrada para evitar datos invalidos en persistencia.
- RNF-07 (Trazabilidad temporal): Fechas en UTC serializadas en formato ISO 8601.
- RNF-08 (Usabilidad): Interfaz simple, accesible y centrada en tareas principales.

## 9. Reglas de negocio

- RB-01: El titulo de tarea es obligatorio y no puede estar vacio.
- RB-02: EstaCompletada inicia en false al crear una tarea.
- RB-03: CreadoEnUtc se asigna solo en el alta.
- RB-04: ActualizadoEnUtc se actualiza en cada modificacion.
- RB-05: No se expone la entidad de persistencia directamente al cliente.

## 10. Contratos API esperados

- GET /api/tareas
- GET /api/tareas/{id}
- POST /api/tareas
- PUT /api/tareas/{id}
- PATCH /api/tareas/{id}/completar
- DELETE /api/tareas/{id}

## 11. Criterios de Definition of Done (DoD)

Una historia/issue se considera completa cuando:

- Cumple criterios de aceptacion.
- Incluye pruebas actualizadas y en verde.
- Respeta arquitectura por capas y convenciones de nomenclatura.
- Mantiene contratos API y validaciones requeridas.
- Actualiza documentacion si hubo cambios funcionales.

## 12. Backlog inicial sugerido (epics e issues)

### Epic 1 - Backend API de tareas

- Issue 1.1: Crear entidad de dominio y DTOs de tarea.
- Issue 1.2: Implementar servicio de aplicacion para CRUD de tareas.
- Issue 1.3: Implementar endpoints REST de tareas.
- Issue 1.4: Agregar validaciones de entrada.
- Issue 1.5: Agregar pruebas unitarias e integracion de endpoints criticos.

### Epic 2 - Frontend de gestion de tareas

- Issue 2.1: Definir tipos y cliente API.
- Issue 2.2: Implementar pantalla de listado.
- Issue 2.3: Implementar alta y edicion de tareas.
- Issue 2.4: Implementar completar y eliminar tarea.
- Issue 2.5: Agregar pruebas de componentes y comportamiento basico.

### Epic 3 - Calidad y estandarizacion

- Issue 3.1: Configurar manejo centralizado de errores backend.
- Issue 3.2: Revisar cumplimiento UTC/ISO 8601 en contratos.
- Issue 3.3: Revisar convenciones de idioma y nomenclatura en todo el proyecto.

## 13. Etiquetas para gestion de issues

Antes de crear issues, se define el siguiente esquema de etiquetas para clasificacion y trazabilidad.

### 13.1 Etiquetas por tipo de trabajo

- tipo:historia-usuario (Historias de usuario funcionales)
- tipo:requisito-funcional (Requisitos funcionales y su implementacion)
- tipo:requisito-no-funcional (Rendimiento, seguridad, mantenibilidad, etc.)
- tipo:tarea-tecnica (Trabajo tecnico interno)
- tipo:bug (Defectos de comportamiento)
- tipo:documentacion (Cambios de documentacion)
- tipo:testing (Pruebas unitarias, integracion o frontend)

### 13.2 Etiquetas por capa

- capa:backend
- capa:frontend
- capa:datos
- capa:api
- capa:infra

### 13.3 Etiquetas por prioridad

- prioridad:alta
- prioridad:media
- prioridad:baja

### 13.4 Etiquetas por estado de flujo

- estado:triage
- estado:listo
- estado:en-progreso
- estado:bloqueado
- estado:en-review
- estado:qa
- estado:cerrado

### 13.5 Etiquetas por tamanio estimado

- tamanio:xs
- tamanio:s
- tamanio:m
- tamanio:l
- tamanio:xl

### 13.6 Etiquetas por epic

- epic:backend-api-tareas
- epic:frontend-gestion-tareas
- epic:calidad-estandarizacion

### 13.7 Reglas de uso de etiquetas

- Cada issue debe tener al menos: 1 etiqueta de tipo, 1 de capa y 1 de prioridad.
- Los issues asociados a una iniciativa mayor deben incluir etiqueta epic.
- Si el issue esta bloqueado, debe llevar estado:bloqueado y detallar dependencia en la descripcion.
- Los bugs productivos deben incluir tipo:bug y prioridad explicita.
- Las historias de usuario deben incluir referencia a HU correspondiente (ejemplo: HU-01).

## 14. Matriz de trazabilidad inicial

- HU-01 -> RF-01, RF-07, RF-09, RB-01, RB-02, RB-03
- HU-02 -> RF-02, RF-08
- HU-03 -> RF-03, RNF-05
- HU-04 -> RF-04, RF-07, RF-09, RB-01, RB-04
- HU-05 -> RF-05, RF-09, RB-04
- HU-06 -> RF-06, RNF-05

## 15. Riesgos y supuestos

Riesgos:

- Cambios de alcance durante implementacion sin actualizar backlog.
- Deriva de arquitectura si se mezcla logica de negocio en controladores/UI.
- Cobertura de pruebas insuficiente en entregas rapidas.

Supuestos:

- La primera version trabaja con un solo contexto de usuario (sin autenticacion).
- El volumen inicial de datos sera moderado.
- La prioridad inicial es estabilidad funcional y claridad del codigo.

## 16. Proximo paso recomendado

Usar este documento como fuente para crear:

- Historias de usuario en el gestor de trabajo.
- Issues tecnicos por epic.
- Checklist de pruebas por historia.

## 17. Plan de milestones y sprints

Se define una planificacion inicial de 3 sprints (2 semanas cada uno) con un milestone por sprint.

### 17.1 Cadencia propuesta

- Duracion de sprint: 2 semanas.
- Frecuencia de planning/review: 1 vez por sprint.
- Cierre de milestone: al finalizar sprint con DoD cumplida.

### 17.2 Milestones

| Milestone | Sprint | Objetivo principal | Alcance sugerido |
|---|---|---|---|
| M1 - Fundaciones Backend | Sprint 1 | Tener API base de tareas operativa y validada | Issue 1.1, 1.2, 1.3, 1.4 |
| M2 - Frontend Funcional | Sprint 2 | Tener flujo completo de gestion de tareas desde UI | Issue 2.1, 2.2, 2.3, 2.4 |
| M3 - Calidad y Consolidacion | Sprint 3 | Endurecer calidad, pruebas y estandares finales | Issue 1.5, 2.5, 3.1, 3.2, 3.3 |

### 17.3 Objetivos y criterios de cierre por sprint

Sprint 1 (M1):

- Objetivo: exponer CRUD + completar tarea en API con validaciones minimas.
- Entregables: endpoints listos, DTOs definidos, reglas de negocio aplicadas.
- Cierre: pruebas de backend criticas en verde y codigos HTTP semanticos funcionando.

Sprint 2 (M2):

- Objetivo: habilitar gestion de tareas desde frontend consumiendo API.
- Entregables: listado, alta, edicion, completar y eliminar en UI.
- Cierre: flujo punta a punta estable en entorno local.

Sprint 3 (M3):

- Objetivo: cerrar brechas de calidad y estandarizacion.
- Entregables: suite de pruebas reforzada, errores centralizados, revision de UTC/idioma.
- Cierre: DoD completa para backlog inicial y documentacion actualizada.

### 17.4 Regla de asignacion de issues a milestone

- Cada issue debe pertenecer a un unico milestone.
- Los issues bloqueados mantienen su milestone y se marcan con estado:bloqueado.
- Si un issue no termina en su sprint, se mueve al milestone siguiente con justificacion.

### 17.5 Fechas de referencia (ajustables)

Tomando como inicio de planificacion el 2026-07-13:

- Sprint 1 (M1): 2026-07-13 al 2026-07-26
- Sprint 2 (M2): 2026-07-27 al 2026-08-09
- Sprint 3 (M3): 2026-08-10 al 2026-08-23
