using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Desktop;

public static class Sesion
{
    public static string Usuario { get; private set; } = "";
    public static string Rol { get; private set; } = "";

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
