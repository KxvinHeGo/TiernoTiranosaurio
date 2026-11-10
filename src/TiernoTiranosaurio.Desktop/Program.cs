namespace TiernoTiranosaurio.Desktop;

// App de escritorio (MDI) de El Tierno Tiranosaurio
// - Integrante 1: DataBase.cs, Login.cs, Menu.cs (MDI), Sesion.cs
// - Integrante 2: formularios hijos (Consulta, Ingresar, Modificar, Retirar, Cotizacion, Venta)
//
// IMPORTANTE: para probar un formulario suelto, cambien temporalmente la línea Application.Run(...)
// y DEVUÉLVANLA a Login antes de hacer el Pull Request.
internal static class Program
{
    [STAThread]
    static void Main()
    {
        ApplicationConfiguration.Initialize();

        // TODO (Integrante 1): reemplazar por  Application.Run(new Login());
        Application.Run(new Form { Text = "El Tierno Tiranosaurio", Width = 800, Height = 500 });
    }
}
