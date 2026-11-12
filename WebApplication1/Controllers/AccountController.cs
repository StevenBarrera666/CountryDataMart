// WebApplication1.Controllers.AccountController
using System;
using System.Configuration;
using System.Data;
using System.Net;
using System.Net.Mail;
using System.Web.Mvc;
using System.Web.Security;
using LogicBo;
using WebApplication1.Models;

public class AccountController : Controller
{
    private readonly AccountBo _accountBo = new AccountBo();
    WorkingAtHeightBo _workingAtHeightBo = new WorkingAtHeightBo();
    private int sedecategoriaid;

    private string email;
    private string descripciones;
    private string piedepaginacards;

    [AllowAnonymous]
    public ActionResult Login(string returnUrl)
    {
        base.ViewBag.ReturnUrl = returnUrl;
        base.ViewBag.CountryDictionary = new SelectList(_accountBo.GetDictionary(), "Key", "Value");
        return View();
    }

    public ActionResult LoginFacade(string returnUrl)
    {
        base.ViewBag.ReturnUrl = returnUrl;
        return View();
    }

    public ActionResult LogOff()
    {
        base.Session["SessionUser"] = null;
        FormsAuthentication.SignOut();
        return RedirectToAction("Login");
    }

    
    [HttpPost]
    public JsonResult ValidateLogin(FormCollection collection)
    {
        try
        {
            AccountBo.UserEntityAccount userEntityAccount = _accountBo.ValidUser(collection["nameUser"], collection["password"]);
            if (string.IsNullOrEmpty(userEntityAccount.LoginUser))
            {
                throw new Exception("No se han encontrado datos de este usuario");
            }
            SessionModels sessionModels = base.Session["SessionUser"] as SessionModels;
            base.Session["CountryID"] = 1;
            string pais = _workingAtHeightBo.GetCountryNameByID(1);
            string bandera = _workingAtHeightBo.GetFlagNameByID(1);

            string lang = "spain";
            string mess = string.Empty;
            string Welcome = string.Empty;
            string invite = string.Empty;
            string textopp1 = string.Empty;
            string textopp2 = string.Empty;
            string textopp3 = string.Empty;
            string textopp4 = string.Empty;
            string modulos = string.Empty;
            string subtitulos = string.Empty;            


            if (sessionModels != null)
            {
                sessionModels.IdUser = userEntityAccount.IdUser;
                sessionModels.NameUser = userEntityAccount.NameUser + " " + userEntityAccount.SurNameUser;
                sessionModels.SedeCategoriaId =1;
                sessionModels.LoginUser = userEntityAccount.LoginUser;
                sessionModels.Idioma = lang;
                sessionModels.Inicio = "Si";
                sessionModels.Estado = 0;                
                base.Session["SessionUser"] = sessionModels;
            }
            else
            {
                SessionModels sessionModels2 = new SessionModels
                {
                    IdUser = userEntityAccount.IdUser,
                    NameUser = userEntityAccount.NameUser + " " + userEntityAccount.SurNameUser,
                    LoginUser = userEntityAccount.LoginUser,
                    Idioma = lang,
                    Inicio = "Si",
                    Estado = 0,
                   
                };
                base.Session["SessionUser"] = sessionModels2;
            }
            return Json(new { result = true, message = mess, url = Url.Action("Index", "Home", new { modulo = "WH" }) }, JsonRequestBehavior.AllowGet);
        }
        catch (Exception ex)
        {
            Exception ex2 = ex;
            return Json(new
            {
                result = false,
                message = ex2.Message
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
