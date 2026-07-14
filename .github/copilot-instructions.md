# Instrucciones de Copilot

## Proyecto
Aplicacion de lista de tareas con:
- Backend: ASP.NET Core 10 Web API
- Frontend: React + Vite + TypeScript
- Datos: SQLite + Entity Framework Core

## Idioma y nomenclatura
- Todo el codigo, comentarios, nombres de clases, interfaces, metodos, propiedades, variables, DTOs y servicios debe escribirse en castellano.
- Evitar nombres en ingles salvo dependencias externas, APIs de framework o contratos de terceros que no puedan cambiarse.
- Mantener consistencia de idioma en backend y frontend.
- Usar camelCase para variables, parametros, campos locales, propiedades de objetos planos y nombres de funciones auxiliares.
- Usar PascalCase para clases, interfaces, DTOs, entidades, servicios, componentes y enums.
- Nombrar las booleanas con prefijos semanticos en castellano como Es, Esta, Tiene, Puede o Debe.
- No dejar booleanas sin valor: deben inicializarse de forma explicita con verdadero o falso.
- Las fechas y horas deben tratarse siempre en UTC y serializarse en formato ISO 8601, preferiblemente con sufijo Z.
- Si se muestra una fecha en pantalla o en un contrato, indicar claramente si es UTC o convertirla al formato local solo cuando el caso de uso lo requiera.
- Documentar con comentarios XML de .NET los metodos publicos o complejos cuyo comportamiento no se entienda solo por el nombre, incluyendo resumen, parametros, valor de retorno y excepciones relevantes.

## Objetivo
Mantener un codigo simple, mantenible y listo para evolucionar sin mezclar responsabilidades.

## Criterio de calidad basado en documentacion
- Antes de implementar cualquier feature nueva, leer primero los documentos de requisitos disponibles en `docs/`.
- Es obligatorio leer `docs/documento-requisitos-aplicacion.md` cuando exista.
- Si existen otros documentos de requisitos o especificacion en `docs/` (por nombre o contenido), tambien deben leerse antes de codificar.
- Antes de escribir codigo, construir una trazabilidad minima: requisito -> capa afectada (Domain/Application/Api/Infrastructure/Frontend/Tests).
- No marcar una feature como terminada si no hay evidencia de cobertura de requisitos en codigo y pruebas.

## Arquitectura esperada
Separar claramente capas y responsabilidades:
- Modelos: entidades de dominio y DTOs
- Logica de negocio: servicios de aplicacion
- Acceso a datos: DbContext, configuraciones EF, repositorios si aplican
- API: controladores/endpoints, validacion de entrada y contratos HTTP

No mezclar logica de negocio dentro de controladores ni en componentes de UI.

## Modelo de tarea
Definir una entidad Tarea (o EntidadTarea para evitar conflictos con System.Threading.Tasks.Task) con estos campos minimos:
- Id (int o Guid, clave primaria)
- Titulo (string, requerido, maximo razonable)
- Descripcion (string?, opcional)
- EstaCompletada (bool)
- CreadoEnUtc (DateTime UTC)
- ActualizadoEnUtc (DateTime UTC)

Reglas base:
- Id es clave primaria obligatoria.
- Id no puede venir vacio ni en valor por defecto (0 para int o Guid.Empty para Guid).
- Titulo no puede venir vacio.
- CreadoEnUtc se asigna al crear.
- ActualizadoEnUtc se actualiza en cada modificacion.
- No exponer la entidad EF directamente: usar DTOs de entrada/salida.

## Convenciones Backend (.NET 10)
- Usar C# moderno y nullable reference types activado.
- Usar async/await para I/O (DB, red, archivos).
- Endpoints REST claros y consistentes:
  - GET /api/tareas
  - GET /api/tareas/{id}
  - POST /api/tareas
  - PUT /api/tareas/{id}
  - PATCH /api/tareas/{id}/completar
  - DELETE /api/tareas/{id}
- Respuestas HTTP semanticas:
  - 200/201 para exito
  - 204 para borrado/actualizacion sin body
  - 400 para validacion invalida
  - 404 cuando no existe recurso
- Manejo centralizado de errores cuando sea posible.
- Evitar logica de acceso a datos en controladores.

## Convenciones de datos (EF Core + SQLite)
- Usar EF Core Code First con migraciones.
- No borrar la base de datos para arreglar esquema salvo instruccion explicita.
- Preferir migraciones incrementales y cambios compatibles.
- Definir indices para consultas frecuentes.
- Usar AsNoTracking en lecturas sin modificacion.
- Incluir CreatedAt/UpdatedAt cuando tenga sentido para auditoria basica.

## Convenciones Frontend (React + Vite + TypeScript)
- Componentes funcionales y hooks.
- Tipado estricto, evitar any.
- Separar:
  - componentes de presentacion
  - estado
  - capa de API (services)
- Centralizar cliente HTTP y manejo de errores.
- Mantener UI simple y accesible.
- Evitar logica de negocio compleja dentro de componentes visuales.

## Contratos API y DTOs
- No exponer entidades EF directamente al frontend.
- Usar DTOs para Crear/Actualizar/Consultar.
- Validar entradas en el backend (DataAnnotations o FluentValidation).
- Mantener contratos estables; si se rompen, documentarlo.

## Calidad y pruebas
- Cada feature debe incluir pruebas.
- Backend:
  - unit tests para servicios
  - integration tests para endpoints criticos
- Frontend:
  - pruebas de componentes y comportamiento basico
- Corregir primero tests rotos antes de agregar nuevas features.

## Estilo de trabajo para Copilot
Cuando generes codigo:
1. Haz cambios minimos y enfocados.
2. Respeta estructura de carpetas existente.
3. No refactorices areas no relacionadas sin pedirlo.
4. Si hay ambiguedad de negocio, deja supuestos explicitos en comentarios o en PR.
5. Al agregar endpoint nuevo, agregar tambien:
   - DTOs
   - validacion
   - test(s)
   - actualizacion de cliente frontend si aplica

## Estructura sugerida
- src/backend/
  - Api/
  - Application/
  - Domain/
  - Infrastructure/
  - Tests/
- src/frontend/
  - src/components/
  - src/pages/
  - src/services/
  - src/types/
  - src/tests/

## Notas
- En desarrollo frontend usar HTTP local (Vite por defecto).
- Backend en HTTPS local con certificado de desarrollo de .NET.
- Si se usa proxy en Vite hacia backend HTTPS, configurar secure: false.

## Convencion de commits Git
- Formato obligatorio del mensaje: tipo(ambito): descripcion corta en castellano y en minusculas.
- El asunto del commit debe ir siempre en una sola linea y, de preferencia, por debajo de 72 caracteres.
- Opcionalmente, se permite una segunda linea breve para agregar contexto.
- El ambito es opcional cuando no aporte contexto.
- Tipos permitidos:
  - feat: nueva funcionalidad
  - fix: correccion de errores
  - docs: cambios de documentacion
  - refactor: reorganizacion interna sin cambiar comportamiento
  - test: altas o ajustes de pruebas
  - chore: tareas de mantenimiento
  - ci: cambios de integracion o pipelines

Ejemplos:
- feat(tareas): agrega endpoint para completar tarea
- fix(api): corrige validacion de id en actualizar
- docs(readme): actualiza guia de arranque local
- test(servicios): cubre caso de titulo vacio
