using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TiernoTiranosaurio.Api.Datos;
using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Api.Controllers;

// POST api/auth/login  →  200 { usuario, rol }  |  401
[ApiController]
[Route("api/auth")]
public class AuthController(BaseDatos bd) : ControllerBase
{
    [HttpPost("login")]
    public ActionResult<LoginResponse> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Usuario) || string.IsNullOrWhiteSpace(request.Clave))
            return Unauthorized();

        var usuarios = bd.Consultar(
            @"SELECT NombreUsuario, Rol
              FROM Usuarios
              WHERE NombreUsuario = @usuario AND Clave = HASHBYTES('SHA2_256', @clave)",
            dr => new LoginResponse { Usuario = dr.GetString(0), Rol = dr.GetString(1) },
            new SqlParameter("@usuario", SqlDbType.VarChar, 30) { Value = request.Usuario.Trim() },
            new SqlParameter("@clave", SqlDbType.VarChar, 100) { Value = request.Clave });

        return usuarios.Count == 1 ? Ok(usuarios[0]) : Unauthorized();
    }
}
