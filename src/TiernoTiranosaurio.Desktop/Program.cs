namespace TiernoTiranosaurio.Desktop;

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
                    return;
            }

            using Menu menu = new Menu();
            Application.Run(menu);

            if (!menu.CerrarSesionSolicitada)
                return;

            Sesion.Cerrar();
        }
    }
}
