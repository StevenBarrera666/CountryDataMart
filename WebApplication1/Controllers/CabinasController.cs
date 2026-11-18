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
    public class CabinasController : Controller
    {
        InspectionsBo _inspectionsBo = new InspectionsBo();

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult Cabinas(FormCollection collection)
        {
            return PartialView(_inspectionsBo.ListarCabinas());
        }

        private CabinaVueloViewModel Cargar(int id)
        {
            DataTable dt = _inspectionsBo.ObtenerCabina(id);
            if (dt == null || dt.Rows.Count == 0) return null;
            DataRow r = dt.Rows[0];
            return new CabinaVueloViewModel
            {
                Id = Convert.ToInt32(r["Id"]),
                Nombre = r["Nombre"].ToString(),
                NombreIngles = r["NombreIngles"] == DBNull.Value ? string.Empty : r["NombreIngles"].ToString(),
                CodigoGDS = r["CodigoGDS"] == DBNull.Value ? string.Empty : r["CodigoGDS"].ToString(),
                Orden = r["Orden"] == DBNull.Value ? (int?)null : Convert.ToInt32(r["Orden"]),
                Activo = r["Activo"] == DBNull.Value || Convert.ToBoolean(r["Activo"])
            };
        }

        public ActionResult Perfil(int id)
        {
            var model = Cargar(id);
            if (model == null) return HttpNotFound("No se encontró la cabina solicitada.");
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Perfil", model) : View("Perfil", model);
        }

        [EscrituraVista("Cabinas", "Cabinas")]
        public ActionResult Registrar(int? id)
        {
            var model = id.HasValue && id.Value > 0 ? Cargar(id.Value) : new CabinaVueloViewModel { Orden = 1 };
            if (model == null) return HttpNotFound("No se encontró la cabina solicitada.");
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Registrar", model) : View("Registrar", model);
        }

        [HttpPost]
        [Escritura("Cabinas", "Cabinas")]
        public JsonResult GuardarCabina(CabinaVueloViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.Nombre))
                    return Json(new { result = false, message = "Ingrese el nombre de la cabina." });
                if (model.Nombre.Trim().Length > 100)
                    return Json(new { result = false, message = "El nombre no puede superar 100 caracteres." });
                if (!string.IsNullOrWhiteSpace(model.NombreIngles) && model.NombreIngles.Trim().Length > 100)
                    return Json(new { result = false, message = "El nombre en inglés no puede superar 100 caracteres." });
                if (!string.IsNullOrWhiteSpace(model.CodigoGDS) && model.CodigoGDS.Trim().Length > 5)
                    return Json(new { result = false, message = "El código GDS no puede superar 5 caracteres." });

                int id = _inspectionsBo.GuardarCabina(model.Id, model.Nombre.Trim(),
                    model.NombreIngles == null ? null : model.NombreIngles.Trim(),
                    model.CodigoGDS == null ? null : model.CodigoGDS.Trim().ToUpper(),
                    model.Orden, model.Activo);
                return Json(new { result = true, idGenerado = id });
            }
            catch (Exception ex)
            {
                // 2627 / 2601: violacion del indice unico UQ_CabinasVuelo_Nombre
                var sql = ex as SqlException ?? ex.InnerException as SqlException;
                if (sql != null && (sql.Number == 2627 || sql.Number == 2601))
                    return Json(new { result = false, message = "Ya existe una cabina con ese nombre." });
                return Json(new { result = false, message = "Error: " + ex.Message });
            }
        }
    }
}
