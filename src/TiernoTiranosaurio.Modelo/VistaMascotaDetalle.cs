namespace TiernoTiranosaurio.Modelo;

/// <summary>Una fila de la vista SQL vw_MascotasDetalle (Mascota + TipoMascota + Cliente).</summary>
public class VistaMascotaDetalle
{
    public int Codigo { get; set; }
    public string NombreMascota { get; set; } = "";
    public string Tipo { get; set; } = "";
    public string Raza { get; set; } = "";
    public int Edad { get; set; }
    public decimal Precio { get; set; }
    public string Estado { get; set; } = "";
    public string? NombreCliente { get; set; }   // null si la mascota está disponible
}
