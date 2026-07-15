# Catalogo de agentes del proyecto

## Objetivo
Documentar los agentes personalizados disponibles en este repositorio para que el equipo sepa cuando usar cada uno.

## Agentes disponibles

### arquitecto-scrum-historias
- Proposito: transformar necesidades de negocio en backlog Scrum refinado.
- Usar cuando: se necesite pasar de objetivo de negocio a epicas, features e historias con Gherkin.

### ayudante-creacion-prompts
- Proposito: mejorar la redaccion de prompts de trabajo.
- Usar cuando: se quiera optimizar un prompt con objetivo, contexto, restricciones y formato.

### desarrollador-desde-issue-o-plan
- Proposito: implementar una feature de punta a punta desde un issue o desde un plan generado por planificador-features.
- Usar cuando: ya existe una unidad de trabajo definida y se quiere ejecutar con trazabilidad, pruebas y verificacion integral.
- Nota clave: evalua y aplica los skills SK00 a SK11 segun alcance.

### mejorador-diseno-word
- Proposito: mejorar el diseno visual de documentos Word.
- Usar cuando: se requiera elevar calidad visual y consistencia de un .docx.

### planificador-features
- Proposito: convertir una peticion en plan de trabajo accionable y crear issues.
- Usar cuando: se necesite descomponer una iniciativa en historias, criterios y tickets de ejecucion.

### priorizador-semaforo-issues
- Proposito: ordenar y clasificar trabajo en GitHub con semaforo y flujo.
- Usar cuando: se requiera priorizar backlog, separar reactivo y planificado, y proponer rebalanceo.

### verificador-requisitos-360
- Proposito: auditar cobertura de requisitos cruzando documento, codigo e issues.
- Usar cuando: se necesite evidencia objetiva de cumplimiento y brechas.

## Flujo recomendado entre agentes

1. planificador-features: define plan y crea issues.
2. desarrollador-desde-issue-o-plan: implementa con pruebas y trazabilidad.
3. verificador-requisitos-360: audita cumplimiento real tras la implementacion.

## Ubicacion tecnica
- Definiciones de agentes: .github/agents/
- Este catalogo: docs/catalogo-agentes-proyecto.md
