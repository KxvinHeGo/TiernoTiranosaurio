namespace TiernoTiranosaurio.Modelo;

public class Mascota
{
    public int Codigo { get; set; }
    public string Nombre { get; set; } = "";
    public int IdTipo { get; set; }
    public string Raza { get; set; } = "";
    public int Edad { get; set; } 
    public decimal Precio { get; set; }
    public string Estado { get; set; } = EstadosMascota.Disponible;
    public int? IdCliente { get; set; }
}

public static class EstadosMascota
{
    public const string Disponible = "Disponible";
    public const string Vendida = "Vendida";
}
