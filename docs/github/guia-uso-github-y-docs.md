# Guia rapida: `.github` vs `docs`

## Objetivo
Aclarar que informacion usa GitHub Copilot como contexto automatico y que informacion queda solo para el equipo.

## Carpeta `.github`
1. Es para GitHub Copilot.
2. Copilot la lee siempre como parte de sus instrucciones y contexto del repositorio.

### Que poner aqui
- Reglas de estilo de codigo.
- Convenciones de arquitectura.
- Restricciones de implementacion.
- Instrucciones operativas para el asistente.

### Impacto
- Lo que escribamos aqui afecta directamente como Copilot propone y genera cambios.

## Carpeta `docs`
1. Es para nosotros (equipo/proyecto).
2. Copilot no la lee nunca como instrucciones automaticas.

### Que poner aqui
- Documentacion funcional y tecnica para personas.
- Decisiones de producto.
- Notas de reuniones.
- Manuales internos.

### Impacto
- Sirve como referencia del equipo, pero no modifica por si sola el comportamiento de Copilot.

## Regla corta para recordar
- `.github`: configura a Copilot.
- `docs`: documenta para el equipo.

## Frase lista para compartir
"Si queremos que Copilot lo aplique siempre, va en `.github`. Si queremos documentarlo para nosotros, va en `docs`."

## Catalogo de agentes del proyecto

- El inventario de agentes personalizados y su uso recomendado esta en `docs/catalogo-agentes-proyecto.md`.
- Las definiciones tecnicas de cada agente estan en `.github/agents/`.
- Flujo sugerido para nuevas iniciativas: `planificador-democopilot` -> `desarrollador-democopilot` -> `verificador-requisitos-360`.
