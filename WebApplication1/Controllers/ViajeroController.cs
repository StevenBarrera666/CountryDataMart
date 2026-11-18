using LogicBo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web.Mvc;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [Access]
    public class ViajeroController : Controller
    {
        InspectionsBo _inspectionsBo = new InspectionsBo();

        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult Viajeros(FormCollection collection)
        {
            return PartialView(_inspectionsBo.ListarViajeros());
        }

        // Copia a las propiedades del modelo las columnas de dbo.Viajeros que tengan el mismo nombre
        private ViajeroRegistroViewModel Cargar(int id)
        {
            DataTable dt = _inspectionsBo.ObtenerViajero(id);
            if (dt == null || dt.Rows.Count == 0) return null;

            DataRow row = dt.Rows[0];
            var model = new ViajeroRegistroViewModel();
            foreach (var prop in typeof(ViajeroRegistroViewModel).GetProperties())
            {
                if (!dt.Columns.Contains(prop.Name) || row[prop.Name] == DBNull.Value) continue;
                Type destino = Nullable.GetUnderlyingType(prop.PropertyType) ?? prop.PropertyType;
                prop.SetValue(model, Convert.ChangeType(row[prop.Name], destino));
            }
            return model;
        }

        private void CargarCatalogos(ViajeroRegistroViewModel model)
        {
            var fichas = new List<object>();
            DataTable dtFichas = _inspectionsBo.GetFichasActivas();
            if (dtFichas != null)
            {
                foreach (DataRow rf in dtFichas.Rows)
                    fichas.Add(new { Id = Convert.ToInt32(rf["Id"]), Nombre = rf["Codigo"] + " | " + rf["Nombre"] });
            }
            ViewBag.DdlFichas = new SelectList(fichas, "Id", "Nombre", model.FichaId);

            // Identificacion_Tipo es varchar(30): se omiten los tipos cuyo nombre no cabe
            var tipos = new List<string>();
            DataTable dtTipos = _inspectionsBo.CargarTiposDocumento();
            if (dtTipos != null)
            {
                foreach (DataRow rt in dtTipos.Rows)
                {
                    string nombre = rt["Nombre"].ToString();
                    if (nombre.Length <= 30) tipos.Add(nombre);
                }
            }
            if (!string.IsNullOrEmpty(model.Identificacion_Tipo) && !tipos.Contains(model.Identificacion_Tipo))
                tipos.Add(model.Identificacion_Tipo);
            ViewBag.DdlTiposDoc = new SelectList(tipos, model.Identificacion_Tipo);

            // Aerolinea del viajero frecuente: proveedores con TipoId = 1; guarda el Id del proveedor (como texto)
            string aerolineaTexto;
            ViewBag.DdlAerolineas = ListaPorId(_inspectionsBo.ListarAerolineas(), "Id", "Nombre", "Activo", model.Pasajero_ViajeroFrecuenteAero, out aerolineaTexto);
            ViewBag.AerolineaTexto = aerolineaTexto;

            // Sexo: lista fija en codigo (no hay tabla); se guarda el texto
            var sexos = new List<string> { "Hombre", "Mujer", "Desconocido" };
            if (!string.IsNullOrEmpty(model.Pasajero_Sexo) && !sexos.Contains(model.Pasajero_Sexo))
                sexos.Add(model.Pasajero_Sexo);
            ViewBag.DdlSexos = new SelectList(sexos, model.Pasajero_Sexo);

            // Paises de nacimiento y de emision (pasaporte y visa): guardan el ID de dbo.Countries (como texto)
            string paisNacTexto, paisPasaporteTexto, paisVisaTexto;
            DataTable dtPaises = _inspectionsBo.ListarCountries();
            ViewBag.DdlPaisNacimiento = ListaPorId(dtPaises, "ID", "Name", null, model.Pasajero_PaisNacimiento, out paisNacTexto);
            ViewBag.PaisNacimientoTexto = paisNacTexto;
            ViewBag.DdlPaisPasaporte = ListaPorId(dtPaises, "ID", "Name", null, model.Pasaporte_PaisEmision, out paisPasaporteTexto);
            ViewBag.PaisPasaporteTexto = paisPasaporteTexto;
            ViewBag.DdlPaisVisa = ListaPorId(dtPaises, "ID", "Name", null, model.Visa_PaisEmision, out paisVisaTexto);
            ViewBag.PaisVisaTexto = paisVisaTexto;

            // Tipo de pasajero, Cabina, Silla y Clase guardan el Id del catalogo (como texto: las columnas son varchar)
            string tipoPasajeroTexto, cabinaTexto, sillaTexto, claseTexto;
            ViewBag.DdlTiposPasajero = ListaPorId(_inspectionsBo.ListarTiposPasajero(), "Id", "Texto", "Activo", model.Pasajero_Tipo, out tipoPasajeroTexto);
            ViewBag.TipoPasajeroTexto = tipoPasajeroTexto;
            ViewBag.DdlCabinas = ListaPorId(_inspectionsBo.ListarCabinas(), "Id", "Nombre", "Activo", model.Pasajero_Cabina, out cabinaTexto);
            ViewBag.CabinaTexto = cabinaTexto;
            ViewBag.DdlSillas = ListaPorId(_inspectionsBo.ListarSillasPreferencia(), "Id", "Nombre", "Activo", model.Pasajero_Silla, out sillaTexto);
            ViewBag.SillaTexto = sillaTexto;
            ViewBag.DdlClases = ListaPorId(_inspectionsBo.CargarAirClasses(), "ID", "Name", null, model.Pasajero_Clase, out claseTexto);
            ViewBag.ClaseTexto = claseTexto;

            ViewBag.FichaTexto = model.FichaId.HasValue
                ? ((SelectList)ViewBag.DdlFichas).Where(i => i.Value == model.FichaId.Value.ToString()).Select(i => i.Text).FirstOrDefault()
                : null;
        }


        /// <summary>
        /// Lista Id/Nombre de un catalogo. Solo incluye los registros activos (si hay columna de estado) y, al editar,
        /// el ya asignado aunque este inactivo. Un valor que no sea un Id (dato anterior guardado como nombre) se conserva.
        /// </summary>
        private static SelectList ListaPorId(DataTable dt, string colId, string colNombre, string colActivo, string valorActual, out string textoActual)
        {
            var items = new List<SelectListItem>();
            textoActual = valorActual;
            if (dt != null)
            {
                foreach (DataRow r in dt.Rows)
                {
                    string id = r[colId].ToString();
                    string nombre = r[colNombre] == DBNull.Value ? string.Empty : r[colNombre].ToString().Trim();
                    bool activo = colActivo == null || r[colActivo] == DBNull.Value || Convert.ToBoolean(r[colActivo]);
                    bool asignado = id == valorActual;
                    if (asignado) textoActual = nombre;
                    if (nombre.Length > 0 && (activo || asignado))
                        items.Add(new SelectListItem { Value = id, Text = nombre, Selected = asignado });
                }
            }
            if (!string.IsNullOrEmpty(valorActual) && !items.Any(i => i.Value == valorActual))
                items.Add(new SelectListItem { Value = valorActual, Text = valorActual, Selected = true });
            return new SelectList(items, "Value", "Text", valorActual);
        }

        public ActionResult Perfil(int id)
        {
            var model = Cargar(id);
            if (model == null) return HttpNotFound("No se encontró el viajero solicitado.");
            CargarCatalogos(model);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Perfil", model) : View("Perfil", model);
        }

        [EscrituraVista("Viajero", "Viajeros")]
        public ActionResult Registrar(int? id)
        {
            var model = id.HasValue && id.Value > 0 ? Cargar(id.Value) : new ViajeroRegistroViewModel();
            if (model == null) return HttpNotFound("No se encontró el viajero solicitado.");
            CargarCatalogos(model);
            return Request.IsAjaxRequest() ? (ActionResult)PartialView("Registrar", model) : View("Registrar", model);
        }

        [HttpPost]
        [Escritura("Viajero", "Viajeros")]
        public JsonResult GuardarViajero(ViajeroRegistroViewModel model)
        {
            try
            {
                if (model == null ||
                    string.IsNullOrWhiteSpace(model.Pasajero_Apellido) ||
                    string.IsNullOrWhiteSpace(model.Pasajero_Nombre) ||
                    string.IsNullOrWhiteSpace(model.Identificacion_Tipo) ||
                    string.IsNullOrWhiteSpace(model.Identificacion_Numero))
                {
                    return Json(new { result = false, message = "Complete los campos obligatorios: apellido, nombre y tipo y número de identificación." });
                }

                int id = _inspectionsBo.GuardarViajero(model);
                return Json(new { result = true, idGenerado = id });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = "Error: " + ex.Message });
            }
        }
    }
}
