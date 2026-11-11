using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TiernoTiranosaurio.Modelo;
using TiernoTiranosaurio.Web.Servicios;

namespace TiernoTiranosaurio.Web.Pages;

public class MaestroDetalleModel(ApiCliente api) : PageModel
{
    public static readonly CultureInfo Pesos = new("es-CO");

    [BindProperty(SupportsGet = true)]
    public int? IdTipo { get; set; }

    public List<TipoMascota> Tipos { get; private set; } = [];
    public List<Mascota> MascotasDelTipo { get; private set; } = [];
    public TipoMascota? TipoSeleccionado { get; private set; }

    // Resumen de la categoría seleccionada
    public int DisponiblesDelTipo { get; private set; }
    public int VendidasDelTipo { get; private set; }
    public decimal ValorTotalDelTipo { get; private set; }

    public string? ErrorApi { get; private set; }

    public async Task OnGetAsync()
    {
        try
        {
            Tipos = await api.ListarTiposAsync();

            if (!IdTipo.HasValue && Tipos.Count > 0)
            {
                IdTipo = Tipos[0].IdTipo;
            }

            if (IdTipo.HasValue)
            {
                TipoSeleccionado = Tipos.FirstOrDefault(t => t.IdTipo == IdTipo.Value);
                MascotasDelTipo = await api.ListarMascotasAsync(idTipo: IdTipo.Value);

                DisponiblesDelTipo = MascotasDelTipo.Count(m => m.Estado == EstadosMascota.Disponible);
                VendidasDelTipo = MascotasDelTipo.Count(m => m.Estado == EstadosMascota.Vendida);
                ValorTotalDelTipo = MascotasDelTipo.Sum(m => m.Precio);
            }
        }
        catch (Exception ex)
        {
            ErrorApi = "No se pudo consultar la API: " + ex.Message;
        }
    }

    public static string ClaseIconoTipo(string nombre) => nombre.ToLowerInvariant() switch
    {
        var n when n.Contains("perro") => "fa-solid fa-dog",
        var n when n.Contains("gato") => "fa-solid fa-cat",
        var n when n.Contains("ave") => "fa-solid fa-dove",
        var n when n.Contains("pez") => "fa-solid fa-fish",
        var n when n.Contains("roedor") => "fa-solid fa-otter",
        var n when n.Contains("reptil") => "fa-solid fa-dragon",
        _ => "fa-solid fa-paw"
    };
}
