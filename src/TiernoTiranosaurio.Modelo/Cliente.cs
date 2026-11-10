namespace TiernoTiranosaurio.Modelo;

/// <summary>Tabla Cliente.</summary>
public class Cliente
{
    public int IdCliente { get; set; }           // PK
    public string Documento { get; set; } = "";
    public string Nombre { get; set; } = "";
    public string Telefono { get; set; } = "";
    public string Correo { get; set; } = "";
}
