using System.Data;
using Microsoft.Data.SqlClient;

namespace TiernoTiranosaurio.Desktop;

/// <summary>Consulta de mascotas por código y por nombre.</summary>
public class FrmConsulta : Form
{
    private readonly TextBox txtCodigo;
    private readonly TextBox txtNombre;
    private readonly DataGridView dgv;

    public FrmConsulta()
    {
        Text = "Consulta de mascotas";
        ClientSize = new Size(760, 440);
        StartPosition = FormStartPosition.CenterScreen;

        txtCodigo = new TextBox { Location = new Point(80, 15), Width = 100 };
        txtNombre = new TextBox { Location = new Point(80, 50), Width = 220 };
        dgv = Ui.Grilla(15, 90, 730, 335);

        Controls.AddRange(new Control[]
        {
            Ui.Etiqueta("Código:", 15, 15), txtCodigo,
            Ui.Principal(Ui.Boton("Buscar por código", 190, 13, (s, e) => BuscarPorCodigo(), 150)),
            Ui.Etiqueta("Nombre:", 15, 50), txtNombre,
            Ui.Principal(Ui.Boton("Buscar por nombre", 310, 48, (s, e) => BuscarPorNombre(), 150)),
            Ui.Boton("Ver todas", 470, 48, (s, e) => VerTodas()),
            dgv
        });

        txtCodigo.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BuscarPorCodigo(); } };
        txtNombre.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; BuscarPorNombre(); } };
        Ui.Encabezado(this, "Consulta de mascotas");
        Load += (s, e) => VerTodas();
    }

    private void VerTodas()
    {
        Ui.Seguro(() => Mostrar(AccesoDatos.Consultar(AccesoDatos.SqlMascotas + "ORDER BY m.Codigo"), false));
    }

    private void BuscarPorCodigo()
    {
        if (!int.TryParse(txtCodigo.Text.Trim(), out int codigo))
        {
            Ui.Aviso("Escribe un código numérico.", txtCodigo);
            return;
        }
        Ui.Seguro(() => Mostrar(AccesoDatos.Consultar(
            AccesoDatos.SqlMascotas + "WHERE m.Codigo = @codigo",
            new SqlParameter("@codigo", SqlDbType.Int) { Value = codigo }), true));
    }

    private void BuscarPorNombre()
    {
        string nombre = txtNombre.Text.Trim();
        if (nombre == "")
        {
            Ui.Aviso("Escribe el nombre (o parte del nombre) de la mascota.", txtNombre);
            return;
        }
        Ui.Seguro(() => Mostrar(AccesoDatos.Consultar(
            AccesoDatos.SqlMascotas + "WHERE m.Nombre LIKE @nombre ORDER BY m.Nombre",
            new SqlParameter("@nombre", SqlDbType.NVarChar, 60) { Value = "%" + nombre + "%" }), true));
    }

    private void Mostrar(DataTable tabla, bool avisarSiVacio)
    {
        dgv.DataSource = tabla;
        Ui.FormatearMascotas(dgv);
        if (avisarSiVacio && tabla.Rows.Count == 0)
            Ui.Info("No se encontraron mascotas.");
    }
}
