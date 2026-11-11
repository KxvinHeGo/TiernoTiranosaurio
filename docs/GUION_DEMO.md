# 🎬 Guion de la Demo — El Tierno Tiranosaurio
**Semana de Ingeniería**  
**Integrantes:** Kevin (API + BD + Login/MDI), Arturo (Escritorio CRUD + Novedades), Juan (Aplicación Web + Identidad Visual)

---

## 🎨 1. Identidad Visual Oficial (Guía para Escritorio y Web)

| Elemento | Uso | Hex (Web) | RGB (Windows Forms) |
|---|---|---|---|
| **Verde Dino (Primario)** | Botones principales, acentos activos | `#2A9D8F` | `42, 157, 143` |
| **Azul Profundo (Secundario)** | Cabeceras, menú MDI, encabezados de tabla | `#264653` | `38, 70, 83` |
| **Ámbar Cálido (Destacado)** | Subtítulos, tarjetas destacadas | `#E9C46A` | `233, 196, 106` |
| **Coral Suave (Alerta)** | Estado Vendida / Botón Retirar | `#E76F51` | `231, 111, 81` |
| **Fondo Claro** | Fondo general de formularios y páginas | `#F4F7F6` | `244, 247, 246` |

- **Tipografía:** `Poppins` / `Segoe UI`.

---

## 👥 2. Credenciales y Datos Clave para la Demo

### Usuarios de Acceso (API `POST /api/auth/login`)
| Usuario | Clave | Rol | Permisos en Menú MDI |
|---|---|---|---|
| `vendedor` | `1234` | **Vendedor** | Consulta, Cotización y Venta *(Actualización bloqueada)* |
| `admin` | `1234` | **Administrador** | Acceso total (Consulta, Ingresar, Modificar, Retirar, Cotización, Venta) |

### Mascotas en Base de Datos (Códigos `101` a `120`)
- **Tipos (`1` a `6`):** `1` Perro, `2` Gato, `3` Ave, `4` Pez, `5` Roedor, `6` Reptil.
- **Mascotas ya vendidas inicialmente (5):** Códigos `103`, `106`, `111`, `115`, `118`.
- **Mascota estrella para vender en vivo durante la demo:** Código **`119`** (Reptil disponible) o **`101`** (`Rex`, Labrador).

---

## ⏱️ 3. Paso a Paso Cronometrado de la Presentación (5 Minutos)

### Minuto 1 — Arquitectura y Control de Acceso vía API (Kevin)
1. Mostrar brevemente Swagger (`https://localhost:7100/swagger` o URL de Azure) y el modelo relacional en SQL Server (`Usuarios`, `TipoMascota`, `Cliente`, `Mascota` y la vista `vw_MascotasDetalle`).
2. Abrir la **App de Escritorio** e iniciar sesión primero como **Vendedor** (`vendedor` / `1234`).
3. Explicar que el Login viaja al API REST y devuelve el rol `Vendedor`, por lo que el menú MDI deshabilita las opciones de *Actualización (Ingresar, Modificar, Retirar)*.

### Minuto 2 — Cotización vs. Estado en la Web (Arturo + Juan)
1. Con el usuario `vendedor`, abrir **Novedades → Cotización** y cotizar la mascota código **`101`** (o **`119`**).
2. Cambiar a la **Aplicación Web (`TiernoTiranosaurio.Web`)** en la página **Consulta** y buscar el código **`101`**.
3. **Punto clave ante el jurado:** Demostrar que la mascota sigue en estado **`Disponible`**, comprobando que *las cotizaciones no afectan la base de datos*.

### Minuto 3 — Rol Administrador y Actualización CRUD (Kevin + Arturo)
1. En la App de Escritorio, cerrar sesión e ingresar como **Administrador** (`admin` / `1234`).
2. Mostrar que ahora el menú **Actualización** está habilitado.
3. Realizar una consulta rápida, ingresar una nueva mascota (código `121` autogenerado) o modificar el precio de una existente.

### Minuto 4 — Venta Real en Escritorio (Arturo)
1. Abrir **Novedades → Venta**.
2. Seleccionar una mascota `Disponible` (ej. código **`119`** o **`101`**) y asignarle un cliente comprador.
3. Confirmar la venta (ejecuta el `UPDATE` cambiando `Estado = 'Vendida'` y asignando `IdCliente`).

### Minuto 5 — Demostración Web en Tiempo Real (Juan)
1. En la **Aplicación Web**:
   - **Inicio (`/Index`):** Mostrar cómo el contador de *Disponibles* bajó y *Vendidas* subió automáticamente.
   - **Consulta (`/Consulta`):** Buscar por código (`101`) y por nombre (`re`) mostrando los resultados desde la API.
   - **Maestro-Detalle (`/MaestroDetalle`):** Hacer clic entre los 6 tipos de mascota (*Perro, Gato, Ave, Pez, Roedor, Reptil*) y ver cómo se filtra el detalle al instante.
   - **Vista SQL (`/Vista`):** Hacer clic en **🔄 Actualizar en vivo** y filtrar por **Vendidas** para mostrar que la mascota recién vendida en escritorio ya aparece con el **nombre completo del cliente comprador** gracias a la vista `vw_MascotasDetalle`.
