---
name: sk-06-persistencia
description: "Implementa acceso a datos y migraciones incrementales con EF Core + SQLite sin romper entornos. Configuracion, indices, restricciones y evolucion segura del esquema."
---

# SK-06 - Persistencia y evolucion de esquema

Define como implementar el acceso a datos con EF Core + SQLite: DbContext, configuracion de entidades, repositorios, migraciones incrementales e indices. El esquema evoluciona de forma segura sin destruir datos existentes.

## Prerequisito: el proyecto debe existir

Antes de aplicar este skill, verificar que la estructura del proyecto esta creada:

```powershell
Test-Path src/backend -PathType Container
Test-Path src/frontend -PathType Container
```

Si alguno devuelve `False`, **detener y aplicar SK-00 primero** (`sk-00-scaffolding-proyecto`).

SK-06 requiere que existan el dominio (SK-01) y la interfaz del repositorio en Application (SK-04). El dominio define la forma de los datos; SK-06 los persiste.

## Cuando usar este skill

- Al implementar el repositorio que satisface la interfaz definida en SK-04.
- Al necesitar crear o modificar el esquema de base de datos.
- Al agregar indices o restricciones a una tabla existente.
- Al corregir un esquema sin borrar la base de datos.

## Objetivo

Producir una capa de persistencia donde:
- El DbContext esta en `Infrastructure`, nunca en `Api` ni en `Application`.
- Las migraciones son incrementales y compatibles con entornos existentes.
- Se usa `AsNoTracking` en todas las consultas de solo lectura.
- Los indices cubren los patrones de consulta frecuentes.

## Estructura esperada

### DbContext

```csharp
// src/backend/Infrastructure/Datos/AplicacionDbContext.cs
public sealed class AplicacionDbContext : DbContext
{
    public AplicacionDbContext(DbContextOptions<AplicacionDbContext> opciones)
        : base(opciones) { }

    public DbSet<EntidadTarea> Tareas => Set<EntidadTarea>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AplicacionDbContext).Assembly);
    }
}
```

### Configuracion de entidad (IEntityTypeConfiguration)

```csharp
// src/backend/Infrastructure/Datos/Configuraciones/TareaConfiguracion.cs
public sealed class TareaConfiguracion : IEntityTypeConfiguration<EntidadTarea>
{
    public void Configure(EntityTypeBuilder<EntidadTarea> builder)
    {
        builder.ToTable("Tareas");
        builder.HasKey(t => t.Id);

        builder.Property(t => t.Titulo)
               .IsRequired()
               .HasMaxLength(200);

        builder.Property(t => t.Descripcion)
               .HasMaxLength(2000);

        builder.Property(t => t.EstaCompletada)
               .IsRequired()
               .HasDefaultValue(false);

        builder.Property(t => t.CreadoEnUtc)
               .IsRequired();

        builder.Property(t => t.ActualizadoEnUtc)
               .IsRequired();

        // Indice para consultas frecuentes por estado
        builder.HasIndex(t => t.EstaCompletada)
               .HasDatabaseName("IX_Tareas_EstaCompletada");
    }
}
```

### Repositorio

```csharp
// src/backend/Infrastructure/Repositorios/TareasRepositorio.cs
public sealed class TareasRepositorio : ITareasRepositorio
{
    private readonly AplicacionDbContext _contexto;

    public TareasRepositorio(AplicacionDbContext contexto)
    {
        _contexto = contexto;
    }

    public async Task<IEnumerable<EntidadTarea>> ObtenerTodasAsync() =>
        await _contexto.Tareas.AsNoTracking().ToListAsync();

    public async Task<EntidadTarea?> ObtenerPorIdAsync(int id) =>
        await _contexto.Tareas.FindAsync(id);

    public async Task AgregarAsync(EntidadTarea tarea)
    {
        await _contexto.Tareas.AddAsync(tarea);
        await _contexto.SaveChangesAsync();
    }

    public async Task EliminarAsync(EntidadTarea tarea)
    {
        _contexto.Tareas.Remove(tarea);
        await _contexto.SaveChangesAsync();
    }

    public async Task GuardarCambiosAsync() =>
        await _contexto.SaveChangesAsync();
}
```

### Registro en Program.cs

```csharp
// src/backend/Api/Program.cs
builder.Services.AddDbContext<AplicacionDbContext>(opciones =>
    opciones.UseSqlite(builder.Configuration.GetConnectionString("BaseDatos")
        ?? "Data Source=tareas.db"));
```

### appsettings.json

```json
{
  "ConnectionStrings": {
    "BaseDatos": "Data Source=tareas.db"
  }
}
```

## Migraciones: reglas de oro

1. **Nunca borrar la base de datos para corregir el esquema** salvo instruccion explicita.
2. Ante un campo nuevo: `ALTER TABLE` via migracion incremental con valor por defecto si el campo es NOT NULL.
3. Ante un campo eliminado: migracion `DROP COLUMN` solo si ningun entorno tiene datos criticos en ese campo.
4. **Comando para crear una migracion**:
   ```powershell
   dotnet ef migrations add NombreMigracion --project src/backend/Infrastructure --startup-project src/backend/Api
   ```
5. **Comando para aplicar migraciones**:
   ```powershell
   dotnet ef database update --project src/backend/Infrastructure --startup-project src/backend/Api
   ```
6. Revisar el archivo de migracion generado antes de aplicarlo.

## Reglas de uso de AsNoTracking

- Usar `AsNoTracking()` en todas las consultas de lectura sin modificacion posterior.
- No usar `AsNoTracking()` cuando se va a modificar la entidad (EF necesita rastrearla).

## Checklist de calidad

- [ ] El DbContext esta en `Infrastructure`, no en `Api` ni en `Application`.
- [ ] Existe una clase `IEntityTypeConfiguration<T>` por cada entidad.
- [ ] Las consultas de solo lectura usan `AsNoTracking()`.
- [ ] Las migraciones son incrementales (no se borra la DB para cambiar el esquema).
- [ ] Los campos obligatorios tienen `IsRequired()` en la configuracion.
- [ ] Los indices existen para los campos mas consultados.
- [ ] La cadena de conexion esta en `appsettings.json`, no hardcodeada en codigo.

## Errores comunes a evitar

- **DbContext en el controlador**: acceder siempre a traves del repositorio.
- **Sin `AsNoTracking` en lecturas**: causa tracking innecesario y consumo de memoria.
- **Borrar la DB para cambiar el esquema**: siempre migraciones incrementales.
- **Logica de negocio en el repositorio**: el repositorio solo hace CRUD; la logica va en el dominio o en el servicio.
- **Conexion hardcodeada**: usar `IConfiguration` o variables de entorno.

## Relacion con otros skills

- **SK-01 (Dominio)**: la entidad de dominio es la que EF persiste (con `private set` soportado via configuracion).
- **SK-04 (Casos de uso)**: el repositorio implementa la interfaz definida en Application.
- **SK-07 (UTC)**: las fechas almacenadas deben ser UTC; SQLite no tiene tipo nativo de zona horaria.
- **SK-09 (Pruebas)**: los tests de integracion usan SQLite en memoria o una DB de test temporal.
