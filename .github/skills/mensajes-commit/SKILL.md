---
name: mensajes-commit
description: Genera mensajes de commit especificos, claros y breves para este repositorio, siguiendo la convencion tipo(ambito): descripcion corta en castellano.
---

# mensajes-commit

Genera mensajes de commit de alta calidad para este repositorio, con foco en precision y trazabilidad del cambio.

## Cuando usar este skill
- Al preparar un commit despues de cambios en codigo, pruebas, documentacion o CI.
- Al querer transformar un diff en un mensaje claro en castellano.
- Al necesitar una alternativa mejor que mensajes vagos.

## Objetivo
Producir mensajes de commit claros, especificos y breves, siguiendo la convencion del proyecto.

## Convencion obligatoria del proyecto (extraida de copilot-instructions)
- Formato del asunto: `tipo(ambito): descripcion corta en castellano y en minusculas`.
- El ambito es opcional si no aporta contexto.
- El asunto debe ir en una sola linea y, de preferencia, por debajo de 72 caracteres.
- El cuerpo es opcional. Si existe, agregar una segunda linea breve para contexto.
- Tipos permitidos:
  - `feat`: nueva funcionalidad
  - `fix`: correccion de errores
  - `docs`: cambios de documentacion
  - `refactor`: reorganizacion interna sin cambiar comportamiento
  - `test`: altas o ajustes de pruebas
  - `chore`: tareas de mantenimiento
  - `ci`: cambios de integracion o pipelines

## Reglas de calidad para redactar el mensaje
1. Se especifico con lo que cambio:
   - Evitar frases vagas como "actualizar fichero" o "ajustes varios".
   - Nombrar concretamente que se toco (componente, endpoint, DTO, test, config, doc, etc.).
2. Escueto no significa vago:
   - Mensajes cortos son correctos solo si describen con precision lo ocurrido.
   - Ejemplo valido: `docs: cambia idioma del codigo de ingles a castellano`.
   - Ejemplo no valido: `docs: actualiza instrucciones`.
3. Cuerpo opcional pero valioso:
   - Si el cambio es trivial y unico, basta una linea (solo asunto).
   - Si el cambio tiene varias piezas, anadir una segunda linea con detalle util.
4. No incluir ruido:
   - No describir intenciones generales si no se reflejan en el diff.
   - No listar cambios que no esten en el commit.

## Proceso recomendado
1. Revisar diff staged (o cambios candidatos a commit).
2. Identificar el tipo principal (`feat`, `fix`, `docs`, etc.).
3. Elegir ambito solo si aporta claridad (por ejemplo: `api`, `tareas`, `readme`, `tests`).
4. Redactar asunto concreto, en minusculas y en castellano.
5. Validar que el asunto sea una sola linea y preferiblemente < 72 caracteres.
6. Si aplica, agregar una segunda linea breve con contexto tecnico real.

## Plantillas
### Asunto
`tipo(ambito): accion especifica sobre elemento concreto`

### Asunto + cuerpo
`tipo(ambito): accion especifica sobre elemento concreto`

`detalle breve del cambio relevante (opcional)`

## Ejemplos buenos
- `feat(tareas): agrega endpoint patch para completar tarea`
- `fix(api): corrige validacion de id en actualizar tarea`
- `docs(readme): aclara arranque local con backend https`
- `test(servicios): cubre titulo vacio en creacion de tarea`
- `chore(ci): ajusta workflow para ejecutar pruebas de backend`

## Checklist rapido antes de confirmar
- El mensaje usa un tipo permitido.
- Esta en castellano y en minusculas.
- Dice exactamente que cambio.
- Evita terminos genericos como "actualizar" sin objeto concreto.
- Si hay cuerpo, agrega contexto real y breve.
