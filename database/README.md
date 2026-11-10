# Base de datos

Responsable: **Kevin (Integrante 1)**, único que modifica estos archivos.

| Archivo | Para qué | Dónde se ejecuta |
|---|---|---|
| `00_crear_bd_local.sql` | Crea la BD vacía `TiernoTiranosaurio` | Solo local, **una vez** |
| `BD_TiernoTiranosaurio.sql` | Tablas + datos de prueba + vista + verificación | Local **y** Azure, cada vez que haya cambios |

## Cómo usarlo (SSMS)

1. Conectarse a su SQL Server local (ej. `localhost\SQLEXPRESS`).
2. Abrir `00_crear_bd_local.sql` → **Ejecutar (F5)**. *(solo la primera vez)*
3. Abrir `BD_TiernoTiranosaurio.sql`, elegir **TiernoTiranosaurio** en el desplegable de bases de datos (arriba a la izquierda, NO `master`) → **Ejecutar (F5)**.
4. En los resultados deben salir: Usuarios 2, TipoMascota 6, Cliente 8, Mascota 20, Vendidas 5, y el login de `admin`.

Cada vez que Kevin avise de un cambio: `git pull` → ejecutar de nuevo el paso 3.

## Ficha rápida para programar

- **Usuarios de prueba:** `admin` / `1234` (Administrador) · `vendedor` / `1234` (Vendedor)
- **Claves cifradas:** para validar el login →
  `WHERE NombreUsuario = @usuario AND Clave = HASHBYTES('SHA2_256', @clave)`
- **Códigos autogenerados:** `Mascota.Codigo` (desde 101), `IdTipo`, `IdCliente`, `IdUsuario` son `IDENTITY`.
  En los `INSERT` **no** se envían. Para saber el código de la mascota recién creada: `SELECT SCOPE_IDENTITY()`.
- **Tipos:** 1 Perro · 2 Gato · 3 Ave · 4 Pez · 5 Roedor · 6 Reptil
- **Mascotas:** 101 a 120. Vendidas: 103, 106, 111, 115, 118.
- **Regla de venta (la BD la exige):** si `Estado = 'Vendida'` debe tener `IdCliente`; si está `'Disponible'`, `IdCliente` debe ser `NULL`.
  → Venta: `UPDATE Mascota SET Estado = 'Vendida', IdCliente = @idCliente WHERE Codigo = @codigo AND Estado = 'Disponible'`
- **Retirar mascota:** las vendidas no se deberían borrar (son historial); validar en el formulario que solo se retiren las `Disponible`.
