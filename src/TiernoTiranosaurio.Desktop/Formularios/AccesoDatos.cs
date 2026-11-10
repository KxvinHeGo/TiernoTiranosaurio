using System.Data;
using Microsoft.Data.SqlClient;

namespace TiernoTiranosaurio.Desktop;

/// <summary>
/// Ayudante de datos de los formularios de Arturo (Integrante 2).
/// Todas las consultas van con SqlParameter: nunca se concatena texto del usuario en el SQL.
///
/// TEMPORAL: mientras Kevin sube DataBase.cs, la conexión se arma aquí con la cadena del README.
/// Cuando DataBase.cs exista, solo hay que cambiar el método Conexion() para que use la suya
/// (ej:  return DataBase.ObtenerConexion();)  y borrar la constante Cadena.
/// </summary>
internal static class AccesoDatos
{
    // Cadena de desarrollo acordada en el README. Si tu SQL Server tiene otro nombre,
    // cámbiala SOLO en tu PC y no subas ese cambio.
    private const string Cadena =
        @"Server=localhost\SQLEXPRESS;Database=TiernoTiranosaurio;Trusted_Connection=True;TrustServerCertificate=True;";

    private static SqlConnection Conexion()
    {
        return new SqlConnection(Cadena);
    }

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
