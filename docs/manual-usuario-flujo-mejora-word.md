# Manual de usuario: flujo de mejora de diseno Word (.docx)

## 1. Objetivo

Este manual explica como usar el flujo completo para mejorar el diseno de documentos Word dentro del repositorio:

- Prompt reutilizable
- Agente especializado
- Skill de soporte para `.docx`

La meta es transformar un documento base en una version mas profesional, manteniendo el contenido funcional.

## 2. Componentes del flujo

### 2.1 Prompt

Archivo:
- `.github/prompts/mejorar-diseno-word.prompt.md`

Rol:
- Estandariza la forma de pedir el trabajo.
- Obliga a recoger entradas clave (ruta del `.docx`, estilo visual, marca, salida).
- Define salida minima esperada (archivo generado + resumen + ajustes opcionales).

### 2.2 Agente

Archivo:
- `.github/agents/mejorador-diseno-word.agent.md`

Rol:
- Ejecuta la mejora de diseno de punta a punta.
- Mantiene reglas de seguridad del contenido (no cambiar significado, no borrar secciones sin permiso).
- Aplica una direccion visual coherente: portada, tipografia, tablas, espaciado, etc.

### 2.3 Skill `docx`

Archivo:
- `.github/skills/docx/SKILL.md`

Rol:
- Aporta conocimiento tecnico para crear, leer y editar `.docx`.
- Define buenas practicas para formato, tablas, estilos y validacion.
- Actua como base operativa cuando se manipulan documentos Word.

## 3. Diferencia entre prompt, agente y skill

- Prompt: guion de invocacion. Te asegura consistencia en lo que pides.
- Agente: comportamiento especializado que realiza la tarea.
- Skill: conocimiento reusable y reglas tecnicas para ese dominio.

Regla practica:
- Si quieres repetir siempre el mismo tipo de encargo: usa Prompt.
- Si quieres un comportamiento experto estable: usa Agente.
- Si quieres conocimiento tecnico del dominio: usa Skill.

## 4. Puesta en marcha (una sola vez)

1. Verificar que existen estos archivos en el repo:
- `.github/prompts/mejorar-diseno-word.prompt.md`
- `.github/agents/mejorador-diseno-word.agent.md`
- `.github/skills/docx/SKILL.md`

2. Abrir VS Code en la raiz del repositorio.

3. Abrir el chat de Copilot en modo agente.

4. Confirmar que el prompt aparece disponible (normalmente como comando slash basado en el nombre del archivo).

## 5. Flujo de uso diario

### Plantilla por defecto (nuevo)

Si no se especifica otra cosa, el flujo aplica automaticamente la plantilla corporativa de referencia basada en:

- `docs/GHCOPMP-temario.docx`

Esto significa que, por defecto, el agente reutiliza su sistema de tipografia, color, listas y tablas para homogeneizar entregables.

### Paso 1. Preparar el documento base

- Ten localizado el `.docx` que quieres mejorar.
- Si aplica, prepara logo y pautas de marca (colores, tono, estilo).

### Paso 2. Invocar el prompt

En el chat, invoca el prompt de mejora (comando slash del prompt) y proporciona:

1. Ruta del `.docx` origen.
2. Estilo deseado (ejecutivo, corporativo, tecnico, comercial).
3. Si hay que respetar identidad de marca.
4. Ruta/nombre de salida (opcional).
5. (Opcional) Documento patron alternativo si quieres reemplazar la plantilla por defecto.

### Paso 3. Resolver preguntas del agente

Si falta contexto, el agente pedira datos minimos antes de editar. Esto evita cambios ambiguos.

Si indicas un documento patron, el agente extraera sus reglas visuales para ese encargo y no usara la plantilla por defecto.

### Paso 4. Revisar la salida

El resultado debe incluir:

1. Ruta del nuevo `.docx`.
2. Resumen de mejoras aplicadas.
3. Ajustes opcionales para una segunda iteracion fina.

## 6. Ejemplo de invocacion

Ejemplo de mensaje (puedes adaptarlo):

"Usa el flujo de mejorar diseno Word para `docs/informe-base.docx` con estilo ejecutivo corporativo. Mantener logo y paleta azul/gris. Genera salida como `docs/informe-base-diseno-mejorado.docx`."

## 7. Criterios de calidad esperados

El documento final debe cumplir:

1. Jerarquia visual clara (H1/H2/H3/cuerpo).
2. Tipografia consistente (sin mezclas arbitrarias).
3. Tablas legibles con cabeceras diferenciadas.
4. Espaciado uniforme y lectura comoda.
5. Contenido funcional intacto.

## 8. Troubleshooting

### 8.1 El prompt no aparece en el chat

- Verifica que el archivo existe en `.github/prompts/`.
- Cierra y abre de nuevo la ventana de VS Code.
- Reabre el chat de Copilot.

### 8.2 El agente no respeta el estilo pedido

- Especifica mejor la direccion visual (ejecutivo sobrio, comercial impactante, etc.).
- Aporta reglas concretas (tipografia, paleta, densidad visual, tono).

### 8.3 Cambios demasiado agresivos

- Indica explicitamente: "mantener estructura y solo mejorar presentacion".
- Limita alcance: portada + titulos + tablas, sin tocar redaccion.

### 8.4 El resultado no conserva marca

- Adjunta reglas de marca de forma explicita (colores HEX, usos de logo, tono).
- Pide "no introducir colores fuera de la paleta".

## 9. Buenas practicas de operacion

1. Guardar siempre el original y generar salida en archivo nuevo.
2. Hacer una pasada rapida de validacion visual antes de enviar al cliente.
3. Pedir segunda iteracion solo sobre puntos concretos (no "hazlo mejor" en general).
4. Estandarizar un estilo por tipo de documento (informe, propuesta, acta).

## 10. Plantilla rapida de solicitud

Usa este bloque para acelerar encargos:

- Archivo origen: `<ruta_docx>`
- Estilo visual: `<ejecutivo|corporativo|tecnico|comercial>`
- Marca: `<si/no + reglas>`
- Salida: `<ruta_salida_docx>`
- Restricciones: `<mantener estructura / no tocar texto / etc.>`
- Patron alternativo (opcional): `<ruta_docx_patron>`

## 11. Resumen ejecutivo

Con este flujo:

- El Prompt asegura consistencia de peticion.
- El Agente ejecuta la mejora de diseno con reglas claras.
- La Skill `docx` aporta base tecnica para manipular Word correctamente.

Resultado: documentos mas profesionales, repetibles y con menos retrabajo.
