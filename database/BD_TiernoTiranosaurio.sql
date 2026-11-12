-- Se crea la base solo local, en Azure se crea desde el portal
IF SERVERPROPERTY('EngineEdition') <> 5 AND DB_ID('TiernoTiranosaurio') IS NULL
    EXEC('CREATE DATABASE TiernoTiranosaurio');
GO

USE TiernoTiranosaurio;
GO

IF DB_NAME() <> 'TiernoTiranosaurio'
BEGIN
    RAISERROR('No se pudo usar la base TiernoTiranosaurio. En Azure, conéctense directamente a esa base.', 16, 1);
    SET NOEXEC ON;
END
GO

DROP VIEW IF EXISTS vw_MascotasDetalle;
DROP TABLE IF EXISTS Mascota;
DROP TABLE IF EXISTS Cliente;
DROP TABLE IF EXISTS TipoMascota;
DROP TABLE IF EXISTS Usuarios;
GO

CREATE TABLE Usuarios (
    IdUsuario INT IDENTITY(1,1) NOT NULL,
    NombreUsuario VARCHAR(30) NOT NULL,
    Clave VARBINARY(32) NOT NULL,
    Rol VARCHAR(20) NOT NULL,
    CONSTRAINT PK_Usuarios PRIMARY KEY (IdUsuario),
    CONSTRAINT UQ_Usuarios_Nombre UNIQUE (NombreUsuario),
    CONSTRAINT CK_Usuarios_Rol CHECK (Rol IN ('Administrador', 'Vendedor'))
);

CREATE TABLE TipoMascota (
    IdTipo INT IDENTITY(1,1) NOT NULL,
    Nombre VARCHAR(30) NOT NULL,
    CONSTRAINT PK_TipoMascota PRIMARY KEY (IdTipo),
    CONSTRAINT UQ_TipoMascota_Nombre UNIQUE (Nombre)
);

CREATE TABLE Cliente (
    IdCliente INT IDENTITY(1,1) NOT NULL,
    Documento VARCHAR(20) NOT NULL,
    Nombre VARCHAR(80) NOT NULL,
    Telefono VARCHAR(20) NOT NULL,
    Correo VARCHAR(80) NOT NULL,
    CONSTRAINT PK_Cliente PRIMARY KEY (IdCliente),
    CONSTRAINT UQ_Cliente_Documento UNIQUE (Documento)
);

-- El codigo de la mascota se genera solo (empieza en 101)
CREATE TABLE Mascota (
    Codigo INT IDENTITY(101,1) NOT NULL,
    Nombre VARCHAR(50) NOT NULL,
    IdTipo INT NOT NULL,
    Raza VARCHAR(50) NOT NULL,
    Edad INT NOT NULL,
    Precio DECIMAL(12,2) NOT NULL,
    Estado VARCHAR(15) NOT NULL CONSTRAINT DF_Mascota_Estado DEFAULT 'Disponible',
    IdCliente INT NULL,
    CONSTRAINT PK_Mascota PRIMARY KEY (Codigo),
    CONSTRAINT FK_Mascota_Tipo FOREIGN KEY (IdTipo) REFERENCES TipoMascota (IdTipo),
    CONSTRAINT FK_Mascota_Cliente FOREIGN KEY (IdCliente) REFERENCES Cliente (IdCliente),
    CONSTRAINT CK_Mascota_Edad CHECK (Edad >= 0),
    CONSTRAINT CK_Mascota_Precio CHECK (Precio >= 0),
    CONSTRAINT CK_Mascota_Estado CHECK (Estado IN ('Disponible', 'Vendida')),
    CONSTRAINT CK_Mascota_Venta CHECK (
        (Estado = 'Disponible' AND IdCliente IS NULL) OR
        (Estado = 'Vendida' AND IdCliente IS NOT NULL)
    )
);
GO

INSERT INTO Usuarios (NombreUsuario, Clave, Rol) VALUES
('admin', HASHBYTES('SHA2_256', '1234'), 'Administrador'),
('vendedor', HASHBYTES('SHA2_256', '1234'), 'Vendedor');

INSERT INTO TipoMascota (Nombre) VALUES
('Perro'), ('Gato'), ('Ave'), ('Pez'), ('Roedor'), ('Reptil');

INSERT INTO Cliente (Documento, Nombre, Telefono, Correo) VALUES
('1001234567', 'Laura Gómez', '3001112233', 'laura.gomez@correo.com'),
('1002345678', 'Andrés Restrepo', '3012223344', 'andres.restrepo@correo.com'),
('1003456789', 'Valentina Ríos', '3023334455', 'valentina.rios@correo.com'),
('1004567890', 'Santiago Herrera', '3034445566', 'santiago.herrera@correo.com'),
('1005678901', 'Camila Ospina', '3045556677', 'camila.ospina@correo.com'),
('1006789012', 'Mateo Vargas', '3056667788', 'mateo.vargas@correo.com'),
('1007890123', 'Isabella Castro', '3067778899', 'isabella.castro@correo.com'),
('1008901234', 'Sebastián López', '3078889900', 'sebastian.lopez@correo.com');

INSERT INTO Mascota (Nombre, IdTipo, Raza, Edad, Precio, Estado, IdCliente) VALUES
('Rex', 1, 'Labrador', 6, 850000, 'Disponible', NULL),
('Toby', 1, 'Beagle', 8, 700000, 'Disponible', NULL),
('Max', 1, 'Golden Retriever', 4, 950000, 'Vendida', 2),
('Lola', 1, 'French Poodle', 12, 600000, 'Disponible', NULL),
('Thor', 1, 'Pastor Alemán', 5, 1100000, 'Disponible', NULL),
('Luna', 2, 'Siamés', 4, 400000, 'Vendida', 1),
('Michi', 2, 'Persa', 6, 650000, 'Disponible', NULL),
('Simba', 2, 'Angora', 3, 500000, 'Disponible', NULL),
('Nala', 2, 'Criollo', 10, 150000, 'Disponible', NULL),
('Piolín', 3, 'Canario', 3, 120000, 'Disponible', NULL),
('Kiwi', 3, 'Periquito', 5, 80000, 'Vendida', 3),
('Lorenzo', 3, 'Loro Amazonas', 24, 900000, 'Disponible', NULL),
('Nemo', 4, 'Pez Payaso', 2, 45000, 'Disponible', NULL),
('Burbuja', 4, 'Goldfish', 3, 20000, 'Disponible', NULL),
('Dory', 4, 'Cirujano Azul', 4, 60000, 'Vendida', 5),
('Copito', 5, 'Hámster Sirio', 2, 35000, 'Disponible', NULL),
('Pelusa', 5, 'Conejo Cabeza León', 5, 120000, 'Disponible', NULL),
('Chispa', 5, 'Cobayo', 3, 50000, 'Vendida', 4),
('Rango', 6, 'Iguana Verde', 18, 250000, 'Disponible', NULL),
('Spike', 6, 'Dragón Barbudo', 12, 480000, 'Disponible', NULL);
GO

-- Vista para la Web a través (GET api/vistamascotas)
CREATE VIEW vw_MascotasDetalle
AS
SELECT
    m.Codigo,
    m.Nombre AS NombreMascota,
    t.Nombre AS Tipo,
    m.Raza,
    m.Edad,
    m.Precio,
    m.Estado,
    c.Nombre AS NombreCliente
FROM Mascota m
INNER JOIN TipoMascota t ON t.IdTipo = m.IdTipo
LEFT JOIN Cliente c ON c.IdCliente = m.IdCliente;
GO

-- Acá es para verificar si nos funciona
SELECT 'Usuarios' AS Tabla, COUNT(*) AS Registros FROM Usuarios UNION ALL
SELECT 'TipoMascota', COUNT(*) FROM TipoMascota UNION ALL
SELECT 'Cliente', COUNT(*) FROM Cliente UNION ALL
SELECT 'Mascota', COUNT(*) FROM Mascota UNION ALL
SELECT 'Vendidas (vista)', COUNT(*) FROM vw_MascotasDetalle WHERE Estado = 'Vendida';

SELECT NombreUsuario, Rol
FROM Usuarios
WHERE NombreUsuario = 'admin' AND Clave = HASHBYTES('SHA2_256', '1234');

SELECT * FROM vw_MascotasDetalle ORDER BY Codigo;
GO

SET NOEXEC OFF;
GO
