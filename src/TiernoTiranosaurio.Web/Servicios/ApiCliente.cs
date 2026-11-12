using System.Net;
using System.Net.Http.Json;
using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Web.Servicios;

/// <summary>Cliente de la API: usa UrlApi (Azure) y, si falla, UrlApiRespaldo (local).</summary>
public class ApiCliente
{
    private readonly HttpClient _http;
    private readonly string[] _urlsBase;

    public ApiCliente(HttpClient http, IConfiguration config)
    {
        _http = http;

        string principal = NormalizarUrl(
            config["UrlApi"],
            "https://tiernotiranosaurio-api-kevin-feapesdphzg4dfcg.brazilsouth-01.azurewebsites.net/");

        string respaldo = NormalizarUrl(
            config["UrlApiRespaldo"],
            "https://localhost:7100/");

        _urlsBase = string.Equals(principal, respaldo, StringComparison.OrdinalIgnoreCase)
            ? [principal]
            : [principal, respaldo];
    }

    public async Task<List<Mascota>> ListarMascotasAsync(string? nombre = null, int? idTipo = null)
    {
        var parametros = new List<string>();
        if (!string.IsNullOrWhiteSpace(nombre))
            parametros.Add($"nombre={Uri.EscapeDataString(nombre.Trim())}");
        if (idTipo.HasValue && idTipo.Value > 0)
            parametros.Add($"idTipo={idTipo.Value}");

        string ruta = "api/mascotas";
        if (parametros.Count > 0)
            ruta += "?" + string.Join("&", parametros);

        return await ObtenerConRespaldoAsync<List<Mascota>>(ruta) ?? [];
    }

    public async Task<Mascota?> ObtenerPorCodigoAsync(int codigo)
    {
        return await ObtenerConRespaldoAsync<Mascota>($"api/mascotas/{codigo}", permitir404: true);
    }

    public async Task<List<TipoMascota>> ListarTiposAsync()
    {
        return await ObtenerConRespaldoAsync<List<TipoMascota>>("api/tiposmascota") ?? [];
    }

    public async Task<List<VistaMascotaDetalle>> ListarVistaDetalleAsync()
    {
        return await ObtenerConRespaldoAsync<List<VistaMascotaDetalle>>("api/vistamascotas") ?? [];
    }

    private async Task<T?> ObtenerConRespaldoAsync<T>(string rutaRelativa, bool permitir404 = false)
    {
        Exception? ultimoError = null;

        foreach (string urlBase in _urlsBase)
        {
            TimeSpan limite = EsLocal(urlBase) ? TimeSpan.FromSeconds(8) : TimeSpan.FromSeconds(15);
            using var cts = new CancellationTokenSource(limite);

            try
            {
                Uri url = new(new Uri(urlBase), rutaRelativa);
                using HttpResponseMessage respuesta = await _http.GetAsync(url, cts.Token);

                if (permitir404 && respuesta.StatusCode == HttpStatusCode.NotFound)
                    return default;

                if ((int)respuesta.StatusCode >= 500)
                {
                    ultimoError = new HttpRequestException($"El servidor respondió {(int)respuesta.StatusCode} ({respuesta.StatusCode}).");
                    continue;
                }

                respuesta.EnsureSuccessStatusCode();
                return await respuesta.Content.ReadFromJsonAsync<T>(cancellationToken: cts.Token);
            }
            catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException or OperationCanceledException)
            {
                ultimoError = ex;
            }
        }

        throw ultimoError ?? new HttpRequestException("No se pudo conectar con ninguna API configurada.");
    }

    private static string NormalizarUrl(string? url, string porDefecto)
    {
        string valor = string.IsNullOrWhiteSpace(url) ? porDefecto : url.Trim();
        return valor.EndsWith('/') ? valor : valor + "/";
    }

    private static bool EsLocal(string url) =>
        url.Contains("localhost", StringComparison.OrdinalIgnoreCase) || url.Contains("127.0.0.1");
}
