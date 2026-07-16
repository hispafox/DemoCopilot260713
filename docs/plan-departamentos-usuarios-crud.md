# Planificacion: asociacion de departamento por usuario y CRUD de departamentos

**Fecha:** 2026-07-16
**Estado:** borrador
**Autor:** planificador-democopilot
**Referencia base:** docs/requisitos/documento-requisitos-aplicacion.md

## 1. Resumen ejecutivo

Se planifica incorporar la capacidad de asociar cada usuario a un departamento y habilitar la gestion completa de departamentos (CRUD) como nueva opcion del menu en frontend.

El alcance incluye:
- Backend: modelo, logica de aplicacion, endpoints y persistencia para departamentos y asociacion con usuarios.
- Frontend: nueva opcion de menu y pantallas para listar, crear, editar y eliminar departamentos.
- Pruebas: unitarias, integracion y frontend para cubrir escenarios felices, errores y bordes.

## 2. Contexto y trazabilidad

La referencia funcional y de calidad base es docs/requisitos/documento-requisitos-aplicacion.md, especialmente:
- RF-08 (uso de DTOs y no exponer entidades de persistencia).
- RF-10 (capa de servicios en frontend para consumo API).
- RNF-01 (separacion por capas).
- RNF-02 (nomenclatura y consistencia en castellano).
- RNF-03 (pruebas obligatorias por funcionalidad).
- RNF-05 (codigos HTTP semanticos).
- RNF-06 (validacion de entradas).
- DoD (criterios de completitud con pruebas y arquitectura).

### Trazabilidad minima: requisito -> capa afectada

| Requisito | Domain | Application | Infrastructure | Api | Frontend | Tests |
|---|---|---|---|---|---|---|
| RF-DPT-01: gestionar departamentos (alta, consulta, actualizacion, baja) | X | X | X | X | X | X |
| RF-DPT-02: asociar cada usuario a un departamento | X | X | X | X | X | X |
| RF-DPT-03: exponer contratos mediante DTOs para departamentos y usuarios |  | X |  | X | X | X |
| RF-DPT-04: mostrar opcion Departamentos en menu y permitir CRUD desde UI |  |  |  |  | X | X |
| RNF-ARQ-01: mantener separacion de capas | X | X | X | X | X | X |
| RNF-CAL-01: incluir pruebas backend y frontend |  |  |  |  |  | X |

### Supuestos explicitos

- Existe una entidad de usuario ya disponible en el sistema y es editable.
- La asociacion inicial es 1 departamento -> N usuarios, y cada usuario tiene exactamente 1 departamento.
- No se requiere jerarquia de departamentos ni metadatos avanzados en esta iteracion.
- Si un departamento tiene usuarios asociados, su eliminacion requerira validacion de negocio (bloquear borrado o reasignar), a confirmar en refinamiento.

## 3. Historias de usuario

### HU-01: modelar departamento y asociacion con usuario

- **Historia:** Como administrador funcional, quiero que cada usuario tenga un departamento asociado, para organizar la informacion por estructura interna.
- **Prioridad:** Must
- **Tamano:** M
- **Dependencias:** disponibilidad del modulo de usuarios actual
- **Supuestos:** la entidad de usuario existe y puede evolucionar contrato/API

#### Criterios de aceptacion

```gherkin
Escenario: asociar departamento valido a un usuario
  Dado que existe un departamento activo
  Y existe un usuario editable
  Cuando se guarda el usuario con ese departamento
  Entonces la asociacion usuario-departamento queda persistida
  Y la API devuelve un resultado exitoso con datos actualizados

Escenario: rechazo por departamento inexistente
  Dado que existe un usuario editable
  Cuando se intenta asociar un identificador de departamento inexistente
  Entonces la API devuelve error de validacion o recurso no encontrado
  Y no se persiste ningun cambio en el usuario

Escenario: consulta de usuario con departamento
  Dado que un usuario tiene departamento asociado
  Cuando se consulta el detalle del usuario
  Entonces la respuesta incluye los datos del departamento asociado
```

#### Capas afectadas

- [x] Domain
- [x] Application
- [x] Infrastructure
- [x] Api
- [x] Frontend
- [x] Tests

### HU-02: CRUD de departamentos en backend

- **Historia:** Como administrador funcional, quiero gestionar departamentos en la API, para mantener actualizado el catalogo organizativo.
- **Prioridad:** Must
- **Tamano:** M
- **Dependencias:** HU-01
- **Supuestos:** se define un identificador unico y nombre obligatorio para departamento

#### Criterios de aceptacion

```gherkin
Escenario: crear departamento correctamente
  Dado que envio un nombre de departamento valido
  Cuando invoco el endpoint de alta de departamentos
  Entonces la API crea el departamento
  Y responde con codigo semantico de exito

Escenario: validacion de nombre obligatorio
  Dado que envio un nombre vacio o nulo
  Cuando invoco el endpoint de alta o actualizacion
  Entonces la API responde con error de validacion
  Y el departamento no se guarda

Escenario: eliminar departamento sin usuarios asociados
  Dado que existe un departamento sin usuarios asociados
  Cuando solicito su eliminacion
  Entonces la API elimina el recurso
  Y responde con codigo 204
```

#### Capas afectadas

- [x] Domain
- [x] Application
- [x] Infrastructure
- [x] Api
- [ ] Frontend
- [x] Tests

### HU-03: opcion de menu y CRUD de departamentos en frontend

- **Historia:** Como usuario de la aplicacion, quiero una opcion Departamentos en el menu con pantallas CRUD, para administrar departamentos desde la interfaz.
- **Prioridad:** Must
- **Tamano:** M
- **Dependencias:** HU-02
- **Supuestos:** existe un menu principal extensible y patron de navegacion ya establecido

#### Criterios de aceptacion

```gherkin
Escenario: acceso al modulo desde menu
  Dado que el usuario navega en la aplicacion
  Cuando visualiza el menu principal
  Entonces aparece la opcion Departamentos
  Y puede acceder al listado de departamentos

Escenario: error de validacion en formulario
  Dado que el usuario abre el formulario de departamento
  Cuando intenta guardar con nombre vacio
  Entonces la interfaz muestra mensaje de validacion
  Y no ejecuta guardado exitoso

Escenario: actualizacion de listado tras alta
  Dado que el usuario crea un departamento valido
  Cuando la operacion finaliza correctamente
  Entonces el listado refleja el nuevo departamento sin inconsistencias
```

#### Capas afectadas

- [ ] Domain
- [ ] Application
- [ ] Infrastructure
- [ ] Api
- [x] Frontend
- [x] Tests

### HU-04: reglas de integridad para borrado de departamentos

- **Historia:** Como administrador funcional, quiero reglas claras al eliminar departamentos con usuarios asociados, para evitar estados inconsistentes.
- **Prioridad:** Should
- **Tamano:** S
- **Dependencias:** HU-01, HU-02
- **Supuestos:** la regla final de negocio se define en refinamiento (bloquear o requerir reasignacion)

#### Criterios de aceptacion

```gherkin
Escenario: bloqueo de eliminacion con usuarios asociados
  Dado que un departamento tiene usuarios asociados
  Cuando se solicita su eliminacion
  Entonces la API rechaza la operacion con codigo semantico de negocio
  Y comunica el motivo al frontend

Escenario: eliminacion permitida cuando no hay usuarios asociados
  Dado que un departamento no tiene usuarios asociados
  Cuando se solicita su eliminacion
  Entonces la operacion se completa correctamente

Escenario: mensaje de error claro en UI
  Dado que el backend rechaza la eliminacion por integridad
  Cuando el frontend recibe el error
  Entonces se muestra un mensaje entendible para el usuario
```

#### Capas afectadas

- [x] Domain
- [x] Application
- [x] Infrastructure
- [x] Api
- [x] Frontend
- [x] Tests

## 4. Pruebas requeridas por alcance

### Backend

- Pruebas unitarias de servicios de aplicacion para:
  - alta/edicion con validaciones de nombre
  - asociacion usuario-departamento
  - regla de eliminacion con y sin usuarios asociados
- Pruebas de integracion para endpoints criticos de departamentos y actualizacion de usuario con departamento.

### Frontend

- Pruebas de componentes para:
  - visualizacion de opcion Departamentos en menu
  - formularios de alta/edicion con validacion
  - feedback de errores de backend
- Pruebas de comportamiento basico del flujo CRUD (listar, crear, editar, eliminar) con servicios mockeados.

## 5. Riesgos y vacios

- Definir si la eliminacion de departamentos con usuarios debe bloquearse siempre o permitir reasignacion guiada.
- Confirmar si el sistema maneja un unico tipo de usuario o multiples perfiles con permisos para administrar departamentos.
- Verificar impacto de migraciones en datos existentes de usuarios sin departamento.

## 6. Propuesta de orden de implementacion

1. HU-01 (modelo y asociacion en usuario), para establecer base de dominio y persistencia.
2. HU-02 (CRUD backend de departamentos), para habilitar contratos y validaciones.
3. HU-03 (menu + CRUD frontend), para exponer la capacidad al usuario final.
4. HU-04 (reglas de integridad de borrado), para cerrar consistencia funcional y mensajes.
