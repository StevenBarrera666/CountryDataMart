-- Autorizacion por opciones: Opcion y Rol_Opcion (Usuario, Rol y usuario_Rol ya existen)
IF OBJECT_ID('dbo.Opcion') IS NULL
CREATE TABLE dbo.Opcion (
    id         INT IDENTITY(1,1) PRIMARY KEY,
    Nombre     VARCHAR(100) NOT NULL,
    Controller VARCHAR(100) NOT NULL,
    Action     VARCHAR(100) NULL,           -- NULL = todo el controller
    Menu       VARCHAR(20)  NOT NULL,       -- PRINCIPAL | ADMIN | TRAZA
    Icono      VARCHAR(50)  NULL,           -- clase bootstrap-icons
    Ajax       BIT NOT NULL DEFAULT 1,      -- carga en #facadeContent
    Orden      INT NOT NULL DEFAULT 0,
    Activo     BIT NOT NULL DEFAULT 1
);

IF OBJECT_ID('dbo.Rol_Opcion') IS NULL
CREATE TABLE dbo.Rol_Opcion (
    id       INT IDENTITY(1,1) PRIMARY KEY,
    RolId    INT NOT NULL,                  -- dbo.Rol.id no tiene PK, sin FK
    OpcionId INT NOT NULL REFERENCES dbo.Opcion(id),
    CONSTRAINT UQ_Rol_Opcion UNIQUE (RolId, OpcionId)
);

IF NOT EXISTS (SELECT 1 FROM dbo.Opcion)
BEGIN
    INSERT INTO dbo.Opcion (Nombre, Controller, Action, Menu, Icono, Ajax, Orden) VALUES
    ('Administrar Expedientes', 'Home',        'AdministradorTraza', 'PRINCIPAL', 'bi-folder2-open',   0, 1),
    ('Administrar Perfiles',    'Home',        'AdministradorP',     'PRINCIPAL', 'bi-person-gear',    0, 2),
    ('Clientes',                'Cliente',     'Clientes',           'ADMIN',     'bi-people',         1, 1),
    ('Empleados',               'Empleado',    'Empleados',          'ADMIN',     'bi-person-badge',   1, 2),
    ('Proveedores',             'Proveedor',   'Proveedores',        'ADMIN',     'bi-truck',          1, 3),
    ('Administrar Usuarios',    'UsersAndRoles', 'Usuarios,GuardarUsuario', 'ADMIN', 'bi-person-lines-fill', 1, 5),
    ('Administrar Roles',       'UsersAndRoles', 'Roles,GuardarRol,GetOpcionesRol,GuardarOpcionesRol', 'ADMIN', 'bi-shield-lock', 1, 6),
    ('Administrar Opciones',    'UsersAndRoles', 'AdministrarOpciones,GuardarOpcion', 'ADMIN', 'bi-list-check', 1, 7),
    ('Traza AIRS',              'Inspections', 'SearchProceduresT',  'TRAZA',     'bi-clock-history',  1, 1),
    ('Documentacion AIR',       'Inspections', 'Documentacion',      'TRAZA',     'bi-file-earmark-text', 1, 2),
    ('Autorizacion',            'Inspections', 'Autorizacion',       'TRAZA',     'bi-shield-check',   1, 3),
    ('Facturacion',             'Inspections', 'Facturacion',        'TRAZA',     'bi-receipt',        1, 4);
END

-- Permiso de solo lectura por rol y opcion
IF COL_LENGTH('dbo.Rol_Opcion', 'SoloLectura') IS NULL
    ALTER TABLE dbo.Rol_Opcion ADD SoloLectura BIT NOT NULL CONSTRAINT DF_Rol_Opcion_SoloLectura DEFAULT 0;
GO

-- Migracion: la opcion unica 'Usuarios y Roles' se divide en tres (Usuarios, Roles, Opciones)
DELETE ro FROM dbo.Rol_Opcion ro JOIN dbo.Opcion o ON o.id = ro.OpcionId
WHERE o.Controller = 'UsersAndRoles' AND o.Action IS NULL;
DELETE FROM dbo.Opcion WHERE Controller = 'UsersAndRoles' AND Action IS NULL;

IF NOT EXISTS (SELECT 1 FROM dbo.Opcion WHERE Nombre = 'Administrar Usuarios')
    INSERT INTO dbo.Opcion (Nombre, Controller, Action, Menu, Icono, Ajax, Orden)
    VALUES ('Administrar Usuarios', 'UsersAndRoles', 'Usuarios,GuardarUsuario', 'ADMIN', 'bi-person-lines-fill', 1, 5);
IF NOT EXISTS (SELECT 1 FROM dbo.Opcion WHERE Nombre = 'Administrar Roles')
    INSERT INTO dbo.Opcion (Nombre, Controller, Action, Menu, Icono, Ajax, Orden)
    VALUES ('Administrar Roles', 'UsersAndRoles', 'Roles,GuardarRol,GetOpcionesRol,GuardarOpcionesRol', 'ADMIN', 'bi-shield-lock', 1, 6);
IF NOT EXISTS (SELECT 1 FROM dbo.Opcion WHERE Nombre = 'Administrar Opciones')
    INSERT INTO dbo.Opcion (Nombre, Controller, Action, Menu, Icono, Ajax, Orden)
    VALUES ('Administrar Opciones', 'UsersAndRoles', 'AdministrarOpciones,GuardarOpcion', 'ADMIN', 'bi-list-check', 1, 7);

-- La opcion 'Opciones' (Inspections/Opciones) se retira del menu Administrador de Perfiles
UPDATE dbo.Opcion SET Activo = 0
WHERE Nombre = 'Opciones' AND Controller = 'Inspections' AND Action = 'Opciones' AND Menu = 'ADMIN';

-- Rol 1 (administrador) con acceso a todo; ajustar segun la tabla Rol
INSERT INTO dbo.Rol_Opcion (RolId, OpcionId)
SELECT 1, o.id FROM dbo.Opcion o
WHERE EXISTS (SELECT 1 FROM dbo.Rol WHERE id = 1)
  AND NOT EXISTS (SELECT 1 FROM dbo.Rol_Opcion ro WHERE ro.RolId = 1 AND ro.OpcionId = o.id);

-- Roles por etapa del expediente: ven todo el menu Administrar Expedientes
-- pero solo editan su propia vista (SoloLectura = 1 en las demas).
-- Solo se insertan los permisos que el rol aun no tenga (no pisa cambios hechos desde la pantalla).
;WITH Perm (Rol, Opcion, SoloLectura) AS (
    SELECT * FROM (VALUES
        ('Documentador', 'Administrar Expedientes', 0),
        ('Documentador', 'Traza AIRS',              0),
        ('Documentador', 'Documentacion AIR',       0),
        ('Documentador', 'Autorizacion',            1),
        ('Documentador', 'Facturacion',             1),
        ('Autorizador',  'Administrar Expedientes', 0),
        ('Autorizador',  'Traza AIRS',              0),
        ('Autorizador',  'Documentacion AIR',       1),
        ('Autorizador',  'Autorizacion',            0),
        ('Autorizador',  'Facturacion',             1),
        ('Facturador',   'Administrar Expedientes', 0),
        ('Facturador',   'Traza AIRS',              0),
        ('Facturador',   'Documentacion AIR',       1),
        ('Facturador',   'Autorizacion',            1),
        ('Facturador',   'Facturacion',             0)
    ) v (Rol, Opcion, SoloLectura)
)
INSERT INTO dbo.Rol_Opcion (RolId, OpcionId, SoloLectura)
SELECT r.id, o.id, p.SoloLectura
FROM Perm p
JOIN dbo.Rol r    ON r.Descripcion = p.Rol
JOIN dbo.Opcion o ON o.Nombre = p.Opcion
WHERE NOT EXISTS (SELECT 1 FROM dbo.Rol_Opcion ro WHERE ro.RolId = r.id AND ro.OpcionId = o.id);
