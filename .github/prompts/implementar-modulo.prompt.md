---
mode: agent
description: "Implementa un modulo completo de la aplicacion (entidad, API, frontend, tests) aplicando todos los skills del catalogo en orden. Usar cuando se pida implementar la lista de tareas u otro modulo nuevo."
---

# Implementar modulo completo

Cuando el usuario pida implementar un modulo (por ejemplo "implementa la lista de tareas", "crea el modulo de categorias", etc.), seguir **obligatoriamente** el orden de skills siguiente. No saltarse ningun paso ni reordenarlos.

## Paso 0 — Verificar que el proyecto existe (SK-00)

Leer el skill `sk-00-scaffolding-proyecto` y comprobar:

```powershell
Test-Path src/backend -PathType Container
Test-Path src/frontend -PathType Container
```

- Si alguno devuelve `False`: **detener** y aplicar SK-00 completo antes de continuar.
- Si ambos existen: continuar al paso 1.

## Paso 1 — Modelado de dominio (SK-01)

Leer el skill `sk-01-modelado-dominio` y aplicarlo:

- Definir la entidad de dominio con sus propiedades, constructor privado y metodo factoria `Crear(...)`.
- Declarar todas las invariantes como excepciones en el metodo factoria y en los metodos de comportamiento.
- Verificar que la entidad no importa ninguna libreria de EF ni de ASP.NET.
- Ubicacion: `src/backend/Domain/`.

## Paso 2 — Contratos y DTOs (SK-02)

Leer el skill `sk-02-contratos-dtos` y aplicarlo:

- Crear el DTO de creacion (`Crear{Entidad}Dto`) con anotaciones `[Required]` y `[MaxLength]`.
- Crear el DTO de actualizacion (`Actualizar{Entidad}Dto`) con sus validaciones.
- Crear el DTO de respuesta (`{Entidad}Dto`) con los campos que el cliente necesita.
- Usar `init` en propiedades de DTOs de entrada.
- Ubicacion: `src/backend/Application/DTOs/`.

## Paso 3 — Mapeadores (SK-03)

Leer el skill `sk-03-mapeo-capas` y aplicarlo:

- Crear la clase `{Entidad}Mapeador` con metodos de extension estaticos.
- Metodo `ADto()`: entidad de dominio → DTO de respuesta.
- Metodo `AListaDto()`: coleccion → lista de DTOs.
- Metodo `ADominio()`: DTO de creacion → entidad de dominio (via metodo factoria).
- Ubicacion: `src/backend/Application/Mapeadores/`.

## Paso 4 — Casos de uso / servicio (SK-04)

Leer el skill `sk-04-casos-de-uso` y aplicarlo:

- Crear la interfaz `I{Entidad}Servicio` con todos los metodos async.
- Crear la interfaz `I{Entidad}Repositorio` en `Application/Repositorios/`.
- Implementar `{Entidad}Servicio` usando el repositorio y los mapeadores.
- El servicio no importa nada de `Microsoft.AspNetCore.*`.
- Ubicacion: `src/backend/Application/Servicios/`.

## Paso 5 — API REST (SK-05)

Leer el skill `sk-05-api-rest` y aplicarlo:

- Crear `{Entidad}Controlador` con los endpoints: GET lista, GET por id, POST, PUT, PATCH/{accion}, DELETE.
- Usar codigos HTTP semanticos: 200, 201 (CreatedAtAction), 204, 400, 404.
- Registrar el middleware `ManejadorExcepcionesMiddleware` en `Program.cs` si no existe.
- El controlador no tiene logica de negocio: solo recibe, delega y responde.
- Ubicacion: `src/backend/Api/Controladores/`.

## Paso 6 — Persistencia (SK-06)

Leer el skill `sk-06-persistencia` y aplicarlo:

- Crear la clase de configuracion EF `{Entidad}Configuracion : IEntityTypeConfiguration<{Entidad}>`.
- Implementar `{Entidad}Repositorio : I{Entidad}Repositorio` usando el DbContext.
- Usar `AsNoTracking()` en todas las consultas de solo lectura.
- Crear la migracion: `dotnet ef migrations add Agregar{Entidad} --project ... --startup-project ...`.
- Registrar DbContext, servicio y repositorio en `Program.cs`.
- Ubicacion infraestructura: `src/backend/Infrastructure/`.

## Paso 7 — Fechas UTC (SK-07)

Leer el skill `sk-07-auditoria-utc` y verificar:

- Todos los `DateTime` usan `DateTime.UtcNow`, no `DateTime.Now`.
- Los campos de fecha en la entidad y en los DTOs tienen sufijo `Utc`.
- Al rehidratar desde SQLite se fuerza `DateTimeKind.Utc` con `DateTime.SpecifyKind`.
- La serializacion JSON esta configurada para producir sufijo `Z`.

## Paso 8 — Cliente frontend (SK-08)

Leer el skill `sk-08-cliente-frontend` y aplicarlo:

- Crear los tipos TypeScript en `src/frontend/src/types/{modulo}.ts` alineados con los DTOs.
- Crear el servicio `{modulo}Servicio.ts` en `src/frontend/src/services/` usando `clienteHttp`.
- Crear `clienteHttp.ts` si no existe todavia.
- Ubicacion: `src/frontend/src/services/` y `src/frontend/src/types/`.

## Paso 9 — Pruebas (SK-09)

Leer el skill `sk-09-pruebas` y aplicarlo. Los tests van con la feature, no en un issue separado:

- Tests unitarios de la entidad de dominio (todas las invariantes y caminos de error).
- Tests unitarios del servicio con mocks NSubstitute del repositorio.
- Tests de integracion de los endpoints criticos con `WebApplicationFactory`.
- Tests de componente/servicio en frontend con Vitest.
- Ubicacion backend: `src/backend/Tests/`.
- Ubicacion frontend: `src/frontend/src/tests/`.

## Paso 10 — Verificacion DoD (SK-10)

Leer el skill `sk-10-documentacion-dod` y ejecutar el checklist completo antes de dar la tarea por terminada:

- Codigo: convenciones, capas, tipos, booleanas, fechas.
- Pruebas: unitarios, integracion, todos verdes.
- API: DTOs validados, codigos HTTP correctos.
- Frontend: tipos alineados, sin fetch directo en componentes.
- Commits: mensajes con formato `tipo(ambito): descripcion corta en castellano`.
- Documentacion: ADR si se tomo una decision tecnica significativa.

---

## Reglas de orquestacion

1. **Leer el skill antes de aplicarlo**: usar la herramienta de lectura de archivos para cargar cada SKILL.md antes de generar codigo del paso correspondiente.
2. **No saltar pasos**: si un paso previo no esta completo, completarlo antes de avanzar.
3. **No mezclar responsabilidades**: cada artefacto (entidad, DTO, mapeador, servicio, controlador, repositorio) va en su capa y carpeta correcta.
4. **Confirmar al usuario** al finalizar cada paso con un resumen de los artefactos creados antes de continuar al siguiente.
5. **Si el proyecto no existe** (paso 0 falla): crear el scaffolding completo antes de escribir una sola linea de logica.
