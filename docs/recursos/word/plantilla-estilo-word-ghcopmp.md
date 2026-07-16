# Plantilla de estilo Word: GHCOPMP

## Origen

Documento patron:

- `docs/GHCOPMP-temario.docx`

## Objetivo

Estandarizar el estilo visual de todos los documentos mejorados por el flujo de diseno Word en este repositorio.

## Tokens visuales extraidos

### Tipografia

- Fuente base: Calibri.
- Cuerpo principal: 10.5 pt - 11 pt.
- Titular principal: 24 pt negrita.
- Subtitulos/entradilla: 13 pt - 15 pt.
- Encabezados de bloque: 12.5 pt negrita.
- Texto en listas: 10 pt - 10.5 pt.

### Colores

- Texto principal: `#444444`.
- Acento principal: `#E86A10`.
- Acentos secundarios detectados: `#B3412B`, `#5C2D91`, `#1E7A5A`.
- Gris auxiliar: `#777777`.
- Blanco para contraste en bloques oscuros: `#FFFFFF`.

### Espaciado y maquetacion

- Margenes del patron:
  - Superior: 2.54 cm
  - Inferior: 2.54 cm
  - Izquierdo: 3.17 cm
  - Derecho: 3.17 cm
- Distancia de cabecera y pie: 1.27 cm.
- Ritmo de parrafo compacto, con predominio de espaciados cortos.
- Listas con sangrias consistentes y orientadas a lectura rapida.

### Tablas

- Tabla de referencia: 1 tabla, 4 columnas.
- Cabecera con fondo `#E86A10`.
- Celdas de datos con fondos suaves alternos (`#F7F7F7`) y bloques auxiliares (`#FBE3CE`) cuando procede.
- Ancho de celda detectado: 2160 dxa.

## Reglas de aplicacion

1. No modificar significado del contenido.
2. No eliminar secciones sin autorizacion explicita.
3. Aplicar estos tokens de forma coherente en todo el documento.
4. Si el usuario proporciona otro documento patron, este reemplaza a la plantilla GHCOPMP para ese encargo.
5. Siempre generar salida en archivo nuevo.

## Nota operativa

Esta plantilla es la base por defecto para el agente `mejorador-diseno-word` y para el prompt `mejorar-diseno-word`.
