using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Desktop;

/// <summary>
/// Guarda quién inició sesión y con qué rol (lo llena Login.cs con la respuesta de la API).
/// Menu.cs lo usa para habilitar o bloquear opciones.
/// </summary>
public static class Sesion
{
    public static string Usuario { get; private set; } = "";
    public static string Rol { get; private set; } = "";

    public static bool Activa => Usuario != "";
    public static bool EsAdministrador => Rol == Roles.Administrador;

    public static void Iniciar(LoginResponse respuesta)
    {
        Usuario = respuesta.Usuario;
        Rol = respuesta.Rol;
    }

    public static void Cerrar()
    {
        Usuario = "";
        Rol = "";
    }
}
