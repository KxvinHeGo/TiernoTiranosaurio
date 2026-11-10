using Microsoft.AspNetCore.Mvc;
using TiernoTiranosaurio.Api.Datos;
using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Api.Controllers;

[ApiController]
[Route("api/tiposmascota")]
public class TiposMascotaController : ControllerBase
{
    // GET api/tiposmascota  -> lista de tipos (Maestro del Maestro-Detalle)
    // TODO (Kevin): SELECT IdTipo, Nombre FROM TipoMascota ORDER BY Nombre
    [HttpGet]
    public ActionResult<List<TipoMascota>> Listar() => Ok(DatosTemporales.Tipos);
}
