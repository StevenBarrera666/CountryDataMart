using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace LogicBo
{
    public class SecurityBo
    {
        #region Properties
        private readonly Entity.ModelEntities entities = new Entity.ModelEntities();
        private readonly Transversal.EncryptData encryptData = new Transversal.EncryptData();
        #endregion

        #region Opciones
        /// <summary>Opciones activas a las que tiene acceso el usuario por cualquiera de sus roles.</summary>
        public List<OpcionModel> GetOpcionesByUsuario(int usuarioId)
        {
            return entities.Database.SqlQuery<OpcionModel>(
                // Con varios roles, la opcion es de solo lectura unicamente si todos sus roles la dan asi
                @"SELECT o.id, o.Nombre, o.Controller, o.Action, o.Menu, o.Icono, o.Ajax, o.Orden,
                         CAST(MIN(CAST(ro.SoloLectura AS INT)) AS BIT) AS SoloLectura
                  FROM dbo.Opcion o
                  JOIN dbo.Rol_Opcion ro ON ro.OpcionId = o.id
                  JOIN dbo.usuario_Rol ur ON ur.RolId = ro.RolId
                  WHERE ur.UsuarioID = @p0 AND o.Activo = 1
                  GROUP BY o.id, o.Nombre, o.Controller, o.Action, o.Menu, o.Icono, o.Ajax, o.Orden
                  ORDER BY o.Menu, o.Orden", usuarioId).ToList();
        }

        /// <summary>Opciones activas (todas) para saber que controller/action estan protegidos.</summary>
        public List<OpcionModel> GetOpcionesProtegidas()
        {
            return entities.Database.SqlQuery<OpcionModel>(
                "SELECT id, Nombre, Controller, Action, Menu, Icono, Ajax, Orden FROM dbo.Opcion WHERE Activo = 1").ToList();
        }

        public List<OpcionModel> GetOpciones()
        {
            return entities.Database.SqlQuery<OpcionModel>(
                "SELECT id, Nombre, Controller, Action, Menu, Icono, Ajax, Orden FROM dbo.Opcion WHERE Activo = 1 ORDER BY Menu, Orden").ToList();
        }

        /// <summary>Catalogo completo de opciones, incluidas las inactivas (pantalla de administracion).</summary>
        public List<OpcionModel> GetOpcionesAdmin()
        {
            return entities.Database.SqlQuery<OpcionModel>(
                "SELECT id, Nombre, Controller, Action, Menu, Icono, Ajax, Orden, Activo FROM dbo.Opcion ORDER BY Menu, Orden").ToList();
        }

        private static readonly string[] MenusValidos = { "PRINCIPAL", "ADMIN", "TRAZA" };

        public int SaveOpcion(OpcionModel o)
        {
            if (string.IsNullOrWhiteSpace(o.Nombre) || string.IsNullOrWhiteSpace(o.Controller))
                throw new Exception("Nombre y controller son obligatorios");
            if (!MenusValidos.Contains(o.Menu))
                throw new Exception("Menu invalido. Use: " + string.Join(", ", MenusValidos));

            var action = string.IsNullOrWhiteSpace(o.Action) ? null : o.Action.Trim();
            var icono = string.IsNullOrWhiteSpace(o.Icono) ? null : o.Icono.Trim();
            if (o.id > 0)
            {
                entities.Database.ExecuteSqlCommand(
                    @"UPDATE dbo.Opcion SET Nombre=@p0, Controller=@p1, Action=@p2, Menu=@p3, Icono=@p4, Ajax=@p5, Orden=@p6, Activo=@p7
                      WHERE id=@p8",
                    o.Nombre.Trim(), o.Controller.Trim(), (object)action ?? DBNull.Value, o.Menu, (object)icono ?? DBNull.Value,
                    o.Ajax, o.Orden, o.Activo, o.id);
                return o.id;
            }
            return entities.Database.SqlQuery<int>(
                @"INSERT INTO dbo.Opcion (Nombre, Controller, Action, Menu, Icono, Ajax, Orden, Activo)
                  VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7);
                  SELECT CAST(SCOPE_IDENTITY() AS INT)",
                o.Nombre.Trim(), o.Controller.Trim(), (object)action ?? DBNull.Value, o.Menu, (object)icono ?? DBNull.Value,
                o.Ajax, o.Orden, o.Activo).Single();
        }

        public List<RolOpcionModel> GetOpcionesByRol(int rolId)
        {
            return entities.Database.SqlQuery<RolOpcionModel>(
                "SELECT OpcionId, SoloLectura FROM dbo.Rol_Opcion WHERE RolId = @p0", rolId).ToList();
        }

        /// <param name="opcionIds">Opciones a las que accede el rol.</param>
        /// <param name="soloLecturaIds">Subconjunto de opciones en las que el rol solo consulta.</param>
        public void SaveOpcionesRol(int rolId, IEnumerable<int> opcionIds, IEnumerable<int> soloLecturaIds)
        {
            var lectura = new HashSet<int>(soloLecturaIds ?? new int[0]);
            using (var tx = entities.Database.BeginTransaction())
            {
                entities.Database.ExecuteSqlCommand("DELETE FROM dbo.Rol_Opcion WHERE RolId = @p0", rolId);
                // Solo lectura implica acceso: una opcion marcada solo en "Solo lectura" tambien se asigna al rol
                foreach (var opcionId in (opcionIds ?? new int[0]).Concat(lectura).Distinct())
                {
                    entities.Database.ExecuteSqlCommand(
                        "INSERT INTO dbo.Rol_Opcion (RolId, OpcionId, SoloLectura) VALUES (@p0, @p1, @p2)",
                        rolId, opcionId, lectura.Contains(opcionId));
                }
                tx.Commit();
            }
        }
        #endregion

        #region Roles
        public List<RolModel> GetRoles()
        {
            return entities.Rol.OrderBy(r => r.Descripcion)
                .Select(r => new RolModel { Id = r.id, Descripcion = r.Descripcion }).ToList();
        }

        public int SaveRol(int id, string descripcion)
        {
            if (string.IsNullOrWhiteSpace(descripcion))
                throw new Exception("La descripcion del rol es obligatoria");

            // Rol no tiene PK en la base: EF la trata como solo lectura, se escribe con SQL directo
            if (id > 0)
            {
                var filas = entities.Database.ExecuteSqlCommand(
                    "UPDATE dbo.Rol SET Descripcion = @p0 WHERE id = @p1", descripcion.Trim(), id);
                if (filas == 0) throw new Exception("Rol no encontrado");
                return id;
            }
            return entities.Database.SqlQuery<int>(
                "INSERT INTO dbo.Rol (Descripcion) VALUES (@p0); SELECT CAST(SCOPE_IDENTITY() AS INT)",
                descripcion.Trim()).Single();
        }
        #endregion

        #region Usuarios
        public List<UsuarioModel> GetUsuarios()
        {
            var usuarios = entities.Usuario.OrderBy(u => u.Usuario1).ToList();
            var roles = entities.usuario_Rol.Where(ur => ur.UsuarioID != null && ur.RolId != null)
                .Select(ur => new { ur.UsuarioID, ur.RolId }).ToList();
            return usuarios.Select(u => new UsuarioModel
            {
                Id = u.Usuario_ID,
                Login = u.Usuario1,
                Nombres = u.Nombres,
                Apellidos = u.Apellidos,
                Email = u.Email,
                Activo = u.Activo == 1,
                RolIds = roles.Where(r => r.UsuarioID == u.Usuario_ID).Select(r => r.RolId.Value).ToList()
            }).ToList();
        }

        /// <summary>Crea o actualiza el usuario. La clave solo se cambia si se envia.</summary>
        public int SaveUsuario(UsuarioModel model, string password, IEnumerable<int> rolIds)
        {
            if (string.IsNullOrWhiteSpace(model.Login) || string.IsNullOrWhiteSpace(model.Nombres))
                throw new Exception("Usuario y nombres son obligatorios");

            var login = model.Login.Trim();
            if (entities.Usuario.Any(u => u.Usuario1 == login && u.Usuario_ID != model.Id))
                throw new Exception("Ya existe un usuario con ese login");

            Entity.Usuario usuario;
            if (model.Id > 0)
            {
                usuario = entities.Usuario.Find(model.Id);
                if (usuario == null) throw new Exception("Usuario no encontrado");
            }
            else
            {
                if (string.IsNullOrEmpty(password))
                    throw new Exception("La clave es obligatoria para un usuario nuevo");
                usuario = new Entity.Usuario { Pass = string.Empty };
                entities.Usuario.Add(usuario);
            }

            usuario.Usuario1 = login;
            usuario.Nombres = (model.Nombres ?? string.Empty).Trim();
            usuario.Apellidos = (model.Apellidos ?? string.Empty).Trim();
            usuario.Email = model.Email;
            usuario.Activo = model.Activo ? 1 : 0;
            if (!string.IsNullOrEmpty(password))
                usuario.PasswordMD5 = encryptData.Encrypt(password, true);
            entities.SaveChanges();

            // usuario_Rol no tiene PK en la base: EF la trata como solo lectura, se escribe con SQL directo
            using (var tx = entities.Database.BeginTransaction())
            {
                entities.Database.ExecuteSqlCommand("DELETE FROM dbo.usuario_Rol WHERE UsuarioID = @p0", usuario.Usuario_ID);
                foreach (var rolId in (rolIds ?? new int[0]).Distinct())
                    entities.Database.ExecuteSqlCommand(
                        "INSERT INTO dbo.usuario_Rol (UsuarioID, RolId) VALUES (@p0, @p1)", usuario.Usuario_ID, rolId);
                tx.Commit();
            }

            return usuario.Usuario_ID;
        }
        #endregion

        #region Entidades
        public class OpcionModel
        {
            public int id { get; set; }
            public string Nombre { get; set; }
            public string Controller { get; set; }
            public string Action { get; set; }
            public string Menu { get; set; }
            public string Icono { get; set; }
            public bool Ajax { get; set; }
            public int Orden { get; set; }
            public bool Activo { get; set; }
            public bool SoloLectura { get; set; }
        }

        public class RolOpcionModel
        {
            public int OpcionId { get; set; }
            public bool SoloLectura { get; set; }
        }

        public class RolModel
        {
            public int Id { get; set; }
            public string Descripcion { get; set; }
        }

        public class UsuarioModel
        {
            public int Id { get; set; }
            public string Login { get; set; }
            public string Nombres { get; set; }
            public string Apellidos { get; set; }
            public string Email { get; set; }
            public bool Activo { get; set; }
            public List<int> RolIds { get; set; }
        }
        #endregion
    }
}
