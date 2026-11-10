namespace TiernoTiranosaurio.Modelo;

/// <summary>Tabla Mascota.</summary>
public class Mascota
{
    public int Codigo { get; set; }              // PK
    public string Nombre { get; set; } = "";
    public int IdTipo { get; set; }              // FK -> TipoMascota
    public string Raza { get; set; } = "";
    public int Edad { get; set; }                // en meses
    public decimal Precio { get; set; }
    public string Estado { get; set; } = EstadosMascota.Disponible;
    public int? IdCliente { get; set; }          // FK -> Cliente (null si no se ha vendido)
}

public static class EstadosMascota
{
    public const string Disponible = "Disponible";
    public const string Vendida = "Vendida";
}
