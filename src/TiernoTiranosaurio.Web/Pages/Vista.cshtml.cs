using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TiernoTiranosaurio.Modelo;
using TiernoTiranosaurio.Web.Servicios;

namespace TiernoTiranosaurio.Web.Pages;

public class VistaModel(ApiCliente api) : PageModel
{
    public static readonly CultureInfo Pesos = new("es-CO");

    [BindProperty(SupportsGet = true)]
    public string? EstadoFiltro { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Buscar { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Orden { get; set; }

    public List<VistaMascotaDetalle> Filas { get; private set; } = [];

    // Extra 1: Indicadores financieros calculados con LINQ en C#
    public decimal TotalRecaudadoVentas { get; private set; }
    public decimal ValorInventarioDisponible { get; private set; }
    public int CantidadDisponibles { get; private set; }
    public int CantidadVendidas { get; private set; }

    public string? ErrorApi { get; private set; }

    public async Task OnGetAsync()
    {
        try
        {
            var todas = await api.ListarVistaDetalleAsync();

            // Cálculos generales sobre la vista completa
            CantidadDisponibles = todas.Count(f => f.Estado == EstadosMascota.Disponible);
            CantidadVendidas = todas.Count(f => f.Estado == EstadosMascota.Vendida);
            TotalRecaudadoVentas = todas
                .Where(f => f.Estado == EstadosMascota.Vendida)
                .Sum(f => f.Precio);
            ValorInventarioDisponible = todas
                .Where(f => f.Estado == EstadosMascota.Disponible)
                .Sum(f => f.Precio);

            // Filtro por Estado (Disponible / Vendida)
            IEnumerable<VistaMascotaDetalle> consulta = todas;

            if (!string.IsNullOrWhiteSpace(EstadoFiltro))
            {
                consulta = consulta.Where(f =>
                    string.Equals(f.Estado, EstadoFiltro, StringComparison.OrdinalIgnoreCase));
            }

            // Extra 2: Filtro por texto (Mascota, Cliente comprador, Raza o Tipo)
            if (!string.IsNullOrWhiteSpace(Buscar))
            {
                string texto = Buscar.Trim();
                consulta = consulta.Where(f =>
                    f.NombreMascota.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                    f.Raza.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                    f.Tipo.Contains(texto, StringComparison.OrdinalIgnoreCase) ||
                    (f.NombreCliente != null && f.NombreCliente.Contains(texto, StringComparison.OrdinalIgnoreCase)));
            }

            // Ordenamiento en C#
            consulta = Orden switch
            {
                "precio_desc" => consulta.OrderByDescending(f => f.Precio),
                "precio_asc" => consulta.OrderBy(f => f.Precio),
                "nombre" => consulta.OrderBy(f => f.NombreMascota),
                _ => consulta.OrderBy(f => f.Codigo)
            };

            Filas = consulta.ToList();
        }
        catch (Exception ex)
        {
            ErrorApi = "No se pudo consultar la vista en la API: " + ex.Message;
        }
    }
}
