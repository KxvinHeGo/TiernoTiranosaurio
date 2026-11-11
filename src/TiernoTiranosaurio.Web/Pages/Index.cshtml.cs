using Microsoft.AspNetCore.Mvc.RazorPages;
using TiernoTiranosaurio.Modelo;
using TiernoTiranosaurio.Web.Servicios;

namespace TiernoTiranosaurio.Web.Pages;

public class IndexModel(ApiCliente api) : PageModel
{
    public int TotalMascotas { get; private set; }
    public int Disponibles { get; private set; }
    public int Vendidas { get; private set; }
    public int TotalTipos { get; private set; }
    public string? ErrorApi { get; private set; }

    public async Task OnGetAsync()
    {
        try
        {
            var mascotas = await api.ListarMascotasAsync();
            var tipos = await api.ListarTiposAsync();

            TotalMascotas = mascotas.Count;
            Disponibles = mascotas.Count(m => m.Estado == EstadosMascota.Disponible);
            Vendidas = mascotas.Count(m => m.Estado == EstadosMascota.Vendida);
            TotalTipos = tipos.Count;
        }
        catch (Exception ex)
        {
            ErrorApi = "No se pudo conectar con la API (" + ex.Message + "). Verifica que el proyecto TiernoTiranosaurio.Api esté en ejecución.";
        }
    }
}
