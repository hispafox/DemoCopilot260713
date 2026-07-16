# Informe de auditoria de requisitos 360

Fecha: 2026-07-15
Proyecto: DemoCopilot260713
Documento base: docs/documento-requisitos-aplicacion.md
Repositorio GitHub auditado: hispafox/DemoCopilot260713

## 1. Resumen ejecutivo

Tipo de auditoria: completa (HU, RF, RNF, RB y contratos API)

Fuentes cruzadas:

1. Documento de requisitos.
2. Codigo backend, frontend y pruebas.
3. Issues de GitHub (24 issues totales, 24 abiertos, 0 cerrados).

Cobertura global estimada por requisito:

- Verde (completo): 28
- Amarillo (parcial): 6
- Rojo (nulo): 0
- Gris (no verificable): 1

Cobertura estricta (solo verdes): 80.0% (28/35)

Lectura ejecutiva:

- El nucleo CRUD de tareas esta implementado en backend y consumido desde frontend.
- Existen pruebas en dominio, servicio, API y frontend, pero con huecos relevantes en escenarios de actualizacion (PUT) y algunos 404.
- La trazabilidad con issues existe por nombre y taxonomia, pero el estado operativo de GitHub no refleja ejecucion real: todo esta en abierto, incluso trabajo aparentemente implementado en codigo.

## 2. Hallazgos por severidad

### Critico

1. Desalineacion de gestion: 24/24 issues en estado OPEN, incluyendo items del backlog inicial con evidencia parcial/completa en codigo.
   - Riesgo: perdida de trazabilidad de avance real y cierre de sprint/milestone no confiable.

### Alto

1. HU-04/RF-04 (editar tarea) con cobertura parcial: backend implementado pero sin flujo de edicion en UI.
   - Evidencia codigo: src/backend/Api/Controladores/TareasControlador.cs, src/backend/Application/Servicios/TareaServicio.cs.
   - Evidencia gap UI: src/frontend/src/App.tsx (sin accion de editar).

2. RNF-03 (calidad de pruebas) parcial en endpoints criticos de actualizacion.
   - Evidencia: src/backend/Tests/Api/TareasEndpointsTests.cs (sin caso explicito PUT 200 y PUT 404).

### Medio

1. RNF-07 (UTC/ISO) parcial en capa de presentacion.
   - Backend y contratos usan campos Utc.
   - La UI renderiza con toLocaleString sin etiqueta explicita de zona horaria.
   - Evidencia: src/frontend/src/App.tsx.

2. HU-03 (consulta por id) parcial desde experiencia de usuario final.
   - Endpoint existe en API.
   - No hay vista/flujo explicito de consulta por id en UI.

3. RNF-08 (usabilidad/accesibilidad) parcial.
   - Interfaz simple y usable, pero sin evidencia de pruebas de accesibilidad mas alla de pruebas basicas de render/comportamiento.

### Bajo

1. RNF-04 (rendimiento local) no verificable con evidencia automatizada en este corte.
   - No se observan pruebas o metricas de rendimiento documentadas.

## 3. Matriz de trazabilidad 360

Leyenda estados:

- Codigo: Verde/Amarillo/Rojo/Gris
- Issues: Verde = hay issue directo y alineado, Amarillo = issue indirecto/parcial, Rojo = sin issue

| Requisito | Estado codigo | Estado issues | Evidencia codigo | Evidencia issue | Riesgo | Accion recomendada |
|---|---|---|---|---|---|---|
| HU-01 Crear tarea | Verde | Verde | src/backend/Api/Controladores/TareasControlador.cs, src/frontend/src/App.tsx | #1, #7, #8, #9 | Bajo | Mantener pruebas y cerrar issue cuando corresponda |
| HU-02 Listar tareas | Verde | Verde | src/backend/Api/Controladores/TareasControlador.cs, src/frontend/src/App.tsx | #2, #13 | Bajo | Cerrar issue tras validacion funcional |
| HU-03 Consultar por id | Amarillo | Verde | src/backend/Api/Controladores/TareasControlador.cs | #3 | Medio | Agregar flujo UI de consulta detalle o justificar alcance API-only |
| HU-04 Editar tarea | Amarillo | Verde | src/backend/Api/Controladores/TareasControlador.cs, src/backend/Application/Servicios/TareaServicio.cs | #4, #14 | Alto | Implementar edicion en frontend y pruebas de flujo |
| HU-05 Completar tarea | Verde | Verde | src/backend/Api/Controladores/TareasControlador.cs, src/frontend/src/App.tsx | #5, #15 | Bajo | Cerrar issue con evidencia de test |
| HU-06 Eliminar tarea | Verde | Verde | src/backend/Api/Controladores/TareasControlador.cs, src/frontend/src/App.tsx | #6, #15 | Bajo | Cerrar issue con evidencia de test |
| RF-01 Crear tareas | Verde | Verde | src/backend/Application/Servicios/TareaServicio.cs | #1, #7 | Bajo | Sin accion critica |
| RF-02 Listar tareas | Verde | Verde | src/backend/Infrastructure/Repositorios/TareaRepositorio.cs | #2, #13 | Bajo | Sin accion critica |
| RF-03 Consultar por id | Verde | Verde | src/backend/Api/Controladores/TareasControlador.cs | #3 | Bajo | Sin accion critica |
| RF-04 Actualizar tareas | Amarillo | Verde | src/backend/Api/Controladores/TareasControlador.cs, src/backend/Application/Servicios/TareaServicio.cs | #4, #14 | Alto | Completar frontend y pruebas e2e basicas |
| RF-05 Completar tarea | Verde | Verde | src/backend/Application/Servicios/TareaServicio.cs | #5 | Bajo | Sin accion critica |
| RF-06 Eliminar tarea | Verde | Verde | src/backend/Application/Servicios/TareaServicio.cs | #6 | Bajo | Sin accion critica |
| RF-07 Validar entradas | Verde | Verde | src/backend/Application/DTOs/CrearTareaDto.cs, src/backend/Domain/Tarea.cs | #10 | Bajo | Ampliar pruebas de validacion en API |
| RF-08 DTOs sin exponer EF | Verde | Verde | src/backend/Application/DTOs/TareaDto.cs, src/backend/Application/Mapeadores/TareaMapeador.cs | #7, #9 | Bajo | Sin accion critica |
| RF-09 Fechas UTC | Verde | Verde | src/backend/Domain/Tarea.cs, src/frontend/src/App.tsx | #18 | Medio | Etiquetar visualmente UTC/local en UI |
| RF-10 Frontend con capa de servicios | Verde | Verde | src/frontend/src/services/tareasServicio.ts, src/frontend/src/services/clienteHttp.ts | #12 | Bajo | Sin accion critica |
| RNF-01 Arquitectura por capas | Verde | Verde | src/backend/Api, src/backend/Application, src/backend/Domain, src/backend/Infrastructure | #7, #8, #9 | Bajo | Sin accion critica |
| RNF-02 Nomenclatura en castellano | Verde | Verde | Clases y DTOs en castellano en backend/frontend | #19 | Bajo | Corregir solo nuevos hallazgos puntuales |
| RNF-03 Pruebas por funcionalidad | Amarillo | Verde | src/backend/Tests, src/frontend/src/tests | #11, #16 | Medio | Agregar PUT 200/404 y casos negativos adicionales |
| RNF-04 Rendimiento local | Gris | Amarillo | Sin pruebas especificas de performance | Sin issue dedicado explicito | Medio | Crear issue de rendimiento con criterio medible |
| RNF-05 Codigos HTTP semanticos | Verde | Verde | src/backend/Api/Controladores/TareasControlador.cs, src/backend/Api/Middleware/ManejadorExcepcionesMiddleware.cs | #9, #17 | Bajo | Agregar test explicito de 404 en PUT |
| RNF-06 Validacion de entrada | Verde | Verde | src/backend/Application/DTOs/ActualizarTareaDto.cs, src/backend/Domain/Tarea.cs | #10 | Bajo | Sin accion critica |
| RNF-07 UTC + ISO 8601 | Amarillo | Verde | src/backend/Domain/Tarea.cs, serializacion JSON en API, conversion toISOString en UI | #18 | Medio | Mostrar etiqueta de zona horaria y reforzar prueba de formato |
| RNF-08 UI simple/accesible | Amarillo | Amarillo | src/frontend/src/App.tsx, src/frontend/src/tests/App.test.tsx | Sin issue de accesibilidad explicito | Medio | Crear issue de accesibilidad y test minimo de navegacion/aria |
| RB-01 Titulo obligatorio | Verde | Verde | src/backend/Domain/Tarea.cs | #1, #10 | Bajo | Sin accion critica |
| RB-02 EstaCompletada inicia false | Verde | Verde | src/backend/Domain/Tarea.cs | #1 | Bajo | Sin accion critica |
| RB-03 CreadoEnUtc solo en alta | Verde | Amarillo | src/backend/Domain/Tarea.cs | Cobertura indirecta en #7/#8 | Bajo | Referenciar RB en issue tecnico de dominio |
| RB-04 ActualizadoEnUtc en cada modificacion | Verde | Amarillo | src/backend/Domain/Tarea.cs | Cobertura indirecta en #8/#9/#15 | Bajo | Referenciar RB en issue tecnico de actualizacion/completar |
| RB-05 No exponer persistencia directa | Verde | Verde | DTOs + mapeadores en Application | #7, #9 | Bajo | Sin accion critica |
| API GET /api/tareas | Verde | Verde | src/backend/Api/Controladores/TareasControlador.cs | #2, #9 | Bajo | Sin accion critica |
| API GET /api/tareas/{id} | Verde | Verde | src/backend/Api/Controladores/TareasControlador.cs | #3, #9 | Bajo | Sin accion critica |
| API POST /api/tareas | Verde | Verde | src/backend/Api/Controladores/TareasControlador.cs | #1, #9 | Bajo | Sin accion critica |
| API PUT /api/tareas/{id} | Verde | Verde | src/backend/Api/Controladores/TareasControlador.cs | #4, #9 | Medio | Agregar test API explicito para 404 |
| API PATCH /api/tareas/{id}/completar | Verde | Verde | src/backend/Api/Controladores/TareasControlador.cs | #5, #9 | Bajo | Sin accion critica |
| API DELETE /api/tareas/{id} | Verde | Verde | src/backend/Api/Controladores/TareasControlador.cs | #6, #9 | Bajo | Sin accion critica |

## 4. Desalineaciones detectadas

1. Issues sin cierre operativo:
   - Todo el backlog trazado esta OPEN, aunque hay evidencia de implementacion parcial o completa en codigo.

2. Requisito parcialmente implementado en experiencia final:
   - HU-04 / RF-04 (editar) no esta materializado en UI.

3. Requisitos no funcionales sin evidencia dura:
   - RNF-04 sin metrica o prueba de rendimiento.
   - RNF-08 sin issue dedicado ni pruebas de accesibilidad concretas.

4. Issues fuera del alcance del documento base 1.0:
   - #20 a #24 (usuarios, asignacion y consultas por usuario) amplian alcance respecto al documento inicial.

## 5. Plan de accion priorizado

1. Actualizar trazabilidad operativa en GitHub.
   - Tipo: issue/proceso
   - Prioridad: alta
   - Resultado esperado: tablero consistente con avance real (cerrar o mover estado según evidencia).

2. Completar HU-04/RF-04 en frontend.
   - Tipo: codigo + test
   - Prioridad: alta
   - Resultado esperado: flujo de edicion en UI con prueba de componente y validacion basica.

3. Ampliar pruebas de API para PUT y escenarios 404.
   - Tipo: test
   - Prioridad: media
   - Resultado esperado: cobertura de contratos criticos de actualizacion.

4. Formalizar criterio RNF-04 con metrica objetiva.
   - Tipo: doc + issue + test no funcional
   - Prioridad: media
   - Resultado esperado: umbral de rendimiento local y evidencia repetible.

5. Cerrar brecha RNF-07/RNF-08 en frontend.
   - Tipo: codigo + test + issue
   - Prioridad: media
   - Resultado esperado: fecha mostrada con contexto de zona horaria y minima validacion de accesibilidad.

## 6. Conclusion

El proyecto presenta una base funcional y arquitectonica solida para el alcance inicial de tareas, con buen cumplimiento en backend y capa de servicios frontend. La principal brecha de auditoria 360 no es la falta total de implementacion, sino la diferencia entre lo que ya existe en codigo y lo que GitHub refleja como estado operativo, junto con huecos puntuales en frontend (edicion) y pruebas de contrato no funcional/actualizacion.
