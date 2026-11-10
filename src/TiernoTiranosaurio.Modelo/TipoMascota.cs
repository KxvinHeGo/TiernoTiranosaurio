namespace TiernoTiranosaurio.Modelo;

/// <summary>Tabla TipoMascota (Maestro del Maestro-Detalle).</summary>
public class TipoMascota
{
    public int IdTipo { get; set; }              // PK
    public string Nombre { get; set; } = "";
}
