using System.Data;

namespace TiernoTiranosaurio.Desktop;

/// <summary>
/// Cotización: calcula subtotal, descuento, IVA y total SOLO en pantalla.
/// No lee ni escribe en la base de datos. (Integrante 2)
/// </summary>
public class FrmCotizacion : Form
{
    private const decimal TasaIva = 0.19m;

    private readonly DataTable items = new DataTable();
    private readonly TextBox txtCliente, txtConcepto, txtPrecio;
    private readonly NumericUpDown nudCantidad, nudDescuento;
    private readonly CheckBox chkIva;
    private readonly DataGridView dgv;
    private readonly Label lblSubtotal, lblDescuento, lblIva, lblTotal;

    public FrmCotizacion()
    {
        Text = "Cotización";
        ClientSize = new Size(720, 490);
        StartPosition = FormStartPosition.CenterScreen;

        items.Columns.Add("Concepto", typeof(string));
        items.Columns.Add("Cantidad", typeof(int));
        items.Columns.Add("PrecioUnitario", typeof(decimal));
        items.Columns.Add("Subtotal", typeof(decimal));

        txtCliente = new TextBox { Location = new Point(100, 15), Width = 260 };
        txtConcepto = new TextBox { Location = new Point(100, 50), Width = 260 };
        nudCantidad = new NumericUpDown { Location = new Point(100, 85), Width = 70, Minimum = 1, Maximum = 100, Value = 1 };
        txtPrecio = new TextBox { Location = new Point(265, 85), Width = 95 };
        Button btnAgregar = Ui.Boton("Agregar", 375, 83, (s, e) => Agregar(), 95);

        dgv = Ui.Grilla(15, 125, 690, 215);
        dgv.DataSource = items;

        nudDescuento = new NumericUpDown { Location = new Point(110, 358), Width = 60, Minimum = 0, Maximum = 100,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
        nudDescuento.ValueChanged += (s, e) => Recalcular();
        chkIva = new CheckBox { Text = "Aplicar IVA 19%", Location = new Point(15, 393), AutoSize = true, Checked = true,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Left };
        chkIva.CheckedChanged += (s, e) => Recalcular();

        Label descuentoTxt = Ui.Etiqueta("Descuento %:", 15, 358);
        descuentoTxt.Anchor = AnchorStyles.Bottom | AnchorStyles.Left;

        Controls.AddRange(new Control[]
        {
            Ui.Etiqueta("Cliente:", 15, 15), txtCliente,
            Ui.Etiqueta("Concepto:", 15, 50), txtConcepto,
            Ui.Etiqueta("Cantidad:", 15, 85), nudCantidad,
            Ui.Etiqueta("Precio unit.:", 185, 85), txtPrecio,
            btnAgregar,
            Ui.Boton("Quitar fila", 480, 83, (s, e) => QuitarFila(), 95),
            Ui.Boton("Nueva", 585, 83, (s, e) => Limpiar(), 95),
            dgv, descuentoTxt, nudDescuento, chkIva
        });

        lblSubtotal = Total("Subtotal:", 355);
        lblDescuento = Total("Descuento:", 383);
        lblIva = Total("IVA:", 411);
        lblTotal = Total("TOTAL:", 445);
        lblTotal.Font = new Font(Font.FontFamily, 12, FontStyle.Bold);

        AcceptButton = btnAgregar;
        Load += (s, e) =>
        {
            dgv.Columns["PrecioUnitario"].HeaderText = "Precio unitario";
            foreach (string col in new[] { "PrecioUnitario", "Subtotal" })
            {
                dgv.Columns[col].DefaultCellStyle.Format = "C0";
                dgv.Columns[col].DefaultCellStyle.FormatProvider = Ui.Pesos;
            }
            Recalcular();
        };
    }

    private Label Total(string texto, int y)
    {
        Controls.Add(new Label { Text = texto, Location = new Point(420, y + 3), AutoSize = true,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right });
        Label valor = new Label
        {
            Location = new Point(505, y),
            Size = new Size(200, 24),
            TextAlign = ContentAlignment.MiddleRight,
            Anchor = AnchorStyles.Bottom | AnchorStyles.Right
        };
        Controls.Add(valor);
        return valor;
    }

    private void Agregar()
    {
        string concepto = txtConcepto.Text.Trim();
        if (concepto == "")
        {
            Ui.Aviso("Escribe el concepto (ej: nombre de la mascota, vacuna, accesorio).", txtConcepto);
            return;
        }
        if (!Ui.LeerPrecio(txtPrecio, out decimal precio)) return;

        int cantidad = (int)nudCantidad.Value;
        items.Rows.Add(concepto, cantidad, precio, cantidad * precio);

        txtConcepto.Clear();
        txtPrecio.Clear();
        nudCantidad.Value = 1;
        txtConcepto.Focus();
        Recalcular();
    }

    private void QuitarFila()
    {
        if (dgv.CurrentRow == null) return;
        items.Rows.RemoveAt(dgv.CurrentRow.Index);
        Recalcular();
    }

    private void Recalcular()
    {
        decimal subtotal = 0;
        foreach (DataRow fila in items.Rows)
            subtotal += (decimal)fila["Subtotal"];

        decimal descuento = Math.Round(subtotal * nudDescuento.Value / 100m, 2);
        decimal baseGravable = subtotal - descuento;
        decimal iva = chkIva.Checked ? Math.Round(baseGravable * TasaIva, 2) : 0;
        decimal total = baseGravable + iva;

        lblSubtotal.Text = subtotal.ToString("C0", Ui.Pesos);
        lblDescuento.Text = "- " + descuento.ToString("C0", Ui.Pesos);
        lblIva.Text = iva.ToString("C0", Ui.Pesos);
        lblTotal.Text = total.ToString("C0", Ui.Pesos);
    }

    private void Limpiar()
    {
        items.Rows.Clear();
        txtCliente.Clear();
        txtConcepto.Clear();
        txtPrecio.Clear();
        nudCantidad.Value = 1;
        nudDescuento.Value = 0;
        chkIva.Checked = true;
        Recalcular();
        txtCliente.Focus();
    }
}
