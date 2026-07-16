# Informe de calidad de la aplicacion

Fecha: 2026-07-15
Proyecto: DemoCopilot260713

## Valoracion global

Puntuacion final: **6.7/10**

### Criterio de calculo (ponderado)

1. Seguridad y dependencias: 25%
2. Arquitectura y separacion de capas: 25%
3. Cumplimiento de requisitos funcionales: 20%
4. Testing y confiabilidad: 20%
5. Mantenibilidad y code smells: 10%

### Resultado por dimension

1. Seguridad: 4.5/10
2. Arquitectura: 8.0/10
3. Requisitos funcionales: 6.0/10
4. Testing: 7.0/10
5. Mantenibilidad: 7.0/10

## Hallazgos principales (por severidad)

### 1) Critico: vulnerabilidades altas en dependencias NuGet

- Evidencia en:
  - src/backend/Infrastructure/AplicacionTareas.Infrastructure.csproj
  - src/backend/Api/AplicacionTareas.Api.csproj
  - src/backend/Tests/AplicacionTareas.Tests.csproj
- En compilacion/restauracion se reportan avisos NU1903 para paquetes con severidad alta (Microsoft.OpenApi 2.0.0 y SQLitePCLRaw.lib.e_sqlite3 2.1.11).
- Impacto: riesgo de seguridad real en cadena de dependencias.

### 2) Alta: exposicion de mensajes internos de excepcion en API

- Evidencia en src/backend/Api/Middleware/ManejadorExcepcionesMiddleware.cs.
- Se devuelve directamente el mensaje de excepcion al cliente.
- Impacto: posible filtracion de detalles internos.

### 3) Alta: gap funcional respecto a requisitos (edicion en frontend)

- Requisito esperado: PUT /api/tareas/{id} en docs/documento-requisitos-aplicacion.md.
- En UI solo se observa flujo de completar/eliminar en src/frontend/src/App.tsx.
- Impacto: cumplimiento parcial de HU-04/RF-04 desde experiencia de usuario.

### 4) Media: cobertura de tests de API incompleta en escenarios criticos

- Existe endpoint de actualizacion en src/backend/Api/Controladores/TareasControlador.cs.
- Tests API actuales no cubren de forma explicita PUT exito/404 y validaciones asociadas.
- Impacto: riesgo de regresiones en contrato HTTP.

### 5) Media: regla temporal acoplada al reloj del sistema

- Evidencia en src/backend/Domain/Tarea.cs (uso de DateTime.UtcNow para validaciones).
- Impacto: menor testabilidad y potencial fragilidad en bordes temporales.

### 6) Media: criterio UTC no explicito en visualizacion frontend

- Evidencia en src/frontend/src/App.tsx (toLocaleString).
- Impacto: ambiguedad funcional respecto a convencion UTC/ISO 8601.

### 7) Baja: componente App concentra demasiada responsabilidad

- Evidencia en src/frontend/src/App.tsx.
- Impacto: mantenibilidad y escalabilidad limitadas a medio plazo.

## Aspectos positivos

1. Separacion de capas backend bien encaminada (Domain/Application/Infrastructure/API).
2. DTOs y mapeo explicito sin exponer entidad EF directamente.
3. Persistencia con indices y lecturas AsNoTracking.
4. Middleware centralizado de excepciones ya integrado.
5. Sanitizacion HTML en frontend para contenido enriquecido.
6. Pruebas ejecutadas en verde:
   - Backend: 18/18 OK.
   - Lint frontend: OK.

## Conclusiones

La base arquitectonica es buena y mantenible, pero la nota global baja por dos frentes prioritarios:

1. Seguridad de dependencias.
2. Cumplimiento funcional parcial en frontend (falta edicion).

Corregidos esos puntos y ampliando pruebas de contrato, el proyecto puede subir a un rango cercano a 8/10.

## Acciones prioritarias recomendadas

1. Actualizar dependencias vulnerables y repetir restauracion + tests.
2. Endurecer manejo de errores para no exponer mensajes internos.
3. Implementar edicion de tarea en frontend para cubrir RF-04/HU-04.
4. Agregar tests API para PUT exito, PUT 404 y validaciones 400.
5. Definir y explicitar politica de visualizacion temporal (UTC o local etiquetado).
