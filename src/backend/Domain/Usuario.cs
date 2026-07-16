namespace AplicacionTareas.Domain;

public sealed class Usuario
{
    public int Id { get; private set; }

    public string Nombre { get; private set; } = string.Empty;

    public int DepartamentoId { get; private set; }

    public Departamento? Departamento { get; private set; }

    public DateTime CreadoEnUtc { get; private set; }

    public DateTime ActualizadoEnUtc { get; private set; }

    private Usuario()
    {
    }

    /// <summary>
    /// Crea un nuevo usuario validando sus invariantes de dominio.
    /// </summary>
    /// <param name="nombre">Nombre obligatorio del usuario.</param>
    /// <param name="departamentoId">Identificador obligatorio del departamento asociado.</param>
    /// <returns>Instancia valida de <see cref="Usuario"/>.</returns>
    /// <exception cref="ArgumentException">Se produce cuando el nombre es invalido.</exception>
    public static Usuario Crear(string nombre, int departamentoId)
    {
        ValidarNombre(nombre);
        ValidarDepartamentoId(departamentoId);

        var ahoraUtc = DateTime.UtcNow;

        return new Usuario
        {
            Nombre = nombre.Trim(),
            DepartamentoId = departamentoId,
            CreadoEnUtc = ahoraUtc,
            ActualizadoEnUtc = ahoraUtc,
        };
    }

    /// <summary>
    /// Actualiza el nombre del usuario y su marca de auditoria.
    /// </summary>
    /// <param name="nombre">Nuevo nombre obligatorio del usuario.</param>
    /// <exception cref="ArgumentException">Se produce cuando el nombre es invalido.</exception>
    public void CambiarNombre(string nombre)
    {
        ValidarNombre(nombre);
        Nombre = nombre.Trim();
        ActualizadoEnUtc = DateTime.UtcNow;
    }

    /// <summary>
    /// Cambia el departamento del usuario y actualiza la marca de auditoria.
    /// </summary>
    /// <param name="departamentoId">Identificador de departamento obligatorio.</param>
    /// <exception cref="ArgumentException">Se produce cuando el departamento es invalido.</exception>
    public void CambiarDepartamento(int departamentoId)
    {
        ValidarDepartamentoId(departamentoId);
        DepartamentoId = departamentoId;
        ActualizadoEnUtc = DateTime.UtcNow;
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

    private static void ValidarDepartamentoId(int departamentoId)
    {
        if (departamentoId <= 0)
        {
            throw new ArgumentException("El departamento del usuario es obligatorio.", nameof(departamentoId));
        }
    }
}