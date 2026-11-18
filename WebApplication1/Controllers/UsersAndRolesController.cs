using LogicBo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Filters;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using System.IO;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Access]
    public class UsersAndRolesController : Controller
    {
        #region Properties
       
        UsersAndRolesBo _UsersAndRolesBo = new UsersAndRolesBo();
        SecurityBo _securityBo = new SecurityBo();
             #endregion
        //public ActionResult UsersAndRoles()
        //{
        //    ViewBag.HeadquarterDictionary = new SelectList(_headquarterBo.GetDictionary(), "Key", "Value");
        //    return PartialView();
        //}
        public ActionResult Certificate()
        {
            return PartialView();
        }
        public ActionResult IndexRoles()
        {
            var result = _UsersAndRolesBo.GetIndexUserAndRol();
            return PartialView(result);
        }
       
       

        public ActionResult Usuarios()
        {
            ViewBag.Usuarios = _securityBo.GetUsuarios();
            ViewBag.Roles = _securityBo.GetRoles();
            return PartialView();
        }

        public ActionResult Roles()
        {
            ViewBag.Roles = _securityBo.GetRoles();
            ViewBag.Opciones = _securityBo.GetOpciones();
            return PartialView();
        }

        public ActionResult AdministrarOpciones()
        {
            ViewBag.Opciones = _securityBo.GetOpcionesAdmin();
            return PartialView();
        }

        #region JsonResult
        [HttpPost]
        public JsonResult GuardarRol(int id, string descripcion)
        {
            try
            {
                return Json(new { result = true, id = _securityBo.SaveRol(id, descripcion) });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GetOpcionesRol(int rolId)
        {
            return Json(_securityBo.GetOpcionesByRol(rolId));
        }

        [HttpPost]
        public JsonResult GuardarOpcionesRol(int rolId, int[] opcionIds, int[] soloLecturaIds)
        {
            try
            {
                _securityBo.SaveOpcionesRol(rolId, opcionIds, soloLecturaIds);
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GuardarOpcion(int id, string nombre, string controller, string action, string menu, string icono, bool ajax, int orden, bool activo)
        {
            try
            {
                var model = new SecurityBo.OpcionModel { id = id, Nombre = nombre, Controller = controller, Action = action, Menu = menu, Icono = icono, Ajax = ajax, Orden = orden, Activo = activo };
                return Json(new { result = true, id = _securityBo.SaveOpcion(model) });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        [HttpPost]
        public JsonResult GuardarUsuario(int id, string login, string nombres, string apellidos, string email, bool activo, string password, int[] rolIds)
        {
            try
            {
                var model = new SecurityBo.UsuarioModel { Id = id, Login = login, Nombres = nombres, Apellidos = apellidos, Email = email, Activo = activo };
                return Json(new { result = true, id = _securityBo.SaveUsuario(model, password, rolIds) });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }
        #endregion

    }
}
