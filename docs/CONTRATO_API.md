# 📜 Contrato de la API — El Tierno Tiranosaurio

> **Este documento es la ley.** Kevin construye la API exactamente así, y Arturo y Juan programan contra esto.
> Si algo tiene que cambiar, Kevin lo actualiza aquí **primero**, lo sube a `main` y avisa al grupo.

## ¿Quién usa qué?

| Aplicación | Cómo obtiene los datos |
|---|---|
| **Escritorio** (Kevin, Arturo) | **Login** → API (`POST api/auth/login`). **Todo lo demás** (consulta, CRUD, venta) → **directo a la BD** con `DataBase.cs` (conexión manual, lo exige el enunciado). |
| **Web** (Juan) | **Todo** a través de la API (los 3 `GET` de abajo). La web **no** se conecta a la BD. |

## URLs base

| Entorno | URL |
|---|---|
| Local | `https://localhost:7100` (Swagger: `https://localhost:7100/swagger`) |
| Azure | _(Kevin la pone aquí al publicar)_ |

---

## Modelos (proyecto `TiernoTiranosaurio.Modelo`)

Las clases viven en **un solo proyecto compartido**. La API, la Web y el Escritorio lo referencian, así **nadie escribe sus propias clases** y los nombres siempre coinciden.

> En el JSON los nombres viajan en **camelCase** (`idTipo`, `nombreMascota`). Si usan las clases del proyecto `Modelo` con `HttpClient.GetFromJsonAsync<T>()`, esto se maneja solo.

### `Mascota`
| Propiedad | Tipo C# | Columna SQL | Notas |
|---|---|---|---|
| `Codigo` | `int` | `Codigo` INT PK IDENTITY | se genera solo (desde 101) |
| `Nombre` | `string` | `Nombre` VARCHAR(50) | |
| `IdTipo` | `int` | `IdTipo` INT FK | → `TipoMascota.IdTipo` |
| `Raza` | `string` | `Raza` VARCHAR(50) | |
| `Edad` | `int` | `Edad` INT | en meses |
| `Precio` | `decimal` | `Precio` DECIMAL(12,2) | |
| `Estado` | `string` | `Estado` VARCHAR(15) | `"Disponible"` o `"Vendida"` |
| `IdCliente` | `int?` | `IdCliente` INT FK NULL | `null` si no se ha vendido |

### `TipoMascota`
| Propiedad | Tipo C# | Columna SQL |
|---|---|---|
| `IdTipo` | `int` | `IdTipo` INT PK IDENTITY |
| `Nombre` | `string` | `Nombre` VARCHAR(30) |

### `Cliente`
| Propiedad | Tipo C# | Columna SQL |
|---|---|---|
| `IdCliente` | `int` | `IdCliente` INT PK IDENTITY |
| `Documento` | `string` | `Documento` VARCHAR(20) UNIQUE |
| `Nombre` | `string` | `Nombre` VARCHAR(80) |
| `Telefono` | `string` | `Telefono` VARCHAR(20) |
| `Correo` | `string` | `Correo` VARCHAR(80) |

### `VistaMascotaDetalle` (vista SQL `vw_MascotasDetalle`)
| Propiedad | Tipo C# | Viene de |
|---|---|---|
| `Codigo` | `int` | `Mascota.Codigo` |
| `NombreMascota` | `string` | `Mascota.Nombre` |
| `Tipo` | `string` | `TipoMascota.Nombre` |
| `Raza` | `string` | `Mascota.Raza` |
| `Edad` | `int` | `Mascota.Edad` |
| `Precio` | `decimal` | `Mascota.Precio` |
| `Estado` | `string` | `Mascota.Estado` |
| `NombreCliente` | `string?` | `Cliente.Nombre` (LEFT JOIN → `null` si está disponible) |

### `LoginRequest` / `LoginResponse` / `Roles`
- `LoginRequest { Usuario, Clave }`
- `LoginResponse { Usuario, Rol }`
- `Roles.Administrador = "Administrador"`, `Roles.Vendedor = "Vendedor"`

---

## Endpoints

### 1. Login — `POST api/auth/login`
Lo usa: **Escritorio** (`Login.cs`).

```json
// Petición
{ "usuario": "admin", "clave": "1234" }

// 200 OK
{ "usuario": "admin", "rol": "Administrador" }

// 401 Unauthorized  → usuario o clave incorrectos (sin cuerpo)
```

### 2. Mascotas — `GET api/mascotas`
Lo usa: **Web → Consulta** y **Web → Maestro-Detalle (Detalle)**.

| Parámetro (opcional) | Ejemplo | Filtra |
|---|---|---|
| `nombre` | `api/mascotas?nombre=re` | Nombre **contiene** el texto (`LIKE '%re%'`) |
| `idTipo` | `api/mascotas?idTipo=1` | Solo mascotas de ese tipo |

Sin parámetros devuelve todas. Se pueden combinar: `api/mascotas?idTipo=1&nombre=to`.

```json
// 200 OK
[
  { "codigo": 101, "nombre": "Rex", "idTipo": 1, "raza": "Labrador", "edad": 6,
    "precio": 850000.00, "estado": "Disponible", "idCliente": null }
]
```

### 3. Mascota por código — `GET api/mascotas/{codigo}`
Lo usa: **Web → Consulta por código**.

- `200 OK` → un objeto `Mascota`
- `404 Not Found` → no existe ese código

### 4. Tipos de mascota — `GET api/tiposmascota`
Lo usa: **Web → Maestro-Detalle (Maestro)**.

```json
// 200 OK
[ { "idTipo": 1, "nombre": "Perro" }, { "idTipo": 2, "nombre": "Gato" } ]
```

### 5. Vista — `GET api/vistamascotas`
Lo usa: **Web → Página de la Vista SQL**. Devuelve las filas de `vw_MascotasDetalle`.

```json
// 200 OK
[
  { "codigo": 102, "nombreMascota": "Luna", "tipo": "Gato", "raza": "Siamés", "edad": 4,
    "precio": 400000.00, "estado": "Vendida", "nombreCliente": "Laura Gómez" }
]
```

---

## Códigos de respuesta

| Código | Significa |
|---|---|
| `200` | Todo bien |
| `401` | Login incorrecto |
| `404` | No existe (mascota por código) |
| `500` | Error del servidor (normalmente la BD) → avisarle a Kevin |

## Estado actual

✅ Los 5 endpoints **ya existen con datos falsos** (`Api/Datos/DatosTemporales.cs`), así que **Juan puede empezar hoy**: correr la API en local, abrir Swagger y programar sus páginas.
⏳ Kevin reemplaza los datos falsos por consultas SQL reales cuando el script de BD esté listo. **El formato de las respuestas no cambia**, así que la Web no tiene que tocar nada.
