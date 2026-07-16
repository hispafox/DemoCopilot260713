namespace AplicacionTareas.Domain;

public sealed class Departamento
{
    public int Id { get; private set; }

    public string Nombre { get; private set; } = string.Empty;

    public DateTime CreadoEnUtc { get; private set; }

    public DateTime ActualizadoEnUtc { get; private set; }

    public ICollection<Usuario> Usuarios { get; private set; } = new List<Usuario>();

    private Departamento()
    {
    }

    /// <summary>
    /// Crea un nuevo departamento validando sus invariantes de dominio.
    /// </summary>
    /// <param name="nombre">Nombre obligatorio del departamento.</param>
    /// <returns>Instancia valida de <see cref="Departamento"/>.</returns>
    /// <exception cref="ArgumentException">Se produce cuando el nombre es invalido.</exception>
    public static Departamento Crear(string nombre)
    {
        ValidarNombre(nombre);

        var ahoraUtc = DateTime.UtcNow;

        return new Departamento
        {
            Nombre = nombre.Trim(),
            CreadoEnUtc = ahoraUtc,
            ActualizadoEnUtc = ahoraUtc,
        };
    }

    /// <summary>
    /// Actualiza el nombre del departamento y su marca de auditoria.
    /// </summary>
    /// <param name="nombre">Nuevo nombre obligatorio.</param>
    /// <exception cref="ArgumentException">Se produce cuando el nombre es invalido.</exception>
    public void ActualizarNombre(string nombre)
    {
        ValidarNombre(nombre);
        Nombre = nombre.Trim();
        ActualizadoEnUtc = DateTime.UtcNow;
    }

    private static void ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre del departamento es obligatorio.", nameof(nombre));
        }

        if (nombre.Trim().Length > 200)
        {
            throw new ArgumentException("El nombre del departamento no puede superar 200 caracteres.", nameof(nombre));
        }
    }
}