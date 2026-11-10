using Microsoft.AspNetCore.Mvc;
using TiernoTiranosaurio.Api.Datos;
using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Api.Controllers;

[ApiController]
[Route("api/vistamascotas")]
public class VistaMascotasController : ControllerBase
{
    // GET api/vistamascotas  -> filas de la vista SQL vw_MascotasDetalle
    // TODO (Kevin): SELECT * FROM vw_MascotasDetalle
    [HttpGet]
    public ActionResult<List<VistaMascotaDetalle>> Listar()
    {
        // Simula el JOIN que hará la vista SQL
        var filas = from m in DatosTemporales.Mascotas
                    join t in DatosTemporales.Tipos on m.IdTipo equals t.IdTipo
                    join c in DatosTemporales.Clientes on m.IdCliente equals c.IdCliente into cs
                    from c in cs.DefaultIfEmpty()
                    select new VistaMascotaDetalle
                    {
                        Codigo = m.Codigo,
                        NombreMascota = m.Nombre,
                        Tipo = t.Nombre,
                        Raza = m.Raza,
                        Edad = m.Edad,
                        Precio = m.Precio,
                        Estado = m.Estado,
                        NombreCliente = c?.Nombre
                    };

        return Ok(filas.ToList());
    }
}
