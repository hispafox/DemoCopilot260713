# Ejecucion de creacion de issues de categorizacion

## Requisitos

- Tener un token de GitHub con permisos sobre el repositorio.
- Definir la variable de entorno GITHUB_TOKEN en la sesion.

## Ejecucion

Desde la raiz del repositorio:

```powershell
$env:GITHUB_TOKEN = "<tu_token>"
./scripts/crear-issues-categorizacion-tareas.ps1
```

## Comportamiento

- Verifica acceso al repo hispafox/DemoCopilot260713.
- Lee labels existentes y omite labels faltantes.
- Lee issues existentes y omite duplicados por titulo exacto.
- Crea los 3 issues de historias de usuario de categorizacion.

## Salida esperada

Por cada issue nuevo:

```text
Issue creado: #<numero> <url>
```

Si ya existe por titulo:

```text
Duplicado detectado, se omite: [HU] ...
```
