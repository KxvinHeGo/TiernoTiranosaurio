using System.Data;
using Microsoft.Data.SqlClient;

namespace TiernoTiranosaurio.Desktop;

/// <summary>
/// Conexión manual a la base de datos (ADO.NET con SqlConnection). Responsable: Kevin (Integrante 1).
///
/// PLANES AUTOMÁTICOS: conexion.json trae una lista de bases de datos ("CadenasConexion").
/// La primera vez se prueban en orden y se usa la primera que conecte
/// (ej: Plan A = BD local, Plan B = BD de Azure). Si la elegida se cae, se vuelve a buscar.
///
/// Uso (igual que siempre):
///   using SqlConnection cn = DataBase.ObtenerConexion();
///   DataTable t = DataBase.Consultar("SELECT ... WHERE Codigo = @codigo", new SqlParameter("@codigo", 101));
///   int filas  = DataBase.Ejecutar("UPDATE ...", parametros...);
///   object v   = DataBase.EjecutarEscalar("INSERT ...; SELECT SCOPE_IDENTITY();", parametros...);
/// </summary>
public static class DataBase
{
    private static readonly object candado = new();
    private static string cadenaActiva;

    /// <summary>Nombre amigable de la BD en uso: "Local", "Azure" o "Sin conexión".</summary>
    public static string Origen { get; private set; } = "Sin conexión";

    /// <summary>Número de plan en uso (1 = A, 2 = B...). 0 si ninguna conecta.</summary>
    public static int Plan { get; private set; }

    /// <summary>
    /// Prueba las bases de datos en orden y se queda con la primera que conecte.
    /// Devuelve null si alguna funcionó, o el resumen de errores si ninguna.
    /// </summary>
    public static string Seleccionar()
    {
        lock (candado)
        {
            string[] cadenas = Configuracion.CadenasConexion;
            List<string> errores = new();

            for (int i = 0; i < cadenas.Length; i++)
            {
                try
                {
                    using SqlConnection cn = new SqlConnection(cadenas[i]);
                    cn.Open();
                    cadenaActiva = cadenas[i];
                    Plan = i + 1;
                    Origen = NombreDe(cadenas[i]);
                    return null;
                }
                catch (Exception ex)
                {
                    errores.Add("Plan " + (char)('A' + i) + " (" + NombreDe(cadenas[i]) + "): " + ex.Message);
                }
            }

            // Ninguna conectó: se deja la primera para que los formularios muestren el error real
            cadenaActiva = cadenas[0];
            Plan = 0;
            Origen = "Sin conexión";
            return string.Join("\n\n", errores);
        }
    }

    /// <summary>Crea una conexión NUEVA y CERRADA. Siempre usarla con "using".</summary>
    public static SqlConnection ObtenerConexion()
    {
        if (cadenaActiva == null || Plan == 0)
            Seleccionar();
        return new SqlConnection(cadenaActiva);
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

    /// <summary>
    /// Verifica la conexión actual. Si se cayó, busca otra base de datos automáticamente.
    /// Devuelve null si hay alguna funcionando, o el mensaje de error.
    /// </summary>
    public static string ProbarConexion()
    {
        try
        {
            using SqlConnection cn = ObtenerConexion();
            cn.Open();
            return null;
        }
        catch
        {
            return Seleccionar();
        }
    }

    private static string NombreDe(string cadena)
    {
        return cadena.Contains("database.windows.net", StringComparison.OrdinalIgnoreCase) ? "Azure" : "Local";
    }
}
