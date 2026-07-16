# Planificacion: categorizar tareas

**Fecha:** 2026-07-15
**Estado:** borrador
**Autor:** planificador-democopilot

## 1. Resumen ejecutivo

Se planifica incorporar la capacidad de categorizar tareas para especificar de que tipo es cada tarea.
El alcance incluye persistencia de categoria en el backend, contratos API y soporte en frontend para crear/editar/visualizar tareas con categoria.
El resultado esperado es que cada tarea pueda tener una categoria valida y consistente durante su ciclo de vida.

## 2. Contexto y trazabilidad

Conexion con requisitos existentes en docs/documento-requisitos-aplicacion.md:
- La feature extiende el modelo de tarea con un atributo de clasificacion sin romper el CRUD existente.
- Se mantiene separacion de capas Domain/Application/Infrastructure/Api/Frontend/Tests.
- Se usan DTOs para entrada/salida, sin exponer entidades EF directamente.

Supuestos explicitos:
- Se considera categoria como dato obligatorio de negocio para tareas nuevas.
- Se usara una lista cerrada de categorias definida en backend para evitar valores inconsistentes.
- Las tareas existentes sin categoria se normalizaran con una categoria por defecto durante migracion o actualizacion de datos.

## 3. Historias de usuario

### HU-1: definir categorias de tarea validas

- **Historia:** Como usuario, quiero que el sistema reconozca categorias de tarea validas, para clasificar mis tareas de forma consistente.
- **Prioridad:** Must
- **Tamano:** S
- **Dependencias:** ninguna
- **Supuestos:** la lista inicial de categorias sera acotada y configurable en codigo

#### Criterios de aceptacion

```gherkin
Escenario: crear tarea con categoria valida
  Dado que existe una categoria valida en el sistema
  Cuando el usuario crea una tarea indicando esa categoria
  Entonces la API responde exito y la tarea queda guardada con esa categoria

Escenario: rechazar categoria invalida
  Dado que el usuario envia una categoria no permitida
  Cuando intenta crear o actualizar una tarea
  Entonces la API responde error de validacion y no persiste cambios

Escenario: normalizar tareas previas sin categoria
  Dado que existen tareas historicas sin categoria
  Cuando se ejecuta la migracion de esquema y datos
  Entonces cada tarea queda con una categoria por defecto definida
```

#### Capas afectadas

- [x] Domain
- [x] Application
- [x] Infrastructure
- [x] Api
- [ ] Frontend
- [x] Tests

### HU-2: asignar y editar categoria en tareas

- **Historia:** Como usuario, quiero asignar y modificar la categoria de una tarea, para reflejar correctamente su tipo durante su gestion.
- **Prioridad:** Must
- **Tamano:** M
- **Dependencias:** HU-1
- **Supuestos:** la categoria se envia en DTO de crear y actualizar tarea

#### Criterios de aceptacion

```gherkin
Escenario: asignar categoria al crear tarea
  Dado que el usuario completa titulo y categoria valida
  Cuando confirma la creacion
  Entonces la tarea se almacena con la categoria indicada

Escenario: actualizar categoria de una tarea existente
  Dado que existe una tarea con una categoria previa
  Cuando el usuario actualiza la tarea con otra categoria valida
  Entonces el sistema guarda la nueva categoria y actualiza ActualizadoEnUtc

Escenario: actualizar tarea inexistente
  Dado un id de tarea que no existe
  Cuando el usuario intenta cambiar su categoria
  Entonces la API responde 404
```

#### Capas afectadas

- [x] Domain
- [x] Application
- [x] Infrastructure
- [x] Api
- [ ] Frontend
- [x] Tests

### HU-3: mostrar y seleccionar categoria en la interfaz

- **Historia:** Como usuario, quiero ver y elegir categoria en la interfaz de tareas, para identificar rapidamente el tipo de cada tarea.
- **Prioridad:** Should
- **Tamano:** M
- **Dependencias:** HU-1, HU-2
- **Supuestos:** la API expone las categorias validas para poblar controles de seleccion

#### Criterios de aceptacion

```gherkin
Escenario: visualizar categoria en listado de tareas
  Dado que existen tareas con categoria
  Cuando el usuario abre el listado
  Entonces cada tarea muestra su categoria de forma legible

Escenario: seleccionar categoria en formulario
  Dado que el usuario abre el formulario de alta o edicion
  Cuando despliega las opciones de categoria
  Entonces visualiza solo categorias validas y puede seleccionar una

Escenario: manejar error de carga de categorias
  Dado que falla la consulta de categorias
  Cuando la interfaz intenta cargar el formulario
  Entonces se muestra un mensaje de error y se evita enviar un valor invalido
```

#### Capas afectadas

- [ ] Domain
- [ ] Application
- [ ] Infrastructure
- [x] Api
- [x] Frontend
- [x] Tests

## 4. Riesgos y vacios

- No se confirmo aun el catalogo exacto de categorias iniciales.
- Falta decidir si categoria debe ser siempre obligatoria tambien para tareas historicas ya cerradas.
- Riesgo de regresion en contratos frontend si no se versionan o coordinan DTOs.

## 5. Propuesta de orden de implementacion

1. HU-1, porque define contrato y validaciones base.
2. HU-2, porque habilita el flujo principal de negocio sobre tareas.
3. HU-3, porque depende de contratos estables de backend para UI.
