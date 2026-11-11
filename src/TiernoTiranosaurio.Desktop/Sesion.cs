using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Desktop;

/// <summary>
/// Guarda quién inició sesión, con qué rol y por cuál plan se validó (lo llena Login.cs).
/// Menu.cs lo usa para habilitar o bloquear opciones y mostrarlo en la barra de estado.
/// </summary>
public static class Sesion
{
    public static string Usuario { get; private set; } = "";
    public static string Rol { get; private set; } = "";

    /// <summary>Cómo se validó el acceso: "API Azure ☁️", "API local" o "Contingencia (BD directa)".</summary>
    public static string Origen { get; private set; } = "";

    public static bool Activa => Usuario != "";
    public static bool EsAdministrador => Rol == Roles.Administrador;

    public static void Iniciar(LoginResponse respuesta, string origen)
    {
        Usuario = respuesta.Usuario;
        Rol = respuesta.Rol;
        Origen = origen;
    }

    public static void Cerrar()
    {
        Usuario = "";
        Rol = "";
        Origen = "";
    }
}
