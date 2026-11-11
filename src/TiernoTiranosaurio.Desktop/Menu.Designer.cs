namespace TiernoTiranosaurio.Desktop
{
    partial class Menu
    {
        /// <summary>Variable del diseñador necesaria.</summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>Limpiar los recursos que se estén usando.</summary>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código generado por el Diseñador de Windows Forms

        private void InitializeComponent()
        {
            menuPrincipal = new MenuStrip();
            mnuConsulta = new ToolStripMenuItem();
            mnuConsultar = new ToolStripMenuItem();
            mnuActualizacion = new ToolStripMenuItem();
            mnuIngresar = new ToolStripMenuItem();
            mnuModificar = new ToolStripMenuItem();
            mnuRetirar = new ToolStripMenuItem();
            mnuNovedades = new ToolStripMenuItem();
            mnuCotizacion = new ToolStripMenuItem();
            mnuVenta = new ToolStripMenuItem();
            mnuSalirMenu = new ToolStripMenuItem();
            mnuCerrarSesion = new ToolStripMenuItem();
            mnuSalir = new ToolStripMenuItem();
            barraEstado = new StatusStrip();
            lblUsuario = new ToolStripStatusLabel();
            lblRol = new ToolStripStatusLabel();
            lblEspacio = new ToolStripStatusLabel();
            lblBaseDatos = new ToolStripStatusLabel();
            lblAcceso = new ToolStripStatusLabel();
            menuPrincipal.SuspendLayout();
            barraEstado.SuspendLayout();
            SuspendLayout();
            // 
            // menuPrincipal
            // 
            menuPrincipal.BackColor = Color.FromArgb(46, 125, 50);
            menuPrincipal.Font = new Font("Segoe UI", 10F);
            menuPrincipal.Items.AddRange(new ToolStripItem[] { mnuConsulta, mnuActualizacion, mnuNovedades, mnuSalirMenu });
            menuPrincipal.Location = new Point(0, 0);
            menuPrincipal.Name = "menuPrincipal";
            menuPrincipal.Padding = new Padding(8, 4, 0, 4);
            menuPrincipal.Size = new Size(1000, 31);
            menuPrincipal.TabIndex = 0;
            // 
            // mnuConsulta
            // 
            mnuConsulta.DropDownItems.AddRange(new ToolStripItem[] { mnuConsultar });
            mnuConsulta.ForeColor = Color.White;
            mnuConsulta.Name = "mnuConsulta";
            mnuConsulta.Size = new Size(77, 23);
            mnuConsulta.Text = "&Consulta";
            // 
            // mnuConsultar
            // 
            mnuConsultar.Name = "mnuConsultar";
            mnuConsultar.ShortcutKeys = Keys.F2;
            mnuConsultar.Size = new Size(300, 24);
            mnuConsultar.Text = "Consultar mascota (por código o nombre)";
            mnuConsultar.Click += mnuConsultar_Click;
            // 
            // mnuActualizacion
            // 
            mnuActualizacion.DropDownItems.AddRange(new ToolStripItem[] { mnuIngresar, mnuModificar, mnuRetirar });
            mnuActualizacion.ForeColor = Color.White;
            mnuActualizacion.Name = "mnuActualizacion";
            mnuActualizacion.Size = new Size(104, 23);
            mnuActualizacion.Text = "&Actualización";
            // 
            // mnuIngresar
            // 
            mnuIngresar.Name = "mnuIngresar";
            mnuIngresar.Size = new Size(240, 24);
            mnuIngresar.Text = "Ingresar mascota";
            mnuIngresar.Click += mnuIngresar_Click;
            // 
            // mnuModificar
            // 
            mnuModificar.Name = "mnuModificar";
            mnuModificar.Size = new Size(240, 24);
            mnuModificar.Text = "Cambiar datos de mascota";
            mnuModificar.Click += mnuModificar_Click;
            // 
            // mnuRetirar
            // 
            mnuRetirar.Name = "mnuRetirar";
            mnuRetirar.Size = new Size(240, 24);
            mnuRetirar.Text = "Retirar mascota";
            mnuRetirar.Click += mnuRetirar_Click;
            // 
            // mnuNovedades
            // 
            mnuNovedades.DropDownItems.AddRange(new ToolStripItem[] { mnuCotizacion, mnuVenta });
            mnuNovedades.ForeColor = Color.White;
            mnuNovedades.Name = "mnuNovedades";
            mnuNovedades.Size = new Size(87, 23);
            mnuNovedades.Text = "&Novedades";
            // 
            // mnuCotizacion
            // 
            mnuCotizacion.Name = "mnuCotizacion";
            mnuCotizacion.Size = new Size(200, 24);
            mnuCotizacion.Text = "Realizar cotización";
            mnuCotizacion.Click += mnuCotizacion_Click;
            // 
            // mnuVenta
            // 
            mnuVenta.Name = "mnuVenta";
            mnuVenta.Size = new Size(200, 24);
            mnuVenta.Text = "Venta de mascota";
            mnuVenta.Click += mnuVenta_Click;
            // 
            // mnuSalirMenu
            // 
            mnuSalirMenu.DropDownItems.AddRange(new ToolStripItem[] { mnuCerrarSesion, mnuSalir });
            mnuSalirMenu.ForeColor = Color.White;
            mnuSalirMenu.Name = "mnuSalirMenu";
            mnuSalirMenu.Size = new Size(48, 23);
            mnuSalirMenu.Text = "&Salir";
            // 
            // mnuCerrarSesion
            // 
            mnuCerrarSesion.Name = "mnuCerrarSesion";
            mnuCerrarSesion.Size = new Size(180, 24);
            mnuCerrarSesion.Text = "Cerrar sesión";
            mnuCerrarSesion.Click += mnuCerrarSesion_Click;
            // 
            // mnuSalir
            // 
            mnuSalir.Name = "mnuSalir";
            mnuSalir.ShortcutKeys = Keys.Alt | Keys.F4;
            mnuSalir.Size = new Size(180, 24);
            mnuSalir.Text = "Salir";
            mnuSalir.Click += mnuSalir_Click;
            // 
            // barraEstado
            // 
            barraEstado.Items.AddRange(new ToolStripItem[] { lblUsuario, lblRol, lblEspacio, lblAcceso, lblBaseDatos });
            barraEstado.Location = new Point(0, 628);
            barraEstado.Name = "barraEstado";
            barraEstado.Size = new Size(1000, 22);
            barraEstado.TabIndex = 1;
            // 
            // lblUsuario
            // 
            lblUsuario.Name = "lblUsuario";
            lblUsuario.Size = new Size(50, 17);
            lblUsuario.Text = "Usuario";
            // 
            // lblRol
            // 
            lblRol.Name = "lblRol";
            lblRol.Size = new Size(26, 17);
            lblRol.Text = "Rol";
            // 
            // lblEspacio
            // 
            lblEspacio.Name = "lblEspacio";
            lblEspacio.Size = new Size(800, 17);
            lblEspacio.Spring = true;
            // 
            // lblBaseDatos
            // 
            lblBaseDatos.Name = "lblBaseDatos";
            lblBaseDatos.Size = new Size(100, 17);
            lblBaseDatos.Text = "Base de datos";
            // 
            // lblAcceso
            // 
            lblAcceso.Name = "lblAcceso";
            lblAcceso.Size = new Size(60, 17);
            lblAcceso.Text = "Acceso";
            // 
            // Menu
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1000, 650);
            Controls.Add(barraEstado);
            Controls.Add(menuPrincipal);
            IsMdiContainer = true;
            MainMenuStrip = menuPrincipal;
            MinimumSize = new Size(800, 500);
            Name = "Menu";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "El Tierno Tiranosaurio";
            WindowState = FormWindowState.Maximized;
            FormClosing += Menu_FormClosing;
            Load += Menu_Load;
            menuPrincipal.ResumeLayout(false);
            menuPrincipal.PerformLayout();
            barraEstado.ResumeLayout(false);
            barraEstado.PerformLayout();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private MenuStrip menuPrincipal;
        private ToolStripMenuItem mnuConsulta;
        private ToolStripMenuItem mnuConsultar;
        private ToolStripMenuItem mnuActualizacion;
        private ToolStripMenuItem mnuIngresar;
        private ToolStripMenuItem mnuModificar;
        private ToolStripMenuItem mnuRetirar;
        private ToolStripMenuItem mnuNovedades;
        private ToolStripMenuItem mnuCotizacion;
        private ToolStripMenuItem mnuVenta;
        private ToolStripMenuItem mnuSalirMenu;
        private ToolStripMenuItem mnuCerrarSesion;
        private ToolStripMenuItem mnuSalir;
        private StatusStrip barraEstado;
        private ToolStripStatusLabel lblUsuario;
        private ToolStripStatusLabel lblRol;
        private ToolStripStatusLabel lblEspacio;
        private ToolStripStatusLabel lblBaseDatos;
        private ToolStripStatusLabel lblAcceso;
    }
}
