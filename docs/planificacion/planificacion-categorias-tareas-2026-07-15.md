# Planificacion: categorizacion de tareas

**Fecha:** 2026-07-15
**Estado:** borrador
**Autor:** planificador-democopilot

## 1. Resumen ejecutivo

Se planifica incorporar la capacidad de categorizar tareas para poder identificar de que tipo es cada una y mejorar su organizacion funcional en backend y frontend. El alcance contempla modelo de datos, contratos API, validaciones y experiencia de uso en la interfaz.

## 2. Contexto y trazabilidad

Esta planificacion se alinea con el objetivo de evolucionar la aplicacion de tareas manteniendo separacion por capas (Domain, Application, Infrastructure, Api, Frontend, Tests) y contratos estables mediante DTOs.

Trazabilidad minima requisito -> capas afectadas:

- Requisito: especificar de que tipo es cada tarea (categorizacion).
  - Capas: Domain, Application, Infrastructure, Api, Frontend, Tests.

Supuestos explicitos:

- Se agrega un campo Categoria en la tarea como texto corto gestionado por el usuario.
- Categoria sera obligatoria al crear y actualizar una tarea.
- No se implementa en esta iteracion un catalogo administrable de categorias ni jerarquias.
- Se mantiene compatibilidad de datos con migracion incremental en SQLite.
- Fechas se mantienen en UTC con serializacion ISO 8601.

## 3. Historias de usuario

### HU-1: incorporar categoria en dominio y persistencia

- **Historia:** Como usuario de la aplicacion, quiero guardar una categoria en cada tarea, para identificar rapidamente el tipo de trabajo.
- **Prioridad:** Must
- **Tamano:** M
- **Dependencias:** ninguna
- **Supuestos:** categoria como texto corto obligatorio

#### Criterios de aceptacion

```gherkin
Escenario: crear tarea con categoria valida
  Dado que el usuario informa un titulo valido y una categoria valida
  Cuando registra la tarea
  Entonces la tarea queda persistida con la categoria informada

Escenario: rechazo por categoria vacia en creacion
  Dado que el usuario informa una categoria vacia o solo espacios
  Cuando intenta registrar la tarea
  Entonces el sistema responde con error de validacion

Escenario: migracion incremental sin perdida de datos
  Dado una base con tareas existentes previas al campo categoria
  Cuando se aplica la migracion
  Entonces el esquema queda actualizado sin eliminar datos existentes
```

#### Capas afectadas

- [x] Domain
- [x] Application
- [x] Infrastructure
- [ ] Api
- [ ] Frontend
- [x] Tests

### HU-2: exponer y validar categoria en contratos API

- **Historia:** Como consumidor de la API, quiero enviar y recibir la categoria en los DTOs de tareas, para integrar correctamente la clasificacion en mis flujos.
- **Prioridad:** Must
- **Tamano:** M
- **Dependencias:** HU-1
- **Supuestos:** el contrato mantiene endpoints actuales y agrega categoria en DTOs

#### Criterios de aceptacion

```gherkin
Escenario: API retorna categoria en consultas
  Dado que existe una tarea con categoria registrada
  Cuando se consulta el listado o detalle de tareas
  Entonces la respuesta incluye la categoria en el DTO de salida

Escenario: API rechaza crear o actualizar sin categoria valida
  Dado una solicitud de crear o actualizar tarea sin categoria valida
  Cuando el backend procesa la solicitud
  Entonces responde 400 con detalle de validacion

Escenario: actualizacion de categoria en tarea existente
  Dado una tarea existente con categoria inicial
  Cuando se actualiza la tarea con otra categoria valida
  Entonces la categoria queda actualizada y ActualizadoEnUtc cambia en UTC
```

#### Capas afectadas

- [ ] Domain
- [x] Application
- [ ] Infrastructure
- [x] Api
- [ ] Frontend
- [x] Tests

### HU-3: permitir seleccionar y visualizar categoria en frontend

- **Historia:** Como usuario final, quiero seleccionar la categoria al crear o editar una tarea y verla en los listados, para organizar mi trabajo de forma visual.
- **Prioridad:** Should
- **Tamano:** M
- **Dependencias:** HU-2
- **Supuestos:** categoria se captura como campo de texto en formulario en esta fase

#### Criterios de aceptacion

```gherkin
Escenario: alta de tarea desde UI con categoria
  Dado que el usuario completa titulo y categoria en el formulario
  Cuando guarda la tarea
  Entonces la tarea se crea y la categoria se muestra en pantalla

Escenario: validacion visual por categoria faltante
  Dado que el usuario intenta guardar sin categoria
  Cuando envia el formulario
  Entonces la UI muestra mensaje de validacion y no envia la solicitud valida

Escenario: visualizacion consistente de categoria en listado y detalle
  Dado que existen tareas con categoria
  Cuando el usuario navega por listado y detalle
  Entonces la categoria se visualiza de manera consistente en ambas vistas
```

#### Capas afectadas

- [ ] Domain
- [ ] Application
- [ ] Infrastructure
- [ ] Api
- [x] Frontend
- [x] Tests

## 4. Riesgos y vacios

- Falta confirmar si categoria debe ser texto libre o seleccion de un catalogo cerrado.
- Falta confirmar longitud maxima permitida para categoria.
- Falta confirmar estrategia para datos existentes sin categoria tras migracion (valor por defecto, nullable temporal o backfill).
- Riesgo de ruptura de contratos en frontend si no se actualizan tipos y servicios de API de forma atomica.

## 5. Propuesta de orden de implementacion

1. HU-1: base de dominio y persistencia con migracion incremental para habilitar almacenamiento de categoria sin deuda tecnica de datos.
2. HU-2: contratos y validaciones API para estabilizar integraciones y reglas de negocio.
3. HU-3: cambios de interfaz y pruebas de comportamiento para cerrar el flujo de usuario extremo a extremo.
