/* =============================================================
   El Tierno Tiranosaurio - Crear la base de datos (SOLO LOCAL)
   -------------------------------------------------------------
   Ejecutar UNA sola vez en su SQL Server local.
   En Azure NO se usa: allá la BD se crea desde el portal.
   Después de esto, ejecutar BD_TiernoTiranosaurio.sql
   ============================================================= */
IF DB_ID('TiernoTiranosaurio') IS NULL
    CREATE DATABASE TiernoTiranosaurio;
GO
PRINT 'Base de datos TiernoTiranosaurio lista. Ahora ejecuten BD_TiernoTiranosaurio.sql';
