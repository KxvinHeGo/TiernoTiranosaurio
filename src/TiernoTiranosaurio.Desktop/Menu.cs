namespace TiernoTiranosaurio.Desktop;

/// <summary>
/// Formulario principal (Maestro MDI). Contiene el menú con los procesos de la tienda
/// y habilita o bloquea opciones según el rol de quien inició sesión.
/// Responsable: Kevin (Integrante 1). Los formularios hijos son de Arturo (carpeta Formularios/).
///
/// Roles:
///   Administrador → todo
///   Vendedor      → Consulta y Novedades (Actualización bloqueada)
/// </summary>
public partial class Menu : Form
{
    /// <summary>True si el usuario eligió "Cerrar sesión" (Program.cs vuelve a mostrar el Login).</summary>
    public bool CerrarSesionSolicitada { get; private set; }

    public Menu()
    {
        InitializeComponent();
    }

    private void Menu_Load(object sender, EventArgs e)
    {
        Text = "El Tierno Tiranosaurio - " + Sesion.Usuario + " (" + Sesion.Rol + ")";
        lblUsuario.Text = "👤 " + Sesion.Usuario;
        lblRol.Text = "Rol: " + Sesion.Rol;
        AplicarPermisos();
        VerificarBaseDatos();
    }

    /// <summary>Bloquea las opciones que el rol no puede usar.</summary>
    private void AplicarPermisos()
    {
        bool admin = Sesion.EsAdministrador;

        mnuActualizacion.Enabled = admin;
        mnuIngresar.Enabled = admin;
        mnuModificar.Enabled = admin;
        mnuRetirar.Enabled = admin;

        if (!admin)
            mnuActualizacion.ToolTipText = "Solo disponible para el rol Administrador";
    }

    private void VerificarBaseDatos()
    {
        string error = DataBase.ProbarConexion();
        lblBaseDatos.Text = error == null ? "🟢 Base de datos conectada" : "🔴 Sin conexión a la base de datos";
        lblBaseDatos.ToolTipText = error ?? "";
    }

    /// <summary>
    /// Abre un formulario hijo dentro del MDI. Si ya estaba abierto, lo trae al frente
    /// en vez de abrir otro igual.
    /// </summary>
    private void Abrir<T>() where T : Form, new()
    {
        foreach (Form hijo in MdiChildren)
        {
            if (hijo is T)
            {
                hijo.WindowState = FormWindowState.Normal;
                hijo.Activate();
                return;
            }
        }

        T nuevo = new T();
        nuevo.MdiParent = this;
        nuevo.StartPosition = FormStartPosition.CenterParent;
        nuevo.Show();
    }

    // ---------- Consulta ----------
    private void mnuConsultar_Click(object sender, EventArgs e) => Abrir<FrmConsulta>();

    // ---------- Actualización (solo Administrador) ----------
    private void mnuIngresar_Click(object sender, EventArgs e) => Abrir<FrmIngresarMascota>();
    private void mnuModificar_Click(object sender, EventArgs e) => Abrir<FrmModificarMascota>();
    private void mnuRetirar_Click(object sender, EventArgs e) => Abrir<FrmRetirarMascota>();

    // ---------- Novedades ----------
    private void mnuCotizacion_Click(object sender, EventArgs e) => Abrir<FrmCotizacion>();
    private void mnuVenta_Click(object sender, EventArgs e) => Abrir<FrmVenta>();

    // ---------- Ventanas ----------
    private void mnuCascada_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.Cascade);
    private void mnuMosaicoH_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.TileHorizontal);
    private void mnuMosaicoV_Click(object sender, EventArgs e) => LayoutMdi(MdiLayout.TileVertical);

    private void mnuCerrarVentanas_Click(object sender, EventArgs e)
    {
        foreach (Form hijo in MdiChildren)
            hijo.Close();
    }

    // ---------- Salir ----------
    private void mnuCerrarSesion_Click(object sender, EventArgs e)
    {
        if (MessageBox.Show("¿Cerrar la sesión de " + Sesion.Usuario + "?", "Cerrar sesión",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            return;

        CerrarSesionSolicitada = true;
        Close();
    }

    private void mnuSalir_Click(object sender, EventArgs e)
    {
        Close();
    }

    private void Menu_FormClosing(object sender, FormClosingEventArgs e)
    {
        if (CerrarSesionSolicitada || e.CloseReason != CloseReason.UserClosing)
            return;

        if (MessageBox.Show("¿Seguro que quieres salir de El Tierno Tiranosaurio?", "Salir",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
            e.Cancel = true;
    }
}
