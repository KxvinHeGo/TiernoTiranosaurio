namespace TiernoTiranosaurio.Desktop;

// App de escritorio (MDI) de El Tierno Tiranosaurio
// - Kevin (Integrante 1): DataBase.cs, Configuracion.cs, Sesion.cs, Login.cs, Menu.cs (MDI)
// - Arturo (Integrante 2): formularios hijos en la carpeta Formularios/
//
// IMPORTANTE: para probar un formulario suelto, cambien temporalmente el contenido de Main
// y DEVUÉLVANLO como está antes de hacer el Pull Request.
internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        using (Login login = new Login())
        {
            if (login.ShowDialog() != DialogResult.OK)
                return;   // el usuario dio "Salir"
        }

        // TODO (Kevin): reemplazar por  Application.Run(new Menu());  cuando exista Menu.cs
        MessageBox.Show("Bienvenido " + Sesion.Usuario + "\nRol: " + Sesion.Rol, "Sesión iniciada");
    }
}
