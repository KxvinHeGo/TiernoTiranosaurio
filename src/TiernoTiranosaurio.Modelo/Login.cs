namespace TiernoTiranosaurio.Modelo;

public class LoginRequest
{
    public string Usuario { get; set; } = "";
    public string Clave { get; set; } = "";
}

public class LoginResponse
{
    public string Usuario { get; set; } = "";
    public string Rol { get; set; } = "";
}

public static class Roles
{
    public const string Administrador = "Administrador";
    public const string Vendedor = "Vendedor";
}
