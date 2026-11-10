# 🦖 El Tierno Tiranosaurio — Tienda de Mascotas

Proyecto de la Semana de Ingeniería. Sistema de gestión para la tienda de mascotas **El Tierno Tiranosaurio**, con:

- **Base de datos** SQL Server (local para desarrollo y Azure SQL para la demo).
- **API REST** publicada en Azure para el control de acceso (login y roles).
- **App de escritorio** Windows Forms (MDI) con conexión manual a la BD.
- **App web** con consulta, maestro-detalle y vista.

Todo en **.NET 8** con **Visual Studio 2022**.

---

## 📁 Estructura del repositorio

```
TiernoTiranosaurio/
├── database/                         ← Integrante 1 (ÚNICO que lo modifica)
├── docs/                             ← Diagramas, capturas, guion de la demo
├── src/
│   ├── TiernoTiranosaurio.Api/       ← Integrante 1
│   ├── TiernoTiranosaurio.Desktop/   ← Integrante 1 (Login, Menu, DataBase) e Integrante 2 (formularios hijos)
│   └── TiernoTiranosaurio.Web/       ← Integrante 3
├── .gitignore
├── README.md
└── TiernoTiranosaurio.sln            ← NO modificar (salvo el Integrante 1)
```

## 👥 Equipo y responsabilidades

| Integrante | Nombre | Rama | Responsabilidad |
|---|---|---|---|
| 1 | _(nombre)_ | `int1-bd-api` | Script de BD, API en Azure, `DataBase.cs`, Login, Menú MDI con roles |
| 2 | _(nombre)_ | `int2-formularios` | Consulta, Ingresar / Modificar / Retirar mascota, Cotización, Venta |
| 3 | _(nombre)_ | `int3-web` | Web (Consulta, Maestro-Detalle, Vista), identidad visual, guion de la demo |

---

## 🤝 Acuerdos de la Fase 0 (contratos)

> Estos nombres son **obligatorios** para todos. Si alguien necesita cambiar algo, se habla primero con el Integrante 1.

### Base de datos: `TiernoTiranosaurio`

| Tabla | Columnas |
|---|---|
| **Usuarios** | `IdUsuario` (PK), `NombreUsuario`, `Clave`, `Rol` |
| **TipoMascota** | `IdTipo` (PK), `Nombre` |
| **Cliente** | `IdCliente` (PK), `Documento`, `Nombre`, `Telefono`, `Correo` |
| **Mascota** | `Codigo` (PK), `Nombre`, `IdTipo` (FK → TipoMascota), `Raza`, `Edad`, `Precio`, `Estado`, `IdCliente` (FK → Cliente, puede ser NULL) |
| **vw_MascotasDetalle** (vista) | JOIN de Mascota + TipoMascota + Cliente |

- `Estado` solo puede ser `'Disponible'` o `'Vendida'`.
- Al vender: `Estado = 'Vendida'` y se asigna `IdCliente`.

### Roles

| Rol | Puede usar |
|---|---|
| **Administrador** | Todo: Consulta, Actualización (ingresar, modificar, retirar), Novedades (cotización, venta) |
| **Vendedor** | Consulta y Novedades (cotización, venta). **No** puede ingresar, modificar ni retirar mascotas |

Usuarios de prueba: `admin` / `1234` (Administrador) y `vendedor` / `1234` (Vendedor).

### API de login

```
POST /api/auth/login
Body:      { "usuario": "admin", "clave": "1234" }
Respuesta: 200 OK            { "usuario": "admin", "rol": "Administrador" }
           401 Unauthorized  (usuario o clave incorrectos)
```

- Local: `https://localhost:7100/api/auth/login` (Swagger en `https://localhost:7100/swagger`)
- Azure: _(el Integrante 1 pone aquí la URL cuando publique)_

### Cadena de conexión (desarrollo local)

```
Server=localhost\SQLEXPRESS;Database=TiernoTiranosaurio;Trusted_Connection=True;TrustServerCertificate=True;
```

Si su SQL Server tiene otro nombre (por ejemplo `localhost` o `(localdb)\MSSQLLocalDB`), cámbienlo **solo en su PC** y no suban ese cambio.

⚠️ **Nunca suban al repo la contraseña de Azure SQL.** Esa cadena se comparte por el chat del grupo y en Azure se configura en el portal (App Service → Configuración → Cadenas de conexión).

---

## 🚀 Cómo empezar (cada integrante)

1. Instalar **Visual Studio 2022** con las cargas de trabajo *Desarrollo de ASP.NET y web* y *Desarrollo de escritorio de .NET*.
2. Instalar **SQL Server Express** y **SQL Server Management Studio (SSMS)**.
3. Clonar el repo (ver `docs/GUIA_GIT.md`).
4. Abrir `TiernoTiranosaurio.sln` en Visual Studio.
5. Cuando el Integrante 1 suba el script, abrirlo en SSMS y ejecutarlo (F5).

Ver la guía de trabajo con Git en **[docs/GUIA_GIT.md](docs/GUIA_GIT.md)**.
