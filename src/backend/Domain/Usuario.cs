namespace AplicacionTareas.Domain;

public sealed class Usuario
{
    public int Id { get; private set; }

    public string Nombre { get; private set; } = string.Empty;

    public DateTime CreadoEnUtc { get; private set; }

    public DateTime ActualizadoEnUtc { get; private set; }

    private Usuario()
    {
    }

    /// <summary>
    /// Crea un nuevo usuario validando sus invariantes de dominio.
    /// </summary>
    /// <param name="nombre">Nombre obligatorio del usuario.</param>
    /// <returns>Instancia valida de <see cref="Usuario"/>.</returns>
    /// <exception cref="ArgumentException">Se produce cuando el nombre es invalido.</exception>
    public static Usuario Crear(string nombre)
    {
        ValidarNombre(nombre);

        var ahoraUtc = DateTime.UtcNow;

        return new Usuario
        {
            Nombre = nombre.Trim(),
            CreadoEnUtc = ahoraUtc,
            ActualizadoEnUtc = ahoraUtc,
        };
    }

    private static void ValidarNombre(string nombre)
    {
        if (string.IsNullOrWhiteSpace(nombre))
        {
            throw new ArgumentException("El nombre del usuario es obligatorio.", nameof(nombre));
        }

        if (nombre.Trim().Length > 200)
        {
            throw new ArgumentException("El nombre del usuario no puede superar 200 caracteres.", nameof(nombre));
        }
    }
}