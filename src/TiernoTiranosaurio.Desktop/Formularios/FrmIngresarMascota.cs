using System.Data;
using Microsoft.Data.SqlClient;

namespace TiernoTiranosaurio.Desktop;

/// <summary>Ingresar mascota: ComboBox cargado desde TipoMascota + INSERT. (Integrante 2)</summary>
public class FrmIngresarMascota : Form
{
    private readonly TextBox txtNombre, txtRaza, txtPrecio;
    private readonly ComboBox cboTipo;
    private readonly NumericUpDown nudEdad;

    public FrmIngresarMascota()
    {
        Text = "Ingresar mascota";
        ClientSize = new Size(360, 260);
        StartPosition = FormStartPosition.CenterScreen;
        FormBorderStyle = FormBorderStyle.FixedDialog;
        MaximizeBox = false;

        txtNombre = new TextBox { Location = new Point(110, 15), Width = 220, MaxLength = 50 };
        cboTipo = new ComboBox { Location = new Point(110, 50), Width = 220, DropDownStyle = ComboBoxStyle.DropDownList };
        txtRaza = new TextBox { Location = new Point(110, 85), Width = 220, MaxLength = 50 };
        nudEdad = new NumericUpDown { Location = new Point(110, 120), Width = 80, Minimum = 0, Maximum = 50 };
        txtPrecio = new TextBox { Location = new Point(110, 155), Width = 140 };

        Button btnGuardar = Ui.Boton("Guardar", 110, 200, (s, e) => Guardar());

        Controls.AddRange(new Control[]
        {
            Ui.Etiqueta("Nombre:", 15, 15), txtNombre,
            Ui.Etiqueta("Tipo:", 15, 50), cboTipo,
            Ui.Etiqueta("Raza:", 15, 85), txtRaza,
            Ui.Etiqueta("Edad (años):", 15, 120), nudEdad,
            Ui.Etiqueta("Precio:", 15, 155), txtPrecio,
            btnGuardar,
            Ui.Boton("Limpiar", 225, 200, (s, e) => Limpiar(), 105)
        });

        AcceptButton = btnGuardar;
        Load += (s, e) => Ui.Seguro(() => AccesoDatos.CargarTipos(cboTipo));
    }

    private void Guardar()
    {
        if (txtNombre.Text.Trim() == "") { Ui.Aviso("El nombre es obligatorio.", txtNombre); return; }
        if (cboTipo.SelectedValue == null) { Ui.Aviso("Selecciona el tipo de mascota.", cboTipo); return; }
        if (!Ui.LeerPrecio(txtPrecio, out decimal precio)) return;

        Ui.Seguro(() =>
        {
            AccesoDatos.Ejecutar(
                @"INSERT INTO Mascota (Nombre, IdTipo, Raza, Edad, Precio, Estado)
                  VALUES (@nombre, @idTipo, @raza, @edad, @precio, 'Disponible')",
                new SqlParameter("@nombre", SqlDbType.NVarChar, 50) { Value = txtNombre.Text.Trim() },
                new SqlParameter("@idTipo", SqlDbType.Int) { Value = Convert.ToInt32(cboTipo.SelectedValue) },
                new SqlParameter("@raza", SqlDbType.NVarChar, 50) { Value = txtRaza.Text.Trim() },
                new SqlParameter("@edad", SqlDbType.Int) { Value = (int)nudEdad.Value },
                new SqlParameter("@precio", SqlDbType.Decimal) { Value = precio, Precision = 12, Scale = 2 });

            Ui.Info("Mascota " + txtNombre.Text.Trim() + " ingresada.");
            Limpiar();
        });
    }

    private void Limpiar()
    {
        txtNombre.Clear();
        txtRaza.Clear();
        txtPrecio.Clear();
        nudEdad.Value = 0;
        cboTipo.SelectedIndex = -1;
        txtNombre.Focus();
    }
}
