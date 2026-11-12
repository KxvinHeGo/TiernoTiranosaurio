using Microsoft.AspNetCore.Mvc;
using TiernoTiranosaurio.Api.Datos;
using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Api.Controllers;

[ApiController]
[Route("api/vistamascotas")]
public class VistaMascotasController(BaseDatos bd) : ControllerBase
{
    // GET api/vistamascotas
    [HttpGet]
    public ActionResult<List<VistaMascotaDetalle>> Listar()
    {
        return Ok(bd.Consultar("SELECT * FROM vw_MascotasDetalle ORDER BY Codigo", Lectores.Vista));
    }
}
