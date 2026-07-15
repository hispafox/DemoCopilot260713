param(
    [string]$Owner = "hispafox",
    [string]$Repo = "DemoCopilot260713",
    [string]$Token = $env:GITHUB_TOKEN
)

$ErrorActionPreference = "Stop"

if ([string]::IsNullOrWhiteSpace($Token)) {
    throw "Debes definir GITHUB_TOKEN o pasar -Token con un PAT que tenga permiso repo."
}

$encabezados = @{
    Authorization = "Bearer $Token"
    Accept = "application/vnd.github+json"
    "X-GitHub-Api-Version" = "2022-11-28"
}

$baseUrl = "https://api.github.com/repos/$Owner/$Repo"

Write-Host "Verificando acceso al repositorio $Owner/$Repo..."
$null = Invoke-RestMethod -Method Get -Uri $baseUrl -Headers $encabezados

Write-Host "Leyendo labels existentes..."
$labelsRepo = Invoke-RestMethod -Method Get -Uri "$baseUrl/labels?per_page=100" -Headers $encabezados
$nombresLabelsRepo = @{}
foreach ($label in $labelsRepo) {
    $nombresLabelsRepo[$label.name] = $true
}

Write-Host "Leyendo issues existentes para evitar duplicados..."
$issuesExistentes = Invoke-RestMethod -Method Get -Uri "$baseUrl/issues?state=all&per_page=100" -Headers $encabezados
$titulosExistentes = @{}
foreach ($issue in $issuesExistentes) {
    if (-not [string]::IsNullOrWhiteSpace($issue.title)) {
        $titulosExistentes[$issue.title.Trim()] = $true
    }
}

$issuesARegistrar = @(
    @{
        titulo = "[HU] incorporar categoria en dominio y persistencia"
        etiquetas = @("🔴 prioridad:alta", "📦 flujo:planificado", "tipo:historia-usuario", "enhancement")
        cuerpo = @"
## Historia de usuario

Como usuario de la aplicacion, quiero guardar una categoria en cada tarea, para identificar rapidamente el tipo de trabajo.

## Criterios de aceptacion

```gherkin
Escenario: crear tarea con categoria valida
  Dado que el usuario informa un titulo valido y una categoria valida
  Cuando registra la tarea
  Entonces la tarea queda persistida con la categoria informada

Escenario: rechazo por categoria vacia en creacion
  Dado que el usuario informa una categoria vacia o solo espacios
  Cuando intenta registrar la tarea
  Entonces el sistema responde con error de validacion

Escenario: migracion incremental sin perdida de datos
  Dado una base con tareas existentes previas al campo categoria
  Cuando se aplica la migracion
  Entonces el esquema queda actualizado sin eliminar datos existentes
```

## Capas afectadas

- [x] Domain
- [x] Application
- [x] Infrastructure
- [ ] Api
- [ ] Frontend
- [x] Tests

## Informacion adicional

- **Prioridad:** Must
- **Tamano estimado:** M
- **Dependencias:** ninguna
- **Documento de planificacion:** docs/planificacion-categorias-tareas-2026-07-15.md
"@
    },
    @{
        titulo = "[HU] exponer y validar categoria en contratos api"
        etiquetas = @("🔴 prioridad:alta", "📦 flujo:planificado", "tipo:historia-usuario", "enhancement")
        cuerpo = @"
## Historia de usuario

Como consumidor de la API, quiero enviar y recibir la categoria en los DTOs de tareas, para integrar correctamente la clasificacion en mis flujos.

## Criterios de aceptacion

```gherkin
Escenario: API retorna categoria en consultas
  Dado que existe una tarea con categoria registrada
  Cuando se consulta el listado o detalle de tareas
  Entonces la respuesta incluye la categoria en el DTO de salida

Escenario: API rechaza crear o actualizar sin categoria valida
  Dado una solicitud de crear o actualizar tarea sin categoria valida
  Cuando el backend procesa la solicitud
  Entonces responde 400 con detalle de validacion

Escenario: actualizacion de categoria en tarea existente
  Dado una tarea existente con categoria inicial
  Cuando se actualiza la tarea con otra categoria valida
  Entonces la categoria queda actualizada y ActualizadoEnUtc cambia en UTC
```

## Capas afectadas

- [ ] Domain
- [x] Application
- [ ] Infrastructure
- [x] Api
- [ ] Frontend
- [x] Tests

## Informacion adicional

- **Prioridad:** Must
- **Tamano estimado:** M
- **Dependencias:** HU-1
- **Documento de planificacion:** docs/planificacion-categorias-tareas-2026-07-15.md
"@
    },
    @{
        titulo = "[HU] permitir seleccionar y visualizar categoria en frontend"
        etiquetas = @("🟡 prioridad:media", "📦 flujo:planificado", "tipo:historia-usuario", "enhancement")
        cuerpo = @"
## Historia de usuario

Como usuario final, quiero seleccionar la categoria al crear o editar una tarea y verla en los listados, para organizar mi trabajo de forma visual.

## Criterios de aceptacion

```gherkin
Escenario: alta de tarea desde UI con categoria
  Dado que el usuario completa titulo y categoria en el formulario
  Cuando guarda la tarea
  Entonces la tarea se crea y la categoria se muestra en pantalla

Escenario: validacion visual por categoria faltante
  Dado que el usuario intenta guardar sin categoria
  Cuando envia el formulario
  Entonces la UI muestra mensaje de validacion y no envia la solicitud valida

Escenario: visualizacion consistente de categoria en listado y detalle
  Dado que existen tareas con categoria
  Cuando el usuario navega por listado y detalle
  Entonces la categoria se visualiza de manera consistente en ambas vistas
```

## Capas afectadas

- [ ] Domain
- [ ] Application
- [ ] Infrastructure
- [ ] Api
- [x] Frontend
- [x] Tests

## Informacion adicional

- **Prioridad:** Should
- **Tamano estimado:** M
- **Dependencias:** HU-2
- **Documento de planificacion:** docs/planificacion-categorias-tareas-2026-07-15.md
"@
    }
)

foreach ($item in $issuesARegistrar) {
    $titulo = $item.titulo.Trim()

    if ($titulosExistentes.ContainsKey($titulo)) {
        Write-Host "Duplicado detectado, se omite: $titulo"
        continue
    }

    $labelsValidas = @()
    foreach ($etiqueta in $item.etiquetas) {
        if ($nombresLabelsRepo.ContainsKey($etiqueta)) {
            $labelsValidas += $etiqueta
        }
        else {
            Write-Host "Label no encontrada en repo, se omite: $etiqueta"
        }
    }

    $payload = @{
        title = $titulo
        body = $item.cuerpo
        labels = $labelsValidas
    } | ConvertTo-Json -Depth 10

    $creado = Invoke-RestMethod -Method Post -Uri "$baseUrl/issues" -Headers $encabezados -Body $payload
    Write-Host "Issue creado: #$($creado.number) $($creado.html_url)"
}

Write-Host "Proceso finalizado."
