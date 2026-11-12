using System.Data;
using Microsoft.Data.SqlClient;

namespace TiernoTiranosaurio.Desktop;

/// <summary>Venta: elegir cliente y mascota y hacer UPDATE con Estado = 'Vendida'.</summary>
public class FrmVenta : Form
{
    private readonly ComboBox cboCliente, cboMascota;
    private readonly Label lblPrecio;
    private readonly DataGridView dgvVendidas;

    public FrmVenta()
    {
        Text = "Venta de mascotas";
        ClientSize = new Size(680, 440);
        StartPosition = FormStartPosition.CenterScreen;

        cboCliente = new ComboBox { Location = new Point(90, 15), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
        cboMascota = new ComboBox { Location = new Point(90, 50), Width = 300, DropDownStyle = ComboBoxStyle.DropDownList };
        cboMascota.SelectedIndexChanged += (s, e) => MostrarPrecio();
        lblPrecio = new Label { Location = new Point(90, 85), Size = new Size(220, 24),
            Font = new Font(Font.FontFamily, 11, FontStyle.Bold) };
        Button btnVender = Ui.Principal(Ui.Boton("Registrar venta", 410, 48, (s, e) => Vender(), 140));
        dgvVendidas = Ui.Grilla(15, 150, 650, 275);

        Controls.AddRange(new Control[]
        {
            Ui.Etiqueta("Cliente:", 15, 15), cboCliente,
            Ui.Etiqueta("Mascota:", 15, 50), cboMascota,
            Ui.Etiqueta("Precio:", 15, 85), lblPrecio,
            btnVender,
            Ui.Etiqueta("Mascotas vendidas:", 15, 125),
            dgvVendidas
        });

        AcceptButton = btnVender;
        Ui.Encabezado(this, "Venta de mascotas");
        Load += (s, e) => Ui.Seguro(CargarTodo);
    }

    private void CargarTodo()
    {
        cboCliente.DisplayMember = "Descripcion";
        cboCliente.ValueMember = "IdCliente";
        cboCliente.DataSource = AccesoDatos.Consultar(
            "SELECT IdCliente, Nombre + ' (' + ISNULL(Documento, '') + ')' AS Descripcion FROM Cliente ORDER BY Nombre");
        cboCliente.SelectedIndex = -1;

        // Solo se pueden vender las mascotas disponibles.
        cboMascota.DisplayMember = "Descripcion";
        cboMascota.ValueMember = "Codigo";
        cboMascota.DataSource = AccesoDatos.Consultar(
            @"SELECT m.Codigo,
                     CAST(m.Codigo AS VARCHAR(10)) + ' - ' + m.Nombre + ' (' + t.Nombre + ')' AS Descripcion,
                     m.Precio
              FROM Mascota m
              INNER JOIN TipoMascota t ON t.IdTipo = m.IdTipo
              WHERE m.Estado = 'Disponible'
              ORDER BY m.Nombre");
        cboMascota.SelectedIndex = -1;
        lblPrecio.Text = "";

        dgvVendidas.DataSource = AccesoDatos.Consultar(
            @"SELECT m.Codigo, m.Nombre AS Mascota, t.Nombre AS Tipo, c.Nombre AS Cliente, m.Precio
              FROM Mascota m
              INNER JOIN TipoMascota t ON t.IdTipo = m.IdTipo
              INNER JOIN Cliente c ON c.IdCliente = m.IdCliente
              WHERE m.Estado = 'Vendida'
              ORDER BY m.Codigo DESC");
        Ui.FormatearMascotas(dgvVendidas);
    }

    private void MostrarPrecio()
    {
        lblPrecio.Text = cboMascota.SelectedItem is DataRowView fila
            ? Convert.ToDecimal(fila["Precio"]).ToString("C0", Ui.Pesos)
            : "";
    }

    private void Vender()
    {
        if (cboCliente.SelectedValue == null) { Ui.Aviso("Selecciona el cliente.", cboCliente); return; }
        if (cboMascota.SelectedValue == null) { Ui.Aviso("Selecciona la mascota.", cboMascota); return; }

        if (!Ui.Confirmar("¿Vender " + cboMascota.Text + "\na " + cboCliente.Text + " por " + lblPrecio.Text + "?"))
            return;

        Ui.Seguro(() =>
        {
            // El "AND Estado = 'Disponible'" evita vender dos veces la misma mascota.
            int filas = AccesoDatos.Ejecutar(
                @"UPDATE Mascota
                  SET Estado = 'Vendida', IdCliente = @idCliente
                  WHERE Codigo = @codigo AND Estado = 'Disponible'",
                new SqlParameter("@idCliente", SqlDbType.Int) { Value = Convert.ToInt32(cboCliente.SelectedValue) },
                new SqlParameter("@codigo", SqlDbType.Int) { Value = Convert.ToInt32(cboMascota.SelectedValue) });

            if (filas > 0) Ui.Info("¡Venta registrada!");
            else Ui.Aviso("Esa mascota ya no está disponible.");

            CargarTodo();
        });
    }
}
