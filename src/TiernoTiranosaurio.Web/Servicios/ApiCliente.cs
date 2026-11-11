using System.Net;
using System.Net.Http.Json;
using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Web.Servicios;

/// <summary>
/// Cliente HTTP centralizado para consumir los endpoints de TiernoTiranosaurio.Api.
/// Sigue el contrato definido en docs/CONTRATO_API.md.
/// </summary>
public class ApiCliente(HttpClient http)
{
    /// <summary>
    /// GET api/mascotas?nombre=...&idTipo=...
    /// </summary>
    public async Task<List<Mascota>> ListarMascotasAsync(string? nombre = null, int? idTipo = null)
    {
        var parametros = new List<string>();
        if (!string.IsNullOrWhiteSpace(nombre))
            parametros.Add($"nombre={Uri.EscapeDataString(nombre.Trim())}");
        if (idTipo.HasValue && idTipo.Value > 0)
            parametros.Add($"idTipo={idTipo.Value}");

        string url = "api/mascotas";
        if (parametros.Count > 0)
            url += "?" + string.Join("&", parametros);

        return await http.GetFromJsonAsync<List<Mascota>>(url) ?? [];
    }

    /// <summary>
    /// GET api/mascotas/{codigo} (devuelve null si responde 404 Not Found).
    /// </summary>
    public async Task<Mascota?> ObtenerPorCodigoAsync(int codigo)
    {
        var respuesta = await http.GetAsync($"api/mascotas/{codigo}");
        if (respuesta.StatusCode == HttpStatusCode.NotFound)
            return null;

        respuesta.EnsureSuccessStatusCode();
        return await respuesta.Content.ReadFromJsonAsync<Mascota>();
    }

    /// <summary>
    /// GET api/tiposmascota (Maestro del Maestro-Detalle).
    /// </summary>
    public async Task<List<TipoMascota>> ListarTiposAsync()
    {
        return await http.GetFromJsonAsync<List<TipoMascota>>("api/tiposmascota") ?? [];
    }

    /// <summary>
    /// GET api/vistamascotas (filas de la vista SQL vw_MascotasDetalle).
    /// </summary>
    public async Task<List<VistaMascotaDetalle>> ListarVistaDetalleAsync()
    {
        return await http.GetFromJsonAsync<List<VistaMascotaDetalle>>("api/vistamascotas") ?? [];
    }
}
