using Microsoft.AspNetCore.Mvc;
using TiernoTiranosaurio.Api.Datos;
using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Api.Controllers;

[ApiController]
[Route("api/mascotas")]
public class MascotasController : ControllerBase
{
    // GET api/mascotas                 -> todas
    // GET api/mascotas?nombre=re       -> nombre contiene "re"
    // GET api/mascotas?idTipo=1        -> solo perros (Detalle del Maestro-Detalle)
    // Los filtros se pueden combinar.
    // TODO (Kevin): SELECT ... FROM Mascota WHERE (@nombre IS NULL OR Nombre LIKE '%'+@nombre+'%') AND (@idTipo IS NULL OR IdTipo=@idTipo)
    [HttpGet]
    public ActionResult<List<Mascota>> Listar([FromQuery] string? nombre, [FromQuery] int? idTipo)
    {
        var lista = DatosTemporales.Mascotas.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(nombre))
            lista = lista.Where(m => m.Nombre.Contains(nombre, StringComparison.OrdinalIgnoreCase));

        if (idTipo.HasValue)
            lista = lista.Where(m => m.IdTipo == idTipo.Value);

        return Ok(lista.ToList());
    }

    // GET api/mascotas/101  -> una mascota por código (404 si no existe)
    // TODO (Kevin): SELECT ... FROM Mascota WHERE Codigo=@codigo
    [HttpGet("{codigo:int}")]
    public ActionResult<Mascota> PorCodigo(int codigo)
    {
        var mascota = DatosTemporales.Mascotas.FirstOrDefault(m => m.Codigo == codigo);
        return mascota is null ? NotFound() : Ok(mascota);
    }
}
