using System.Data;
using System.Net;
using System.Net.Http.Json;
using Microsoft.Data.SqlClient;
using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Desktop;

/// <summary>Inicio de sesión: prueba las APIs en orden y, si ninguna responde, valida directo en la BD.</summary>
public partial class Login : Form
{
    // Sin límite global: cada intento tiene su propio tiempo máximo
    private static readonly HttpClient http = new HttpClient { Timeout = Timeout.InfiniteTimeSpan };

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
        List<string> fallos = new();
        try
        {
            // ---------- Planes con API (A, B...) ----------
            string[] urls = Configuracion.UrlsApi;
            for (int i = 0; i < urls.Length; i++)
            {
                string nombre = NombreApi(urls[i]);
                Estado("Plan " + (char)('A' + i) + ": validando con " + nombre + "...");

                ResultadoApi r = await IntentarApi(urls[i], usuario, clave);

                if (r.Respuesta != null)
                {
                    await Entrar(r.Respuesta, nombre);
                    return;
                }
                if (r.ClaveIncorrecta)
                {
                    Rechazar();
                    return;
                }
                fallos.Add(nombre + ": " + r.Error);
            }

            // ---------- Plan de contingencia: BD directa ----------
            Estado("Contingencia: validando directo en la base de datos...");
            (LoginResponse resp, string error) = await Task.Run(() => ValidarEnBaseDatos(usuario, clave));

            if (resp != null)
            {
                await Entrar(resp, "Contingencia (BD directa)");
                return;
            }
            if (error == null)
            {
                Rechazar();
                return;
            }
            fallos.Add("Base de datos: " + error);

            MessageBox.Show(
                "No se pudo validar el acceso por ningún medio.\n\n" + string.Join("\n\n", fallos),
                "Sin conexión", MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        catch (Exception ex)
        {
            MessageBox.Show("Error inesperado:\n" + ex.Message, "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            if (!IsDisposed)
            {
                Ocupado(false);
                Estado("");
            }
        }
    }

    private record ResultadoApi(LoginResponse Respuesta, bool ClaveIncorrecta, string Error);

    /// <summary>Intenta el login en una API con un tiempo máximo.</summary>
    private static async Task<ResultadoApi> IntentarApi(string urlBase, string usuario, string clave)
    {
        // Azure puede tardar en "despertar"; la API local responde rápido o no existe.
        TimeSpan limite = EsLocal(urlBase) ? TimeSpan.FromSeconds(8) : TimeSpan.FromSeconds(40);
        using CancellationTokenSource cts = new CancellationTokenSource(limite);

        try
        {
            Uri url = new Uri(new Uri(urlBase), "api/auth/login");
            using HttpResponseMessage resp = await http.PostAsJsonAsync(url,
                new LoginRequest { Usuario = usuario, Clave = clave }, cts.Token);

            if (resp.StatusCode == HttpStatusCode.Unauthorized)
                return new ResultadoApi(null, true, null);

            if (!resp.IsSuccessStatusCode)
                return new ResultadoApi(null, false, "respondió " + (int)resp.StatusCode);

            LoginResponse datos = await resp.Content.ReadFromJsonAsync<LoginResponse>(cts.Token);
            return new ResultadoApi(datos, false, null);
        }
        catch (OperationCanceledException)
        {
            return new ResultadoApi(null, false, "no respondió en " + limite.TotalSeconds + " s");
        }
        catch (Exception ex)
        {
            return new ResultadoApi(null, false, ex.Message);
        }
    }

    /// <summary>
    /// Contingencia: valida contra la tabla Usuarios sin pasar por la API.
    /// Devuelve (respuesta, null) si es válido, (null, null) si la clave es incorrecta,
    /// o (null, error) si no hay base de datos.
    /// </summary>
    private static (LoginResponse, string) ValidarEnBaseDatos(string usuario, string clave)
    {
        try
        {
            string error = DataBase.ProbarConexion();
            if (error != null) return (null, error);

            // @clave VARCHAR para que el hash coincida con el del script
            DataTable t = DataBase.Consultar(
                @"SELECT NombreUsuario, Rol FROM Usuarios
                  WHERE NombreUsuario = @usuario AND Clave = HASHBYTES('SHA2_256', @clave)",
                new SqlParameter("@usuario", SqlDbType.VarChar, 30) { Value = usuario },
                new SqlParameter("@clave", SqlDbType.VarChar, 100) { Value = clave });

            if (t.Rows.Count == 0) return (null, null);

            return (new LoginResponse
            {
                Usuario = t.Rows[0]["NombreUsuario"].ToString(),
                Rol = t.Rows[0]["Rol"].ToString()
            }, null);
        }
        catch (Exception ex)
        {
            return (null, ex.Message);
        }
    }

    /// <summary>Acceso correcto: deja lista la base de datos y abre el Menú.</summary>
    private async Task Entrar(LoginResponse datos, string origen)
    {
        Estado("Acceso correcto. Conectando a la base de datos...");
        await Task.Run(() => DataBase.ProbarConexion());   // elige BD (Plan A local / Plan B Azure)

        Sesion.Iniciar(datos, origen);
        DialogResult = DialogResult.OK;           // cierra el Login y Program.cs abre el Menú
    }

    private void Rechazar()
    {
        Avisar("Usuario o clave incorrectos.", txtClave);
        txtClave.SelectAll();
    }

    private static bool EsLocal(string url) =>
        url.Contains("localhost", StringComparison.OrdinalIgnoreCase) || url.Contains("127.0.0.1");

    private static string NombreApi(string url) => EsLocal(url) ? "API local" : "API Azure ☁️";

    private void btnSalir_Click(object sender, EventArgs e)
    {
        DialogResult = DialogResult.Cancel;
    }

    private void Estado(string texto)
    {
        lblEstado.Text = texto;
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
