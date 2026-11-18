using LogicBo;
using System;
using System.Data;
using System.Data.SqlClient;
using System.Web.Mvc;
using WebApplication1.Filters;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Access]
    public class FichasController : Controller
    {
        InspectionsBo _inspectionsBo = new InspectionsBo();

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult Fichas(FormCollection collection)
        {
            return PartialView(_inspectionsBo.ListarFichas());
        }

        private FichaViewModel Cargar(int id)
        {
            DataTable dt = _inspectionsBo.ObtenerFicha(id);
            if (dt == null || dt.Rows.Count == 0) return null;
            DataRow r = dt.Rows[0];
            return new FichaViewModel
            {
                Id = Convert.ToInt32(r["Id"]),
                Codigo = r["Codigo"].ToString(),
                Nombre = r["Nombre"].ToString(),
                Descripcion = r["Descripcion"] == DBNull.Value ? string.Empty : r["Descripcion"].ToString(),
                FechaCreacion = r["FechaCreacion"] == DBNull.Value ? (DateTime?)null : Convert.ToDateTime(r["FechaCreacion"]),
                Activo = r["Activo"] == DBNull.Value || Convert.ToBoolean(r["Activo"])
            };
        }

        public ActionResult Perfil(int id)
        {
            var model = Cargar(id);
            if (model == null) return HttpNotFound("No se encontró la ficha solicitada.");
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Perfil", model) : View("Perfil", model);
        }

        [EscrituraVista("Fichas", "Fichas")]
        public ActionResult Registrar(int? id)
        {
            var model = id.HasValue && id.Value > 0 ? Cargar(id.Value) : new FichaViewModel();
            if (model == null) return HttpNotFound("No se encontró la ficha solicitada.");
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Registrar", model) : View("Registrar", model);
        }

        [HttpPost]
        [Escritura("Fichas", "Fichas")]
        public JsonResult GuardarFicha(FichaViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.Codigo) || string.IsNullOrWhiteSpace(model.Nombre))
                    return Json(new { result = false, message = "Ingrese el código y el nombre de la ficha." });
                if (model.Codigo.Trim().Length > 20)
                    return Json(new { result = false, message = "El código no puede superar 20 caracteres." });
                if (model.Nombre.Trim().Length > 120)
                    return Json(new { result = false, message = "El nombre no puede superar 120 caracteres." });
                if (!string.IsNullOrWhiteSpace(model.Descripcion) && model.Descripcion.Trim().Length > 300)
                    return Json(new { result = false, message = "La descripción no puede superar 300 caracteres." });

                int id = _inspectionsBo.GuardarFicha(model.Id, model.Codigo.Trim(), model.Nombre.Trim(),
                    model.Descripcion == null ? null : model.Descripcion.Trim(), model.Activo);
                return Json(new { result = true, idGenerado = id });
            }
            catch (Exception ex)
            {
                // 2627 / 2601: violacion del indice unico UQ_Fichas_Codigo
                var sql = ex as SqlException ?? ex.InnerException as SqlException;
                if (sql != null && (sql.Number == 2627 || sql.Number == 2601))
                    return Json(new { result = false, message = "Ya existe una ficha con ese código." });
                return Json(new { result = false, message = "Error: " + ex.Message });
            }
        }
    }
}
