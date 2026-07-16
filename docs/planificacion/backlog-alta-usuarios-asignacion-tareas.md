# Backlog Scrum - Alta de usuarios y asignacion de tareas

Fecha: 2026-07-15

## 1. Resumen de contexto y alcance

- Objetivo de negocio: lograr trazabilidad de tareas por usuario para conocer cuantas tareas tiene cada persona.
- Contexto operativo: en la aplicacion de tareas, el administrador da de alta usuarios y gestiona la asignacion de tareas.
- Restricciones aplicables:
  - Una tarea debe tener un usuario asignado.
  - El usuario debe tener nombre en el alta.
- Supuestos explicitos:
  - Supuesto 1: la asignacion es de una tarea a un unico usuario en esta fase.
  - Supuesto 2: el conteo requerido minimo es cantidad total de tareas por usuario.
  - Supuesto 3: la reasignacion de tareas esta permitida por negocio.

## 2. Backlog jerarquico

- Epica E1: Trazabilidad de responsabilidades en tareas.

- Feature F1.1: Alta de usuarios administrada con datos minimos validos.
  - HU1: Como administrador, quiero dar de alta un usuario con nombre obligatorio, para poder asignarle tareas.

- Feature F1.2: Asignacion obligatoria de usuario en tareas.
  - HU2: Como administrador, quiero crear tareas solo con usuario asignado, para garantizar que ninguna tarea quede sin responsable.
  - HU3: Como administrador, quiero reasignar una tarea de un usuario a otro, para mantener la trazabilidad cuando cambia la responsabilidad.

- Feature F1.3: Visibilidad de carga de trabajo por usuario.
  - HU4: Como administrador, quiero ver cuantas tareas tiene un usuario, para monitorear carga y distribucion de trabajo.
  - HU5: Como administrador, quiero listar las tareas de un usuario especifico, para auditar su carga de trabajo concreta.

## 3. Historias detalladas

### HU1 - Alta de usuario por administrador
- Valor de negocio: habilita la entidad usuaria para asignaciones y evita tareas sin responsable identificable.
- Prioridad MoSCoW: Must.
- Estimacion sugerida: S.
- Dependencias: modelo de usuario y validaciones de entrada.
- Supuestos: no se requiere otro campo obligatorio ademas de nombre en esta fase.

### HU2 - Creacion de tarea con usuario obligatorio
- Valor de negocio: garantiza cumplimiento de la regla clave de negocio.
- Prioridad MoSCoW: Must.
- Estimacion sugerida: M.
- Dependencias: HU1.
- Supuestos: el usuario asignado debe existir.

### HU3 - Reasignacion de tarea
- Valor de negocio: mantiene exactitud operativa cuando cambia el responsable.
- Prioridad MoSCoW: Should.
- Estimacion sugerida: M.
- Dependencias: HU2.
- Supuestos: se registra el responsable actual como fuente de verdad.

### HU4 - Conteo de tareas por usuario
- Valor de negocio: visibilidad inmediata para decision operativa.
- Prioridad MoSCoW: Must.
- Estimacion sugerida: S.
- Dependencias: HU2.
- Supuestos: conteo total sin desglose por estado en primera iteracion.

### HU5 - Listado de tareas por usuario
- Valor de negocio: trazabilidad accionable del detalle de carga.
- Prioridad MoSCoW: Should.
- Estimacion sugerida: M.
- Dependencias: HU2 y HU4.
- Supuestos: incluye tareas activas y completadas en primera iteracion.

## 4. Criterios de aceptacion en Gherkin

### HU1
Escenario: caso feliz
Dado que soy administrador autenticado
Cuando registro un usuario con nombre valido
Entonces el sistema crea el usuario y confirma el alta

Escenario: alterno
Dado que soy administrador autenticado
Cuando intento crear un usuario con nombre invalido
Entonces el sistema rechaza el alta e informa validacion

Escenario: borde
Dado que soy administrador autenticado
Cuando ingreso un nombre con longitud maxima permitida
Entonces el sistema acepta el alta sin truncar informacion

Escenario: validacion y error
Dado que soy administrador autenticado
Cuando intento crear un usuario sin nombre o con solo espacios
Entonces el sistema responde error de validacion

### HU2
Escenario: caso feliz
Dado que existe un usuario valido
Cuando creo una tarea con titulo y usuario asignado
Entonces la tarea se guarda con el usuario asociado

Escenario: alterno
Dado que existe un usuario valido
Cuando creo una tarea correctamente
Entonces la respuesta incluye el identificador del usuario asignado

Escenario: borde
Dado que el usuario asignado existe
Cuando creo una tarea con datos al limite permitido
Entonces la tarea se registra manteniendo la asignacion

Escenario: validacion y error
Dado que intento crear una tarea sin usuario asignado
Cuando envio la solicitud
Entonces el sistema rechaza la creacion con error de validacion

### HU3
Escenario: caso feliz
Dado que una tarea esta asignada al Usuario A
Cuando el administrador reasigna la tarea al Usuario B
Entonces la tarea queda asignada al Usuario B

Escenario: alterno
Dado que una tarea fue reasignada
Cuando se consulta la tarea
Entonces se visualiza al responsable actual correcto

Escenario: borde
Dado que se intenta reasignar al mismo usuario actual
Cuando se confirma la operacion
Entonces el sistema no aplica cambios

Escenario: validacion y error
Dado que el usuario destino no existe
Cuando intento reasignar la tarea
Entonces el sistema rechaza la operacion

### HU4
Escenario: caso feliz
Dado que un usuario tiene tareas asignadas
Cuando consulto su resumen de carga
Entonces el sistema devuelve la cantidad total de tareas

Escenario: alterno
Dado que un usuario existe y no tiene tareas
Cuando consulto su resumen de carga
Entonces el sistema devuelve cantidad cero

Escenario: borde
Dado que un usuario tiene un volumen alto de tareas
Cuando consulto el conteo
Entonces el sistema responde dentro del tiempo de servicio acordado

Escenario: validacion y error
Dado que el usuario no existe
Cuando consulto su conteo de tareas
Entonces el sistema responde no encontrado

### HU5
Escenario: caso feliz
Dado que un usuario tiene tareas asignadas
Cuando solicito el listado de tareas por usuario
Entonces el sistema devuelve unicamente sus tareas

Escenario: alterno
Dado que un usuario no tiene tareas asignadas
Cuando solicito el listado
Entonces el sistema devuelve una lista vacia

Escenario: borde
Dado que el usuario tiene muchas tareas
Cuando solicito una pagina especifica
Entonces el sistema devuelve la pagina solicitada sin mezclar tareas

Escenario: validacion y error
Dado que el identificador de usuario es invalido
Cuando solicito el listado
Entonces el sistema responde error de validacion

## 5. Checklist INVEST por historia

### HU1
- Independent: Cumple. Puede implementarse de forma aislada.
- Negotiable: Cumple. Campos opcionales pueden definirse despues.
- Valuable: Cumple. Aporta valor base para asignacion.
- Estimable: Cumple. Alcance acotado.
- Small: Cumple. Tamaño de sprint.
- Testable: Cumple. Casos de validacion claros.

### HU2
- Independent: Cumple. Incremento vertical claro.
- Negotiable: Cumple. Contrato ajustable sin perder objetivo.
- Valuable: Cumple. Regla principal de negocio.
- Estimable: Cumple. Alcance y validaciones definidos.
- Small: Cumple. Tamaño medio.
- Testable: Cumple. Escenarios verificables.

### HU3
- Independent: Cumple. Se entrega tras HU2 sin bloquear Must.
- Negotiable: Cumple. Se ajusta nivel de trazabilidad.
- Valuable: Cumple. Mantiene informacion vigente.
- Estimable: Cumple. Flujo concreto.
- Small: Cumple. Tamaño medio.
- Testable: Cumple. Reasignacion y errores medibles.

### HU4
- Independent: Cumple. Consulta agregada separada.
- Negotiable: Cumple. Se puede ampliar con desglose por estado luego.
- Valuable: Cumple. Responde objetivo principal.
- Estimable: Cumple. Consulta acotada.
- Small: Cumple. Tamaño corto.
- Testable: Cumple. 0, n y no encontrado.

### HU5
- Independent: Cumple. Complementa sin bloquear valor minimo.
- Negotiable: Cumple. Filtros y paginacion evolutivos.
- Valuable: Cumple. Habilita auditoria operativa.
- Estimable: Cumple. Alcance claro.
- Small: Cumple. Tamaño medio.
- Testable: Cumple. Filtrado y paginado verificables.

## 6. Definition of Ready por historia

- HU1: validaciones de nombre definidas, permisos de administrador confirmados, contrato de alta acordado.
- HU2: relacion tarea-usuario definida, regla de obligatoriedad aprobada, contrato de creacion actualizado.
- HU3: regla de reasignacion confirmada, politica minima de trazabilidad definida.
- HU4: definicion exacta de conteo validada, contrato de respuesta acordado.
- HU5: filtro por usuario y paginacion definidos, campos de respuesta acordados.

## 7. Definition of Done sugerida para sprint

- Criterios Gherkin cumplidos para historias comprometidas.
- Pruebas unitarias backend en verde.
- Pruebas de integracion de endpoints criticos en verde.
- Pruebas frontend de flujos basicos en verde.
- Revision por pares aprobada.
- Sin defectos criticos abiertos.
- Trazabilidad historia-prueba-criterio actualizada.

## 8. Riesgos y vacios

- Riesgo funcional: no se definio unicidad de usuario (solo nombre puede ser ambiguo).
- Riesgo funcional: no se definio estado activo o inactivo del usuario.
- Riesgo tecnico: posibles cambios de contrato frontend-backend si no se congela DTO.
- Vacio de informacion: confirmar si el conteo debe separar pendientes y completadas.

## 9. Propuesta de objetivo de sprint y alcance recomendado

- Objetivo de sprint: implementar trazabilidad base de asignacion obligatoria para conocer la carga por usuario.
- Alcance recomendado minimo: HU1, HU2, HU4.
- Alcance extendido segun capacidad: agregar HU3 y luego HU5.

## 10. Trazabilidad

- Objetivo de negocio: trazar tareas por usuario y conocer cuantas tareas tiene cada uno.
- Objetivo -> Epica E1 -> Feature F1.1 -> HU1 -> Criterios HU1.
- Objetivo -> Epica E1 -> Feature F1.2 -> HU2 y HU3 -> Criterios HU2 y HU3.
- Objetivo -> Epica E1 -> Feature F1.3 -> HU4 y HU5 -> Criterios HU4 y HU5.