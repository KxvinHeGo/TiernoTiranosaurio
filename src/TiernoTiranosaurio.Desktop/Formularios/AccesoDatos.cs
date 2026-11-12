using System.Data;
using Microsoft.Data.SqlClient;

namespace TiernoTiranosaurio.Desktop;

/// <summary>Consultas de los formularios. Todo el SQL usa SqlParameter.</summary>
internal static class AccesoDatos
{
    private static SqlConnection Conexion() => DataBase.ObtenerConexion();

    /// <summary>Ejecuta un SELECT y devuelve las filas en un DataTable.</summary>
    public static DataTable Consultar(string sql, params SqlParameter[] parametros)
    {
        using SqlConnection cn = Conexion();
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
        using SqlConnection cn = Conexion();
        using SqlCommand cmd = new SqlCommand(sql, cn);
        cmd.Parameters.AddRange(parametros);
        cn.Open();
        return cmd.ExecuteNonQuery();
    }

    /// <summary>Ejecuta y devuelve un solo valor (ej: el código de la mascota recién creada).</summary>
    public static object EjecutarEscalar(string sql, params SqlParameter[] parametros)
    {
        using SqlConnection cn = Conexion();
        using SqlCommand cmd = new SqlCommand(sql, cn);
        cmd.Parameters.AddRange(parametros);
        cn.Open();
        return cmd.ExecuteScalar();
    }

    // ---------- Consultas que comparten varios formularios ----------

    public const string SqlMascotas =
        @"SELECT m.Codigo, m.Nombre, m.IdTipo, t.Nombre AS Tipo, m.Raza, m.Edad, m.Precio, m.Estado
          FROM Mascota m
          INNER JOIN TipoMascota t ON t.IdTipo = m.IdTipo ";

    /// <summary>Busca una mascota por su código. Devuelve null si no existe.</summary>
    public static DataRow BuscarMascota(int codigo)
    {
        DataTable t = Consultar(SqlMascotas + "WHERE m.Codigo = @codigo",
            new SqlParameter("@codigo", SqlDbType.Int) { Value = codigo });
        return t.Rows.Count == 0 ? null : t.Rows[0];
    }

    /// <summary>Llena un ComboBox con los registros de TipoMascota.</summary>
    public static void CargarTipos(ComboBox combo)
    {
        combo.DisplayMember = "Nombre";
        combo.ValueMember = "IdTipo";
        combo.DataSource = Consultar("SELECT IdTipo, Nombre FROM TipoMascota ORDER BY Nombre");
        combo.SelectedIndex = -1;
    }
}
