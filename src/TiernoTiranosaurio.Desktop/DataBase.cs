using System.Data;
using Microsoft.Data.SqlClient;

namespace TiernoTiranosaurio.Desktop;

/// <summary>Conexión manual a la BD: usa la primera cadena de conexion.json que conecte.</summary>
public static class DataBase
{
    private static readonly object candado = new();
    private static string cadenaActiva;

    /// <summary>Nombre amigable de la BD en uso: "Local", "Azure" o "Sin conexión".</summary>
    public static string Origen { get; private set; } = "Sin conexión";

    public static int Plan { get; private set; }

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

            cadenaActiva = cadenas[0];
            Plan = 0;
            Origen = "Sin conexión";
            return string.Join("\n\n", errores);
        }
    }

    public static SqlConnection ObtenerConexion()
    {
        if (cadenaActiva == null || Plan == 0)
            Seleccionar();
        return new SqlConnection(cadenaActiva);
    }

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

    public static int Ejecutar(string sql, params SqlParameter[] parametros)
    {
        using SqlConnection cn = ObtenerConexion();
        using SqlCommand cmd = new SqlCommand(sql, cn);
        cmd.Parameters.AddRange(parametros);
        cn.Open();
        return cmd.ExecuteNonQuery();
    }

    public static object EjecutarEscalar(string sql, params SqlParameter[] parametros)
    {
        using SqlConnection cn = ObtenerConexion();
        using SqlCommand cmd = new SqlCommand(sql, cn);
        cmd.Parameters.AddRange(parametros);
        cn.Open();
        return cmd.ExecuteScalar();
    }

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
