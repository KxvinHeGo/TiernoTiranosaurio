using System.Text.Json;

namespace TiernoTiranosaurio.Desktop;

/// <summary>Lee la configuración de cada PC desde conexion.json (UrlsApi y CadenasConexion).</summary>
public static class Configuracion
{
    private const string Archivo = "conexion.json";
    private const string UrlApiPorDefecto = "https://localhost:7100/";

    private static readonly Lazy<JsonElement> raiz = new(Leer);

    /// <summary>Bases de datos en orden de preferencia.</summary>
    public static string[] CadenasConexion
    {
        get
        {
            string[] lista = Lista("CadenasConexion", "CadenaConexion");
            if (lista.Length == 0)
                throw new InvalidOperationException("Falta CadenasConexion en " + Archivo + ".");
            return lista;
        }
    }

    /// <summary>APIs de login en orden de preferencia, cada una terminada en "/".</summary>
    public static string[] UrlsApi
    {
        get
        {
            string[] lista = Lista("UrlsApi", "UrlApi");
            if (lista.Length == 0) lista = new[] { UrlApiPorDefecto };
            return lista.Select(u => u.EndsWith('/') ? u : u + "/").ToArray();
        }
    }

    /// <summary>Lee una lista (formato nuevo) o un solo valor (formato viejo).</summary>
    private static string[] Lista(string propiedadLista, string propiedadUnica)
    {
        JsonElement r = raiz.Value;

        if (r.TryGetProperty(propiedadLista, out JsonElement arr) && arr.ValueKind == JsonValueKind.Array)
            return arr.EnumerateArray()
                      .Select(e => e.GetString())
                      .Where(s => !string.IsNullOrWhiteSpace(s))
                      .ToArray();

        if (r.TryGetProperty(propiedadUnica, out JsonElement v) && !string.IsNullOrWhiteSpace(v.GetString()))
            return new[] { v.GetString() };

        return Array.Empty<string>();
    }

    private static JsonElement Leer()
    {
        string ruta = Path.Combine(AppContext.BaseDirectory, Archivo);

        if (!File.Exists(ruta))
            throw new InvalidOperationException(
                "No se encontró el archivo " + Archivo + ".\n\n" +
                "Copien conexion.ejemplo.json, renómbrenlo a conexion.json (en la carpeta del proyecto Desktop), " +
                "pongan el nombre de su servidor SQL y vuelvan a compilar.");

        using JsonDocument json = JsonDocument.Parse(File.ReadAllText(ruta));
        return json.RootElement.Clone();
    }
}
