---
name: sk-00-scaffolding-proyecto
description: "Crea la estructura inicial del proyecto (backend ASP.NET Core, frontend React+Vite+TypeScript, configuracion de workspace) desde cero. Prerequisito obligatorio antes de aplicar cualquier otro skill."
---

# SK-00 - Creacion y scaffolding del proyecto

Genera la estructura inicial completa del proyecto: backend .NET, frontend React+Vite+TypeScript, configuracion de workspace y proxy. Es el punto de partida obligatorio antes de aplicar SK-01 en adelante.

## Cuando usar este skill

- Al arrancar un proyecto desde un repositorio vacio o con solo documentacion.
- Antes de aplicar cualquier otro skill del catalogo (SK-01..SK-10).
- Al necesitar regenerar la estructura de un proyecto existente que se ha desviado de la convencion.

## Prerequisitos del entorno

Verificar que estas herramientas esten disponibles antes de ejecutar:

| Herramienta | Comando de verificacion | Version minima |
|-------------|-------------------------|----------------|
| .NET SDK | `dotnet --version` | 10.x |
| Node.js | `node --version` | 20.x LTS |
| npm | `npm --version` | 10.x |
| Git | `git --version` | cualquiera |

## Estructura de carpetas objetivo

```
raiz-del-repo/
  src/
    backend/
      Api/                  <- Proyecto ASP.NET Core Web API
      Application/          <- Servicios de aplicacion (casos de uso)
      Domain/               <- Entidades y reglas de negocio
      Infrastructure/       <- DbContext, migraciones EF, repositorios
      Tests/                <- xUnit: unitarios e integracion
    frontend/               <- Proyecto React + Vite + TypeScript
      src/
        components/
        pages/
        services/
        types/
        tests/
  docs/
  .github/
```

## Proceso paso a paso

### 1. Crear proyectos de backend

```powershell
# Desde la raiz del repo
dotnet new sln -n NombreProyecto -o src/backend

# Proyecto Web API (sin controladores, usando minimal API o con controladores segun preferencia)
dotnet new webapi -n NombreProyecto.Api -o src/backend/Api --use-controllers

# Proyectos de biblioteca para capas internas
dotnet new classlib -n NombreProyecto.Application -o src/backend/Application
dotnet new classlib -n NombreProyecto.Domain       -o src/backend/Domain
dotnet new classlib -n NombreProyecto.Infrastructure -o src/backend/Infrastructure

# Proyecto de tests
dotnet new xunit -n NombreProyecto.Tests -o src/backend/Tests

# Agregar todos los proyectos a la solucion
cd src/backend
dotnet sln add Api/NombreProyecto.Api.csproj
dotnet sln add Application/NombreProyecto.Application.csproj
dotnet sln add Domain/NombreProyecto.Domain.csproj
dotnet sln add Infrastructure/NombreProyecto.Infrastructure.csproj
dotnet sln add Tests/NombreProyecto.Tests.csproj
cd ../..
```

### 2. Establecer referencias entre proyectos de backend

```powershell
cd src/backend

# Api depende de Application e Infrastructure
dotnet add Api/NombreProyecto.Api.csproj reference Application/NombreProyecto.Application.csproj
dotnet add Api/NombreProyecto.Api.csproj reference Infrastructure/NombreProyecto.Infrastructure.csproj

# Application depende de Domain
dotnet add Application/NombreProyecto.Application.csproj reference Domain/NombreProyecto.Domain.csproj

# Infrastructure depende de Domain y Application
dotnet add Infrastructure/NombreProyecto.Infrastructure.csproj reference Domain/NombreProyecto.Domain.csproj
dotnet add Infrastructure/NombreProyecto.Infrastructure.csproj reference Application/NombreProyecto.Application.csproj

# Tests depende de todos (para pruebas unitarias e integracion)
dotnet add Tests/NombreProyecto.Tests.csproj reference Api/NombreProyecto.Api.csproj
dotnet add Tests/NombreProyecto.Tests.csproj reference Application/NombreProyecto.Application.csproj
dotnet add Tests/NombreProyecto.Tests.csproj reference Domain/NombreProyecto.Domain.csproj

cd ../..
```

### 3. Instalar paquetes NuGet esenciales

```powershell
cd src/backend

# EF Core + SQLite en Infrastructure
dotnet add Infrastructure/NombreProyecto.Infrastructure.csproj package Microsoft.EntityFrameworkCore
dotnet add Infrastructure/NombreProyecto.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Sqlite
dotnet add Infrastructure/NombreProyecto.Infrastructure.csproj package Microsoft.EntityFrameworkCore.Design

# EF herramientas en Api (necesario para migraciones via dotnet ef)
dotnet add Api/NombreProyecto.Api.csproj package Microsoft.EntityFrameworkCore.Design

# Mocks para tests
dotnet add Tests/NombreProyecto.Tests.csproj package NSubstitute
dotnet add Tests/NombreProyecto.Tests.csproj package Microsoft.AspNetCore.Mvc.Testing

cd ../..
```

### 4. Verificar que el backend compila

```powershell
dotnet build src/backend/NombreProyecto.sln
```

El resultado esperado es `Build succeeded` sin errores.

### 5. Crear proyecto de frontend

```powershell
# Desde la raiz del repo
npm create vite@latest src/frontend -- --template react-ts
cd src/frontend
npm install
cd ../..
```

### 6. Configurar proxy de Vite hacia el backend HTTPS

Editar `src/frontend/vite.config.ts`:

```typescript
import { defineConfig } from 'vite'
import react from '@vitejs/plugin-react'

export default defineConfig({
  plugins: [react()],
  server: {
    proxy: {
      '/api': {
        target: 'https://localhost:5001',
        changeOrigin: true,
        secure: false  // necesario con dev cert de .NET
      }
    }
  }
})
```

> **Importante**: `secure: false` es obligatorio cuando el backend usa el certificado de desarrollo de .NET. Sin este flag, el browser pierde el header `Authorization` en el redirect 307 y las peticiones autenticadas fallan con 401.

### 7. Crear estructura de carpetas del frontend

```powershell
$base = "src/frontend/src"
New-Item -ItemType Directory -Force -Path "$base/components"
New-Item -ItemType Directory -Force -Path "$base/pages"
New-Item -ItemType Directory -Force -Path "$base/services"
New-Item -ItemType Directory -Force -Path "$base/types"
New-Item -ItemType Directory -Force -Path "$base/tests"
```

### 8. Configurar launchSettings.json del backend

En `src/backend/Api/Properties/launchSettings.json`, asegurarse de que el perfil HTTPS usa el puerto 5001:

```json
{
  "profiles": {
    "https": {
      "commandName": "Project",
      "applicationUrl": "https://localhost:5001;http://localhost:5000",
      "environmentVariables": {
        "ASPNETCORE_ENVIRONMENT": "Development"
      }
    }
  }
}
```

### 9. Verificacion final

```powershell
# Backend: debe compilar sin errores
dotnet build src/backend/NombreProyecto.sln

# Frontend: debe instalar sin errores
cd src/frontend && npm install && cd ../..
```

## Bloque de prerequisito para otros skills

Todos los skills del catalogo (SK-01..SK-10) deben incluir al inicio la siguiente comprobacion. Copiar este bloque en cada nuevo skill:

```powershell
# Comprobar que el proyecto existe antes de continuar
if (-not (Test-Path src/backend -PathType Container) -or -not (Test-Path src/frontend -PathType Container)) {
    Write-Error "El proyecto no existe. Aplicar SK-00 (sk-00-scaffolding-proyecto) antes de continuar."
    exit 1
}
```

Si la comprobacion falla, el agente debe detenerse e indicar al usuario que ejecute SK-00 primero.

## Checklist de finalizacion

- [ ] `dotnet build` pasa sin errores ni warnings de referencia.
- [ ] Las referencias entre proyectos respetan la direccion correcta (Api → Application → Domain, Infrastructure → Domain).
- [ ] El frontend tiene `vite.config.ts` con proxy apuntando a `https://localhost:5001` y `secure: false`.
- [ ] Las carpetas `components/`, `pages/`, `services/`, `types/` y `tests/` existen en `src/frontend/src/`.
- [ ] La solucion `.sln` incluye todos los proyectos.
- [ ] No hay logica de negocio ni de datos en el proyecto `Api` todavia (solo configuracion de arranque).

## Errores comunes a evitar

- **Crear todo en un solo proyecto**: la capa de dominio y la de infraestructura deben estar separadas desde el inicio.
- **Frontend apuntando a HTTP cuando el backend usa HTTPS**: causa redirects 307 que rompen las peticiones autenticadas.
- **No agregar los proyectos a la solucion .sln**: `dotnet build` desde la raiz no los encontrara.
- **Instalar paquetes EF en el proyecto Domain**: Domain no debe tener dependencias de EF ni de infraestructura.

## Relacion con otros skills

- **SK-01 (Modelado de dominio)**: una vez creada la carpeta `Domain/`, este skill define que va dentro.
- **SK-06 (Persistencia)**: usa la carpeta `Infrastructure/` y el paquete EF ya instalado por SK-00.
- **SK-08 (Cliente frontend)**: usa la carpeta `services/` y la configuracion de proxy ya establecida por SK-00.
- **SK-09 (Pruebas)**: usa el proyecto `Tests/` y las dependencias de test ya instaladas por SK-00.
