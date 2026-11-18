-- Clases aereas (dbo.AirClasses: ID, Name) para el listado Pasajero_Clase de Viajeros
IF OBJECT_ID('dbo.SP_LOADAIRCLASSES', 'P') IS NOT NULL
    DROP PROCEDURE dbo.SP_LOADAIRCLASSES;
GO
CREATE PROCEDURE dbo.SP_LOADAIRCLASSES
AS
BEGIN
    SET NOCOUNT ON;
    SELECT ID, Name FROM dbo.AirClasses ORDER BY Name;
END
GO
