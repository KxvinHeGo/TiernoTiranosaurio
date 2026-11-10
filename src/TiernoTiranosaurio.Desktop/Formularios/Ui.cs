using System.Globalization;

namespace TiernoTiranosaurio.Desktop;

/// <summary>Utilidades de pantalla compartidas por los formularios de Arturo (Integrante 2).</summary>
internal static class Ui
{
    public static readonly CultureInfo Pesos = new CultureInfo("es-CO");

    /// <summary>Ejecuta una acción y muestra un mensaje si la base de datos falla.</summary>
    public static void Seguro(Action accion)
    {
        try
        {
            accion();
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error con la base de datos:\n" + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
    }

    public static void Aviso(string texto, Control enfocar = null)
    {
        MessageBox.Show(texto, "Aviso", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        enfocar?.Focus();
    }

    public static void Info(string texto)
    {
        MessageBox.Show(texto, "El Tierno Tiranosaurio", MessageBoxButtons.OK, MessageBoxIcon.Information);
    }

    public static bool Confirmar(string texto)
    {
        return MessageBox.Show(texto, "Confirmar", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
               == DialogResult.Yes;
    }

    public static Label Etiqueta(string texto, int x, int y)
    {
        return new Label { Text = texto, Location = new Point(x, y + 3), AutoSize = true };
    }

    public static Button Boton(string texto, int x, int y, EventHandler clic, int ancho = 110)
    {
        Button b = new Button { Text = texto, Location = new Point(x, y), Width = ancho, Height = 28 };
        b.Click += clic;
        return b;
    }

    /// <summary>DataGridView de solo lectura que selecciona filas completas.</summary>
    public static DataGridView Grilla(int x, int y, int ancho, int alto)
    {
        return new DataGridView
        {
            Location = new Point(x, y),
            Size = new Size(ancho, alto),
            Anchor = AnchorStyles.Top | AnchorStyles.Bottom | AnchorStyles.Left | AnchorStyles.Right,
            ReadOnly = true,
            AllowUserToAddRows = false,
            AllowUserToDeleteRows = false,
            MultiSelect = false,
            RowHeadersVisible = false,
            SelectionMode = DataGridViewSelectionMode.FullRowSelect,
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill
        };
    }

    /// <summary>Oculta IdTipo y da formato de pesos a la columna Precio.</summary>
    public static void FormatearMascotas(DataGridView g)
    {
        if (g.Columns.Contains("IdTipo")) g.Columns["IdTipo"].Visible = false;
        if (g.Columns.Contains("Precio"))
        {
            g.Columns["Precio"].DefaultCellStyle.Format = "C0";
            g.Columns["Precio"].DefaultCellStyle.FormatProvider = Pesos;
        }
    }

    /// <summary>Lee un precio mayor que cero (acepta 850000, 850.000 o 850000,50).</summary>
    public static bool LeerPrecio(TextBox txt, out decimal precio)
    {
        string texto = txt.Text.Trim().Replace("$", "").Replace(" ", "");
        bool ok = decimal.TryParse(texto, NumberStyles.Number, Pesos, out precio) && precio > 0;
        if (!ok) Aviso("Escribe un precio válido mayor que 0.", txt);
        return ok;
    }
}
