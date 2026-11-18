-- Opcion de menu "Viajeros" en Administrador de Perfiles
IF NOT EXISTS (SELECT 1 FROM dbo.Opcion WHERE Controller = 'Viajero')
    INSERT INTO dbo.Opcion (Nombre, Controller, Action, Menu, Icono, Ajax, Orden)
    VALUES ('Viajeros', 'Viajero', 'Viajeros,Perfil,Registrar,GuardarViajero', 'ADMIN', 'bi-airplane', 1, 9);

-- Acceso para el rol 1 y para todo rol que ya administre opciones ('Administrar Opciones')
INSERT INTO dbo.Rol_Opcion (RolId, OpcionId)
SELECT r.RolId, o.id
FROM dbo.Opcion o
CROSS JOIN (
    SELECT 1 AS RolId WHERE EXISTS (SELECT 1 FROM dbo.Rol WHERE id = 1)
    UNION
    SELECT ro2.RolId FROM dbo.Rol_Opcion ro2 JOIN dbo.Opcion o2 ON o2.id = ro2.OpcionId WHERE o2.Nombre = 'Administrar Opciones'
) r
WHERE o.Controller = 'Viajero'
  AND NOT EXISTS (SELECT 1 FROM dbo.Rol_Opcion ro WHERE ro.RolId = r.RolId AND ro.OpcionId = o.id);
