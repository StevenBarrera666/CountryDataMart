using LogicBo;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using WebApplication1.Filters;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Access]
    public class HomeController : Controller
    {
        #region Properties
        HomeBo _homeBo = new HomeBo();
        string _pathImge = ConfigurationManager.AppSettings["PathImage"].ToString();
        #endregion
        private int _index = 0;

        public int Index1 { get => _index; set => _index = value; }

        public ActionResult Index(string modulo)
        {
            SessionModels sessionModels = base.Session["SessionUser"] as SessionModels;
            sessionModels.ModuloSeleccionado = modulo;
            return View();
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";
            return PartialView();
        }
        public ActionResult AdministradorP(string modulo)
        {
            SessionModels sessionModels = base.Session["SessionUser"] as SessionModels;
            sessionModels.ModuloSeleccionado = modulo;
            return View();
        }
        public ActionResult AdministradorTraza(string modulo)
        {
            SessionModels sessionModels = base.Session["SessionUser"] as SessionModels;
            sessionModels.ModuloSeleccionado = modulo;
            return View();
        }
    }
}