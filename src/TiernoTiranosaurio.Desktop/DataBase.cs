using System.Data;
using Microsoft.Data.SqlClient;

namespace TiernoTiranosaurio.Desktop;

/// <summary>
/// Conexión manual a la base de datos (ADO.NET con SqlConnection). Responsable: Kevin (Integrante 1).
///
/// La cadena de conexión NO está escrita en el código: se lee de conexion.json (ver Configuracion.cs),
/// que cada integrante tiene en su PC con el nombre de SU servidor (ese archivo no se sube a GitHub).
///
/// Uso:
///   using SqlConnection cn = DataBase.ObtenerConexion();
///   DataTable t = DataBase.Consultar("SELECT ... WHERE Codigo = @codigo", new SqlParameter("@codigo", 101));
///   int filas  = DataBase.Ejecutar("UPDATE ...", parametros...);
///   object v   = DataBase.EjecutarEscalar("INSERT ...; SELECT SCOPE_IDENTITY();", parametros...);
/// </summary>
public static class DataBase
{
    /// <summary>Cadena de conexión que se está usando (útil para mostrar errores).</summary>
    public static string CadenaConexion => Configuracion.CadenaConexion;

    /// <summary>Crea una conexión NUEVA y CERRADA. Siempre usarla con "using".</summary>
    public static SqlConnection ObtenerConexion()
    {
        return new SqlConnection(Configuracion.CadenaConexion);
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
}
