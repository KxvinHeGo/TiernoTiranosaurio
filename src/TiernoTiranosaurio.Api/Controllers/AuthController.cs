using Microsoft.AspNetCore.Mvc;

namespace TiernoTiranosaurio.Api.Controllers;

// TODO (Integrante 1): validar contra la tabla Usuarios de la base de datos.
// Contrato acordado en la Fase 0 (ver README.md):
//   Petición:   POST /api/auth/login   { "usuario": "admin", "clave": "1234" }
//   Respuesta:  200 { "usuario": "admin", "rol": "Administrador" }
//               401 si el usuario o la clave no son válidos
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public IActionResult Login([FromBody] LoginRequest request)
    {
        // Respuesta temporal para que el escritorio pueda probar el login antes de tener la BD.
        if (request.Usuario == "admin" && request.Clave == "1234")
            return Ok(new LoginResponse(request.Usuario, "Administrador"));

        if (request.Usuario == "vendedor" && request.Clave == "1234")
            return Ok(new LoginResponse(request.Usuario, "Vendedor"));

        return Unauthorized();
    }
}

public record LoginRequest(string Usuario, string Clave);
public record LoginResponse(string Usuario, string Rol);
