---
name: sk-11-verificacion-integral
description: "Define la verificacion integral de la aplicacion (backend, frontend, contratos, migraciones y calidad) antes de cerrar una feature o hito."
---

# SK-11 - Verificacion integral de la aplicacion

Estandariza como comprobar que una implementacion realmente funciona de punta a punta sin depender de verificacion manual ad-hoc.

## Prerequisito: el proyecto debe existir

Antes de aplicar este skill, verificar que la estructura del proyecto esta creada:

```powershell
Test-Path src/backend -PathType Container
Test-Path src/frontend -PathType Container
```

Si alguno devuelve `False`, detener y aplicar SK-00 primero.

## Cuando usar este skill

- Siempre al finalizar un modulo completo.
- Antes de cerrar una tarea o hito.
- Antes de abrir o fusionar un PR.
- Cuando se corrige un bug de infraestructura o datos (ejemplo: tabla inexistente).

## Objetivo

Validar que backend, frontend, persistencia y pruebas estan alineados y funcionando en conjunto.

## Secuencia de verificacion recomendada

### 1) Backend: compilacion y pruebas

```powershell
Set-Location src/backend
dotnet test AplicacionTareas.slnx
```

Criterio de salida:
- Todos los tests en verde.
- Sin errores de compilacion.

### 2) Frontend: pruebas de componente/servicio

```powershell
Set-Location src/frontend
npm run test -- --run
```

Criterio de salida:
- Todos los tests en verde.

### 3) Frontend: build de produccion

```powershell
Set-Location src/frontend
npm run build
```

Criterio de salida:
- Build completado sin errores.

### 4) Persistencia: validacion de migraciones y esquema

Reglas:
- Confirmar que existe al menos una migracion para la entidad nueva o modificada.
- Confirmar que la API aplica migraciones al arranque con `Database.Migrate()`.
- Confirmar que los tests de integracion fallarian si falta la tabla.

### 5) Verificacion funcional minima por API

Cobertura minima esperada:
- GET lista.
- POST crear.
- PATCH accion principal (por ejemplo, completar).
- DELETE.

La evidencia puede venir de tests de integracion automatizados.

### 6) Verificacion de contratos backend-frontend

- Tipos frontend alineados con DTOs de backend.
- Sin uso de `any` para contratos de API.
- Cliente HTTP centralizado y manejo de error consistente.

## Matriz minima de evidencia

- Resultado de `dotnet test`.
- Resultado de `npm run test -- --run`.
- Resultado de `npm run build`.
- Evidencia de migracion y aplicacion de esquema.
- Lista de endpoints criticos cubiertos por tests de integracion.

## Checklist de aprobacion

- [ ] Backend compila y todos los tests pasan.
- [ ] Frontend tests pasan.
- [ ] Frontend build pasa.
- [ ] Migraciones incrementales presentes y consistentes.
- [ ] Riesgo de "no such table" mitigado (migraciones al arranque + test smoke).
- [ ] Contratos backend/frontend alineados.
- [ ] No quedan errores bloqueantes en la funcionalidad entregada.

## Errores comunes a evitar

- Cerrar una feature solo con prueba manual local.
- Considerar valido un cambio sin ejecutar tests de backend y frontend.
- No validar migraciones tras cambios de persistencia.
- Dar por buena la API sin cobertura de endpoints criticos.

## Relacion con otros skills

- SK-06 (Persistencia): verifica que el esquema se aplique correctamente.
- SK-09 (Pruebas): usa la estrategia de pruebas por capa como base.
- SK-10 (Documentacion y DoD): aporta evidencia objetiva para cerrar la entrega.
