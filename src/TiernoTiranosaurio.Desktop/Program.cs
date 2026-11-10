namespace TiernoTiranosaurio.Desktop;

// App de escritorio (MDI) de El Tierno Tiranosaurio
// - Kevin (Integrante 1): DataBase.cs, Configuracion.cs, Sesion.cs, Login.cs, Menu.cs (MDI)
// - Arturo (Integrante 2): formularios hijos en la carpeta Formularios/
//
// Flujo: Login → Menú MDI → (Cerrar sesión vuelve al Login | Salir cierra la app)
//
// IMPORTANTE: para probar un formulario suelto, cambien temporalmente el contenido de Main
// y DEVUÉLVANLO como está antes de hacer el Pull Request.
internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        while (true)
        {
            using (Login login = new Login())
            {
                if (login.ShowDialog() != DialogResult.OK)
                    return;   // el usuario dio "Salir" en el Login
            }

            using Menu menu = new Menu();
            Application.Run(menu);

            if (!menu.CerrarSesionSolicitada)
                return;       // salió de la aplicación

            Sesion.Cerrar();  // vuelve a mostrar el Login
        }
    }
}
