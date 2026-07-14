# Catalogo de skills reutilizables

## 1. Objetivo
Definir un conjunto pequeno de skills genericas y reutilizables para cualquier dominio del proyecto (no solo Tareas), evitando skills atadas a una tabla, un endpoint o un DTO concreto.

## 2. Principios de reutilizacion
- Una skill debe aplicarse a multiples modulos/entidades.
- La skill describe un patron de trabajo, no una implementacion puntual.
- Debe poder usarse en backend y/o frontend segun corresponda.
- Si una skill solo sirve para un caso concreto, no entra al catalogo.

## 3. Catalogo reducido (12 skills)

### SK-00 - Creacion y scaffolding del proyecto
- Para que sirve: crear la estructura inicial del proyecto (backend .NET, frontend React+Vite, configuracion de workspace) a partir de cero antes de escribir cualquier codigo de dominio o de API.
- Entradas: stack tecnologico elegido, nombre del proyecto y estructura de carpetas deseada.
- Salidas: proyecto compilable y ejecutable localmente, con estructura de carpetas correcta, proxy configurado y listo para aplicar los skills siguientes.
- Reutilizacion: alta (aplica cada vez que se arranca un proyecto nuevo con este stack).

### SK-01 - Modelado de dominio y reglas
- Para que sirve: definir entidades, invariantes y reglas de negocio sin acoplarlas a la API ni a EF.
- Entradas: requerimientos funcionales de cualquier modulo.
- Salidas: modelo de dominio consistente, reglas explicitadas y validables.
- Reutilizacion: alta (aplica a cualquier entidad).

### SK-02 - Diseno de contratos y DTOs
- Para que sirve: crear contratos de entrada/salida estables y desacoplados del modelo de persistencia.
- Entradas: casos de uso y reglas de negocio.
- Salidas: DTOs de comando/consulta, convenciones de nombres y versionado basico.
- Reutilizacion: alta.

### SK-03 - Mapeo entre capas
- Para que sirve: transformar dominio, DTOs y entidades de persistencia de forma estandar.
- Entradas: modelos de cada capa.
- Salidas: mapeadores reutilizables y faciles de probar.
- Reutilizacion: alta.

### SK-04 - Implementacion de casos de uso
- Para que sirve: estructurar logica de aplicacion en servicios/casos de uso sin mezclar responsabilidades.
- Entradas: contrato de caso de uso.
- Salidas: servicios de aplicacion con flujo claro y testeable.
- Reutilizacion: alta.

### SK-05 - API REST consistente
- Para que sirve: exponer endpoints con convenciones uniformes (rutas, codigos HTTP, validaciones, errores).
- Entradas: casos de uso y contratos.
- Salidas: controladores/endpoints consistentes y previsibles.
- Reutilizacion: alta.

### SK-06 - Persistencia y evolucion de esquema
- Para que sirve: implementar acceso a datos y migraciones incrementales sin romper entornos.
- Entradas: modelo de datos y patrones de consulta.
- Salidas: configuracion EF Core, migraciones seguras, indices y restricciones.
- Reutilizacion: alta.

### SK-07 - Trazabilidad temporal y auditoria UTC
- Para que sirve: estandarizar manejo de fechas/auditoria en UTC e ISO 8601.
- Entradas: operaciones de escritura y contratos de salida.
- Salidas: reglas comunes de timestamp y serializacion temporal.
- Reutilizacion: media-alta.

### SK-08 - Cliente frontend y sincronizacion de tipos
- Para que sirve: centralizar cliente HTTP y mantener tipos frontend alineados con contratos backend.
- Entradas: especificacion de API.
- Salidas: capa de servicios frontend tipada y manejo de errores comun.
- Reutilizacion: alta.

### SK-09 - Estrategia de pruebas por capa
- Para que sirve: definir como probar dominio, aplicacion, API y UI sin duplicidades.
- Entradas: arquitectura y casos de uso.
- Salidas: matriz de pruebas (unitarias, integracion, componente) y criterios de cobertura minima.
- Reutilizacion: alta.

### SK-10 - Documentacion operativa y DoD
- Para que sirve: estandarizar documentacion tecnica, criterios de aceptacion y Definition of Done.
- Entradas: cambios funcionales y tecnicos.
- Salidas: plantillas de decision tecnica, checklist de entrega y guia de evolucion.
- Reutilizacion: alta.

### SK-11 - Verificacion integral de la aplicacion
- Para que sirve: verificar de forma objetiva que backend, frontend, persistencia y contratos funcionan en conjunto antes de cerrar una feature.
- Entradas: implementacion completa del modulo y suite de pruebas disponible.
- Salidas: evidencia de verificacion (tests backend/frontend, build frontend, validacion de migraciones y endpoints criticos).
- Reutilizacion: alta.

## 4. Priorizacion recomendada

## Fase A - Fundacion
- SK-00, SK-01, SK-02, SK-03, SK-04, SK-05, SK-06.

## Fase B - Robustez
- SK-07, SK-08, SK-09.

## Fase C - Estandar operativo
- SK-10.

## Fase D - Cierre con evidencia
- SK-11.

## 5. Lo que se elimina respecto al enfoque anterior
- Skills por endpoint concreto (por ejemplo, "endpoint de listado").
- Skills por DTO concreto (por ejemplo, "DTO de crear X").
- Skills por tabla o entidad especifica.

Todo eso pasa a ser implementacion de una skill generica, no una skill nueva.

## 6. Regla practica para decidir si crear una skill nueva
Solo crear una skill nueva si cumple las 3 condiciones:

- Se reutiliza en al menos 3 modulos o contextos.
- Introduce una capacidad distinta (no una variante menor).
- Requiere criterios de calidad propios.

Si no cumple las 3, debe resolverse dentro de una skill existente.

## 7. Plantilla corta para especificar cada skill
- Nombre.
- Alcance reutilizable.
- Entradas y salidas.
- Limites (que no cubre).
- Criterios de aceptacion.
- Pruebas minimas.
