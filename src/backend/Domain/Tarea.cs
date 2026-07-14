namespace AplicacionTareas.Domain;

public sealed class Tarea
{
    public int Id { get; private set; }

    public string Titulo { get; private set; } = string.Empty;

    public string? Descripcion { get; private set; }

    public bool EstaCompletada { get; private set; } = false;

    public DateTime CreadoEnUtc { get; private set; }

    public DateTime ActualizadoEnUtc { get; private set; }

    private Tarea()
    {
    }

    /// <summary>
    /// Crea una nueva tarea validando sus invariantes de dominio.
    /// </summary>
    /// <param name="titulo">Titulo obligatorio de la tarea.</param>
    /// <param name="descripcion">Descripcion opcional de la tarea.</param>
    /// <returns>Instancia valida de <see cref="Tarea"/>.</returns>
    /// <exception cref="ArgumentException">Se produce cuando el titulo esta vacio o excede la longitud maxima.</exception>
    public static Tarea Crear(string titulo, string? descripcion)
    {
        ValidarTitulo(titulo);

        var ahoraUtc = DateTime.UtcNow;

        return new Tarea
        {
            Titulo = titulo.Trim(),
            Descripcion = NormalizarDescripcion(descripcion),
            EstaCompletada = false,
            CreadoEnUtc = ahoraUtc,
            ActualizadoEnUtc = ahoraUtc,
        };
    }

    /// <summary>
    /// Actualiza los datos editables de la tarea y su marca de auditoria.
    /// </summary>
    /// <param name="titulo">Nuevo titulo obligatorio.</param>
    /// <param name="descripcion">Nueva descripcion opcional.</param>
    /// <exception cref="ArgumentException">Se produce cuando el titulo es invalido.</exception>
    public void Actualizar(string titulo, string? descripcion)
    {
        ValidarTitulo(titulo);

        Titulo = titulo.Trim();
        Descripcion = NormalizarDescripcion(descripcion);
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
}
