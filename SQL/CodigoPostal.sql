-- Codigo Postal para Clientes, Proveedores y Empleados
IF COL_LENGTH('dbo.Clientes', 'CodigoPostal') IS NULL
    ALTER TABLE dbo.Clientes ADD CodigoPostal VARCHAR(20) NULL;
IF COL_LENGTH('dbo.Proveedores', 'CodigoPostal') IS NULL
    ALTER TABLE dbo.Proveedores ADD CodigoPostal VARCHAR(20) NULL;
IF COL_LENGTH('dbo.Empleados', 'CodigoPostal') IS NULL
    ALTER TABLE dbo.Empleados ADD CodigoPostal VARCHAR(20) NULL;

-- Dia del mes en que cierra la facturacion del cliente (1-31)
IF COL_LENGTH('dbo.Clientes', 'DiaCierreFacturacion') IS NULL
    ALTER TABLE dbo.Clientes ADD DiaCierreFacturacion TINYINT NULL;
GO
IF NOT EXISTS (SELECT 1 FROM sys.check_constraints WHERE name = 'CK_Clientes_DiaCierreFacturacion')
    ALTER TABLE dbo.Clientes ADD CONSTRAINT CK_Clientes_DiaCierreFacturacion CHECK (DiaCierreFacturacion BETWEEN 1 AND 31);
