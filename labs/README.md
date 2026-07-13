# Índice de laboratorios

Laboratorios del curso **GitHub Copilot para desarrolladores .NET**, ordenados por módulo.

---

## Módulo 1 — Fundamentos

| Lab | Título | Qué se trabaja |
|-----|--------|----------------|
| [M1.1](GHCOPTL-M1.1-lab.md) | Contexto, setup y las reglas de la casa | Configuración del entorno de trabajo, estructura del repositorio y convenciones del curso. |

---

## Módulo 2 — Skills

| Lab | Título | Qué se trabaja |
|-----|--------|----------------|
| [M2.1](GHCOPTL-M2.1-lab.md) | Tu primer skill: `commit-message` | Construcción del primer `SKILL.md` desde cero: descripción, activación y uso para generar mensajes de commit reales. |
| [M2.2](GHCOPTL-M2.2-lab.md) | Ecosistema y seguridad de los skills | Búsqueda, inspección y evaluación de seguridad de un skill externo antes de instalarlo en el proyecto. |

---

## Módulo 3 — Análisis y modelo

| Lab | Título | Qué se trabaja |
|-----|--------|----------------|
| [M3.1](GHCOPTL-M3.1-lab.md) | El documento de análisis | Creación del skill `diseño-analisis` y generación del documento de análisis como única fuente de verdad del proyecto. |
| [M3.2](GHCOPTL-M3.2-lab.md) | Del documento al código: el modelo | Generación de la capa de dominio (entidades) a partir del documento de análisis; se vive y desmonta la trampa de incrustar entidades en el skill. |

---

## Módulo 4 — Arquitectura en capas

| Lab | Título | Qué se trabaja |
|-----|--------|----------------|
| [M4.1](GHCOPTL-M4.1-lab.md) | La capa HTTP: el controlador | Creación del skill `controlador` y generación de los controladores REST y el fichero `.http`; el proyecto no compila aún, a propósito. |
| [M4.2](GHCOPTL-M4.2-lab.md) | Los contratos: el DTO | Skill `dto` y generación de los DTOs de entrada/salida para cada recurso; el controlador se refactoriza para usar los contratos. |
| [M4.3](GHCOPTL-M4.3-lab.md) | La orquestación: el servicio | Skill `servicio` y capa de traducción entre DTO y entidad; se lee cómo el servicio delega sin ejecutar lógica propia. |
| [M4.4](GHCOPTL-M4.4-lab.md) | Las reglas: la lógica de negocio | Capa de lógica que habla con la base de datos e implementa la regla estrella de la aplicación (recurrencia de tareas). |
| [M4.5](GHCOPTL-M4.5-lab.md) | La persistencia: la base de datos | `AppDbContext`, migración y seeder; por primera vez el proyecto compila y los datos persisten entre reinicios. |
| [M4.6](GHCOPTL-M4.6-lab.md) | Las validaciones | Validaciones de entrada con DataAnnotations (`400` automático) y guardas de estado en la lógica de negocio. |
| [M4.7](GHCOPTL-M4.7-lab.md) | La arquitectura de los skills | Generación del documento `docs/skills-orquestacion.md` con catálogo, orden de invocación y grafo de dependencias en Mermaid. |

---

## Módulo 5 — Orquestación y depuración

| Lab | Título | Qué se trabaja |
|-----|--------|----------------|
| [M5.1](GHCOPTL-M5.1-lab.md) | Orquestar los skills: `nueva-feature` | Construcción del skill orquestador que recorre toda la cadena (análisis → commit) con una sola frase; primer ciclo virtuoso de mejora. |
| [M5.2](GHCOPTL-M5.2-lab.md) | Depurar lo que el orquestador rompe | Reproducción en vivo de los fallos que el orquestador dejó pasar, diagnóstico hasta la causa raíz y corrección del skill que los originó. |

---

## Módulo 6 — Agentes

| Lab | Título | Qué se trabaja |
|-----|--------|----------------|
| [M6.1](GHCOPTL-M6.1-lab.md) | El plano del equipo | Lectura e interpretación del documento de diseño del equipo de agentes; se aprende a leer un sistema antes de tocarlo. |
| [M6.2](GHCOPTL-M6.2-lab.md) | El agente sin manos | Construcción del agente `prompt-engineer` (`tools: []`) y uso para transformar una idea a medio formar en un prompt con los cuatro pilares completos. |
| [M6.3](GHCOPTL-M6.3-lab.md) | El jefe de obra | Creación del agente `planificador` y conversión del prompt del lab anterior en un plan estructurado de diez secciones. |
| [M6.4](GHCOPTL-M6.4-lab.md) | De albañil a encargado de obra | Construcción del agente `desarrollador` en dos versiones (completa y delgada), y primera generación de código real a partir de un plan. |

---

## Módulo 7 — Verificación y ciclo virtuoso

| Lab | Título | Qué se trabaja |
|-----|--------|----------------|
| [M7.1](GHCOPTL-M7.1-lab.md) | El inspector con las manos atadas | Agente `verificador` que comprueba si el código cumple el plan; se provoca un fallo a propósito para ver cambiar el veredicto. |
| [M7.2](GHCOPTL-M7.2-lab.md) | El abogado del diablo | Agente `auditor-calidad` que busca problemas sin lista previa y los puntúa por severidad sobre el proyecto real. |
| [M7.3](GHCOPTL-M7.3-lab.md) | El molde, no la copia | Se amplía el auditor para detectar fallos de skill (no de instancia) y se corrige el molde que los origina. |
| [M7.4](GHCOPTL-M7.4-lab.md) | El director de orquesta | Agente `orquestador` que coordina a planificador, desarrollador y verificador con una sola frase; se prueba el bucle de corrección. |
| [M7.5](GHCOPTL-M7.5-lab.md) | De una frase a `main` (o al freno honesto) | Recorrido completo del sistema en sus dos salidas válidas: el commit exitoso y el freno honesto, sobre el proyecto propio. |

---

## Módulo 8 — Interfaz y frontend

| Lab | Título | Qué se trabaja |
|-----|--------|----------------|
| [M8.1](GHCOPTL-M8.1-lab.md) | Ponle cara a tu API | Instalación de Scalar para documentación OpenAPI interactiva; se verifica que desaparece fuera del entorno de desarrollo. |
| [M8.2](GHCOPTL-M8.2-lab.md) | La pantalla que le faltaba a tu API | Skill `frontend-react` y generación del frontend completo; diagnóstico del proxy Vite contra los puertos reales del `launchSettings.json`. |
| [M8.3](GHCOPTL-M8.3-lab.md) | El catálogo que confiesa su propio hueco | Instalación y auditoría del skill `ui-ux-pro-max`: verificación de URL, lectura del `SKILL.md` y decisión informada sobre si merece confianza. |

---

## Módulo 9 — Documentación

| Lab | Título | Qué se trabaja |
|-----|--------|----------------|
| [M9.1](GHCOPTL-M9.1-lab.md) | Pon la documentación al día con el código | Auditoría de documentación desincronizada, corrección de diagramas Mermaid rotos y construcción del skill que automatiza la auditoría. |
| [M9.2](GHCOPTL-M9.2-lab.md) | El análisis, en el formato que cada uno sabe abrir | Exportación del documento de análisis a Word y PDF con skills integrados; revisión obligatoria de licencias antes de usar cada skill. |
| [M9.3](GHCOPTL-M9.3-lab.md) | Un agente que documenta para quien no programa | Agente `documentador-usuario` con contrato mínimo de herramientas; genera el manual de usuario con huecos etiquetados para capturas. |
| [M9.4](GHCOPTL-M9.4-lab.md) | Las capturas del manual, como código que se regenera | Script de Playwright generado por Copilot para automatizar las capturas del manual; se rellena un hueco real con la imagen resultante. |

---

## Módulo 10 — Colaboración y calidad

| Lab | Título | Qué se trabaja |
|-----|--------|----------------|
| [M10.1](GHCOPTL-M10.1-lab.md) | GitHub: del hallazgo al merge | Conexión del MCP de GitHub al orquestador; un issue recorre solo el camino desde `#N` hasta el pull request fusionado sin comandos de `git`. |
| [M10.2](GHCOPTL-M10.2-lab.md) | La pirámide de pruebas | Skill `tests-unitarios` y agente ejecutor; primera batería de tests unitarios en verde, incluyendo el que blinda el fallo del issue anterior. |
| [M10.3](GHCOPTL-M10.3-lab.md) | Integración continua | Workflow de GitHub Actions con build, tests y cobertura en cada push/PR; health checks en producción; rotura de test a propósito para ver al guardián actuar. |
