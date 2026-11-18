using LogicBo;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Filters;
using WebApplication1.Models;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    [Access]
    public class EmpleadoController : Controller
    {
        InspectionsBo _inspectionsBo = new InspectionsBo();


        [AcceptVerbs(HttpVerbs.Get | HttpVerbs.Post)]
        public PartialViewResult Empleados(FormCollection collection)
        {
            try
            {
                var result = _inspectionsBo.GetlistEmpleados();

                // Agrega la columna FichaCodigo resolviendo fichaid contra el catálogo de fichas (si el SP no la trae)
                if (result != null && !result.Columns.Contains("FichaCodigo") && result.Columns.Contains("fichaid"))
                {
                    var fichas = new Dictionary<int, string>();
                    DataTable dtFichas = _inspectionsBo.GetFichasActivas();
                    if (dtFichas != null)
                    {
                        foreach (DataRow rf in dtFichas.Rows)
                        {
                            fichas[Convert.ToInt32(rf["Id"])] = rf["Codigo"] + " | " + rf["Nombre"];
                        }
                    }

                    result.Columns.Add("FichaCodigo", typeof(string));
                    foreach (DataRow r in result.Rows)
                    {
                        int fid;
                        string txt;
                        r["FichaCodigo"] = r["fichaid"] != DBNull.Value && int.TryParse(r["fichaid"].ToString(), out fid) && fichas.TryGetValue(fid, out txt)
                            ? txt : string.Empty;
                    }
                }

                return PartialView(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public ActionResult Perfil(int id, int? fichaId = null)
        {
            if (id <= 0)
            {
                return RedirectToAction("Empleados");
            }

            var model = new EmpleadoPerfilViewModel();

            try
            {

                // Ejecución que devuelve un DataSet con 5 tablas
                DataSet ds = _inspectionsBo.ObtenerPerfilEmpleadoDataSet(id, fichaId);

                if (ds == null || ds.Tables.Count == 0 || ds.Tables[0].Rows.Count == 0)
                {
                    return HttpNotFound("No se encontró el registro del empleado solicitado.");
                }

                // 1. TABLA 0: Datos Generales e Identificación
                DataRow rEmp = ds.Tables[0].Rows[0];
                model.Id = Convert.ToInt32(rEmp["Id"]);
                model.FichaCodigo = rEmp["FichaCodigo"] != DBNull.Value ? rEmp["FichaCodigo"].ToString() : "F1";
                model.FichaIdActual = rEmp["FichaIdActual"] != DBNull.Value ? Convert.ToInt32(rEmp["FichaIdActual"]) : 0;
                model.EtiquetaEstado = rEmp["EtiquetaEstado"] != DBNull.Value ? rEmp["EtiquetaEstado"].ToString() : "Activo";
                model.FotoUrl = rEmp["FotoUrl"] != DBNull.Value ? rEmp["FotoUrl"].ToString() : string.Empty;
                model.FotoUrl = ObtenerFotoEmpleado(id) ?? model.FotoUrl ?? string.Empty;
                model.NombreCompleto = rEmp["NombreCompleto"] != DBNull.Value ? rEmp["NombreCompleto"].ToString() : string.Empty;
                model.TipoIdentificacion = rEmp["TipoIdentificacion"] != DBNull.Value ? rEmp["TipoIdentificacion"].ToString() : string.Empty;
                model.NumeroIdentificacion = rEmp["NumeroIdentificacion"] != DBNull.Value ? rEmp["NumeroIdentificacion"].ToString() : string.Empty;
                model.CorreoCorporativo = rEmp["CorreoCorporativo"] != DBNull.Value ? rEmp["CorreoCorporativo"].ToString() : string.Empty;
                model.TelefonoParticular = rEmp["TelefonoParticular"] != DBNull.Value ? rEmp["TelefonoParticular"].ToString() : string.Empty;
                model.TelefonoOficina = rEmp["TelefonoOficina"] != DBNull.Value ? rEmp["TelefonoOficina"].ToString() : string.Empty;
                model.FechaNacimiento = rEmp["FechaNacimiento"] != DBNull.Value ? Convert.ToDateTime(rEmp["FechaNacimiento"]) : DateTime.MinValue;
                model.Edad = rEmp["Edad"] != DBNull.Value ? Convert.ToInt32(rEmp["Edad"]) : 0;
                model.Direccion = rEmp["Direccion"] != DBNull.Value ? rEmp["Direccion"].ToString() : string.Empty;
                model.FechaIngreso = rEmp["FechaIngreso"] != DBNull.Value ? Convert.ToDateTime(rEmp["FechaIngreso"]) : DateTime.MinValue;
                model.AntiguedadTexto = rEmp["AntiguedadTexto"] != DBNull.Value ? rEmp["AntiguedadTexto"].ToString() : string.Empty;
                model.EpsPrevision = rEmp["EpsPrevision"] != DBNull.Value ? rEmp["EpsPrevision"].ToString() : string.Empty;
                model.FondoPensiones = rEmp["FondoPensiones"] != DBNull.Value ? rEmp["FondoPensiones"].ToString() : string.Empty;
                model.Cargo = rEmp["Cargo"] != DBNull.Value ? rEmp["Cargo"].ToString() : string.Empty;
                model.Area = rEmp["Area"] != DBNull.Value ? rEmp["Area"].ToString() : string.Empty;
                model.Division = rEmp["Division"] != DBNull.Value ? rEmp["Division"].ToString() : string.Empty;
                model.Empresa = rEmp["Empresa"] != DBNull.Value ? rEmp["Empresa"].ToString() : string.Empty;
                model.CentroCostos = rEmp["CentroCostos"] != DBNull.Value ? rEmp["CentroCostos"].ToString() : string.Empty;
                model.Supervisor = rEmp["Supervisor"] != DBNull.Value ? rEmp["Supervisor"].ToString() : string.Empty;
                model.Suplente = rEmp["Suplente"] != DBNull.Value ? rEmp["Suplente"].ToString() : string.Empty;
                model.TipoContrato = rEmp["TipoContrato"] != DBNull.Value ? rEmp["TipoContrato"].ToString() : string.Empty;
                model.JornadaLaboral = rEmp["JornadaLaboral"] != DBNull.Value ? rEmp["JornadaLaboral"].ToString() : string.Empty;
                model.SaldoVacacionesDias = rEmp["SaldoVacacionesDias"] != DBNull.Value ? Convert.ToDecimal(rEmp["SaldoVacacionesDias"]) : 0.0m;

                // 2. TABLA 1: Fichas asignadas al empleado (Selector multi-ficha)
                if (ds.Tables.Count > 1)
                {
                    foreach (DataRow r in ds.Tables[1].Rows)
                    {
                        model.FichasDisponibles.Add(new FichaItemViewModel
                        {
                            FichaId = Convert.ToInt32(r["FichaId"]),
                            FichaCodigo = r["FichaCodigo"].ToString(),
                            FichaNombre = r["FichaNombre"].ToString(),
                            EsPrincipal = r["EsPrincipal"] != DBNull.Value && Convert.ToBoolean(r["EsPrincipal"])
                        });
                    }
                }

                // 3. TABLA 2: Histórico de Comprobantes de Pago
                if (ds.Tables.Count > 2)
                {
                    foreach (DataRow r in ds.Tables[2].Rows)
                    {
                        model.Comprobantes.Add(new Models.ComprobanteItemViewModel
                        {
                            Id = Convert.ToInt32(r["Id"]),
                            PeriodoMes = r["PeriodoMes"].ToString(),
                            SueldoBruto = r["SueldoBruto"] != DBNull.Value ? Convert.ToDecimal(r["SueldoBruto"]) : 0m,
                            TotalNeto = r["TotalNeto"] != DBNull.Value ? Convert.ToDecimal(r["TotalNeto"]) : 0m,
                            AportePatronal = r["AportePatronal"] != DBNull.Value ? Convert.ToDecimal(r["AportePatronal"]) : 0m,
                            HorasExtras = r["HorasExtras"] != DBNull.Value ? Convert.ToDecimal(r["HorasExtras"]) : 0m,
                            HorasRecargos = r["HorasRecargos"] != DBNull.Value ? Convert.ToDecimal(r["HorasRecargos"]) : 0m,
                            HorasNoTrabajadas = r["HorasNoTrabajadas"] != DBNull.Value ? Convert.ToDecimal(r["HorasNoTrabajadas"]) : 0m,
                            DiasNoTrabajados = r["DiasNoTrabajados"] != DBNull.Value ? Convert.ToDecimal(r["DiasNoTrabajados"]) : 0m,
                            RutaArchivoPdf = r["RutaArchivoPdf"] != DBNull.Value ? r["RutaArchivoPdf"].ToString() : string.Empty
                        });
                    }
                }

                // 4. TABLA 3: Documentos del Expediente
                if (ds.Tables.Count > 3)
                {
                    foreach (DataRow r in ds.Tables[3].Rows)
                    {
                        model.Documentos.Add(new DocumentoItemViewModel
                        {
                            Id = Convert.ToInt32(r["Id"]),
                            NombreDocumento = r["NombreDocumento"].ToString(),
                            Categoria = r["Categoria"] != DBNull.Value ? r["Categoria"].ToString() : "General",
                            RutaArchivo = r["RutaArchivo"] != DBNull.Value ? r["RutaArchivo"].ToString() : string.Empty,
                            FechaCarga = r["FechaCarga"] != DBNull.Value ? Convert.ToDateTime(r["FechaCarga"]) : DateTime.MinValue
                        });
                    }
                }

                // 5. TABLA 4: Historial de Vacaciones
                if (ds.Tables.Count > 4)
                {
                    foreach (DataRow r in ds.Tables[4].Rows)
                    {
                        model.Vacaciones.Add(new VacacionItemViewModel
                        {
                            Id = Convert.ToInt32(r["Id"]),
                            PeriodoCalculo = r["PeriodoCalculo"].ToString(),
                            DiasTomados = r["DiasTomados"] != DBNull.Value ? Convert.ToDecimal(r["DiasTomados"]) : 0m,
                            FechaInicio = r["FechaInicio"] != DBNull.Value ? Convert.ToDateTime(r["FechaInicio"]) : DateTime.MinValue,
                            FechaFin = r["FechaFin"] != DBNull.Value ? Convert.ToDateTime(r["FechaFin"]) : DateTime.MinValue,
                            FechaRetorno = r["FechaRetorno"] != DBNull.Value ? Convert.ToDateTime(r["FechaRetorno"]) : DateTime.MinValue,
                            Estado = r["Estado"] != DBNull.Value ? r["Estado"].ToString() : "Aprobado",
                            Observaciones = r["Observaciones"] != DBNull.Value ? r["Observaciones"].ToString() : string.Empty
                        });
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = "Ocurrió un error al cargar la información del perfil: " + ex.Message;
            }

            // Retorno AJAX para inyectar en #facadeContent sin layout maestro
            if (Request.IsAjaxRequest())
            {
                return PartialView("Perfil", model);
            }

            return View(model);
        }


        // POST: Empleado/UploadAvatar
        private static readonly string[] ExtensionesFoto = { ".png", ".jpg", ".jpeg" };

        /// <summary>
        /// Devuelve la URL de la foto del empleado (Uploads/Avatares/emp_{id}.ext) o null si no tiene.
        /// </summary>
        private string ObtenerFotoEmpleado(int id)
        {
            if (id <= 0) return null;
            string carpeta = Server.MapPath("~/Uploads/Avatares/");
            foreach (var ext in ExtensionesFoto)
            {
                string ruta = Path.Combine(carpeta, "emp_" + id + ext);
                if (System.IO.File.Exists(ruta))
                {
                    // El parámetro v evita que el navegador muestre una foto anterior en caché
                    return "/Uploads/Avatares/emp_" + id + ext + "?v=" + System.IO.File.GetLastWriteTimeUtc(ruta).Ticks;
                }
            }
            return null;
        }

        /// <summary>
        /// Guarda un archivo como foto definitiva del empleado, reemplazando la anterior.
        /// </summary>
        private string GuardarFotoEmpleado(int id, string rutaOrigenFisica, string ext)
        {
            string carpeta = Server.MapPath("~/Uploads/Avatares/");
            foreach (var e in ExtensionesFoto)
            {
                string anterior = Path.Combine(carpeta, "emp_" + id + e);
                if (System.IO.File.Exists(anterior)) System.IO.File.Delete(anterior);
            }
            string destino = Path.Combine(carpeta, "emp_" + id + ext.ToLowerInvariant());
            System.IO.File.Copy(rutaOrigenFisica, destino, true);
            return ObtenerFotoEmpleado(id);
        }

        /// <summary>
        /// Si se cargó una foto antes de guardar el empleado (alta), la asocia al Id recién creado.
        /// </summary>
        private void AsignarFotoTemporal(int empleadoId)
        {
            string temporal = Session["EmpleadoFotoUrl"] as string;
            if (string.IsNullOrEmpty(temporal)) return;

            string fisica = Server.MapPath("~" + temporal);
            if (System.IO.File.Exists(fisica))
            {
                GuardarFotoEmpleado(empleadoId, fisica, Path.GetExtension(fisica));
                System.IO.File.Delete(fisica);
            }
            Session.Remove("EmpleadoFotoUrl");
        }

        [HttpPost]
        [Escritura("Empleado", "Empleados")]
        public JsonResult UploadAvatar(int id = 0)
        {
            try
            {
                if (Request.Files.Count == 0)
                    return Json(new { result = false, message = "No se ha seleccionado ninguna foto." });

                HttpPostedFileBase file = Request.Files[0];
                if (file == null || file.ContentLength == 0)
                    return Json(new { result = false, message = "El archivo se encuentra vacío." });

                string ext = Path.GetExtension(file.FileName).ToUpperInvariant();
                if (ext != ".PNG" && ext != ".JPG" && ext != ".JPEG")
                    return Json(new { result = false, message = "Solo se permiten imágenes JPG o PNG." });

                string folderPath = Server.MapPath("~/Uploads/Avatares/");
                if (!Directory.Exists(folderPath))
                    Directory.CreateDirectory(folderPath);

                string uniqueFileName = $"avatar_{Guid.NewGuid()}{ext}";
                string fullPath = Path.Combine(folderPath, uniqueFileName);
                file.SaveAs(fullPath);

                // Empleado existente: queda asociada de inmediato. Alta: queda temporal hasta Guardar.
                if (id > 0)
                {
                    string url = GuardarFotoEmpleado(id, fullPath, ext);
                    System.IO.File.Delete(fullPath);
                    return Json(new { result = true, url });
                }

                Session["EmpleadoFotoUrl"] = "/Uploads/Avatares/" + uniqueFileName;
                return Json(new { result = true, url = "/Uploads/Avatares/" + uniqueFileName });
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message });
            }
        }
        /// <summary>
        /// POST: /Empleado/Guardar
        /// Inserta o actualiza el empleado y sincroniza sus fichas en dbo.EmpleadoFichas.
        /// </summary>
        [HttpPost]
        [Escritura("Empleado", "Empleados")]
        [ValidateAntiForgeryToken]
        public JsonResult Guardar(EmpleadoFormViewModel form)
        {
            try
            {
                // Validación básica de campos requeridos
                if (string.IsNullOrWhiteSpace(form.Nombres) ||
                    string.IsNullOrWhiteSpace(form.Apellidos) ||
                    string.IsNullOrWhiteSpace(form.NumeroIdentificacion) ||
                    string.IsNullOrWhiteSpace(form.CorreoCorporativo) ||
                    string.IsNullOrWhiteSpace(form.Cargo) ||
                    string.IsNullOrWhiteSpace(form.Area))
                {
                    return Json(new { success = false, message = "Complete todos los campos obligatorios marcados con (*)." });
                }

                if (form.FichasSeleccionadas == null || form.FichasSeleccionadas.Count == 0)
                {
                    return Json(new { success = false, message = "Debe asignar al menos una Ficha al empleado." });
                }

                // Convertir la lista de IDs de fichas a cadena separada por comas (CSV)
                string fichasCsv = string.Join(",", form.FichasSeleccionadas);

                var parametros = new List<SqlParameter>
                {
                    new SqlParameter("@Id", form.Id),
                    new SqlParameter("@Nombres", form.Nombres.Trim()),
                    new SqlParameter("@Apellidos", form.Apellidos.Trim()),
                    new SqlParameter("@TipoIdentificacion", form.TipoIdentificacion),
                    new SqlParameter("@NumeroIdentificacion", form.NumeroIdentificacion.Trim()),
                    new SqlParameter("@CorreoCorporativo", form.CorreoCorporativo.Trim()),
                    new SqlParameter("@TelefonoParticular", (object)form.TelefonoParticular?.Trim() ?? DBNull.Value),
                    new SqlParameter("@TelefonoOficina", (object)form.TelefonoOficina?.Trim() ?? DBNull.Value),
                    new SqlParameter("@FechaNacimiento", form.FechaNacimiento),
                    new SqlParameter("@Direccion", (object)form.Direccion?.Trim() ?? DBNull.Value),
                    new SqlParameter("@FechaIngreso", form.FechaIngreso),
                    new SqlParameter("@Cargo", form.Cargo.Trim()),
                    new SqlParameter("@Area", form.Area.Trim()),
                    new SqlParameter("@Empresa", (object)form.Empresa?.Trim() ?? DBNull.Value),
                    new SqlParameter("@CentroCostos", (object)form.CentroCostos?.Trim() ?? DBNull.Value),
                    new SqlParameter("@Supervisor", (object)form.Supervisor?.Trim() ?? DBNull.Value),
                    new SqlParameter("@Suplente", (object)form.Suplente?.Trim() ?? DBNull.Value),
                    new SqlParameter("@TipoContrato", (object)form.TipoContrato?.Trim() ?? DBNull.Value),
                    new SqlParameter("@JornadaLaboral", (object)form.JornadaLaboral?.Trim() ?? DBNull.Value),
                    new SqlParameter("@EpsPrevisionId", (object)form.EpsPrevisionId ?? DBNull.Value),
                    new SqlParameter("@FondoPensiones", (object)form.FondoPensiones?.Trim() ?? DBNull.Value),
                    new SqlParameter("@FichasIdsCsv", fichasCsv)
                };

                // Ejecuta sp_GuardarEmpleado que inserta/actualiza y retorna el Id resultante
                object resId = _inspectionsBo.ExecuteScalar("dbo.sp_GuardarEmpleado", parametros);
                int empleadoId = Convert.ToInt32(resId);

                // sp_GuardarEmpleado no recibe el estado: se actualiza aparte
                string estado = string.Equals(form.EtiquetaEstado, "Inactivo", StringComparison.OrdinalIgnoreCase) ? "Inactivo" : "Activo";
                _inspectionsBo.ExecuteScalar("UPDATE dbo.Empleados SET EtiquetaEstado = @Estado WHERE Id = @Id",
                    new List<SqlParameter> { new SqlParameter("@Estado", estado), new SqlParameter("@Id", empleadoId) },
                    CommandType.Text);

                _inspectionsBo.GuardarCodigoPostal("Empleados", empleadoId, form.CodigoPostal);

                AsignarFotoTemporal(empleadoId);

                return Json(new { success = true, id = empleadoId });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error al procesar el guardado: " + ex.Message });
            }
        }
        // 1. Guardar Comprobante de Pago + Archivo PDF
        [HttpPost]
        [Escritura("Empleado", "Empleados")]
        [ValidateAntiForgeryToken]
        public JsonResult GuardarComprobante(ComprobanteFormViewModel form)
        {
            try
            {
                string rutaRelativaPdf = string.Empty;

                // Subida física de archivo PDF si fue adjuntado
                if (form.ArchivoPdf != null && form.ArchivoPdf.ContentLength > 0)
                {
                    string carpetaDestino = Server.MapPath("~/Content/Uploads/Comprobantes/" + form.EmpleadoId);
                    if (!Directory.Exists(carpetaDestino)) Directory.CreateDirectory(carpetaDestino);

                    string nombreArchivo = "Comprobante_" + form.PeriodoMes + "_" + Guid.NewGuid().ToString().Substring(0, 8) + ".pdf";
                    string rutaFisica = Path.Combine(carpetaDestino, nombreArchivo);
                    form.ArchivoPdf.SaveAs(rutaFisica);
                    rutaRelativaPdf = "/Content/Uploads/Comprobantes/" + form.EmpleadoId + "/" + nombreArchivo;
                }

                var p = new List<SqlParameter>
        {
            new SqlParameter("@EmpleadoId", form.EmpleadoId),
            new SqlParameter("@FichaId", (object)form.FichaId ?? DBNull.Value),
            new SqlParameter("@PeriodoMes", form.PeriodoMes),
            new SqlParameter("@SueldoBruto", form.SueldoBruto),
            new SqlParameter("@TotalNeto", form.TotalNeto),
            new SqlParameter("@AportePatronal", form.AportePatronal),
            new SqlParameter("@HorasExtras", form.HorasExtras),
            new SqlParameter("@HorasRecargos", form.HorasRecargos),
            new SqlParameter("@HorasNoTrabajadas", form.HorasNoTrabajadas),
            new SqlParameter("@DiasNoTrabajados", form.DiasNoTrabajados),
            new SqlParameter("@RutaArchivoPdf", (object)rutaRelativaPdf ?? DBNull.Value)
        };

                _inspectionsBo.ExecuteScalar("dbo.sp_GuardarComprobante", p);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // 2. Guardar Documento Digital
        [HttpPost]
        [Escritura("Empleado", "Empleados")]
        [ValidateAntiForgeryToken]
        public JsonResult GuardarDocumento(DocumentoFormViewModel form)
        {
            try
            {
                if (form.Archivo == null || form.Archivo.ContentLength == 0)
                {
                    return Json(new { success = false, message = "Debe seleccionar un archivo válido." });
                }

                string carpetaDestino = Server.MapPath("~/Content/Uploads/Expedientes/" + form.EmpleadoId);
                if (!Directory.Exists(carpetaDestino)) Directory.CreateDirectory(carpetaDestino);

                string extension = Path.GetExtension(form.Archivo.FileName);
                string nombreArchivo = "Doc_" + Guid.NewGuid().ToString().Substring(0, 8) + extension;
                string rutaFisica = Path.Combine(carpetaDestino, nombreArchivo);
                form.Archivo.SaveAs(rutaFisica);

                string rutaRelativa = "/Content/Uploads/Expedientes/" + form.EmpleadoId + "/" + nombreArchivo;

                var p = new List<SqlParameter>
        {
            new SqlParameter("@EmpleadoId", form.EmpleadoId),
            new SqlParameter("@NombreDocumento", form.NombreDocumento),
            new SqlParameter("@Categoria", form.Categoria),
            new SqlParameter("@RutaArchivo", rutaRelativa)
        };

                _inspectionsBo.ExecuteScalar("dbo.sp_GuardarDocumento", p);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

        // 3. Registrar Vacaciones
        [HttpPost]
        [Escritura("Empleado", "Empleados")]
        [ValidateAntiForgeryToken]
        public JsonResult GuardarVacaciones(VacacionFormViewModel form)
        {
            try
            {
                var p = new List<SqlParameter>
        {
            new SqlParameter("@EmpleadoId", form.EmpleadoId),
            new SqlParameter("@PeriodoCalculo", form.PeriodoCalculo),
            new SqlParameter("@DiasTomados", form.DiasTomados),
            new SqlParameter("@FechaInicio", form.FechaInicio),
            new SqlParameter("@FechaFin", form.FechaFin),
            new SqlParameter("@FechaRetorno", form.FechaRetorno),
            new SqlParameter("@Observaciones", (object)form.Observaciones ?? DBNull.Value)
        };

                _inspectionsBo.ExecuteScalar("dbo.sp_GuardarVacaciones", p);
                return Json(new { success = true });
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = ex.Message });
            }
        }

           
        [EscrituraVista("Empleado", "Empleados")]
        public ActionResult Registrar(int id = 0, string tab = null)
        {
            var model = new EmpleadoFormViewModel();
            if (!string.IsNullOrWhiteSpace(tab)) model.TabActiva = tab;
            Session.Remove("EmpleadoFotoUrl"); // descarta fotos temporales de altas abandonadas

            try
            {
                // 1. Cargar catálogo de EPS
                DataTable dtEps = _inspectionsBo.ExecuteDataTable("SELECT Id, Nombre FROM dbo.EpsPrevisiones ORDER BY Nombre ASC", null);
                if (dtEps != null)
                {
                    foreach (DataRow re in dtEps.Rows)
                    {
                        model.EpsCatalogo.Add(new CatalogoItem
                        {
                            Id = Convert.ToInt32(re["Id"]),
                            Nombre = re["Nombre"].ToString()
                        });
                    }
                }

                // 1.1 Cargar tipos de documento (SP_LOADTiposDocumento)
                DataTable dtTiposDoc = _inspectionsBo.CargarTiposDocumento();
                if (dtTiposDoc != null)
                {
                    foreach (DataRow rt in dtTiposDoc.Rows)
                    {
                        model.TiposDocumentoCatalogo.Add(new CatalogoItem
                        {
                            Id = Convert.ToInt32(rt["Id"]),
                            Nombre = rt["Nombre"].ToString()
                        });
                    }
                }

                // 2. Obtener fichas activas
                DataTable dtFichas = _inspectionsBo.GetFichasActivas();
                if (dtFichas != null)
                {
                    foreach (DataRow rf in dtFichas.Rows)
                    {
                        model.FichasCatalogo.Add(new FichaItemViewModel
                        {
                            FichaId = Convert.ToInt32(rf["Id"]),
                            FichaCodigo = rf["Codigo"].ToString(),
                            FichaNombre = rf["Nombre"].ToString()
                        });
                    }
                }

                // 3. Si es edición (id > 0)
                if (id > 0)
                {
                    DataTable dtEmp = _inspectionsBo.ObtenerPerfilEmpleado(id);

                    if (dtEmp != null && dtEmp.Rows.Count > 0)
                    {
                        DataRow r = dtEmp.Rows[0];
                        model.Id = id;
                        model.FotoUrl = ObtenerFotoEmpleado(id);
                        model.Nombres = r.Table.Columns.Contains("Nombres") && r["Nombres"] != DBNull.Value ? r["Nombres"].ToString() : string.Empty;
                        model.Apellidos = r.Table.Columns.Contains("Apellidos") && r["Apellidos"] != DBNull.Value ? r["Apellidos"].ToString() : string.Empty;
                        model.EtiquetaEstado = r.Table.Columns.Contains("EtiquetaEstado") && r["EtiquetaEstado"] != DBNull.Value ? r["EtiquetaEstado"].ToString() : "Activo";
                        model.TipoIdentificacion = r.Table.Columns.Contains("TipoIdentificacion") && r["TipoIdentificacion"] != DBNull.Value ? r["TipoIdentificacion"].ToString() : "Cédula de Ciudadanía";
                        model.NumeroIdentificacion = r.Table.Columns.Contains("NumeroIdentificacion") && r["NumeroIdentificacion"] != DBNull.Value ? r["NumeroIdentificacion"].ToString() : string.Empty;
                        model.CorreoCorporativo = r.Table.Columns.Contains("CorreoCorporativo") && r["CorreoCorporativo"] != DBNull.Value ? r["CorreoCorporativo"].ToString() : string.Empty;
                        model.TelefonoParticular = r.Table.Columns.Contains("TelefonoParticular") && r["TelefonoParticular"] != DBNull.Value ? r["TelefonoParticular"].ToString() : string.Empty;
                        model.TelefonoOficina = r.Table.Columns.Contains("TelefonoOficina") && r["TelefonoOficina"] != DBNull.Value ? r["TelefonoOficina"].ToString() : string.Empty;
                        model.FechaNacimiento = r.Table.Columns.Contains("FechaNacimiento") && r["FechaNacimiento"] != DBNull.Value ? Convert.ToDateTime(r["FechaNacimiento"]) : DateTime.Today.AddYears(-25);
                        model.CodigoPostal = _inspectionsBo.ObtenerCodigoPostal("Empleados", id);
                        model.Direccion = r.Table.Columns.Contains("Direccion") && r["Direccion"] != DBNull.Value ? r["Direccion"].ToString() : string.Empty;
                        model.FechaIngreso = r.Table.Columns.Contains("FechaIngreso") && r["FechaIngreso"] != DBNull.Value ? Convert.ToDateTime(r["FechaIngreso"]) : DateTime.Today;
                        model.Cargo = r.Table.Columns.Contains("Cargo") && r["Cargo"] != DBNull.Value ? r["Cargo"].ToString() : string.Empty;
                        model.Area = r.Table.Columns.Contains("Area") && r["Area"] != DBNull.Value ? r["Area"].ToString() : string.Empty;
                        model.Empresa = r.Table.Columns.Contains("Empresa") && r["Empresa"] != DBNull.Value ? r["Empresa"].ToString() : string.Empty;
                        model.CentroCostos = r.Table.Columns.Contains("CentroCostos") && r["CentroCostos"] != DBNull.Value ? r["CentroCostos"].ToString() : string.Empty;
                        model.Supervisor = r.Table.Columns.Contains("Supervisor") && r["Supervisor"] != DBNull.Value ? r["Supervisor"].ToString() : string.Empty;
                        model.Suplente = r.Table.Columns.Contains("Suplente") && r["Suplente"] != DBNull.Value ? r["Suplente"].ToString() : string.Empty;
                        model.TipoContrato = r.Table.Columns.Contains("TipoContrato") && r["TipoContrato"] != DBNull.Value ? r["TipoContrato"].ToString() : string.Empty;
                        model.JornadaLaboral = r.Table.Columns.Contains("JornadaLaboral") && r["JornadaLaboral"] != DBNull.Value ? r["JornadaLaboral"].ToString() : string.Empty;
                        model.EpsPrevisionId = r.Table.Columns.Contains("EpsPrevisionId") && r["EpsPrevisionId"] != DBNull.Value ? Convert.ToInt32(r["EpsPrevisionId"]) : (int?)null;
                        model.FondoPensiones = r.Table.Columns.Contains("FondoPensiones") && r["FondoPensiones"] != DBNull.Value ? r["FondoPensiones"].ToString() : string.Empty;
                    }

                    DataTable dtAsignadas = _inspectionsBo.ObtenerPerfilEmpleadoFichas(id);
                    if (dtAsignadas != null)
                    {
                        foreach (DataRow ra in dtAsignadas.Rows)
                        {
                            model.FichasSeleccionadas.Add(Convert.ToInt32(ra["FichaId"]));
                        }
                    }

                    // Comprobantes ya registrados (mejor esfuerzo: si el SP no responde, el formulario sigue operando)
                    try
                    {
                        DataTable dtComp = _inspectionsBo.ObtenerComprobantesEmpleadoDataTable(id);
                        if (dtComp != null)
                        {
                            Func<DataRow, string, decimal> dec = (row, col) =>
                                row.Table.Columns.Contains(col) && row[col] != DBNull.Value ? Convert.ToDecimal(row[col]) : 0m;
                            foreach (DataRow rc in dtComp.Rows)
                            {
                                model.Comprobantes.Add(new Models.ComprobanteItemViewModel
                                {
                                    Id = rc.Table.Columns.Contains("Id") && rc["Id"] != DBNull.Value ? Convert.ToInt32(rc["Id"]) : 0,
                                    PeriodoMes = rc.Table.Columns.Contains("PeriodoMes") ? rc["PeriodoMes"].ToString() : string.Empty,
                                    SueldoBruto = dec(rc, "SueldoBruto"),
                                    TotalNeto = dec(rc, "TotalNeto"),
                                    AportePatronal = dec(rc, "AportePatronal"),
                                    RutaArchivoPdf = rc.Table.Columns.Contains("RutaArchivoPdf") && rc["RutaArchivoPdf"] != DBNull.Value ? rc["RutaArchivoPdf"].ToString() : string.Empty
                                });
                            }
                        }
                    }
                    catch (Exception) { /* lista opcional */ }

                    // Documentos (result set 3) y Vacaciones (result set 4) de sp_ConsultarPerfilEmpleado (mejor esfuerzo)
                    try
                    {
                        DataSet dsPerfil = _inspectionsBo.ObtenerPerfilEmpleadoDataSet(id, null);
                        if (dsPerfil != null && dsPerfil.Tables.Count > 3)
                        {
                            foreach (DataRow rd in dsPerfil.Tables[3].Rows)
                            {
                                model.Documentos.Add(new DocumentoItemViewModel
                                {
                                    Id = Convert.ToInt32(rd["Id"]),
                                    NombreDocumento = rd["NombreDocumento"].ToString(),
                                    Categoria = rd["Categoria"] != DBNull.Value ? rd["Categoria"].ToString() : "General",
                                    RutaArchivo = rd["RutaArchivo"] != DBNull.Value ? rd["RutaArchivo"].ToString() : string.Empty,
                                    FechaCarga = rd["FechaCarga"] != DBNull.Value ? Convert.ToDateTime(rd["FechaCarga"]) : DateTime.MinValue
                                });
                            }
                        }
                        if (dsPerfil != null && dsPerfil.Tables.Count > 4)
                        {
                            foreach (DataRow rv in dsPerfil.Tables[4].Rows)
                            {
                                model.Vacaciones.Add(new VacacionItemViewModel
                                {
                                    Id = Convert.ToInt32(rv["Id"]),
                                    PeriodoCalculo = rv["PeriodoCalculo"].ToString(),
                                    DiasTomados = rv["DiasTomados"] != DBNull.Value ? Convert.ToDecimal(rv["DiasTomados"]) : 0m,
                                    FechaInicio = rv["FechaInicio"] != DBNull.Value ? Convert.ToDateTime(rv["FechaInicio"]) : DateTime.MinValue,
                                    FechaFin = rv["FechaFin"] != DBNull.Value ? Convert.ToDateTime(rv["FechaFin"]) : DateTime.MinValue,
                                    FechaRetorno = rv["FechaRetorno"] != DBNull.Value ? Convert.ToDateTime(rv["FechaRetorno"]) : DateTime.MinValue,
                                    Estado = rv["Estado"] != DBNull.Value ? rv["Estado"].ToString() : "Aprobado",
                                    Observaciones = rv["Observaciones"] != DBNull.Value ? rv["Observaciones"].ToString() : string.Empty
                                });
                            }
                        }
                    }
                    catch (Exception) { /* listas opcionales */ }

                    model.FichaIdActual = model.FichasSeleccionadas.Count > 0 ? model.FichasSeleccionadas[0] : (int?)null;
                }
            }
            catch (Exception ex)
            {
                // Si hay error en base de datos, lo devolvemos claramente en vez de dejar la pantalla en blanco
                return Content("<div class='alert alert-danger m-3'><b>Error al cargar el formulario:</b> " + ex.Message + "</div>");
            }

            if (Request.IsAjaxRequest())
            {
                return PartialView("Registrar", model);
            }

            return View(model);
        }

    }
}


