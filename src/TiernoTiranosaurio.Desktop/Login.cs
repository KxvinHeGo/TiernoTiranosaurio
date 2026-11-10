using System.Net;
using System.Net.Http.Json;
using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Desktop;

/// <summary>
/// Inicio de sesión: valida usuario y clave contra la API publicada (POST api/auth/login).
/// Si es correcto guarda el usuario y el rol en Sesion y cierra con DialogResult.OK.
/// Responsable: Kevin (Integrante 1).
/// </summary>
public partial class Login : Form
{
    private static readonly HttpClient http = new HttpClient { Timeout = TimeSpan.FromSeconds(20) };

    public Login()
    {
        InitializeComponent();
    }

    private async void btnIngresar_Click(object sender, EventArgs e)
    {
        string usuario = txtUsuario.Text.Trim();
        string clave = txtClave.Text;

        if (usuario == "") { Avisar("Escribe el usuario.", txtUsuario); return; }
        if (clave == "") { Avisar("Escribe la clave.", txtClave); return; }

        Ocupado(true);
        try
        {
            Uri url = new Uri(new Uri(Configuracion.UrlApi), "api/auth/login");
            using HttpResponseMessage resp = await http.PostAsJsonAsync(url,
                new LoginRequest { Usuario = usuario, Clave = clave });

            if (resp.StatusCode == HttpStatusCode.Unauthorized)
            {
                Avisar("Usuario o clave incorrectos.", txtClave);
                txtClave.SelectAll();
                return;
            }

            resp.EnsureSuccessStatusCode();
            LoginResponse datos = await resp.Content.ReadFromJsonAsync<LoginResponse>();

            Sesion.Iniciar(datos);
            DialogResult = DialogResult.OK;   // cierra el Login y Program.cs abre el Menú
        }
        catch (Exception ex) when (ex is HttpRequestException or TaskCanceledException)
        {
            MessageBox.Show(
                "No se pudo conectar con la API de acceso.\n\n" +
                "Dirección: " + Configuracion.UrlApi + "\n" +
                "Detalle: " + ex.Message + "\n\n" +
                "Verifiquen que la API esté corriendo (o publicada en Azure) y la UrlApi de conexion.json.",
                "Sin conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error inesperado:\n" + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Ocupado(false);
        }
    }

    private void btnSalir_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    private void Ocupado(bool si)
    {
        btnIngresar.Enabled = btnSalir.Enabled = txtUsuario.Enabled = txtClave.Enabled = !si;
        btnIngresar.Text = si ? "Validando..." : "Ingresar";
        UseWaitCursor = si;
    }

    private static void Avisar(string texto, Control enfocar)
    {
        MessageBox.Show(texto, "Inicio de sesión", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        enfocar.Focus();
    }
}
