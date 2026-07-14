---
name: sk-10-documentacion-dod
description: "Estandariza la documentacion tecnica, los criterios de aceptacion y la Definition of Done del proyecto. Plantillas de decision tecnica, checklist de entrega y guia de evolucion."
---

# SK-10 - Documentacion operativa y DoD

Define los estandares de documentacion tecnica del proyecto: Definition of Done, criterios de aceptacion por historia de usuario, plantilla de decision tecnica (ADR) y checklist de entrega. Garantiza que cada feature entregada esta completa y documentada de forma coherente.

## Prerequisito: el proyecto debe existir

Antes de aplicar este skill, verificar que la estructura del proyecto esta creada:

```powershell
Test-Path src/backend -PathType Container
Test-Path src/frontend -PathType Container
```

Si alguno devuelve `False`, **detener y aplicar SK-00 primero** (`sk-00-scaffolding-proyecto`).

## Cuando usar este skill

- Al preparar la entrega de una historia de usuario o un sprint.
- Al tomar una decision tecnica significativa que deba quedar registrada.
- Al revisar si una feature cumple el DoD antes de marcarla como terminada.
- Al incorporar un miembro nuevo al equipo que necesite entender las convenciones.

## Objetivo

Garantizar que:
- Cada feature entregada cumple un conjunto verificable de criterios.
- Las decisiones tecnicas importantes quedan registradas con contexto y razonamiento.
- El equipo tiene una referencia clara de "que significa terminado".

## Definition of Done (DoD) del proyecto

Una historia de usuario esta **Terminada** cuando cumple **todos** los criterios siguientes:

### Codigo
- [ ] El codigo sigue las convenciones del proyecto (castellano, nomenclatura, separacion de capas).
- [ ] No hay logica de negocio en controladores ni en componentes visuales.
- [ ] No hay `any` en TypeScript ni `object` sin tipar en C#.
- [ ] Las booleanas tienen valor inicial explicito.
- [ ] Las fechas son UTC con sufijo `Utc` en campos y nombres.

### Pruebas
- [ ] Existe al menos un test unitario para cada regla de negocio nueva.
- [ ] Existe al menos un test de integracion para cada endpoint nuevo.
- [ ] Todos los tests existentes pasan sin modificaciones.
- [ ] No se han dejado tests en rojo para "arreglar despues".

### API y contratos
- [ ] Los DTOs de entrada tienen validaciones (`[Required]`, `[MaxLength]`, etc.).
- [ ] Los codigos HTTP de respuesta son semanticos (201, 204, 400, 404).
- [ ] Los contratos del frontend (tipos TypeScript) estan alineados con los DTOs del backend.

### Commits y trazabilidad
- [ ] Los mensajes de commit siguen el formato `tipo(ambito): descripcion corta en castellano`.
- [ ] No hay commits con mensajes vacios ni genericos ("wip", "fix", "changes").

### Documentacion
- [ ] Si se tomo una decision tecnica significativa, existe un ADR en `docs/decisiones/`.
- [ ] Los metodos publicos complejos tienen comentario XML en .NET.
- [ ] El `README.md` esta actualizado si cambia algo de la forma de arrancar o configurar el proyecto.

## Plantilla de decision tecnica (ADR)

Guardar en `docs/decisiones/YYYYMMDD-titulo-breve.md`:

```markdown
# ADR: [Titulo breve de la decision]

- **Fecha**: YYYY-MM-DD
- **Estado**: Propuesta | Aceptada | Rechazada | Obsoleta

## Contexto

Describir el problema o la situacion que motiva la decision.
Incluir restricciones relevantes (tiempo, tecnologia, equipo).

## Opciones consideradas

1. **Opcion A**: descripcion breve. Ventajas / Inconvenientes.
2. **Opcion B**: descripcion breve. Ventajas / Inconvenientes.

## Decision

Opcion elegida y razonamiento principal.

## Consecuencias

- Lo que mejora con esta decision.
- Lo que se complica o las deudas tecnicas que genera.
- Acciones de seguimiento si las hay.
```

## Criterios de aceptacion por historia de usuario

Formato estandar para definir criterios antes de implementar:

```
Dado [contexto o precondicion]
Cuando [accion del usuario o del sistema]
Entonces [resultado esperado verificable]
```

Ejemplo para HU-01 (Crear tarea):

```
Dado que el usuario envia un titulo valido
Cuando realiza POST /api/tareas con ese titulo
Entonces la API responde 201 con la tarea creada y EstaCompletada = false

Dado que el usuario envia un titulo vacio
Cuando realiza POST /api/tareas con ese titulo
Entonces la API responde 400 con un mensaje de error de validacion
```

## Checklist de entrega (resumen ejecutivo)

Antes de marcar una tarea como **Terminada** en el tablero:

```
[ ] Codigo: convenciones, capas, tipos
[ ] Tests: unitarios, integracion, todos verdes
[ ] API: DTOs validados, codigos HTTP correctos
[ ] Frontend: tipos alineados, sin fetch directo en componentes
[ ] Commits: mensajes con formato correcto
[ ] Documentacion: ADR si aplica, XML si aplica, README si aplica
```

## Guia de evolucion del proyecto

Al introducir un cambio que afecte a capas transversales:

| Cambio | Acciones requeridas |
|--------|---------------------|
| Nueva entidad | SK-01 → SK-02 → SK-03 → SK-04 → SK-05 → SK-06 → SK-08 → SK-09 |
| Nuevo campo en entidad existente | SK-01 (dominio) + SK-02 (DTO) + SK-03 (mapeador) + SK-06 (migracion) + SK-08 (tipo TS) |
| Nuevo endpoint | SK-04 (caso de uso) + SK-05 (controlador) + SK-08 (servicio frontend) + SK-09 (test integracion) |
| Cambio de esquema de DB | SK-06 (migracion incremental, sin borrar DB) |
| Cambio de contrato API | SK-02 (DTO) + SK-08 (tipo TS) + SK-05 (respuesta HTTP) + documentar en ADR |

## Checklist de calidad del propio skill SK-10

- [ ] El DoD esta accesible y visible para todo el equipo.
- [ ] Los ADRs se crean en el momento de la decision, no a posteriori.
- [ ] Los criterios de aceptacion se definen antes de implementar (no despues).
- [ ] La guia de evolucion se actualiza cuando se agrega un nuevo skill al catalogo.

## Relacion con otros skills

- **SK-00..SK-09**: SK-10 es el paraguas que define cuando cada uno de ellos se considera correctamente aplicado.
- **mensajes-commit**: el DoD incluye el formato de commits como criterio de entrega.
