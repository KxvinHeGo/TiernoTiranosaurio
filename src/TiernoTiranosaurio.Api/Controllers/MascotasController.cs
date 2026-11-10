using System.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using TiernoTiranosaurio.Api.Datos;
using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Api.Controllers;

[ApiController]
[Route("api/mascotas")]
public class MascotasController(BaseDatos bd) : ControllerBase
{
    // GET api/mascotas                 -> todas
    // GET api/mascotas?nombre=re       -> nombre contiene "re"
    // GET api/mascotas?idTipo=1        -> solo perros (Detalle del Maestro-Detalle)
    // Los filtros se pueden combinar.
    [HttpGet]
    public ActionResult<List<Mascota>> Listar([FromQuery] string? nombre, [FromQuery] int? idTipo)
    {
        string? filtroNombre = string.IsNullOrWhiteSpace(nombre) ? null : nombre.Trim();

        var lista = bd.Consultar(
            $@"SELECT {Lectores.ColumnasMascota}
               FROM Mascota
               WHERE (@nombre IS NULL OR Nombre LIKE '%' + @nombre + '%')
                 AND (@idTipo IS NULL OR IdTipo = @idTipo)
               ORDER BY Codigo",
            Lectores.Mascota,
            new SqlParameter("@nombre", SqlDbType.VarChar, 50) { Value = BaseDatos.Valor(filtroNombre) },
            new SqlParameter("@idTipo", SqlDbType.Int) { Value = BaseDatos.Valor(idTipo) });

        return Ok(lista);
    }

    // GET api/mascotas/101  -> una mascota por código (404 si no existe)
    [HttpGet("{codigo:int}")]
    public ActionResult<Mascota> PorCodigo(int codigo)
    {
        var lista = bd.Consultar(
            $"SELECT {Lectores.ColumnasMascota} FROM Mascota WHERE Codigo = @codigo",
            Lectores.Mascota,
            new SqlParameter("@codigo", SqlDbType.Int) { Value = codigo });

        return lista.Count == 0 ? NotFound() : Ok(lista[0]);
    }
}
