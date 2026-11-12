using System.Globalization;

namespace TiernoTiranosaurio.Desktop;

/// <summary>Utilidades de pantalla compartidas por los formularios.</summary>
internal static class Ui
{
    public static readonly CultureInfo Pesos = new CultureInfo("es-CO");

    // Mismos colores del Login y el Menú, para que toda la app se vea igual.
    public static readonly Color Verde = Color.FromArgb(46, 125, 50);        // #2E7D32
    public static readonly Color VerdeClaro = Color.FromArgb(232, 245, 233); // #E8F5E9
    public static readonly Color VerdeSeleccion = Color.FromArgb(200, 230, 201);
    public static readonly Color Rojo = Color.FromArgb(198, 40, 40);         // acciones que borran

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

    /// <summary>Pinta un botón como acción principal (verde, como "Ingresar" del Login).</summary>
    public static Button Principal(Button b) => Pintar(b, Verde);

    /// <summary>Pinta un botón como acción que borra datos (rojo).</summary>
    public static Button Peligro(Button b) => Pintar(b, Rojo);

    private static Button Pintar(Button b, Color fondo)
    {
        b.BackColor = fondo;
        b.ForeColor = Color.White;
        b.FlatStyle = FlatStyle.Flat;
        b.FlatAppearance.BorderSize = 0;
        b.Font = new Font("Segoe UI", 9.75F, FontStyle.Bold);
        b.UseVisualStyleBackColor = false;
        return b;
    }

    /// <summary>
    /// Agrega una franja verde con el título arriba del formulario (como el encabezado del Login)
    /// y baja los demás controles para que quepa. Llamarlo al FINAL del constructor.
    /// </summary>
    public static void Encabezado(Form form, string titulo)
    {
        const int alto = 48;

        // Se quitan las anclas mientras se mueve todo, para que no se estiren de más.
        var anclas = new Dictionary<Control, AnchorStyles>();
        foreach (Control c in form.Controls)
        {
            anclas[c] = c.Anchor;
            c.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            c.Top += alto;
        }
        form.ClientSize = new Size(form.ClientSize.Width, form.ClientSize.Height + alto);
        foreach (var par in anclas)
            par.Key.Anchor = par.Value;

        Panel franja = new Panel { Dock = DockStyle.Top, Height = alto, BackColor = Verde };
        franja.Controls.Add(new Label
        {
            Text = titulo,
            Dock = DockStyle.Fill,
            ForeColor = Color.White,
            Font = new Font("Segoe UI", 14F, FontStyle.Bold),
            TextAlign = ContentAlignment.MiddleLeft,
            Padding = new Padding(12, 0, 0, 0)
        });
        form.Controls.Add(franja);
        form.BackColor = Color.White;
    }

    /// <summary>DataGridView de solo lectura que selecciona filas completas.</summary>
    public static DataGridView Grilla(int x, int y, int ancho, int alto)
    {
        DataGridView g = new DataGridView
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
            AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill,
            BackgroundColor = Color.White,
            EnableHeadersVisualStyles = false,
            ColumnHeadersHeight = 30,
            ColumnHeadersHeightSizeMode = DataGridViewColumnHeadersHeightSizeMode.DisableResizing
        };
        // Encabezados verdes y filas alternas en verde claro.
        g.ColumnHeadersDefaultCellStyle.BackColor = Verde;
        g.ColumnHeadersDefaultCellStyle.ForeColor = Color.White;
        g.ColumnHeadersDefaultCellStyle.SelectionBackColor = Verde;
        g.ColumnHeadersDefaultCellStyle.Font = new Font("Segoe UI", 9F, FontStyle.Bold);
        g.AlternatingRowsDefaultCellStyle.BackColor = VerdeClaro;
        g.DefaultCellStyle.SelectionBackColor = VerdeSeleccion;
        g.DefaultCellStyle.SelectionForeColor = Color.Black;
        return g;
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
