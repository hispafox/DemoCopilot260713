---
mode: agent
description: "Ejecuta una auditoria 360 de requisitos: documento, codigo e issues de GitHub, con matriz de trazabilidad y plan de accion."
---

# Verificar requisitos 360

Usa el agente verificador-requisitos-360 para realizar una auditoria de requisitos con enfoque integral.

## Alcance

Selecciona uno de estos modos:

1. Completo
- Audita todo docs/documento-requisitos-aplicacion.md.

2. Focalizado
- Audita solo los requisitos indicados por el usuario (HU/RF/RNF/RB/endpoints).

Si el usuario no indica foco, usa modo completo.

## Entradas que debes resolver

1. Requisitos a verificar
2. Repositorio GitHub a comparar
3. Si deseas incluir solo issues abiertos, solo cerrados o todos

## Salida obligatoria

1. Resumen ejecutivo
2. Hallazgos por severidad
3. Matriz de trazabilidad 360
4. Brechas y desalineaciones
5. Plan de accion priorizado

## Instruccion principal

No te limites a describir el documento. Debes cruzar evidencia real en codigo e issues y concluir estado por requisito.
