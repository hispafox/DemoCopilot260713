---
mode: agent
description: "Verifica una implementación usando un documento de plan previamente generado en docs/ y devuelve un veredicto APROBADO o REVISAR."
---

# Verificar plan

Usa el agente verificador-democopilot para comprobar si la implementación cumple con el plan documentado.

## Entrada obligatoria

Debes recibir o resolver la ruta del documento de plan, por ejemplo:

- docs/plan-paginacion-2026-07-16.md
- docs/plan-alta-usuarios-2026-07-16.md

Si el usuario no aporta la ruta, pide que la indique o que seleccione el plan relevante.

## Proceso

1. Lee el plan completo.
2. Busca evidencia de implementación en el repositorio.
3. Ejecuta verificación técnica mínima cuando proceda.
4. Devuelve:
   - resumen ejecutivo
   - evidencia revisada
   - hallazgos
   - veredicto final: APROBADO o REVISAR

## Instrucción principal

No verifiques de forma genérica; verifica contra el contenido exacto del plan recibido.
