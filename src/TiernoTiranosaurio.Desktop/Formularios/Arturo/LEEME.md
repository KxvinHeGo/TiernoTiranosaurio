# Formularios de escritorio — Arturo (Integrante 2)

| Formulario | Qué hace | Rol |
|---|---|---|
| `FrmConsulta` | Consulta por código y por nombre (DataGridView) | Administrador y Vendedor |
| `FrmIngresarMascota` | ComboBox de TipoMascota + `INSERT` | Solo Administrador |
| `FrmModificarMascota` | Buscar, editar y `UPDATE` | Solo Administrador |
| `FrmRetirarMascota` | Confirmación + `DELETE` (solo mascotas Disponibles) | Solo Administrador |
| `FrmCotizacion` | Calcula subtotal, descuento, IVA 19 % y total en pantalla, sin BD | Administrador y Vendedor |
| `FrmVenta` | Cliente + mascota → `UPDATE` a `Estado = 'Vendida'` e `IdCliente` | Administrador y Vendedor |

Todo el SQL usa `SqlParameter`. `Retirar` es `DELETE` porque el contrato dice que `Estado` solo es `'Disponible'` o `'Vendida'`.

## Conexión

`AccesoDatos.cs` arma la conexión con la cadena del README **mientras llega `DataBase.cs`**.
Cuando Kevin lo suba, solo se cambia el método `Conexion()` de `AccesoDatos.cs` para que use `DataBase`.

## Probar un formulario suelto

En `Program.cs`, temporalmente:

```csharp
Application.Run(new FrmVenta());
```

y **devolverlo al Login antes del PR**.

## Enlazar al Menu.cs (Kevin)

```csharp
private void Abrir(Form hijo)
{
    hijo.MdiParent = this;
    hijo.Show();
}

// Ejemplos:
// btnConsulta.Click   += (s, e) => Abrir(new FrmConsulta());
// btnIngresar.Click   += (s, e) => Abrir(new FrmIngresarMascota());
// btnModificar.Click  += (s, e) => Abrir(new FrmModificarMascota());
// btnRetirar.Click    += (s, e) => Abrir(new FrmRetirarMascota());
// btnCotizacion.Click += (s, e) => Abrir(new FrmCotizacion());
// btnVenta.Click      += (s, e) => Abrir(new FrmVenta());
```
