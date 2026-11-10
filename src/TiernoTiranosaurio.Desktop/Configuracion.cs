using System.Text.Json;

namespace TiernoTiranosaurio.Desktop;

/// <summary>
/// Lee la configuración de cada PC desde conexion.json (no se sube a GitHub).
///   - CadenaConexion → conexión manual a SQL Server (la usa DataBase.cs)
///   - UrlApi         → dirección de la API de login (local o Azure)
/// Para crearlo: copiar conexion.ejemplo.json → conexion.json y ajustar los valores.
/// </summary>
public static class Configuracion
{
    private const string Archivo = "conexion.json";
    private const string UrlApiPorDefecto = "https://localhost:7100/";

    private static readonly Lazy<JsonElement> raiz = new(Leer);

    public static string CadenaConexion
    {
        get
        {
            string valor = Texto("CadenaConexion");
            if (string.IsNullOrWhiteSpace(valor))
                throw new InvalidOperationException("Falta CadenaConexion en " + Archivo + ".");
            return valor;
        }
    }

    /// <summary>Dirección base de la API, siempre terminada en "/".</summary>
    public static string UrlApi
    {
        get
        {
            string valor = Texto("UrlApi");
            if (string.IsNullOrWhiteSpace(valor)) valor = UrlApiPorDefecto;
            return valor.EndsWith('/') ? valor : valor + "/";
        }
    }

    private static string Texto(string propiedad)
    {
        return raiz.Value.TryGetProperty(propiedad, out JsonElement v) ? v.GetString() : null;
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
