using Microsoft.Data.SqlClient;

namespace TiernoTiranosaurio.Api.Datos;

/// <summary>Acceso a la base de datos de la API (ADO.NET con SqlConnection).</summary>
public class BaseDatos
{
    private readonly string cadena;

    public BaseDatos(IConfiguration config)
    {
        cadena = config.GetConnectionString("TiernoTiranosaurio")
                 ?? throw new InvalidOperationException("Falta ConnectionStrings:TiernoTiranosaurio en la configuración.");
    }

    /// <summary>Ejecuta un SELECT y convierte cada fila con la función "leer".</summary>
    public List<T> Consultar<T>(string sql, Func<SqlDataReader, T> leer, params SqlParameter[] parametros)
    {
        using SqlConnection cn = new SqlConnection(cadena);
        using SqlCommand cmd = new SqlCommand(sql, cn);
        cmd.Parameters.AddRange(parametros);
        cn.Open();

        using SqlDataReader dr = cmd.ExecuteReader();
        List<T> lista = new List<T>();
        while (dr.Read())
            lista.Add(leer(dr));
        return lista;
    }

    /// <summary>Convierte null de C# en DBNull para los parámetros opcionales.</summary>
    public static object Valor(object? v) => v ?? DBNull.Value;
}
