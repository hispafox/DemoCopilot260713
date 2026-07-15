---
mode: agent
description: "Mejora el diseno visual de un archivo .docx con una plantilla profesional manteniendo el contenido funcional."
---

# Mejorar diseno de Word

Usa el agente `mejorador-diseno-word` para transformar un documento `.docx` en una version mas profesional y consistente, sin alterar el significado del contenido.

## Objetivo

Mejorar presentacion visual, jerarquia tipografica, legibilidad y consistencia global del documento.

Si no se indica una plantilla alternativa, usar por defecto la plantilla corporativa definida desde:

- `docs/GHCOPMP-temario.docx`

## Entradas que debes resolver

1. Ruta del archivo `.docx` origen.
2. Estilo visual deseado (ejecutivo, corporativo, tecnico, comercial).
3. Si se deben respetar logo, colores o identidad de marca.
4. Nombre o ruta de salida (si no se indica, usar sufijo `-diseno-mejorado.docx`).
5. (Opcional) Documento patron alternativo para sobreescribir la plantilla por defecto.

Si falta alguna entrada critica, preguntarla antes de editar.

## Reglas obligatorias

1. No cambiar el significado del texto.
2. No eliminar secciones sin aprobacion explicita del usuario.
3. Mantener estructura semantica de titulos, listas y tablas.
4. Priorizar legibilidad real sobre adornos visuales.
5. Aplicar por defecto el sistema visual de `docs/GHCOPMP-temario.docx` (tipografia, colores, listas y tablas), salvo que el usuario indique otro patron.

## Salida obligatoria

1. Ruta del archivo `.docx` generado.
2. Resumen breve de mejoras aplicadas.
3. Ajustes opcionales propuestos para una segunda iteracion.

## Instruccion principal

No te limites a "maquillar" el documento: aplica una direccion de diseno coherente de punta a punta (portada, tipografias, espaciados, tablas y pies) manteniendo el contenido funcional intacto.
