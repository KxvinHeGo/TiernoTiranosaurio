using System.Data;
using System.Text.Json;
using Microsoft.Data.SqlClient;

namespace TiernoTiranosaurio.Desktop;

/// <summary>
/// Conexión manual a la base de datos (ADO.NET con SqlConnection). Responsable: Kevin (Integrante 1).
///
/// La cadena de conexión NO está escrita en el código: se lee del archivo conexion.json,
/// que cada integrante tiene en su PC con el nombre de SU servidor (ese archivo no se sube a GitHub).
/// Para crearlo: copiar conexion.ejemplo.json → conexion.json y cambiar el Server.
///
/// Uso:
///   using SqlConnection cn = DataBase.ObtenerConexion();
///   DataTable t = DataBase.Consultar("SELECT ... WHERE Codigo = @codigo", new SqlParameter("@codigo", 101));
///   int filas  = DataBase.Ejecutar("UPDATE ...", parametros...);
///   object v   = DataBase.EjecutarEscalar("INSERT ...; SELECT SCOPE_IDENTITY();", parametros...);
/// </summary>
public static class DataBase
{
    private const string ArchivoConfig = "conexion.json";

    // Se lee una sola vez, la primera vez que alguien pide una conexión.
    private static readonly Lazy<string> cadena = new(LeerCadena);

    /// <summary>Cadena de conexión que se está usando (útil para mostrar errores).</summary>
    public static string CadenaConexion => cadena.Value;

    /// <summary>Crea una conexión NUEVA y CERRADA. Siempre usarla con "using".</summary>
    public static SqlConnection ObtenerConexion()
    {
        return new SqlConnection(cadena.Value);
    }

    /// <summary>Ejecuta un SELECT y devuelve las filas en un DataTable.</summary>
    public static DataTable Consultar(string sql, params SqlParameter[] parametros)
    {
        using SqlConnection cn = ObtenerConexion();
        using SqlCommand cmd = new SqlCommand(sql, cn);
        cmd.Parameters.AddRange(parametros);
        using SqlDataAdapter da = new SqlDataAdapter(cmd);
        DataTable tabla = new DataTable();
        da.Fill(tabla);
        return tabla;
    }

    /// <summary>Ejecuta un INSERT, UPDATE o DELETE y devuelve cuántas filas cambió.</summary>
    public static int Ejecutar(string sql, params SqlParameter[] parametros)
    {
        using SqlConnection cn = ObtenerConexion();
        using SqlCommand cmd = new SqlCommand(sql, cn);
        cmd.Parameters.AddRange(parametros);
        cn.Open();
        return cmd.ExecuteNonQuery();
    }

    /// <summary>Ejecuta y devuelve un solo valor (ej: COUNT(*) o SCOPE_IDENTITY()).</summary>
    public static object EjecutarEscalar(string sql, params SqlParameter[] parametros)
    {
        using SqlConnection cn = ObtenerConexion();
        using SqlCommand cmd = new SqlCommand(sql, cn);
        cmd.Parameters.AddRange(parametros);
        cn.Open();
        return cmd.ExecuteScalar();
    }

    /// <summary>Intenta abrir la conexión. Devuelve null si funciona, o el mensaje de error.</summary>
    public static string ProbarConexion()
    {
        try
        {
            using SqlConnection cn = ObtenerConexion();
            cn.Open();
            return null;
        }
        catch (Exception ex)
        {
            return ex.Message;
        }
    }

    private static string LeerCadena()
    {
        string ruta = Path.Combine(AppContext.BaseDirectory, ArchivoConfig);

        if (!File.Exists(ruta))
            throw new InvalidOperationException(
                "No se encontró el archivo " + ArchivoConfig + ".\n\n" +
                "Copien conexion.ejemplo.json, renómbrenlo a conexion.json (en la carpeta del proyecto Desktop), " +
                "pongan el nombre de su servidor SQL y vuelvan a compilar.");

        using JsonDocument json = JsonDocument.Parse(File.ReadAllText(ruta));
        string valor = json.RootElement.GetProperty("CadenaConexion").GetString();

        if (string.IsNullOrWhiteSpace(valor))
            throw new InvalidOperationException("La propiedad CadenaConexion de " + ArchivoConfig + " está vacía.");

        return valor;
    }
}
