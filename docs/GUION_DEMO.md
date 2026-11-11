# 🎤 Guion de la demo — El Tierno Tiranosaurio

Duración objetivo: **12 a 14 minutos** + preguntas. Cada integrante presenta lo que hizo.

| Bloque | Quién | Tiempo | Qué se muestra |
|---|---|---|---|
| 0. Introducción | Kevin | 1 min | El problema, el equipo y las 3 partes del sistema |
| 1. BD, API y Login | Kevin | 3 min | Script, API en Azure, Login y roles |
| 2. Consulta y CRUD | Arturo | 3 min | Consulta, Ingresar, Cambiar datos, Retirar (como `admin`) |
| 3. Roles en el Menú | Kevin | 1 min | Cerrar sesión → entrar como `vendedor` |
| 4. Cotización y Venta | Arturo | 2 min | Cotización y Venta (como `vendedor`) |
| 5. Web | Juan | 3 min | Consulta, Maestro-Detalle y Vista (se ve la venta recién hecha) |
| 6. Cierre | Juan | 30 s | Resumen y preguntas |

> El orden sigue el flujo real de un usuario: **Login → Menú por roles → Consulta → CRUD → Cotización → Venta → Web.**

---

## ✅ Preparación (30 minutos antes)

- [ ] `git pull` de `main` en el PC de la demo y **Recompilar solución**.
- [ ] Ejecutar `database/BD_TiernoTiranosaurio.sql` para tener los datos limpios: 20 mascotas (101 a 120), 5 vendidas (103, 106, 111, 115, 118).
- [ ] `conexion.json` del Desktop y `appsettings.Local.json` de la API con el servidor correcto.
- [ ] Proyectos de inicio: **Api + Desktop** → F5. Verificar que el Login entra con `admin` / `1234` y que abajo en el Menú sale 🟢 BD.
- [ ] La Web abierta en otra pestaña del navegador.
- [ ] Hacer la demo completa una vez y **volver a correr el script** para dejar los datos limpios.

### Datos que se usan en la demo

| Uso | Dato |
|---|---|
| Usuarios | `admin` / `1234` (Administrador) · `vendedor` / `1234` (Vendedor) |
| Consulta por código | **101** (Rex, Labrador) |
| Consulta por nombre | **"to"** (encuentra Toby) |
| Mascota nueva | **Coco** · Ave · Periquito · 5 meses · $90.000 → recibe el código **121** |
| Cambiar datos | **121**: precio de $90.000 a $95.000 |
| Retirar (bloqueado) | **103** (ya está vendida, no se deja retirar) |
| Retirar (permitido) | **121** (Coco) |
| Cotización | Rex $850.000 + Michi $650.000, 10 % de descuento → **total $1.606.500** |
| Venta | **102 Toby** a **Laura Gómez** |

---

## 0. Introducción — Kevin (1 min)

> "Somos el equipo de El Tierno Tiranosaurio, una tienda de mascotas. Hicimos un sistema con tres partes: una base de datos en SQL Server, una API publicada en Azure para el control de acceso, una aplicación de escritorio para la operación diaria de la tienda y una página web para consultar el catálogo."

## 1. Base de datos, API y Login — Kevin (3 min)

1. Mostrar el script en SSMS: tablas, datos de prueba y la vista `vw_MascotasDetalle`.
2. Mostrar Swagger de la API en Azure y probar `POST /api/auth/login`.
3. Abrir el Desktop → Login con **`admin` / `1234`** → se abre el Menú MDI.
4. Señalar la barra inferior: usuario, rol y 🟢 BD conectada.

**Puntos clave:** las claves se guardan cifradas (SHA-256); el Login consulta la API y la API consulta la BD; los planes de respaldo si Azure no responde.

## 2. Consulta y CRUD — Arturo (3 min)

Sigue la sesión de **admin**.

1. **Consulta → Consultar mascota.** Buscar el código **101**, luego el nombre **"to"**.
   > "Busca por código exacto o por parte del nombre. Todas las consultas usan `SqlParameter`, así lo que escribe el usuario nunca se mezcla con el SQL y se evita la inyección."
2. **Actualización → Ingresar.** Crear a **Coco** (Ave, Periquito, 5 meses, $90.000).
   > "El tipo sale de la tabla TipoMascota. El código no se pide porque lo genera la base de datos; el formulario me muestra el nuevo: 121."
3. **Actualización → Cambiar datos.** Buscar **121** y cambiar el precio a **$95.000**.
   > "Los campos están bloqueados hasta que escojo una mascota, así no se puede guardar sin haber seleccionado una."
4. **Actualización → Retirar.** Buscar **103**: el formulario avisa que ya fue vendida y no deja retirarla. Luego buscar **121** → Retirar → confirmar.
   > "Solo se pueden retirar mascotas disponibles; las vendidas son historial de ventas. Además pide confirmación antes de borrar."

## 3. Roles en el Menú — Kevin (1 min)

1. **Salir → Cerrar sesión** y entrar con **`vendedor` / `1234`**.
2. Mostrar que **Actualización** está bloqueada y que Consulta y Novedades sí funcionan.
   > "El rol viene de la API al iniciar sesión, y el Menú habilita o bloquea las opciones según ese rol."

## 4. Cotización y Venta — Arturo (2 min)

Sigue la sesión de **vendedor**.

1. **Novedades → Cotización.** Agregar Rex ($850.000) y Michi ($650.000), poner **10 %** de descuento.
   > "Calcula subtotal, descuento, IVA del 19 % y total, todo en pantalla y sin tocar la base de datos. Total: $1.606.500."
2. **Novedades → Venta.** Cliente **Laura Gómez**, mascota **102 Toby** → Registrar venta → confirmar.
   > "En la lista solo salen las mascotas disponibles. La venta cambia el estado a Vendida y guarda el cliente. El UPDATE lleva la condición `Estado = 'Disponible'` para que una mascota no se pueda vender dos veces."
3. Mostrar que Toby aparece en **Mascotas vendidas** y ya no sale en la lista para vender.

## 5. Web — Juan (3 min)

1. **Consulta web:** buscar mascotas y verlas en la tabla.
2. **Maestro-Detalle:** escoger un tipo (por ejemplo, Perro) y ver solo sus mascotas.
3. **Vista SQL:** mostrar `vw_MascotasDetalle` y señalar que **Toby ya aparece vendida a Laura Gómez**, la venta que se acaba de hacer en el escritorio.
   > "La web y el escritorio comparten la misma base de datos, así que los cambios se ven en las dos al instante."

## 6. Cierre — Juan (30 s)

> "En resumen: una base de datos con reglas que protegen los datos, una API en Azure para el acceso, una app de escritorio con roles para la operación y una web para consultar. Trabajamos con ramas y Pull Requests en GitHub. ¿Tienen preguntas?"

---

## 🛟 Plan B

| Si pasa esto | Hacemos esto |
|---|---|
| Azure no responde en el Login | El Login usa sus planes de respaldo (API local o BD directa). Lo explica Kevin. |
| Abajo en el Menú sale 🔴 BD | Revisar `conexion.json` y que SQL Server esté encendido; si no, cambiar al PC de respaldo. |
| Ya se usó el código 121 o los datos no cuadran | Correr de nuevo `BD_TiernoTiranosaurio.sql` y reiniciar la app. |
| Falla la Web | Juan muestra la vista directamente en SSMS: `SELECT * FROM vw_MascotasDetalle`. |

## ❓ Preguntas probables y quién responde

| Pregunta | Responde |
|---|---|
| ¿Cómo se guardan las claves? ¿Cómo funciona la API? | Kevin |
| ¿Cómo se controlan los roles? | Kevin |
| ¿Cómo evitan la inyección SQL? | Arturo |
| ¿Por qué Retirar solo deja borrar mascotas disponibles? | Arturo |
| ¿Qué pasa si dos personas venden la misma mascota? | Arturo |
| ¿Qué es la vista y para qué sirve el Maestro-Detalle? | Juan |
| ¿Cómo trabajaron en equipo? | Cualquiera: ramas por integrante y Pull Requests revisados |
