using Entity;
using iTextSharp.text;
using iTextSharp.text.pdf;
using LogicBo;
using OfficeOpenXml;
using OfficeOpenXml.Drawing;
using OfficeOpenXml.FormulaParsing.Excel.Functions.Math;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Web;
using System.Web.Mvc;
using System.Web.UI.WebControls;
using System.Windows.Media.Media3D;
using Utils;
using WebApplication1.Filters;
using WebApplication1.Models;
using Font = iTextSharp.text.Font;
using Image = System.Drawing.Image;
using Rectangle = iTextSharp.text.Rectangle;

namespace WebApplication1.Controllers
{
    [Access]
    public class WorkingAtHeightController : Controller
    {
        #region Properties
        private readonly AccountBo _accountBo = new AccountBo();
        WorkingAtHeightBo _workingAtHeightBo = new WorkingAtHeightBo();
        InspectionsBo _inspectionsBo = new InspectionsBo();
        HeadquarterBo _headquarterBo = new HeadquarterBo();
        CategoryBo _categoryBo = new CategoryBo();
        ElementBo _elementBo = new ElementBo();
        LocationBo _locationBo = new LocationBo();
        MarkBo _markBo = new MarkBo();
        CurriculumBo _curriculumBo = new CurriculumBo();
        TechnicalInformationBo _technicalInformationBo = new TechnicalInformationBo();
        EquipmentBo _equipmentBo = new EquipmentBo();
        Util util;
        IzageBo _izageBo = new IzageBo();
        #endregion
        public ActionResult Stock()
        {

            int countryID = Convert.ToInt32(base.Session["CountryID"]);
            var model = _workingAtHeightBo.GetStock(countryID);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult StockByHeadquarter(int headquarterId)
        {
            ViewBag.id = headquarterId;
            var model = _workingAtHeightBo.GetStockByHeadquarter(headquarterId);
            return PartialView(model);
        }
      
        [HttpPost]
        public ActionResult StockByHeadquarterMant(int headquarterId)
        {
            ViewBag.id = headquarterId;
            var model = _workingAtHeightBo.GetStockByHeadquarterMant(headquarterId);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult StockByHeadquarterDados(int headquarterId)
        {
            ViewBag.id = headquarterId;
            var model = _workingAtHeightBo.GetStockByHeadquarterDados(headquarterId);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult StockByHeadquarterRechazos(int headquarterId)
        {
            ViewBag.id = headquarterId;
            var model = _workingAtHeightBo.GetStockByHeadquarterRechazos(headquarterId);
            return PartialView(model);
        }
        [HttpPost]
        public ActionResult StockByHeadquarterByStateVig(int headquarterId)
        {
            ViewBag.id = headquarterId;
            var model = _workingAtHeightBo.GetStockByHeadquarterStateVig(headquarterId);
            return PartialView(model);
        }

        [HttpPost]
        public ActionResult StockByHeadquarterByStateVen(int headquarterId)
        {
            ViewBag.id = headquarterId;
            var model = _workingAtHeightBo.GetStockByHeadquarterStateVen(headquarterId);
            return PartialView(model);
        }



        [HttpPost]
        public ActionResult StockByHeadquarterByStatePor(int headquarterId)
        {
            ViewBag.id = headquarterId;
            var model = _workingAtHeightBo.GetStockByHeadquarterStatePor(headquarterId);
            return PartialView(model);
        }







        public ActionResult Curriculum()
        {
            //var model = _workingAtHeightBo.GetStock();
            ViewBag.HeadquarterDictionary = new SelectList(_headquarterBo.GetDictionary(Convert.ToInt32(Session["CountryID"])), "Key", "Value");
            ViewBag.CategoryDictionary = new SelectList(_categoryBo.GetDictionary(), "Key", "Value");
            return PartialView();
        }
        public ActionResult InspectionCert()
        {
            //var model = _workingAtHeightBo.GetStock();
            ViewBag.HeadquarterDictionary = new SelectList(_headquarterBo.GetDictionary(Convert.ToInt32(Session["CountryID"])), "Key", "Value");
            ViewBag.CategoryDictionary = new SelectList(_categoryBo.GetDictionary(), "Key", "Value");
            return PartialView();
        }
        public ActionResult LoadMatrix()
        {
            //var model = _workingAtHeightBo.GetStock();
            ViewBag.HeadquarterDictionary = new SelectList(_headquarterBo.GetDictionary(Convert.ToInt32(Session["CountryID"])), "Key", "Value");
            ViewBag.CategoryDictionary = new SelectList(_categoryBo.GetDictionary(), "Key", "Value");
            return PartialView();
        }
        



        public ActionResult TechnicalInformation()
        {
            ViewBag.HeadquarterDictionary = new SelectList(_headquarterBo.GetDictionary(Convert.ToInt32(Session["CountryID"])), "Key", "Value");
            ViewBag.CategoryDictionary = new SelectList(_categoryBo.GetDictionary(), "Key", "Value");
            return PartialView();
        }
        public ActionResult IndexEquipment()
        {
            var result = _equipmentBo.GetIndex(Convert.ToInt32(Session["CountryID"]));
            return PartialView(result);
        }
        public ActionResult IndexSede()
        {
            var result = _headquarterBo.GetIndex(Convert.ToInt32(Session["CountryID"]));
            return PartialView(result);
        }
        public ActionResult IndexPais()
        {
            var result = _accountBo.GetCountries();
            return PartialView(result);
        }

        public ActionResult IndexSedeCategoria()
        {
            var result = _accountBo.GetCategories();
            return PartialView(result);
        }


        public ActionResult DownloadResult()
        {
            return PartialView();
        }
        public ActionResult CreateEquipment()
        {
            ViewBag.HeadquarterDictionary = new SelectList(_headquarterBo.GetDictionary(Convert.ToInt32(Session["CountryID"])), "Key", "Value");
            ViewBag.ElementDictionary = new SelectList(_elementBo.GetDictionary(), "Key", "Value");
            ViewBag.LocationDictionary = new SelectList(_locationBo.GetDictionary(), "Key", "Value");
            return PartialView();
        }

        public ActionResult CreateSede()
        {
            base.ViewBag.CountryDictionary = new SelectList(_accountBo.GetDictionary(), "Key", "Value");
            base.ViewBag.CategoryDictionary = new SelectList(_accountBo.GetCategoryDictionary(), "Key", "Value");


            return PartialView();
        }

        public ActionResult CreatePais()
        {
            return PartialView();
        }
        public ActionResult CreateCategoriaSede()
        {
            return PartialView();
        }
        public ActionResult CreateEquipmentIzage()
        {
            ViewBag.HeadquarterDictionary = new SelectList(_headquarterBo.GetDictionary(Convert.ToInt32(Session["CountryID"])), "Key", "Value");
            ViewBag.UnidadMedidaDictionary = new SelectList(_izageBo.GetUnidadDictionary(), "Key", "Value");
            ViewBag.ElementDictionary = new SelectList(_elementBo.GetDictionary(), "Key", "Value");
            ViewBag.LocationDictionary = new SelectList(_locationBo.GetDictionary(), "Key", "Value");
            return PartialView();
        }


        #region JsonResult
        [HttpPost]
        public JsonResult GetElementByCategory(int idCategory)
        {
            try
            {
                var result = _elementBo.GetDictionaryByCategory(idCategory);
                return Json(new { result = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        [HttpPost]
        public JsonResult GetMarkByHeadquarterElement(int idHeadquarter, int idElement)
        {
            try
            {
                var result = _markBo.GetDictionaryByHeadquarterElement(idHeadquarter, idElement);
                return Json(new { result = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        [HttpPost]
        public JsonResult ProcessCreateEquipment(FormCollection collection, HttpPostedFileBase file)
        {
            try
            {
                var session = Session["SessionUser"] as SessionModels;
                if (session == null)
                    throw new Exception("Se ha perdido la sesión del Usuario");

                string rFID = collection["rFID"];
                string serial = collection["serial"];
                string model = collection["model"];
                string material = collection["material"];
                DateTime fabricationDate = Convert.ToDateTime(collection["cfabricationDate"]);
                string mark = collection["mark"];
                string lot = collection["lot"];
                int element = collection["cbxElement"].ToString() != string.Empty ? Convert.ToInt32(collection["cbxElement"].ToString()) : 0; ;
                int headquarter = collection["cbxHeadquarter"].ToString() != string.Empty ? Convert.ToInt32(collection["cbxHeadquarter"].ToString()) : 0;
                DateTime purchaseDate = Convert.ToDateTime(collection["cpurchaseDate"]);
                int cbxAssigned = collection["cbxAssigned"].ToString() != string.Empty ? Convert.ToInt32(collection["cbxAssigned"].ToString()) : 0;
                int? areaId = null;
                int? employedId = null;
                int ubicationID = collection["cbxLocation"].ToString() != string.Empty ? Convert.ToInt32(collection["cbxLocation"].ToString()) : 0;

                if (cbxAssigned == 1)
                {
                    areaId = Convert.ToInt32(collection["cbxFieldAssigned"].ToString());
                }
                else if (cbxAssigned == 2)
                {
                    employedId = Convert.ToInt32(collection["cbxFieldAssigned"].ToString());
                }
                int cbxAssignedDate = collection["cbxAssignedDate"].ToString() != string.Empty ? Convert.ToInt32(collection["cbxAssignedDate"].ToString()) : 0;

                var idEquipment = _curriculumBo.Create(rFID, serial, model, material, fabricationDate, mark, lot, element, headquarter, purchaseDate, areaId, employedId, cbxAssignedDate, ubicationID);

                util = new Util();
                util.CreateCurriculum(idEquipment);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }



        [HttpPost]
        public JsonResult ProcessCreatePais(FormCollection collection)
        {
            try
            {
                var session = Session["SessionUser"] as SessionModels;
                if (session == null)
                    throw new Exception("Se ha perdido la sesión del Usuario");

                string pais = collection["pais"];
                var idEquipment = _curriculumBo.CreatePais(pais);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }




        [HttpPost]
        public JsonResult ProcessCreateSede(FormCollection collection)
        {
            try
            {
                var session = Session["SessionUser"] as SessionModels;
                if (session == null)
                    throw new Exception("Se ha perdido la sesión del Usuario");






                string nombreSede = collection["sede"];

                int paisid = collection["cbxCountry"].ToString() != string.Empty ? Convert.ToInt32(collection["cbxCountry"].ToString()) : 0; ;
                int sedecategoriaid = collection["cbxSedeCategoria"].ToString() != string.Empty ? Convert.ToInt32(collection["cbxSedeCategoria"].ToString()) : 0; ;
                string codeENEL = collection["codeENEL"];
                string ubicacion = collection["ubicacion"];

                var idEquipment = _curriculumBo.CreateSede(nombreSede, paisid, sedecategoriaid, ubicacion, codeENEL);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }




        [HttpPost]
        public JsonResult ProcessCreateCategoriaSede(FormCollection collection)
        {
            try
            {
                var session = Session["SessionUser"] as SessionModels;
                if (session == null)
                    throw new Exception("Se ha perdido la sesión del Usuario");

                string categoria = collection["sedecategoria"];
                var idEquipment = _curriculumBo.CreateSedeCategoria(categoria);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }



        //[HttpPost]
        //public JsonResult ProcessCreateEquipmentIzage(FormCollection collection, HttpPostedFileBase file)
        //{
        //    try
        //    {
        //        var session = Session["SessionUser"] as SessionModels;
        //        if (session == null)
        //            throw new Exception("Se ha perdido la sesión del Usuario");
        //        EquipoIzage equipoI=new EquipoIzage();
        //        equipoI.Equipo=collection["nombreEquipo"];
        //        equipoI.Marca = collection["marca"];
        //        equipoI.Modelo = collection["model"];
        //        equipoI.Serial=collection["serial"];
        //        equipoI.Lote = collection["lote"];
        //        equipoI.Tag = collection["rfid"];
        //        equipoI.Maxcapacidad = collection["maxCapacidad"];
        //        equipoI.Unidadcapacidad= collection["cbxUnidad"].ToString() != string.Empty ? Convert.ToInt32(collection["cbxUnidad"].ToString()) : 0;
        //        equipoI.Accesorio = collection["accesorio"];
        //        equipoI.Asignadoa = collection["asignadoa"];
        //        equipoI.FechaCompra=Convert.ToDateTime(collection["cpurchaseDate"]);
        //        equipoI.FechaFabricacion=Convert.ToDateTime(collection["cfabricationDate"]);
        //        equipoI.Ubicacion = collection["cbxUbicacion"].ToString() != string.Empty ? Convert.ToInt32(collection["cbxUbicacion"].ToString()) : 0;
        //        equipoI.Sede= collection["cbxHeadquarter"].ToString() != string.Empty ? Convert.ToInt32(collection["cbxHeadquarter"].ToString()) : 0;
        //        int fechaInspInicial= collection["cbxAssignedDate"].ToString() != string.Empty ? Convert.ToInt32(collection["cbxAssignedDate"].ToString()) : 0;
        //        if(fechaInspInicial==1)
        //            equipoI.FechaInspeccionInicial= Convert.ToDateTime(collection["cpurchaseDate"]);
        //        else if(fechaInspInicial==2)
        //         equipoI.FechaInspeccionInicial= Convert.ToDateTime(collection["cfabricationDate"]);
        //        equipoI.Observaciones = collection["observacion"];

        //        var idEquipment = _izageBo.Create(equipoI);

        //        return Json(new { result = true }, JsonRequestBehavior.AllowGet);
        //    }
        //    catch (Exception ex)
        //    {
        //        return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
        //        throw;
        //    }
        //}



        public ActionResult EditEquipment(string RFID)
        {
            var model = _workingAtHeightBo.GetInfoEquipment(RFID);
            ViewBag.HeadquarterDictionary = new SelectList(_headquarterBo.GetDictionary(Convert.ToInt32(Session["CountryID"])), "Key", "Value", model.Rows[0]["Sedeid"].ToString());
            return PartialView(model);
        }
        [HttpPost, ValidateInput(false)]
        public JsonResult ProcessEditEquipment(FormCollection collection)
        {
            try
            {
                var session = Session["SessionUser"] as SessionModels;
                if (session == null)
                    throw new Exception("Se ha perdido la sesión del Usuario");

                string RFID = collection["RFID"];
                int areaid = int.Parse(collection["areaid"]); ;
                int empleadoid = int.Parse(collection["empleadoid"]);


                _workingAtHeightBo.EditEquipment(RFID, areaid, empleadoid);
                return Json(new { result = true, RFID, areaid, empleadoid }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }


        [HttpPost]
        public JsonResult ProcessUnsuscribe(FormCollection collection)
        {
            try
            {
                var session = Session["SessionUser"] as SessionModels;
                if (session == null)
                    throw new Exception("Se ha perdido la sesión del Usuario");

                string tag = collection["TAG"];
                string comment = collection["comment"];

                _workingAtHeightBo.Unsuscribe(tag, comment);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }


        [HttpPost]
        public JsonResult GetAreaDictionary()
        {
            try
            {
                var result = _workingAtHeightBo.GetAreaDictionary();
                return Json(new { result = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }
        [HttpPost]
        public JsonResult GetResourceByHeadquarter(int idHeadquarter)
        {
            try
            {
                var result = _workingAtHeightBo.GetResourceByHeadquarter(idHeadquarter);
                return Json(new { result = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }
        [HttpPost]
        public JsonResult AssignEquipmentSave(FormCollection collection)
        {
            try
            {
                string tag = collection["tag"];
                int headquarter = collection["sedeid"].ToString() != string.Empty ? Convert.ToInt32(collection["sedeid"].ToString()) : 0;
                int cbxAssigned = collection["cbxAssigned"].ToString() != string.Empty ? Convert.ToInt32(collection["cbxAssigned"].ToString()) : 0;
                int? areaId = null;
                int? employedId = null;
                if (cbxAssigned == 1)
                {
                    areaId = Convert.ToInt32(collection["cbxFieldAssigned"].ToString());
                }
                else if (cbxAssigned == 2)
                {
                    employedId = Convert.ToInt32(collection["cbxFieldAssigned"].ToString());
                }
                var result = _workingAtHeightBo.AssignEquipmentSave(tag, headquarter, areaId, employedId);
                return Json(new { result = true }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }
        #endregion

        [HttpPost]
        public PartialViewResult SearchCurriculum(FormCollection collection)
        {
            try
            {
                string rFID = string.IsNullOrEmpty(collection["rFID"].ToString()) ? null : collection["rFID"].ToString();
                string serial = string.IsNullOrEmpty(collection["serial"].ToString()) ? null : collection["serial"].ToString();
                string precinto = string.IsNullOrEmpty(collection["precinto"].ToString()) ? null : collection["precinto"].ToString();

                int tempcbxHeadquarter;
                int tempcbxElement;
                int? cbxHeadquarter = int.TryParse(collection["cbxHeadquarter"].ToString(), out tempcbxHeadquarter) ? tempcbxHeadquarter : (int?)null;
                int? cbxElement = Int32.TryParse(collection["cbxElement"].ToString(), out tempcbxElement) ? tempcbxElement : (int?)null;

                var result = _curriculumBo.GetListByElement(cbxHeadquarter, cbxElement, rFID,serial,precinto);
                return PartialView(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }
        [HttpPost]
        public JsonResult Upload(HttpPostedFileBase file) // <-- Cambiado a JsonResult
        {
            int totalrowCount = 0;
            if (file != null && file.ContentLength > 0)
            {
                var fileExtension = Path.GetExtension(file.FileName).ToLower();

                if (fileExtension == ".xls" || fileExtension == ".xlsx")
                {
                    try
                    {
                        // Creamos una lista para guardar temporalmente los datos que mostraremos en la grilla
                        var filasProcesadas = new List<object>();
                        using (var stream = file.InputStream)
                        {
                            using (var package = new OfficeOpenXml.ExcelPackage(stream))
                            {
                                var worksheet = package.Workbook.Worksheets.First();
                                int rowCount = worksheet.Dimension.End.Row;
                                totalrowCount = rowCount - 1;
                                for (int row = 2; row <= rowCount; row++)
                                {
                                    var rfid = "";
                                    var SedeID = worksheet.Cells[row, 1].Text;
                                    var Ubicacion = worksheet.Cells[row, 2].Text;
                                    var Serial = worksheet.Cells[row, 4].Text;
                                    var Modelo = worksheet.Cells[row, 5].Text;
                                    var Marca = worksheet.Cells[row, 6].Text;
                                    var Lote = worksheet.Cells[row, 11].Text;
                                    var Elemento = worksheet.Cells[row, 3].Text;
                                    var Tag = worksheet.Cells[row, 10].Text;
                                    var Precinto = worksheet.Cells[row, 12].Text;
                                    var EstadoFinalId = worksheet.Cells[row, 8].Text;
                                    var InspectorID = worksheet.Cells[row, 9].Text;
                                    var FechaInspeccion = worksheet.Cells[row, 7].Text;

                                    if (!String.IsNullOrEmpty(SedeID))
                                    {
                                        CurriculumBo curriculumBo = new CurriculumBo();
                                        int Encabezadoid = curriculumBo.CreateEncabezado(rfid, Serial, Modelo,
                                            Marca, Lote, Elemento, SedeID, Ubicacion, Tag,
                                            Precinto, EstadoFinalId, InspectorID, Convert.ToDateTime(FechaInspeccion));

                                        util = new Util();
                                        util.CreateCurriculum(Encabezadoid);
                                        util.lanzarCertificados(Encabezadoid);

                                        // AGREGADO: Guardamos los datos de esta fila para la grilla
                                        filasProcesadas.Add(new
                                        {
                                            Sede = SedeID,
                                            Ubicacion = Ubicacion,
                                            Serial = Serial,
                                            Elemento = Elemento,
                                            Rfid=rfid
                                        });

                                    }
                                }
                            }
                        }

                        // Retornamos éxito en formato JSON para el JavaScript
                        return Json(new { success = true, mensaje = "Archivo procesado correctamente. Total de Elementos Inspeccionados: " + totalrowCount.ToString() + " para el archivo: " + file.FileName,
                        datos=filasProcesadas
                        });
                        
                    }
                    catch (Exception ex)
                    {
                        return Json(new { success = false, mensaje = "Error al procesar los datos: " + ex.Message });
                    }
                }
                else
                {
                    return Json(new { success = false, mensaje = "Por favor, sube un archivo Excel válido (.xls o .xlsx)." });
                }
            }

            return Json(new { success = false, mensaje = "Por favor, selecciona un archivo." });
        }
        //[HttpPost]
        //public ActionResult Upload(HttpPostedFileBase file)
        //{
        //    if (file != null && file.ContentLength > 0)
        //    {
        //        var fileExtension = Path.GetExtension(file.FileName).ToLower();

        //        if (fileExtension == ".xls" || fileExtension == ".xlsx")
        //        {
        //            using (var stream = file.InputStream)
        //            {
        //                using (var package = new OfficeOpenXml.ExcelPackage(stream))
        //                {
        //                    var worksheet = package.Workbook.Worksheets.First();
        //                    int rowCount = worksheet.Dimension.End.Row;

        //                    for (int row = 2; row <= rowCount; row++) // Asumiendo encabezado en fila 1
        //                    {
        //                        var rfid = "";
        //                        var SedeID = worksheet.Cells[row, 1].Text;
        //                        var Ubicacion = worksheet.Cells[row, 2].Text;
        //                        var Serial = worksheet.Cells[row, 4].Text;
        //                        var Modelo = worksheet.Cells[row, 5].Text;
        //                        var Marca= worksheet.Cells[row, 6].Text;
        //                        var Lote= worksheet.Cells[row,11 ].Text;
        //                        var Elemento = worksheet.Cells[row,3 ].Text;
        //                        var  Tag= worksheet.Cells[row,10].Text;
        //                        var Precinto= worksheet.Cells[row,12 ].Text;
        //                        var EstadoFinalId= worksheet.Cells[row, 8].Text;
        //                        var InspectorID= worksheet.Cells[row, 9].Text;
        //                        var FechaInspeccion= worksheet.Cells[row, 7].Text;
        //                        if(!String.IsNullOrEmpty(SedeID))
        //                        {
        //                            CurriculumBo curriculumBo = new CurriculumBo();
        //                            int Encabezadoid = curriculumBo.CreateEncabezado(rfid, Serial, Modelo,
        //                                Marca,
        //                                Lote, Elemento, SedeID, Ubicacion, Tag,
        //                                Precinto, EstadoFinalId, InspectorID, Convert.ToDateTime(FechaInspeccion));

        //                            util = new Util();
        //                            util.CreateCurriculum(Encabezadoid);
        //                            util.lanzarCertificados(Encabezadoid);
        //                        }
                               
        //                    }

        //                    //db.SaveChanges();
        //                }
        //            }


        //            //ViewBag.Message = "Archivo procesado correctamente.";
        //        }
        //        else
        //        {
        //            ModelState.AddModelError("", "Por favor, sube un archivo Excel válido (.xls o .xlsx).");
        //        }
        //    }
        //    else
        //    {
        //        ModelState.AddModelError("", "Por favor, selecciona un archivo.");
        //    }
        //     return View("LoadMatrix"); ;
        //    //return PartialView("_MenuPartial.cshtml"); ;
        //}

      
        [HttpPost]
        public PartialViewResult SearchTechnicalInformation(FormCollection collection)
        {
            try
            {
                int cbxMark = Int32.Parse(collection["cbxMark"].ToString());

                var result = _technicalInformationBo.GetByMark(cbxMark);
                return PartialView(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        [HttpPost]
        public PartialViewResult LoadTechnicalInformation(FormCollection collection)
        {
            try
            {
                int cbxMark = Int32.Parse(collection["cbxMark"].ToString());

                var result = _technicalInformationBo.GetByMark(cbxMark);
                return PartialView(result);
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        [HttpPost]
        public PartialViewResult AssignEquipment(string tag, int sedeid)
        {
            ViewBag.tag = tag;
            ViewBag.sedeid = sedeid;
            return PartialView();
        }


        [HttpPost]
        public ActionResult UploadFile(FormCollection collection)
        {
            try
            {
                var path = String.Empty;

                foreach (string filetmps in Request.Files)
                {
                    var utlityModels = Session["FilesInspections"] as List<UtilityModels>;
                    HttpPostedFileBase filetmp = Request.Files[filetmps];
                    if (filetmp != null)
                    {
                        var b = new BinaryReader(filetmp.InputStream);
                        int length = Convert.ToInt32(filetmp.InputStream.Length);
                        byte[] binaryData = b.ReadBytes(length);
                        var ext = Path.GetExtension(filetmp.FileName);
                        var name = Path.GetFileNameWithoutExtension(filetmp.FileName);
                        if (utlityModels == null)
                        {
                            var objectSession = new List<UtilityModels>
                            {
                                new UtilityModels{
                                file= binaryData,
                                name=name,
                                extension = ext,
                                type=collection["type"]

                                }

                            };
                            Session["FilesInspections"] = objectSession;
                        }
                        else
                        {
                            utlityModels.Add(new UtilityModels
                            {
                                extension = ext,
                                name = name,
                                file = binaryData,
                                type = collection["type"]
                            });
                            Session["FilesInspections"] = utlityModels;

                        }
                    }
                }

                return Json(new { result = true }, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message }, JsonRequestBehavior.AllowGet);
                throw;
            }

        }



    }
}
