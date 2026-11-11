using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using TiernoTiranosaurio.Modelo;
using TiernoTiranosaurio.Web.Servicios;

namespace TiernoTiranosaurio.Web.Pages;

public class ConsultaModel(ApiCliente api) : PageModel
{
    public static readonly CultureInfo Pesos = new("es-CO");

    [BindProperty(SupportsGet = true)]
    public int? Codigo { get; set; }

    [BindProperty(SupportsGet = true)]
    public string? Nombre { get; set; }

    public List<Mascota> Mascotas { get; private set; } = [];
    public Dictionary<int, string> NombresTipo { get; private set; } = [];
    public string? ErrorApi { get; private set; }

    public async Task OnGetAsync()
    {
        try
        {
            var tipos = await api.ListarTiposAsync();
            NombresTipo = tipos.ToDictionary(t => t.IdTipo, t => t.Nombre);

            if (Codigo.HasValue && Codigo.Value > 0)
            {
                var encontrada = await api.ObtenerPorCodigoAsync(Codigo.Value);
                Mascotas = encontrada is not null ? [encontrada] : [];
            }
            else
            {
                Mascotas = await api.ListarMascotasAsync(nombre: Nombre);
            }
        }
        catch (Exception ex)
        {
            ErrorApi = "No se pudo consultar la API: " + ex.Message;
        }
    }

    public string ObtenerNombreTipo(int idTipo) =>
        NombresTipo.TryGetValue(idTipo, out string? nombre) ? nombre : $"Tipo #{idTipo}";
}
