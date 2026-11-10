using Microsoft.AspNetCore.Mvc;
using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Api.Controllers;

// POST api/auth/login
// TODO (Kevin): validar contra la tabla Usuarios de la base de datos.
[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        // Respuesta temporal para que el escritorio pueda probar el login antes de tener la BD.
        if (request.Usuario == "admin" && request.Clave == "1234")
            return Ok(new LoginResponse { Usuario = request.Usuario, Rol = Roles.Administrador });

        if (request.Usuario == "vendedor" && request.Clave == "1234")
            return Ok(new LoginResponse { Usuario = request.Usuario, Rol = Roles.Vendedor });

        return Unauthorized();
    }
}
