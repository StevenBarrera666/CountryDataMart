using LogicBo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.IO;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Filters;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Access]
    public class ProveedorController : Controller
    {
        InspectionsBo _inspectionsBo = new InspectionsBo();
        // GET: Cliente/Perfil/1
      
        [EscrituraVista("Proveedor", "Proveedores")]
        public ActionResult Registrar(int? id)
        {
            Response.Charset = "utf-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            var model = new ProveedorRegistroViewModel();
            string paisNombre = null; // nombre del pais del perfil (dbo.Paises), para ubicarlo en dbo.Countries

            // 1. Carga o inicialización de datos
            if (id.HasValue && id.Value > 0)
            {
                DataTable dt = _inspectionsBo.ObtenerPerfilProveedorDataTable(id.Value);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    model.Id = id.Value;
                    model.CodigoFicha = "PRO-" + id.Value.ToString(); 
                    model.EstadoEtiqueta = row.Field<string>("EstadoEtiqueta");
                    model.LogoUrl = row.Field<string>("LogoUrl");

                    model.NaturalezaId = Convert.ToInt32(row["NaturalezaId"]);
                    model.TipoId = Convert.ToInt32(row["TipoId"]);
                    model.RazonSocial = row.Field<string>("RazonSocial");
                    model.EstablecimientoComercial = row.Field<string>("EstablecimientoComercial");
                    model.NombreComercial = row.Field<string>("NombreComercial");
                    model.TipoDocumentoId = Convert.ToInt32(row["TipoDocumentoId"]);
                    model.NumeroDocumento = row.Field<string>("NumeroDocumento");
                    model.Telefono1 = row.Field<string>("Telefono1");
                    model.Telefono2 = row.Field<string>("Telefono2");
                    model.CorreoPrincipal = row.Field<string>("CorreoPrincipal");
                    model.Correo2 = row.Field<string>("Correo2");
                    model.Direccion = row.Field<string>("Direccion");
                    model.CodigoPostal = _inspectionsBo.ObtenerCodigoPostal("Proveedores", id.Value);

                    model.Origen = row.Field<string>("Origen");
                    model.PaisId = Convert.ToInt32(row["PaisId"]);
                    paisNombre = row.Table.Columns.Contains("Pais") && row["Pais"] != DBNull.Value ? row["Pais"].ToString() : null;
                    model.DepartamentoId = Convert.ToInt32(row["DepartamentoId"]);
                    model.CiudadId = Convert.ToInt32(row["CiudadId"]);

                    model.TipoCobroId = Convert.ToInt32(row["TipoCobroId"]);
                    model.CategoriaTarifaId = Convert.ToInt32(row["CategoriaTarifaId"]);
                    model.TarifaAdministrativa = row.Field<decimal>("TarifaAdministrativa");
                    model.Descuentos = row.Field<decimal>("Descuentos");

                    model.ActividadEconomicaId = Convert.ToInt32(row["ActividadEconomicaId"]);
                    model.IndustriaId = Convert.ToInt32(row["IndustriaId"]);
                    model.RutaCamaraComercio = row.Field<string>("RutaCamaraComercio");
                    model.RutaRut = row.Field<string>("RutaRut");

                    Session["ClienteLogoUrl"] = model.LogoUrl;
                }
            }
            else
            {
                model.CodigoFicha = "PRO-" + DateTime.Now.ToString("yyMMddHHmm");
                model.EstadoEtiqueta = "Activo";
                model.Origen = "Nacional";
                model.PaisId = 1;
                model.DepartamentoId = 1;
                model.CiudadId = 1;
                model.TarifaAdministrativa = 0m;
                model.Descuentos = 0m;
                Session.Remove("ClienteLogoUrl");
            }

            // 2. Catálogos Maestros (SelectLists)
            ViewBag.DdlNaturalezas = new SelectList(new[] {
                new { Id = 1, Nombre = "Jurídica" },
                new { Id = 2, Nombre = "Natural" }
            }, "Id", "Nombre", model.NaturalezaId);

            ViewBag.DdlTiposCliente = new SelectList(new[] {
                new { Id = 1, Nombre = "Aerolínea" },
                new { Id = 2, Nombre = "Hotel" },
                new { Id = 3, Nombre = "Rentadoras de Autos" },
                new { Id = 4, Nombre = "Cruceros" },
                new { Id = 5, Nombre = "Consolidadora" },
                new { Id = 6, Nombre = "Agencia de Viajes" },
                new { Id = 7, Nombre = "Gasto Administrativo" },
                new { Id = 8, Nombre = "Tarjeta de Asistencia" },
                new { Id = 9, Nombre = "Otros Proveedores" }
            }, "Id", "Nombre", model.TipoId);
                         





            var tiposDoc = new List<object>();
            DataTable dtTiposDoc = _inspectionsBo.CargarTiposDocumento();
            if (dtTiposDoc != null)
            {
                foreach (DataRow rt in dtTiposDoc.Rows)
                    tiposDoc.Add(new { Id = Convert.ToInt32(rt["Id"]), Nombre = rt["Nombre"].ToString() });
            }
            ViewBag.DdlTiposDoc = new SelectList(tiposDoc, "Id", "Nombre", model.TipoDocumentoId);

            ViewBag.DdlOrigen = new SelectList(new[] { "Nacional", "Internacional" }, model.Origen);

            // Paises desde dbo.Countries; al editar se ubica por nombre y, en un alta, se preselecciona Colombia
            var paises = new List<object>();
            int? idColombia = null, idPaisPerfil = null;
            DataTable dtPaises = _inspectionsBo.ListarCountries();
            if (dtPaises != null)
            {
                foreach (DataRow rp in dtPaises.Rows)
                {
                    int idPais = Convert.ToInt32(rp["ID"]);
                    string nombrePais = rp["Name"].ToString().Trim();
                    paises.Add(new { Id = idPais, Nombre = nombrePais });
                    if (string.Equals(nombrePais, "Colombia", StringComparison.OrdinalIgnoreCase)) idColombia = idPais;
                    if (paisNombre != null && string.Equals(nombrePais, paisNombre.Trim(), StringComparison.OrdinalIgnoreCase)) idPaisPerfil = idPais;
                }
            }
            if (id.HasValue && id.Value > 0) { if (idPaisPerfil.HasValue) model.PaisId = idPaisPerfil.Value; }
            else if (idColombia.HasValue) model.PaisId = idColombia.Value;
            ViewBag.DdlPaises = new SelectList(paises, "Id", "Nombre", model.PaisId);

            ViewBag.DdlDepartamentos = new SelectList(new[] {
                new { Id = 1, Nombre = "Bogotá D.C." },
                new { Id = 2, Nombre = "Antioquia" },
                new { Id = 3, Nombre = "Valle del Cauca" }
            }, "Id", "Nombre", model.DepartamentoId);

            ViewBag.DdlCiudades = new SelectList(new[] {
                new { Id = 1, Nombre = "Bogotá" },
                new { Id = 2, Nombre = "Medellín" },
                new { Id = 3, Nombre = "Cali" }
            }, "Id", "Nombre", model.CiudadId);

            var tiposCobro = new List<object>();
            DataTable dtTiposCobro = _inspectionsBo.CargarMetodosPago();
            if (dtTiposCobro != null)
            {
                foreach (DataRow rp in dtTiposCobro.Rows)
                    tiposCobro.Add(new { Id = Convert.ToInt32(rp["ID"]), Nombre = rp["name"].ToString() });
            }
            ViewBag.DdlTiposCobro = new SelectList(tiposCobro, "Id", "Nombre", model.TipoCobroId);

            var categoriasTarifa = new List<object>();
            DataTable dtCategorias = _inspectionsBo.CargarCategoriasTarifa();
            if (dtCategorias != null)
            {
                foreach (DataRow rc in dtCategorias.Rows)
                    categoriasTarifa.Add(new { Id = Convert.ToInt32(rc["Id"]), Nombre = rc["Nombre"].ToString() });
            }
            ViewBag.DdlCategoriasTarifa = new SelectList(categoriasTarifa, "Id", "Nombre", model.CategoriaTarifaId);

            var actividades = new List<object>();
            DataTable dtActividades = _inspectionsBo.CargarActividades();
            if (dtActividades != null)
            {
                foreach (DataRow ra in dtActividades.Rows)
                    actividades.Add(new { Id = Convert.ToInt32(ra["Id"]), Nombre = ra["Nombre"].ToString() });
            }
            ViewBag.DdlActividades = new SelectList(actividades, "Id", "Nombre", model.ActividadEconomicaId);

            ViewBag.DdlIndustrias = new SelectList(new[] {
                new { Id = 1, Nombre = "Ingeniería y Construcción" },
                new { Id = 2, Nombre = "Tecnología y Telecomunicaciones" },
                new { Id = 3, Nombre = "Servicios Financieros" }
            }, "Id", "Nombre", model.IndustriaId);

            if (Request.IsAjaxRequest())
            {
                return PartialView("Registrar", model);
            }

            return View("Registrar", model);
        }

        [HttpPost]
        [Escritura("Proveedor", "Proveedores")]
        public JsonResult UploadLogoCliente()
        {
            try
            {
                if (Request.Files.Count == 0)
                    return Json(new { result = false, message = "No se ha seleccionado el logo." });

                HttpPostedFileBase file = Request.Files[0];
                string ext = Path.GetExtension(file.FileName).ToUpperInvariant();
                if (ext != ".PNG" && ext != ".JPG" && ext != ".JPEG")
                    return Json(new { result = false, message = "Formato no válido. Use PNG o JPG." });

                string folderPath = Server.MapPath("~/Uploads/Clientes/Logos/");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                string fileName = $"logo_{Guid.NewGuid()}{ext}";
                file.SaveAs(Path.Combine(folderPath, fileName));

                Session["ClienteLogoUrl"] = "/Uploads/Clientes/Logos/" + fileName;
                return Json(new { result = true });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Escritura("Proveedor", "Proveedores")]
        public JsonResult UploadDocumentoFiscal(string tipoDoc)
        {
            try
            {
                if (Request.Files.Count == 0)
                    return Json(new { result = false, message = "No se envió el archivo." });

                HttpPostedFileBase file = Request.Files[0];
                string ext = Path.GetExtension(file.FileName).ToUpperInvariant();
                if (ext != ".PDF")
                    return Json(new { result = false, message = "Solo se permiten archivos PDF." });

                string folderPath = Server.MapPath("~/Uploads/Clientes/Fiscales/");
                if (!Directory.Exists(folderPath)) Directory.CreateDirectory(folderPath);

                string fileName = $"{tipoDoc}_{Guid.NewGuid()}{ext}";
                string fullPath = "/Uploads/Clientes/Fiscales/" + fileName;
                file.SaveAs(Path.Combine(folderPath, fileName));

                return Json(new { result = true, filePath = fullPath });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }

        [HttpPost]
        [Escritura("Proveedor", "Proveedores")]
        public JsonResult GuardarProveedor(ProveedorRegistroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                var campos = new System.Collections.Generic.List<string>();
                foreach (var kv in ModelState)
                {
                    if (kv.Value.Errors.Count > 0) campos.Add(kv.Key);
                }
                return Json(new { result = false, message = "Por favor verifique los campos obligatorios: " + string.Join(", ", campos) });
            }

            string logoCargado = Session["ClienteLogoUrl"] as string;
            string logoFinal = !string.IsNullOrEmpty(logoCargado) ? logoCargado : (model.LogoUrl ?? "/Content/Images/company-avatar.png");

            try
            {
                DataTable dtResultado = _inspectionsBo.RegistrarOActualizarProveedor(model, logoFinal);

                if (dtResultado != null && dtResultado.Rows.Count > 0)
                {
                    int idGenerado = Convert.ToInt32(dtResultado.Rows[0]["IdGenerado"]);

                    if (idGenerado > 0)
                    {
                        _inspectionsBo.GuardarCodigoPostal("Proveedores", idGenerado, model.CodigoPostal);
                        Session.Remove("ClienteLogoUrl");
                        return Json(new { result = true, idGenerado = idGenerado });
                    }

                    string errorSp = dtResultado.Rows[0]["Mensaje"].ToString();
                    return Json(new { result = false, message = errorSp });
                }

                return Json(new { result = false, message = "No se recibió respuesta del servidor." });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = "Error: " + ex.Message });
            }
        }
        public ActionResult Perfil(int id)
        {
            DataTable dtProveedor = _inspectionsBo.ObtenerPerfilProveedorDataTable(id);

            if (dtProveedor == null || dtProveedor.Rows.Count == 0)
            {
                return HttpNotFound("No se encontró el perfil del proveedor solicitado.");
            }

            DataRow row = dtProveedor.Rows[0];

            var model = new ProveedorPerfilViewModel
            {
                Id = id,
                CodigoFicha = row.Table.Columns.Contains("CodigoFicha") && row["CodigoFicha"] != DBNull.Value
                              ? row["CodigoFicha"].ToString()
                              : "PRV-" + id,
                EstadoEtiqueta = row.Table.Columns.Contains("EstadoEtiqueta") && row["EstadoEtiqueta"] != DBNull.Value
                                 ? row["EstadoEtiqueta"].ToString()
                                 : "Activo",
                LogoUrl = row.Table.Columns.Contains("LogoUrl") && !string.IsNullOrWhiteSpace(row["LogoUrl"].ToString())
                          ? row["LogoUrl"].ToString()
                          : "/Content/Images/company-avatar.png",

                // 1. Datos Personales
                Naturaleza = row.Table.Columns.Contains("Naturaleza") ? row["Naturaleza"].ToString() : string.Empty,
                Tipo = row.Table.Columns.Contains("Tipo") ? row["Tipo"].ToString() : string.Empty,
                RazonSocial = row.Table.Columns.Contains("RazonSocial") ? row["RazonSocial"].ToString() : string.Empty,
                EstablecimientoComercial = row.Table.Columns.Contains("EstablecimientoComercial") ? row["EstablecimientoComercial"].ToString() : string.Empty,
                NombreComercial = row.Table.Columns.Contains("NombreComercial") ? row["NombreComercial"].ToString() : string.Empty,
                TipoDocumento = row.Table.Columns.Contains("TipoDocumento") ? row["TipoDocumento"].ToString() : string.Empty,
                NumeroDocumento = row.Table.Columns.Contains("NumeroDocumento") ? row["NumeroDocumento"].ToString() : string.Empty,
                Telefono1 = row.Table.Columns.Contains("Telefono1") ? row["Telefono1"].ToString() : string.Empty,
                Telefono2 = row.Table.Columns.Contains("Telefono2") ? row["Telefono2"].ToString() : string.Empty,
                CorreoPrincipal = row.Table.Columns.Contains("CorreoPrincipal") ? row["CorreoPrincipal"].ToString() : string.Empty,
                Correo2 = row.Table.Columns.Contains("Correo2") ? row["Correo2"].ToString() : string.Empty,
                Direccion = row.Table.Columns.Contains("Direccion") ? row["Direccion"].ToString() : string.Empty,

                // 2. Ubicación
                Origen = row.Table.Columns.Contains("Origen") ? row["Origen"].ToString() : "Nacional",
                Pais = row.Table.Columns.Contains("Pais") ? row["Pais"].ToString() : string.Empty,
                Departamento = row.Table.Columns.Contains("Departamento") ? row["Departamento"].ToString() : string.Empty,
                Ciudad = row.Table.Columns.Contains("Ciudad") ? row["Ciudad"].ToString() : string.Empty,

                // 3. Cobros (Tarifa Única)
                TipoCobro = row.Table.Columns.Contains("TipoCobro") ? row["TipoCobro"].ToString() : string.Empty,
                CategoriaTarifa = row.Table.Columns.Contains("CategoriaTarifa") ? row["CategoriaTarifa"].ToString() : string.Empty,
                TarifaAdministrativa = row.Table.Columns.Contains("TarifaAdministrativa") && row["TarifaAdministrativa"] != DBNull.Value
                                       ? Convert.ToDecimal(row["TarifaAdministrativa"]) : 0m,
                Descuentos = row.Table.Columns.Contains("Descuentos") && row["Descuentos"] != DBNull.Value
                             ? Convert.ToDecimal(row["Descuentos"]) : 0m,

                // 4. Datos Fiscales y Bancarios
                ActividadEconomica = row.Table.Columns.Contains("ActividadEconomica") ? row["ActividadEconomica"].ToString() : string.Empty,
                Industria = row.Table.Columns.Contains("Industria") ? row["Industria"].ToString() : string.Empty,
                RutaCamaraComercio = row.Table.Columns.Contains("RutaCamaraComercio") ? row["RutaCamaraComercio"].ToString() : string.Empty,
                RutaRut = row.Table.Columns.Contains("RutaRut") ? row["RutaRut"].ToString() : string.Empty,
                RutaCertificadoBancario = row.Table.Columns.Contains("RutaCertificadoBancario") ? row["RutaCertificadoBancario"].ToString() : string.Empty
            };

            if (Request.IsAjaxRequest())
            {
                return PartialView("Perfil", model);
            }

            return View(model);
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult Proveedores(FormCollection collection)
        {
            try
            {
                var result = _inspectionsBo.GetlistProveedores();

                // Alias de columnas para que la vista las reconozca (si el SP las trae con otro nombre)
                if (result != null)
                {
                    foreach (var alias in new[] { new[] { "CodigoFicha", "FichaCodigo" }, new[] { "EstadoEtiqueta", "Estado" } })
                    {
                        if (result.Columns.Contains(alias[0]) && !result.Columns.Contains(alias[1]))
                        {
                            result.Columns.Add(alias[1], typeof(object));
                            foreach (DataRow r in result.Rows) r[alias[1]] = r[alias[0]];
                        }
                    }
                }

                return PartialView(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


    }
}