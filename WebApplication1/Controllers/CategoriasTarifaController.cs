using LogicBo;
using System;
using System.Data;
using System.Web.Mvc;
using WebApplication1.Filters;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Access]
    public class CategoriasTarifaController : Controller
    {
        InspectionsBo _inspectionsBo = new InspectionsBo();

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult CategoriasTarifa(FormCollection collection)
        {
            return PartialView(_inspectionsBo.CargarCategoriasTarifa());
        }

        private CategoriaTarifaViewModel Cargar(int id)
        {
            DataTable dt = _inspectionsBo.ObtenerCategoriaTarifa(id);
            if (dt == null || dt.Rows.Count == 0) return null;
            return new CategoriaTarifaViewModel
            {
                Id = Convert.ToInt32(dt.Rows[0]["Id"]),
                Nombre = dt.Rows[0]["Nombre"].ToString()
            };
        }

        public ActionResult Perfil(int id)
        {
            var model = Cargar(id);
            if (model == null) return HttpNotFound("No se encontró la categoría de tarifa solicitada.");
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Perfil", model) : View("Perfil", model);
        }

        [EscrituraVista("CategoriasTarifa", "CategoriasTarifa")]
        public ActionResult Registrar(int? id)
        {
            var model = id.HasValue && id.Value > 0 ? Cargar(id.Value) : new CategoriaTarifaViewModel();
            if (model == null) return HttpNotFound("No se encontró la categoría de tarifa solicitada.");
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Registrar", model) : View("Registrar", model);
        }

        [HttpPost]
        [Escritura("CategoriasTarifa", "CategoriasTarifa")]
        public JsonResult GuardarCategoria(CategoriaTarifaViewModel model)
        {
            try
            {
                if (model == null || string.IsNullOrWhiteSpace(model.Nombre))
                    return Json(new { result = false, message = "Ingrese el nombre de la categoría." });
                if (model.Nombre.Trim().Length > 200)
                    return Json(new { result = false, message = "El nombre no puede superar 200 caracteres." });

                int id = _inspectionsBo.GuardarCategoriaTarifa(model.Id, model.Nombre.Trim());
                return Json(new { result = true, idGenerado = id });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = "Error: " + ex.Message });
            }
        }
    }
}
