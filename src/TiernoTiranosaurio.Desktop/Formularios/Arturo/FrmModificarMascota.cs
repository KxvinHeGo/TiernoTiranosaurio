using System.Data;
using Microsoft.Data.SqlClient;

namespace TiernoTiranosaurio.Desktop;

/// <summary>Cambiar datos: buscar la mascota, editar y hacer el UPDATE. (Integrante 2)</summary>
public class FrmModificarMascota : Form
{
    private readonly TextBox txtBuscar, txtNombre, txtRaza, txtPrecio;
    private readonly Label lblCodigo, lblEstado;
    private readonly ComboBox cboTipo;
    private readonly NumericUpDown nudEdad;
    private readonly Button btnGuardar;
    private readonly DataGridView dgv;
    private int codigoActual = -1;

    public FrmModificarMascota()
    {
        Text = "Cambiar datos de mascota";
        ClientSize = new Size(820, 420);
        StartPosition = FormStartPosition.CenterScreen;

        txtBuscar = new TextBox { Location = new Point(110, 15), Width = 80 };
        lblCodigo = new Label { Location = new Point(110, 58), AutoSize = true, Font = new Font(Font, FontStyle.Bold) };
        lblEstado = new Label { Location = new Point(200, 58), AutoSize = true };
        txtNombre = new TextBox { Location = new Point(110, 90), Width = 200, MaxLength = 50 };
        cboTipo = new ComboBox { Location = new Point(110, 125), Width = 200, DropDownStyle = ComboBoxStyle.DropDownList };
        txtRaza = new TextBox { Location = new Point(110, 160), Width = 200, MaxLength = 50 };
        nudEdad = new NumericUpDown { Location = new Point(110, 195), Width = 80, Minimum = 0, Maximum = 50 };
        txtPrecio = new TextBox { Location = new Point(110, 230), Width = 140 };
        btnGuardar = Ui.Boton("Guardar cambios", 110, 275, (s, e) => Guardar(), 130);
        dgv = Ui.Grilla(330, 15, 475, 390);
        dgv.CellClick += (s, e) =>
        {
            if (e.RowIndex >= 0) Cargar(Convert.ToInt32(dgv.Rows[e.RowIndex].Cells["Codigo"].Value));
        };

        Controls.AddRange(new Control[]
        {
            Ui.Etiqueta("Código:", 15, 15), txtBuscar,
            Ui.Boton("Buscar", 200, 13, (s, e) => Buscar(), 90),
            Ui.Etiqueta("Editando:", 15, 55), lblCodigo, lblEstado,
            Ui.Etiqueta("Nombre:", 15, 90), txtNombre,
            Ui.Etiqueta("Tipo:", 15, 125), cboTipo,
            Ui.Etiqueta("Raza:", 15, 160), txtRaza,
            Ui.Etiqueta("Edad (años):", 15, 195), nudEdad,
            Ui.Etiqueta("Precio:", 15, 230), txtPrecio,
            btnGuardar,
            new Label { Text = "También puedes hacer clic en una mascota de la tabla.", Location = new Point(15, 320),
                Size = new Size(300, 40), ForeColor = SystemColors.GrayText },
            dgv
        });

        txtBuscar.KeyDown += (s, e) => { if (e.KeyCode == Keys.Enter) { e.SuppressKeyPress = true; Buscar(); } };
        Load += (s, e) => Ui.Seguro(() =>
        {
            AccesoDatos.CargarTipos(cboTipo);
            CargarLista();
            Limpiar();
        });
    }

    private void CargarLista()
    {
        dgv.DataSource = AccesoDatos.Consultar(AccesoDatos.SqlMascotas + "ORDER BY m.Codigo");
        Ui.FormatearMascotas(dgv);
    }

    private void Buscar()
    {
        if (!int.TryParse(txtBuscar.Text.Trim(), out int codigo))
        {
            Ui.Aviso("Escribe un código numérico.", txtBuscar);
            return;
        }
        Cargar(codigo);
    }

    private void Cargar(int codigo)
    {
        Ui.Seguro(() =>
        {
            DataRow m = AccesoDatos.BuscarMascota(codigo);
            if (m == null)
            {
                Ui.Info("No existe una mascota con el código " + codigo + ".");
                Limpiar();
                return;
            }

            codigoActual = codigo;
            lblCodigo.Text = codigo.ToString();
            lblEstado.Text = "(" + m["Estado"] + ")";
            txtNombre.Text = m["Nombre"].ToString();
            cboTipo.SelectedValue = Convert.ToInt32(m["IdTipo"]);
            txtRaza.Text = Convert.ToString(m["Raza"]);
            int edad = m["Edad"] == DBNull.Value ? 0 : Convert.ToInt32(m["Edad"]);
            nudEdad.Value = Math.Min(Math.Max(edad, (int)nudEdad.Minimum), (int)nudEdad.Maximum);
            txtPrecio.Text = Convert.ToDecimal(m["Precio"]).ToString("0.##", Ui.Pesos);
            HabilitarEdicion(true);
            txtNombre.Focus();
        });
    }

    private void Guardar()
    {
        if (codigoActual < 0) { Ui.Aviso("Primero busca la mascota que quieres cambiar.", txtBuscar); return; }
        if (txtNombre.Text.Trim() == "") { Ui.Aviso("El nombre es obligatorio.", txtNombre); return; }
        if (cboTipo.SelectedValue == null) { Ui.Aviso("Selecciona el tipo de mascota.", cboTipo); return; }
        if (!Ui.LeerPrecio(txtPrecio, out decimal precio)) return;

        Ui.Seguro(() =>
        {
            int filas = AccesoDatos.Ejecutar(
                @"UPDATE Mascota
                  SET Nombre = @nombre, IdTipo = @idTipo, Raza = @raza, Edad = @edad, Precio = @precio
                  WHERE Codigo = @codigo",
                new SqlParameter("@nombre", SqlDbType.NVarChar, 50) { Value = txtNombre.Text.Trim() },
                new SqlParameter("@idTipo", SqlDbType.Int) { Value = Convert.ToInt32(cboTipo.SelectedValue) },
                new SqlParameter("@raza", SqlDbType.NVarChar, 50) { Value = txtRaza.Text.Trim() },
                new SqlParameter("@edad", SqlDbType.Int) { Value = (int)nudEdad.Value },
                new SqlParameter("@precio", SqlDbType.Decimal) { Value = precio, Precision = 12, Scale = 2 },
                new SqlParameter("@codigo", SqlDbType.Int) { Value = codigoActual });

            Ui.Info(filas > 0 ? "Datos actualizados." : "La mascota ya no existe.");
            CargarLista();
            Limpiar();
        });
    }

    private void Limpiar()
    {
        codigoActual = -1;
        lblCodigo.Text = "—";
        lblEstado.Text = "";
        txtBuscar.Clear();
        txtNombre.Clear();
        txtRaza.Clear();
        txtPrecio.Clear();
        nudEdad.Value = 0;
        cboTipo.SelectedIndex = -1;
        HabilitarEdicion(false);
        txtBuscar.Focus();
    }

    private void HabilitarEdicion(bool si)
    {
        txtNombre.Enabled = cboTipo.Enabled = txtRaza.Enabled = nudEdad.Enabled = txtPrecio.Enabled = btnGuardar.Enabled = si;
    }
}
