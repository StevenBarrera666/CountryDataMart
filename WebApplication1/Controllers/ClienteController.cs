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
    public class ClienteController : Controller
    {
        InspectionsBo _inspectionsBo = new InspectionsBo();


        [EscrituraVista("Cliente", "Clientes")]
        public ActionResult Registrar(int? id)
        {
            Response.Charset = "utf-8";
            Response.ContentEncoding = System.Text.Encoding.UTF8;

            var model = new ClienteRegistroViewModel();
            string paisNombre = null; // nombre del pais del perfil (dbo.Paises), para ubicarlo en dbo.Countries

            // 1. Carga de datos existentes o inicialización
            if (id.HasValue && id.Value > 0)
            {
                DataTable dt = _inspectionsBo.ObtenerPerfilClienteDataTable(id.Value);
                if (dt != null && dt.Rows.Count > 0)
                {
                    DataRow row = dt.Rows[0];
                    model.Id = id.Value;
                    model.CodigoFicha = row.Table.Columns.Contains("CodigoFicha") && row["CodigoFicha"] != DBNull.Value
                                        ? row["CodigoFicha"].ToString()
                                        : "CLI-" + id.Value;
                    model.EstadoEtiqueta = row.Table.Columns.Contains("EstadoEtiqueta") && row["EstadoEtiqueta"] != DBNull.Value
                                           ? row["EstadoEtiqueta"].ToString()
                                           : "Activo";
                    model.LogoUrl = row.Table.Columns.Contains("LogoUrl") ? row["LogoUrl"].ToString() : null;

                    // Datos Personales
                    model.NaturalezaId = Convert.ToInt32(row["NaturalezaId"]);
                    model.TipoId = Convert.ToInt32(row["TipoId"]);
                    model.RazonSocial = row["RazonSocial"].ToString();
                    model.EstablecimientoComercial = row["EstablecimientoComercial"].ToString();
                    model.NombreComercial = row["NombreComercial"].ToString();
                    model.TipoDocumentoId = Convert.ToInt32(row["TipoDocumentoId"]);
                    model.NumeroDocumento = row["NumeroDocumento"].ToString();
                    model.Telefono1 = row["Telefono1"].ToString();
                    model.Telefono2 = row["Telefono2"].ToString();
                    model.CorreoPrincipal = row["CorreoPrincipal"].ToString();
                    model.Correo2 = row["Correo2"].ToString();
                    model.Direccion = row["Direccion"].ToString();
                    model.CodigoPostal = _inspectionsBo.ObtenerCodigoPostal("Clientes", id.Value);
                    model.DiaCierreFacturacion = _inspectionsBo.ObtenerDiaCierreFacturacion(id.Value);

                    // Ubicación
                    model.Origen = row["Origen"].ToString();
                    model.PaisId = Convert.ToInt32(row["PaisId"]);
                    paisNombre = row.Table.Columns.Contains("Pais") && row["Pais"] != DBNull.Value ? row["Pais"].ToString() : null;
                    model.DepartamentoId = Convert.ToInt32(row["DepartamentoId"]);
                    model.CiudadId = Convert.ToInt32(row["CiudadId"]);

                    // Cobros (Solo el tipo general)
                    model.TipoCobroId = Convert.ToInt32(row["TipoCobroId"]);

                    // Fiscales
                    model.ActividadEconomicaId = Convert.ToInt32(row["ActividadEconomicaId"]);
                    model.IndustriaId = Convert.ToInt32(row["IndustriaId"]);
                    model.RutaCamaraComercio = row["RutaCamaraComercio"].ToString();
                    model.RutaRut = row["RutaRut"].ToString();

                    Session["ClienteLogoUrl"] = model.LogoUrl;
                }

                // Cargar las tarifas dinámicas previamente guardadas para este cliente
                DataTable dtTarifas = _inspectionsBo.ObtenerTarifasClienteDataTable(id.Value);
                if (dtTarifas != null && dtTarifas.Rows.Count > 0)
                {
                    foreach (DataRow rTarifa in dtTarifas.Rows)
                    {
                        model.Tarifas.Add(new ClienteTarifaItemViewModel
                        {
                            CategoriaTarifaId = Convert.ToInt32(rTarifa["CategoriaTarifaId"]),
                            CategoriaTarifaNombre = rTarifa["CategoriaTarifaNombre"].ToString(),
                            TarifaAdministrativa = Convert.ToDecimal(rTarifa["TarifaAdministrativa"]),
                            Descuento = Convert.ToDecimal(rTarifa["Descuento"])
                        });
                    }
                }
            }
            else
            {
                model.CodigoFicha = "CLI-" + DateTime.Now.ToString("yyMMddHHmm");
                model.EstadoEtiqueta = "Activo";
                model.Origen = "Nacional";
                model.PaisId = 1;
                model.DepartamentoId = 1;
                model.CiudadId = 1;
                model.TipoCobroId = 1;
                Session.Remove("ClienteLogoUrl");
            }

            // 2. Catálogos Maestros (SelectLists)
            ViewBag.DdlNaturalezas = new SelectList(new[] {
        new { Id = 1, Nombre = "Jurídica" },
        new { Id = 2, Nombre = "Natural" }
    }, "Id", "Nombre", model.NaturalezaId);

            ViewBag.DdlTiposCliente = new SelectList(new[] {
        new { Id = 1, Nombre = "Cliente Corporativo" },
        new { Id = 2, Nombre = "Cliente Estatal" },
        new { Id = 3, Nombre = "PYME" },
        new { Id = 4, Nombre = "Particular" }
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

            // Lista para el dropdown temporal que agrega tarifas dinámicas
            var categoriasTarifa = new List<object>();
            DataTable dtCategorias = _inspectionsBo.CargarCategoriasTarifa();
            if (dtCategorias != null)
            {
                foreach (DataRow rc in dtCategorias.Rows)
                    categoriasTarifa.Add(new { Id = Convert.ToInt32(rc["Id"]), Nombre = rc["Nombre"].ToString() });
            }
            ViewBag.DdlCategoriasTarifa = new SelectList(categoriasTarifa, "Id", "Nombre");

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
        [Escritura("Cliente", "Clientes")]
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
        [Escritura("Cliente", "Clientes")]
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
        [Escritura("Cliente", "Clientes")]
        public JsonResult GuardarCliente(ClienteRegistroViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json(new { result = false, message = "Por favor verifique los campos obligatorios." });
            }

            if (model.DiaCierreFacturacion.HasValue && (model.DiaCierreFacturacion < 1 || model.DiaCierreFacturacion > 31))
            {
                return Json(new { result = false, message = "La fecha de cierre debe ser un día del mes entre 1 y 31." });
            }

            string logoCargado = Session["ClienteLogoUrl"] as string;
            string logoFinal = !string.IsNullOrEmpty(logoCargado) ? logoCargado : (model.LogoUrl ?? "/Content/Images/company-avatar.png");

            try
            {
                DataTable dtResultado = _inspectionsBo.RegistrarOActualizarCliente(model, logoFinal);

                if (dtResultado != null && dtResultado.Rows.Count > 0)
                {
                    int idGenerado = Convert.ToInt32(dtResultado.Rows[0]["IdGenerado"]);

                    if (idGenerado > 0)
                    {
                        _inspectionsBo.GuardarCodigoPostal("Clientes", idGenerado, model.CodigoPostal);
                        _inspectionsBo.GuardarDiaCierreFacturacion(idGenerado, model.DiaCierreFacturacion);
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
            DataTable dtCliente = _inspectionsBo.ObtenerPerfilClienteDataTable(id);

            if (dtCliente == null || dtCliente.Rows.Count == 0)
            {
                return HttpNotFound("No se encontró la ficha del cliente especificado.");
            }

            DataRow row = dtCliente.Rows[0];

            var model = new ClientePerfilViewModel
            {
                Id = id,
                CodigoFicha = row.Table.Columns.Contains("CodigoFicha") && row["CodigoFicha"] != DBNull.Value
                              ? row["CodigoFicha"].ToString()
                              : "CLI-" + id.ToString(),
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

                // 2. Datos Ubicación
                Origen = row.Table.Columns.Contains("Origen") ? row["Origen"].ToString() : "Nacional",
                Pais = row.Table.Columns.Contains("Pais") ? row["Pais"].ToString() : string.Empty,
                Departamento = row.Table.Columns.Contains("Departamento") ? row["Departamento"].ToString() : string.Empty,
                Ciudad = row.Table.Columns.Contains("Ciudad") ? row["Ciudad"].ToString() : string.Empty,

                // 3. Cobros (General)
                TipoCobro = row.Table.Columns.Contains("TipoCobro") ? row["TipoCobro"].ToString() : string.Empty,
                DiaCierreFacturacion = _inspectionsBo.ObtenerDiaCierreFacturacion(id),

                // 4. Datos Fiscales
                ActividadEconomica = row.Table.Columns.Contains("ActividadEconomica") ? row["ActividadEconomica"].ToString() : string.Empty,
                Industria = row.Table.Columns.Contains("Industria") ? row["Industria"].ToString() : string.Empty,
                RutaCamaraComercio = row.Table.Columns.Contains("RutaCamaraComercio") ? row["RutaCamaraComercio"].ToString() : string.Empty,
                RutaRut = row.Table.Columns.Contains("RutaRut") ? row["RutaRut"].ToString() : string.Empty
            };

            // 5. Cargar lista dinámica de tarifas asociadas
            DataTable dtTarifas = _inspectionsBo.ObtenerTarifasClienteDataTable(id);
            if (dtTarifas != null && dtTarifas.Rows.Count > 0)
            {
                foreach (DataRow rTarifa in dtTarifas.Rows)
                {
                    model.Tarifas.Add(new ClienteTarifaItemViewModel
                    {
                        CategoriaTarifaId = Convert.ToInt32(rTarifa["CategoriaTarifaId"]),
                        CategoriaTarifaNombre = rTarifa["CategoriaTarifaNombre"].ToString(),
                        TarifaAdministrativa = Convert.ToDecimal(rTarifa["TarifaAdministrativa"]),
                        Descuento = Convert.ToDecimal(rTarifa["Descuento"])
                    });
                }
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("Perfil", model);
            }

            return View(model);
        }


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult Clientes(FormCollection collection)
        {
            try
            {
                var result = _inspectionsBo.GetlistClientes();

                // La fecha de cierre no viene en el SP del listado: se consulta de una vez para todos los clientes
                if (result != null && result.Columns.Contains("Id") && !result.Columns.Contains("DiaCierreFacturacion"))
                {
                    DataTable dtCierres = _inspectionsBo.ExecuteDataTable(
                        "SELECT Id, DiaCierreFacturacion FROM dbo.Clientes", null, CommandType.Text);
                    var cierres = new Dictionary<string, object>();
                    if (dtCierres != null)
                        foreach (DataRow rc in dtCierres.Rows) cierres[rc["Id"].ToString()] = rc["DiaCierreFacturacion"];

                    result.Columns.Add("DiaCierreFacturacion", typeof(object));
                    foreach (DataRow r in result.Rows)
                    {
                        object dia;
                        r["DiaCierreFacturacion"] = cierres.TryGetValue(r["Id"].ToString(), out dia) ? dia : DBNull.Value;
                    }
                }

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