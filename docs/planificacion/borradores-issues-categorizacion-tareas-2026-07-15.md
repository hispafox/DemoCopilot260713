# Borradores de issues: categorizacion de tareas

## Issue 1

**Titulo:** [HU] incorporar categoria en dominio y persistencia

**Etiquetas sugeridas:**
- 🔴 prioridad:alta
- 📦 flujo:planificado
- tipo:historia-usuario
- enhancement

**Cuerpo:**

```markdown
## Historia de usuario

Como usuario de la aplicacion, quiero guardar una categoria en cada tarea, para identificar rapidamente el tipo de trabajo.

## Criterios de aceptacion

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

## Capas afectadas

- [x] Domain
- [x] Application
- [x] Infrastructure
- [ ] Api
- [ ] Frontend
- [x] Tests

## Informacion adicional

- **Prioridad:** Must
- **Tamano estimado:** M
- **Dependencias:** ninguna
- **Documento de planificacion:** docs/planificacion-categorias-tareas-2026-07-15.md
```

## Issue 2

**Titulo:** [HU] exponer y validar categoria en contratos api

**Etiquetas sugeridas:**
- 🔴 prioridad:alta
- 📦 flujo:planificado
- tipo:historia-usuario
- enhancement

**Cuerpo:**

```markdown
## Historia de usuario

Como consumidor de la API, quiero enviar y recibir la categoria en los DTOs de tareas, para integrar correctamente la clasificacion en mis flujos.

## Criterios de aceptacion

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

## Capas afectadas

- [ ] Domain
- [x] Application
- [ ] Infrastructure
- [x] Api
- [ ] Frontend
- [x] Tests

## Informacion adicional

- **Prioridad:** Must
- **Tamano estimado:** M
- **Dependencias:** HU-1
- **Documento de planificacion:** docs/planificacion-categorias-tareas-2026-07-15.md
```

## Issue 3

**Titulo:** [HU] permitir seleccionar y visualizar categoria en frontend

**Etiquetas sugeridas:**
- 🟡 prioridad:media
- 📦 flujo:planificado
- tipo:historia-usuario
- enhancement

**Cuerpo:**

```markdown
## Historia de usuario

Como usuario final, quiero seleccionar la categoria al crear o editar una tarea y verla en los listados, para organizar mi trabajo de forma visual.

## Criterios de aceptacion

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

## Capas afectadas

- [ ] Domain
- [ ] Application
- [ ] Infrastructure
- [ ] Api
- [x] Frontend
- [x] Tests

## Informacion adicional

- **Prioridad:** Should
- **Tamano estimado:** M
- **Dependencias:** HU-2
- **Documento de planificacion:** docs/planificacion-categorias-tareas-2026-07-15.md
```
