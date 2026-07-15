# Guia de uso - agente verificador de requisitos 360

## Objetivo

Definir como usar el agente verificador para auditar cumplimiento del proyecto con vision 360:

- Documento de requisitos
- Codigo implementado
- Issues/tickets de GitHub

## Donde esta el agente

- .github/agents/verificador-requisitos-360.agent.md

## Que puede revisar

1. Documento completo
- Recorre HU, RF, RNF, RB y endpoints esperados.

2. Revision parcial (por foco)
- Revisa solo IDs o temas indicados.
- Ejemplos:
  - HU-01
  - RF-07
  - RNF-03
  - Endpoints REST
  - Pruebas
  - UTC/ISO 8601

## Salida esperada

El agente devuelve siempre:

1. Resumen ejecutivo
2. Hallazgos por severidad
3. Matriz de trazabilidad 360
4. Desalineaciones documento-issues-codigo
5. Plan de accion priorizado

## Estados de cobertura

- Verde: completo
- Amarillo: parcial
- Rojo: brecha critica
- Gris: no verificable

## Prompts listos para usar

1. Auditoria completa

Revisa todo el documento de requisitos y genera una vision 360 comparando requisitos, codigo e issues de GitHub. Incluye matriz y plan de accion.

2. Auditoria por requisitos concretos

Verifica HU-04, RF-07 y RF-09 contra codigo e issues. Marca brechas y riesgos con prioridad.

3. Auditoria de cierre de tickets

Compara los issues cerrados con evidencia real en codigo y pruebas. Senala cierres prematuros si no hay trazabilidad.

## Recomendacion operativa

Ejecutar esta auditoria al menos en estos hitos:

1. Antes de cierre de sprint
2. Antes de demo con cliente
3. Antes de cerrar milestone
4. Antes de publicar informe de calidad

## Nota

El agente audita y recomienda. No aplica cambios de codigo automaticamente salvo peticion explicita del usuario.
