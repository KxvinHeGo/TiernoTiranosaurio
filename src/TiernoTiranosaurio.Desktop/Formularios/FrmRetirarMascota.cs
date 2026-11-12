using System.Data;
using Microsoft.Data.SqlClient;

namespace TiernoTiranosaurio.Desktop;

/// <summary>Retirar mascota con confirmación y DELETE. Solo se retiran mascotas Disponibles.</summary>
public class FrmRetirarMascota : Form
{
    private readonly TextBox txtCodigo;
    private readonly Label lblDatos;
    private readonly Button btnRetirar;
    private readonly DataGridView dgv;
    private int codigoActual = -1;
    private string nombreActual = "";

    public FrmRetirarMascota()
    {
        Text = "Retirar mascota";
        ClientSize = new Size(720, 430);
        StartPosition = FormStartPosition.CenterScreen;

        txtCodigo = new TextBox { Location = new Point(80, 15), Width = 80 };
        lblDatos = new Label { Location = new Point(15, 55), Size = new Size(560, 40), Font = new Font(Font.FontFamily, 10) };
        btnRetirar = Ui.Peligro(Ui.Boton("Retirar", 590, 55, (s, e) => Retirar(), 115));
        btnRetirar.Enabled = false;
        dgv = Ui.Grilla(15, 105, 690, 310);
        dgv.CellClick += (s, e) =>
        {
            if (e.RowIndex >= 0) Cargar(Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Codigo"].Value));
        };

        Controls.AddRange(new Control[]
        {
            Ui.Etiqueta("Código:", 15, 15), txtCodigo,
            Ui.Principal(Ui.Boton("Buscar", 170, 13, (s, e) => Buscar(), 90)),
            lblDatos, btnRetirar, dgv
        });

        txtCodigo.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Buscar(); } };
        Ui.Encabezado(this, "Retirar mascota");
        Load += (s, e) => Ui.Seguro(CargarLista);
    }

    private void CargarLista()
    {
        dgv.DataSource = AccesoDatos.Consultar(
            AccesoDatos.SqlMascotas + "WHERE m.Estado = 'Disponible' ORDER BY m.Codigo");
        Ui.FormatearMascotas(dgv);
    }

    private void Buscar()
    {
        if (!int.TryParse(txtCodigo.Text.Trim(), out int codigo))
        {
            Ui.Aviso("Escribe un código numérico.", txtCodigo);
            return;
        }
        Cargar(codigo);
    }

    private void Cargar(int codigo)
    {
        Ui.Seguro(() =>
        {
            DataRow m = AccesoDatos.BuscarMascota(codigo);
            codigoActual = -1;
            btnRetirar.Enabled = false;

            if (m == null)
            {
                lblDatos.Text = "No existe una mascota con el código " + codigo + ".";
                return;
            }

            nombreActual = m["Nombre"].ToString();
            lblDatos.Text = "#" + codigo + "  " + nombreActual + " — " + m["Tipo"] + ", " + m["Raza"] +
                            " — " + Convert.ToDecimal(m["Precio"]).ToString("C0", Ui.Pesos) + "  (" + m["Estado"] + ")";

            if (m["Estado"].ToString() == "Vendida")
            {
                lblDatos.Text += "\nYa fue vendida: no se puede retirar.";
                return;
            }

            codigoActual = codigo;
            btnRetirar.Enabled = true;
        });
    }

    private void Retirar()
    {
        if (codigoActual < 0) return;
        if (!Ui.Confirmar("¿Seguro que quieres retirar a " + nombreActual + " (código " + codigoActual + ")?\n" +
                          "Se borrará de la base de datos.")) return;

        Ui.Seguro(() =>
        {
            int filas = AccesoDatos.Ejecutar(
                "DELETE FROM Mascota WHERE Codigo = @codigo AND Estado = 'Disponible'",
                new SqlParameter("@codigo", SqlDbType.Int) { Value = codigoActual });

            Ui.Info(filas > 0 ? nombreActual + " fue retirada." : "No se pudo retirar: ya no está disponible.");
            codigoActual = -1;
            btnRetirar.Enabled = false;
            lblDatos.Text = "";
            txtCodigo.Clear();
            CargarLista();
        });
    }
}
