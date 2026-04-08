USE master;
GO

IF DB_ID(N'Bibliotecas Sistema') IS NOT NULL
BEGIN
    ALTER DATABASE [Bibliotecas Sistema] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [Bibliotecas Sistema];
END
GO

CREATE DATABASE [Bibliotecas Sistema];
GO

USE [Bibliotecas Sistema];
GO

-- =========================
-- ROLES
-- =========================
CREATE TABLE Tbl_Roles (
    IdRol INT IDENTITY PRIMARY KEY,
    NombreRol VARCHAR(50) UNIQUE NOT NULL
);

INSERT INTO Tbl_Roles (NombreRol)
VALUES ('ADMIN'), ('USER');
GO

-- =========================
-- USUARIOS (LOGIN)
-- =========================
CREATE TABLE Tbl_Usuarios (
    IdUsuario INT IDENTITY PRIMARY KEY,
    Usuario VARCHAR(50) UNIQUE NOT NULL,
    Clave VARCHAR(100) NOT NULL,
    IdRol INT NOT NULL,
    Activo BIT DEFAULT 1,
    FechaCreacion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IdRol) REFERENCES Tbl_Roles(IdRol)
);
GO

-- =========================
-- PERFILES (PERSONAS)
-- =========================
CREATE TABLE Tbl_Perfiles (
    IdPerfil INT IDENTITY PRIMARY KEY,
    Dni VARCHAR(20),
    PrimerNombre VARCHAR(50),
    SegundoNombre VARCHAR(50),
    PrimerApellido VARCHAR(50),
    SegundoApellido VARCHAR(50),
    NumeroDocumento VARCHAR(20),
    Correo VARCHAR(100),
    Telefono VARCHAR(20),
    Direccion VARCHAR(100),
    FechaNacimiento DATE,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1,
    IdUsuario INT NOT NULL,
    FOREIGN KEY (IdUsuario) REFERENCES Tbl_Usuarios(IdUsuario)
);
GO

-- =========================
-- AUTORES
-- =========================
CREATE TABLE Tbl_Autores (
    IdAutor INT IDENTITY PRIMARY KEY,
    PrimerNombre VARCHAR(50),
    SegundoNombre VARCHAR(50),
    PrimerApellido VARCHAR(50),
    SegundoApellido VARCHAR(50),
    Nacionalidad VARCHAR(50),
    Biografia VARCHAR(MAX),
    FechaCreacion DATETIME DEFAULT GETDATE(),
    Activo BIT DEFAULT 1
);

-- =========================
-- CATEGORIAS
-- =========================
CREATE TABLE Tbl_Categorias (
    IdCategoria INT IDENTITY PRIMARY KEY,
    Nombre VARCHAR(50),
    Descripcion VARCHAR(100),
    FechaCreacion DATETIME DEFAULT GETDATE()
);

-- =========================
-- LIBROS
-- =========================
CREATE TABLE Tbl_Libros (
    IdLibro INT IDENTITY PRIMARY KEY,
    ISBN VARCHAR(20),
    Titulo VARCHAR(100),
    IdAutor INT,
    IdCategoria INT,
    Editorial VARCHAR(50),
    FechaPublicacion DATE,
    Edicion VARCHAR(20),
    StockTotal INT DEFAULT 0,
    FechaRegistro DATETIME DEFAULT GETDATE(),
    Ubicacion VARCHAR(50),
    Activo BIT DEFAULT 1,
    FOREIGN KEY (IdAutor) REFERENCES Tbl_Autores(IdAutor),
    FOREIGN KEY (IdCategoria) REFERENCES Tbl_Categorias(IdCategoria)
);

-- =========================
-- PRESTAMOS
-- =========================
CREATE TABLE Tbl_Prestamos (
    IdPrestamo INT IDENTITY PRIMARY KEY,
    IdLibro INT NOT NULL,
    IdUsuario INT NOT NULL,           -- PERFIL (lector)
    IdUsuarioAtendio INT NOT NULL,    -- USUARIO (empleado)
    FechaPrestamo DATETIME DEFAULT GETDATE(),
    FechaDevolucionEsperada DATE,
    FechaDevolucionReal DATE,
    Estado VARCHAR(20) DEFAULT 'PRESTADO',
    Observaciones VARCHAR(200),
    FOREIGN KEY (IdLibro) REFERENCES Tbl_Libros(IdLibro),
    FOREIGN KEY (IdUsuario) REFERENCES Tbl_Usuarios(IdUsuario),
    FOREIGN KEY (IdUsuarioAtendio) REFERENCES Tbl_Usuarios(IdUsuario)
);

-- =========================
-- MULTAS
-- =========================
CREATE TABLE Tbl_Multas (
    IdMulta INT IDENTITY PRIMARY KEY,
    IdPrestamo INT NOT NULL,
    IdUsuario INT NOT NULL,  -- PERFIL (lector)
    DiaRetraso INT,
    MontoMulta DECIMAL(10,2),
    MontoPagado DECIMAL(10,2),
    FechaGeneracion DATETIME DEFAULT GETDATE(),
    FOREIGN KEY (IdPrestamo) REFERENCES Tbl_Prestamos(IdPrestamo),
    FOREIGN KEY (IdUsuario) REFERENCES Tbl_Perfiles(IdPerfil)
);

-- =========================
-- DATOS INICIALES
-- =========================

-- Usuarios del sistema
INSERT INTO Tbl_Usuarios (Usuario, Clave, IdRol)
VALUES 
('admin', '1234', 1),
('user', '1234', 2),
('001', '1234', 2),
('002', '1234', 2);

-- Perfiles (personas)
INSERT INTO Tbl_Perfiles (Dni, PrimerNombre, PrimerApellido, IdUsuario)
VALUES 
('001', 'Juan', 'Perez', 3),
('002', 'Maria', 'Lopez', 4);

-- Autores
INSERT INTO Tbl_Autores (PrimerNombre, PrimerApellido)
VALUES ('Gabriel', 'Garcia'), ('Mario', 'Vargas');

-- Categorías
INSERT INTO Tbl_Categorias (Nombre, Descripcion)
VALUES ('Novela', 'Narrativa'), ('Historia', 'Histórico');

-- Libros
INSERT INTO Tbl_Libros (Titulo, IdAutor, IdCategoria, StockTotal)
VALUES 
('Cien años de soledad', 1, 1, 5),
('Historia Universal', 2, 2, 3);

GO

-- =========================
-- CONSULTAS DE PRUEBA
-- =========================
SELECT * FROM Tbl_Usuarios;
SELECT * FROM Tbl_Perfiles;
SELECT * FROM Tbl_Roles;
SELECT * FROM Tbl_Libros;