namespace AplicacionTareas.Domain;

public sealed class Tarea
{
    public int Id { get; private set; }

    public string Titulo { get; private set; } = string.Empty;

    public string? Descripcion { get; private set; }

    public string Categoria { get; private set; } = string.Empty;

    public int UsuarioAsignadoId { get; private set; }

    public bool EstaCompletada { get; private set; } = false;

    public DateTime CreadoEnUtc { get; private set; }

    public DateTime ActualizadoEnUtc { get; private set; }

    public DateTime? VenceEnUtc { get; private set; }

    private Tarea()
    {
    }

    /// <summary>
    /// Crea una nueva tarea validando sus invariantes de dominio.
    /// </summary>
    /// <param name="titulo">Titulo obligatorio de la tarea.</param>
    /// <param name="descripcion">Descripcion opcional de la tarea.</param>
    /// <param name="categoria">Categoria obligatoria de la tarea.</param>
    /// <param name="usuarioAsignadoId">Identificador obligatorio del usuario responsable.</param>
    /// <returns>Instancia valida de <see cref="Tarea"/>.</returns>
    /// <exception cref="ArgumentException">Se produce cuando el titulo, la categoria o el usuario asignado son invalidos.</exception>
    public static Tarea Crear(string titulo, string? descripcion, string categoria, int usuarioAsignadoId, DateTime? venceEnUtc)
    {
        ValidarTitulo(titulo);
        ValidarCategoria(categoria);
        ValidarUsuarioAsignadoId(usuarioAsignadoId);

        var ahoraUtc = DateTime.UtcNow;

        return new Tarea
        {
            Titulo = titulo.Trim(),
            Descripcion = NormalizarDescripcion(descripcion),
            Categoria = categoria.Trim(),
            UsuarioAsignadoId = usuarioAsignadoId,
            EstaCompletada = false,
            CreadoEnUtc = ahoraUtc,
            ActualizadoEnUtc = ahoraUtc,
            VenceEnUtc = NormalizarFechaUtc(venceEnUtc),
        };
    }

    /// <summary>
    /// Actualiza los datos editables de la tarea y su marca de auditoria.
    /// </summary>
    /// <param name="titulo">Nuevo titulo obligatorio.</param>
    /// <param name="descripcion">Nueva descripcion opcional.</param>
    /// <param name="categoria">Nueva categoria obligatoria.</param>
    /// <exception cref="ArgumentException">Se produce cuando el titulo o la categoria son invalidos.</exception>
    public void Actualizar(string titulo, string? descripcion, string categoria, DateTime? venceEnUtc)
    {
        ValidarTitulo(titulo);
        ValidarCategoria(categoria);

        Titulo = titulo.Trim();
        Descripcion = NormalizarDescripcion(descripcion);
        Categoria = categoria.Trim();
        VenceEnUtc = NormalizarFechaUtc(venceEnUtc);
        ActualizadoEnUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca la tarea como completada.
    /// </summary>
    /// <exception cref="InvalidOperationException">Se produce cuando la tarea ya estaba completada.</exception>
    public void Completar()
    {
        if (EstaCompletada)
        {
            throw new InvalidOperationException("La tarea ya esta completada.");
        }

        EstaCompletada = true;
        ActualizadoEnUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Marca la tarea como pendiente.
    /// </summary>
    /// <exception cref="InvalidOperationException">Se produce cuando la tarea ya estaba pendiente.</exception>
    public void MarcarPendiente()
    {
        if (!EstaCompletada)
        {
            throw new InvalidOperationException("La tarea ya esta pendiente.");
        }

        EstaCompletada = false;
        ActualizadoEnUtc = DateTime.UtcNow;
    }

    private static void ValidarTitulo(string titulo)
    {
        if (string.IsNullOrWhiteSpace(titulo))
        {
            throw new ArgumentException("El titulo de la tarea es obligatorio.", nameof(titulo));
        }

        if (titulo.Trim().Length > 200)
        {
            throw new ArgumentException("El titulo de la tarea no puede superar 200 caracteres.", nameof(titulo));
        }
    }

    private static string? NormalizarDescripcion(string? descripcion)
    {
        if (string.IsNullOrWhiteSpace(descripcion))
        {
            return null;
        }

        var descripcionNormalizada = descripcion.Trim();

        if (descripcionNormalizada.Length > 1000)
        {
            throw new ArgumentException("La descripcion no puede superar 1000 caracteres.", nameof(descripcion));
        }

        return descripcionNormalizada;
    }

    private static void ValidarCategoria(string categoria)
    {
        if (string.IsNullOrWhiteSpace(categoria))
        {
            throw new ArgumentException("La categoria de la tarea es obligatoria.", nameof(categoria));
        }

        if (categoria.Trim().Length > 100)
        {
            throw new ArgumentException("La categoria de la tarea no puede superar 100 caracteres.", nameof(categoria));
        }
    }

    private static void ValidarUsuarioAsignadoId(int usuarioAsignadoId)
    {
        if (usuarioAsignadoId <= 0)
        {
            throw new ArgumentException("El usuario asignado es obligatorio.", nameof(usuarioAsignadoId));
        }
    }

    private static DateTime? NormalizarFechaUtc(DateTime? fechaUtc)
    {
        if (!fechaUtc.HasValue)
        {
            return null;
        }

        if (fechaUtc.Value == default)
        {
            throw new ArgumentException("La fecha de vencimiento no puede ser la fecha por defecto.", nameof(fechaUtc));
        }

        var fechaNormalizadaUtc = fechaUtc.Value.Kind switch
        {
            DateTimeKind.Utc => fechaUtc,
            DateTimeKind.Local => fechaUtc.Value.ToUniversalTime(),
            _ => DateTime.SpecifyKind(fechaUtc.Value, DateTimeKind.Utc),
        };

        if (fechaNormalizadaUtc.Value < DateTime.UtcNow)
        {
            throw new ArgumentException("La fecha de vencimiento no puede estar en el pasado.", nameof(fechaUtc));
        }

        return fechaNormalizadaUtc;
    }
}
