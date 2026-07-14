# Catalogo de skills y conocimiento agnostico de tecnologia

## 1. Proposito
Este documento define un marco para identificar, describir y gestionar skills reutilizables en el desarrollo de soluciones, sin depender de un lenguaje, framework, nube o herramienta concreta.

Su objetivo es didactico y metodologico: mostrar como estructurar conocimiento transferible entre distintos contextos tecnicos.

## 2. Que significa "agnostico de tecnologia"
Una skill agnostica de tecnologia:

- Describe capacidades y decisiones, no herramientas especificas.
- Se expresa en terminos de principios, entradas, salidas y criterios de calidad.
- Puede aplicarse en diferentes stacks con ajustes minimos de implementacion.
- Evita ejemplos dependientes de una plataforma concreta.

## 3. Principios del catalogo

- Reutilizacion: una skill debe servir en multiples dominios o proyectos.
- Claridad: cada skill debe tener objetivo, limites y resultado verificable.
- Trazabilidad: toda skill debe poder vincularse a entregables y evidencias.
- Evolucion: el catalogo debe revisarse periodicamente y simplificarse cuando sea necesario.
- Neutralidad tecnica: el foco esta en el "que" y el "para que", no en el "con que".

## 4. Estructura canonica de una skill
Cada skill debe documentarse con esta estructura:

- Nombre de la skill.
- Problema que resuelve.
- Contexto de aplicacion.
- Entradas esperadas.
- Proceso o enfoque recomendado.
- Salidas/artefactos esperados.
- Criterios de aceptacion.
- Riesgos frecuentes.
- Limites (que no cubre).
- Indicadores de calidad.

## 5. Catalogo base de skills reutilizables

### SK-01 - Descubrimiento y definicion de problema
- Proposito: transformar una necesidad difusa en una definicion clara y accionable.
- Salida tipica: objetivo, alcance, supuestos y restricciones.

### SK-02 - Modelado conceptual del dominio
- Proposito: representar conceptos, reglas y relaciones clave del problema.
- Salida tipica: modelo conceptual compartido por negocio y equipo tecnico.

### SK-03 - Diseno de contratos de comunicacion
- Proposito: definir acuerdos de intercambio de informacion entre componentes o actores.
- Salida tipica: especificacion de entradas, salidas, validaciones y errores.

### SK-04 - Orquestacion de casos de uso
- Proposito: estructurar flujos de trabajo extremo a extremo y responsabilidades.
- Salida tipica: mapa de casos de uso, pasos, decisiones y puntos de control.

### SK-05 - Persistencia y ciclo de vida de la informacion
- Proposito: definir como se conserva, versiona y consulta la informacion.
- Salida tipica: reglas de integridad, evolucion y acceso eficiente a datos.

### SK-06 - Manejo de errores y resiliencia
- Proposito: estandarizar respuesta ante fallos esperados y no esperados.
- Salida tipica: politica de errores, recuperacion y observabilidad minima.

### SK-07 - Calidad y estrategia de pruebas
- Proposito: asegurar confiabilidad con criterios de verificacion por niveles.
- Salida tipica: matriz de pruebas, cobertura objetivo y criterios de aprobacion.

### SK-08 - Integracion y alineacion entre capas o modulos
- Proposito: mantener coherencia entre modelos internos y representaciones externas.
- Salida tipica: reglas de transformacion, compatibilidad y control de cambios.

### SK-09 - Gobernanza de cambios y versionado
- Proposito: introducir cambios sin degradar estabilidad ni contratos vigentes.
- Salida tipica: politica de versionado, compatibilidad y gestion de deprecaciones.

### SK-10 - Documentacion operativa y transferencia de conocimiento
- Proposito: asegurar que el conocimiento sea reutilizable por otras personas/equipos.
- Salida tipica: guias de uso, criterios de entrega y checklist de calidad.

## 6. Priorizacion sugerida para ensenanza

## Nivel 1 (fundamentos)
- SK-01, SK-02, SK-03, SK-04.

## Nivel 2 (solidez tecnica)
- SK-05, SK-06, SK-07, SK-08.

## Nivel 3 (madurez)
- SK-09, SK-10.

## 7. Rubrica breve para evaluar una skill
Una skill esta bien definida si:

- Es comprensible sin citar tecnologias.
- Tiene salidas observables y verificables.
- Se puede aplicar en mas de un contexto.
- Tiene limites claros para evitar solapamientos.
- Incluye criterios de calidad concretos.

## 8. Regla para evitar inflacion del catalogo
Crear una skill nueva solo si cumple simultaneamente:

- Reutilizacion demostrable en 3 o mas escenarios.
- Diferenciacion clara respecto a skills existentes.
- Necesidad real de criterios de calidad propios.

Si no cumple estas 3 condiciones, debe integrarse en una skill ya existente.

## 9. Plantilla docente (lista para copiar)

- Nombre de la skill:
- Problema que resuelve:
- Contextos donde aplica:
- Entradas:
- Proceso recomendado:
- Salidas esperadas:
- Criterios de aceptacion:
- Riesgos comunes:
- Limites:
- Evidencia de uso:

## 10. Nota de uso en aula
Este catalogo no sustituye el diseno de una solucion concreta.
Sirve como marco para ensenar pensamiento estructurado, trazabilidad y gestion de conocimiento en proyectos de software, independientemente de la tecnologia elegida.
