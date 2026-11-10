using TiernoTiranosaurio.Modelo;

namespace TiernoTiranosaurio.Api.Datos;

// ⚠️ DATOS FALSOS TEMPORALES
// Existen solo para que Juan (Web) y Arturo puedan probar YA, sin esperar la base de datos.
// TODO (Kevin): cuando el script esté listo, reemplazar cada uso por consultas SQL reales.
public static class DatosTemporales
{
    public static readonly List<TipoMascota> Tipos =
    [
        new() { IdTipo = 1, Nombre = "Perro" },
        new() { IdTipo = 2, Nombre = "Gato" },
        new() { IdTipo = 3, Nombre = "Ave" },
    ];

    public static readonly List<Cliente> Clientes =
    [
        new() { IdCliente = 1, Documento = "1001", Nombre = "Laura Gómez", Telefono = "3001112233", Correo = "laura@correo.com" },
    ];

    public static readonly List<Mascota> Mascotas =
    [
        new() { Codigo = 101, Nombre = "Rex",    IdTipo = 1, Raza = "Labrador",  Edad = 6, Precio = 850000m, Estado = EstadosMascota.Disponible },
        new() { Codigo = 102, Nombre = "Luna",   IdTipo = 2, Raza = "Siamés",    Edad = 4, Precio = 400000m, Estado = EstadosMascota.Vendida, IdCliente = 1 },
        new() { Codigo = 103, Nombre = "Piolín", IdTipo = 3, Raza = "Canario",   Edad = 3, Precio = 120000m, Estado = EstadosMascota.Disponible },
        new() { Codigo = 104, Nombre = "Toby",   IdTipo = 1, Raza = "Beagle",    Edad = 8, Precio = 700000m, Estado = EstadosMascota.Disponible },
    ];
}
