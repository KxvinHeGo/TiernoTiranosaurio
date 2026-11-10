namespace TiernoTiranosaurio.Modelo;

/// <summary>Lo que el Login de escritorio envía a POST api/auth/login.</summary>
public class LoginRequest
{
    public string Usuario { get; set; } = "";
    public string Clave { get; set; } = "";
}

/// <summary>Lo que la API responde si el login es correcto.</summary>
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
