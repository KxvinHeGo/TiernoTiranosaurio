using Microsoft.AspNetCore.Mvc;
using TiernoTiranosaurio.Api.Datos;
using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Api.Controllers;

[ApiController]
[Route("api/tiposmascota")]
public class TiposMascotaController(BaseDatos bd) : ControllerBase
{
    // GET api/tiposmascota
    [HttpGet]
    public ActionResult<List<TipoMascota>> Listar()
    {
        return Ok(bd.Consultar("SELECT IdTipo, Nombre FROM TipoMascota ORDER BY Nombre", Lectores.Tipo));
    }
}
