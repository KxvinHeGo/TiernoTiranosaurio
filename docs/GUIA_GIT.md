# Guía de trabajo con Git y GitHub

## Reglas del equipo

1. **Nadie trabaja directo en `main`.** Cada uno trabaja en su rama.
2. Todo entra a `main` por **Pull Request (PR)**, y otro integrante lo revisa antes de aprobarlo.
3. **Antes de empezar a trabajar**, siempre traer lo último de `main`.
4. Cada uno toca **solo sus archivos** (ver tabla del README).
5. El `.sln` y la carpeta `database/` solo los modifica Kevin.
6. Mensajes de commit claros: `Agrega formulario de consulta por código`, no `cambios`.

## Primera vez (cada integrante)

```bash
git clone https://github.com/KxvinHeGo/TiernoTiranosaurio.git
cd TiernoTiranosaurio
git checkout arturo      # cada uno usa SU rama: kevin, arturo o juan
```

| Integrante | Rama |
|---|---|
| 1 — Kevin | `kevin` |
| 2 — Arturo | `arturo` |
| 3 — Juan | `juan` |

Las ramas ya existen en GitHub: no hay que crearlas, solo hacer `checkout`.

## Día a día

```bash
# 1. Al empezar: traer lo último de main a mi rama
git checkout main
git pull origin main
git checkout arturo        # su rama
git merge main

# 2. Trabajar... y guardar avances
git add .
git commit -m "Agrega formulario de consulta por nombre"
git push
```

## Cuando terminen una parte: Pull Request

1. Hacer `push` de su rama.
2. En GitHub → **Pull requests** → **New pull request**.
3. *base:* `main` ← *compare:* su rama.
4. Llenar la plantilla, asignar a un compañero como revisor y crear el PR.
5. El revisor lo prueba y lo aprueba → **Merge pull request**.
6. Avisar en el grupo: "ya está en main, hagan pull".

## Si aparece un conflicto

Git marca el archivo así:

```
<<<<<<< HEAD
lo que tenía yo
=======
lo que venía de main
>>>>>>> main
```

Dejen la versión correcta, borren las marcas `<<<<<<<`, `=======`, `>>>>>>>`, y luego:

```bash
git add .
git commit -m "Resuelve conflicto con main"
git push
```

Si el conflicto es en un archivo que no es suyo, **pregunten al dueño** antes de resolverlo.

## Alternativa sin consola

Pueden usar **GitHub Desktop** o la ventana **Cambios de Git** de Visual Studio: los pasos son los mismos (cambiar de rama, pull, commit, push).
