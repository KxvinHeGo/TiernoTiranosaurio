using Microsoft.Data.SqlClient;
using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Api.Datos;

/// <summary>Convierte una fila de SQL en el objeto del proyecto Modelo.</summary>
public static class Lectores
{
    public const string ColumnasMascota = "Codigo, Nombre, IdTipo, Raza, Edad, Precio, Estado, IdCliente";

    public static Mascota Mascota(SqlDataReader dr) => new()
    {
        Codigo    = dr.GetInt32(dr.GetOrdinal("Codigo")),
        Nombre    = dr.GetString(dr.GetOrdinal("Nombre")),
        IdTipo    = dr.GetInt32(dr.GetOrdinal("IdTipo")),
        Raza      = dr.GetString(dr.GetOrdinal("Raza")),
        Edad      = dr.GetInt32(dr.GetOrdinal("Edad")),
        Precio    = dr.GetDecimal(dr.GetOrdinal("Precio")),
        Estado    = dr.GetString(dr.GetOrdinal("Estado")),
        IdCliente = dr.IsDBNull(dr.GetOrdinal("IdCliente")) ? null : dr.GetInt32(dr.GetOrdinal("IdCliente"))
    };

    public static TipoMascota Tipo(SqlDataReader dr) => new()
    {
        IdTipo = dr.GetInt32(dr.GetOrdinal("IdTipo")),
        Nombre = dr.GetString(dr.GetOrdinal("Nombre"))
    };

    public static VistaMascotaDetalle Vista(SqlDataReader dr) => new()
    {
        Codigo        = dr.GetInt32(dr.GetOrdinal("Codigo")),
        NombreMascota = dr.GetString(dr.GetOrdinal("NombreMascota")),
        Tipo          = dr.GetString(dr.GetOrdinal("Tipo")),
        Raza          = dr.GetString(dr.GetOrdinal("Raza")),
        Edad          = dr.GetInt32(dr.GetOrdinal("Edad")),
        Precio        = dr.GetDecimal(dr.GetOrdinal("Precio")),
        Estado        = dr.GetString(dr.GetOrdinal("Estado")),
        NombreCliente = dr.IsDBNull(dr.GetOrdinal("NombreCliente")) ? null : dr.GetString(dr.GetOrdinal("NombreCliente"))
    };
}
