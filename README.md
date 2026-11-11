# El Tierno Tiranosaurio

Sistema para la tienda de mascotas El Tierno Tiranosaurio, desarrollado para la Semana de Ingeniería.

El proyecto tiene cuatro partes que comparten la misma base de datos:

- Una base de datos en SQL Server con las mascotas, sus tipos, los clientes y los usuarios del sistema.
- Una API REST publicada en Azure que se encarga del inicio de sesión y entrega los datos a la web.
- Una aplicación de escritorio (Windows Forms, MDI) para la operación de la tienda: consulta, registro, modificación y retiro de mascotas, cotizaciones y ventas.
- Una aplicación web para consultar el catálogo, con un maestro-detalle por tipo de mascota y una página con la vista SQL.

## Tecnologías

- .NET 8 y C#
- SQL Server y Azure SQL Database
- ASP.NET Core Web API, publicada en Azure App Service
- Windows Forms con ADO.NET (conexión manual con `SqlConnection`)
- ASP.NET Core Razor Pages

## Estructura

```
database/                         Scripts de la base de datos
docs/                             Contrato de la API y guía de Git
src/
  TiernoTiranosaurio.Modelo/      Clases compartidas (Mascota, TipoMascota, Cliente...)
  TiernoTiranosaurio.Api/         API REST
  TiernoTiranosaurio.Desktop/     Aplicación de escritorio
  TiernoTiranosaurio.Web/         Aplicación web
TiernoTiranosaurio.sln
```

## API

Publicada en:
`https://tiernotiranosaurio-api-kevin-feapesdphzg4dfcg.brazilsouth-01.azurewebsites.net/swagger`

| Método | Ruta | Uso |
|---|---|---|
| POST | `/api/auth/login` | Inicio de sesión. Devuelve el usuario y su rol. |
| GET | `/api/mascotas` | Lista de mascotas. Acepta los filtros `nombre` e `idTipo`. |
| GET | `/api/mascotas/{codigo}` | Una mascota por su código. |
| GET | `/api/tiposmascota` | Tipos de mascota. |
| GET | `/api/vistamascotas` | Contenido de la vista `vw_MascotasDetalle`. |

El detalle de cada endpoint está en [docs/CONTRATO_API.md](docs/CONTRATO_API.md).

## Cómo ejecutarlo

Requisitos: Visual Studio 2022 con .NET 8, SQL Server y SQL Server Management Studio.

1. Crear la base de datos local ejecutando en SSMS `database/00_crear_bd_local.sql` y después `database/BD_TiernoTiranosaurio.sql`.
2. En `src/TiernoTiranosaurio.Desktop`, copiar `conexion.ejemplo.json` como `conexion.json` y poner el nombre del servidor SQL.
3. Si se va a usar la API local, en `src/TiernoTiranosaurio.Api` copiar `appsettings.Local.ejemplo.json` como `appsettings.Local.json` y hacer lo mismo.
4. Abrir `TiernoTiranosaurio.sln`, elegir los proyectos de inicio (Desktop, Web y, si se necesita, Api) y ejecutar.

Los archivos `conexion.json` y `appsettings.Local.json` son propios de cada equipo y no se suben al repositorio.

La aplicación de escritorio intenta primero la API de Azure y, si no responde, usa la API local o valida directamente en la base de datos. Con las bases de datos hace lo mismo, en el orden definido en `conexion.json`. La web también tiene una API principal y una de respaldo.

## Equipo

| Integrantes |
|---|
| Kevin Henao Gómez |
| Carlos Arturo Manzano Rochel |
| Juan José Alvarez Rios |