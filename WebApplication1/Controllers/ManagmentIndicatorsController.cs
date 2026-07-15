using LogicBo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Filters;

namespace WebApplication1.Controllers
{
    [Access]
    public class ManagmentIndicatorsController : Controller
    {

        #region Properties
        ManagmentIndicatorsBo _managmentIndicatorsBo = new ManagmentIndicatorsBo();
        HeadquarterBo _headquarterBo = new HeadquarterBo();
        #endregion
        public ActionResult PageInitial()
        {
            ViewBag.HeadquarterTypeDictionary = new SelectList(_headquarterBo.GetTypeHeadQuarterDictionary(Convert.ToInt32(Session["CountryID"])), "Key", "Value");
            ViewBag.HeadquarterDictionary = new SelectList(_headquarterBo.GetDictionary(Convert.ToInt32(Session["CountryID"])), "Key", "Value");
            return PartialView();
        }
        public JsonResult GetStock(int headtypequarterId, int year, int headquarterId)
        {
            try
            {
                var result = _managmentIndicatorsBo.GetStock(headtypequarterId, year, headquarterId);
                return Json(new { result = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }

        public JsonResult GetAims(int headquarterId, int year)
        {
            try
            {
                var result = _managmentIndicatorsBo.GetAims(headquarterId, year);
                return Json(new { result = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }
        public JsonResult GetCoverage(int headquarterId, int year)
        {
            try
            {
                var result = _managmentIndicatorsBo.GetCoverage(headquarterId, year);
                return Json(new { result = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }
        public JsonResult GetAimsTrainning(int headquarterId, int year)
        {
            try
            {
                var result = _managmentIndicatorsBo.GetAimsTrainning(headquarterId, year);
                return Json(new { result = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }
        public JsonResult GetFindingsByMonth(int headquarterId, int year)
        {
            try
            {
                var result = _managmentIndicatorsBo.GetFindingsByMonth(headquarterId, year);
                return Json(new { result = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }
        public JsonResult GetFindingsTotal(int headquarterId, int year)
        {
            try
            {
                var result = _managmentIndicatorsBo.GetFindingsTotal(headquarterId, year);
                return Json(new { result = true, data = result }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { result = false, message = ex.Message.ToString() }, JsonRequestBehavior.AllowGet);
                throw;
            }
        }
    }
}