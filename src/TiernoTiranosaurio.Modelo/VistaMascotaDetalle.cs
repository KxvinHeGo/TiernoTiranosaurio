namespace TiernoTiranosaurio.Modelo;
public class VistaMascotaDetalle
{
    public int Codigo { get; set; }
    public string NombreMascota { get; set; } = "";
    public string Tipo { get; set; } = "";
    public string Raza { get; set; } = "";
    public int Edad { get; set; }
    public decimal Precio { get; set; }
    public string Estado { get; set; } = "";
    public string? NombreCliente { get; set; }
}
