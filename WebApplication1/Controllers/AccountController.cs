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

    public void SendMail(string Message, DataTable dsContactos)
    {
        try
        {
            string text = ConfigurationManager.AppSettings["servicioalcliente"].ToString();
            string password = ConfigurationManager.AppSettings["Passservicioalcliente"].ToString();
            foreach (DataRow row in dsContactos.Rows)
            {
                MailMessage mailMessage = new MailMessage(new MailAddress(text), new MailAddress(row["Email"].ToString()))
                {
                    Subject = "Alerta de vencimiento de Certificado de aptitud"
                };
                string body = "<br /><font size='4' color='red'><b> ¡Atención! </b></font><br />  <h4><b>" + Message + "</b></h4>";
                mailMessage.IsBodyHtml = true;
                mailMessage.Body = body;
                mailMessage.From = new MailAddress(text);
                if (!string.IsNullOrEmpty(ConfigurationManager.AppSettings["gerentegralmail"].ToString()))
                {
                    MailAddress item = new MailAddress(ConfigurationManager.AppSettings["gerentegralmail"].ToString());
                    mailMessage.CC.Add(item);
                }
                MailAddress item2 = new MailAddress(text);
                mailMessage.CC.Add(item2);
                SmtpClient smtpClient = new SmtpClient();
                smtpClient.Host = ConfigurationManager.AppSettings["fromsmtp"].ToString();
                smtpClient.Port = Convert.ToInt16(ConfigurationManager.AppSettings["PortSmtp"].ToString());
                smtpClient.Credentials = new NetworkCredential(text, password);
                smtpClient.Send(mailMessage);
            }
        }
        catch (Exception)
        {
        }
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
             if (lang == "spain")
            {
                piedepaginacards = "Últimas inspecciones|Hace 6 meses";
                mess = "Proceso completado con éxito";
                Welcome = "BIENVENIDO AL PORTAL PPROTECC 2.0";
                invite = "¡TE INVITAMOS A HACER PARTE DE ESTA EXPERIENCIA!";
                textopp1 = "PPROTECC,  es un software diseñado para garantizar la administración en tiempo real del programa para trabajo en alturas de la organización ENEL,  en este encontraras";
                textopp2 = "diferentes módulos que te guiaran en la gestión del programa, para cada una de las sedes de la organización";
                textopp3 = "PPROTECC fue pensado como herramienta de gestión para la prevención de accidentes de trabajo en alturas, para fortalecer la aplicación de las políticas organizacionales  y";
                textopp4 = "velar por el cumplimiento oportuno de la  legislación y normatividad vigente.";
                modulos = "DataMart|";
                subtitulos = "La mejor forma de administrar un riesgo es conocerlo|¿Es el izaje de cargas el proceso más importante?|Próximamente|Próximamente|Próximamente";
                descripciones = "Trabajo en altura es aquel que se realiza en cualquier lugar donde, si no se han adoptado las precauciones necesarias, una persona puede caer desde una altura que puede provocar lesiones (una caída a través de un tejado frágil, por un foso de ascensor sin protección, por el hueco de una escalera).|Primero, ¿qué es un izaje? Podemos entender lo que es un izaje como una forma de levantar o mover objetos con ayuda de algunos dispositivos, el cual se hace de una forma segura, controlada y bien calculada.|Un espacio confinado o recinto confinado es aquel que dispone de aberturas de entrada reducidas, una ventilación natural desfavorable y no está concebido para permanecer en su interior. Por ello, puede presentar una atmósfera irrespirable y albergar gases, vapores o partículas tóxicas o inflamables.|Se considera riesgo eléctrico cuándo existe una posibilidad de contacto del cuerpo humano con la corriente eléctrica y que puede resultar un peligro para la integridad de las personas.|De forma más concreta se podría definir un extintor como un aparato autónomo, diseñado como un cilindro, que puede ser desplazado por una sola persona y que usando un mecanismo de impulsión bajo presión de un gas o presión mecánica, lanza un agente extintor hacia la base del fuego, para lograr extinguirlo.";
            }


            if (sessionModels != null)
            {
                sessionModels.IdUser = userEntityAccount.IdUser;
                sessionModels.NameUser = userEntityAccount.NameUser + " " + userEntityAccount.SurNameUser;
                sessionModels.SedeCategoriaId =1;
                sessionModels.LoginUser = userEntityAccount.LoginUser;
                sessionModels.Pais = pais;
                sessionModels.Bandera = bandera;
                sessionModels.Welcome = Welcome;
                sessionModels.Invite = invite;
                sessionModels.TextoPro1 = textopp1;
                sessionModels.TextoPro2 = textopp2;
                sessionModels.TextoPro3 = textopp3;
                sessionModels.TextoPro4 = textopp4;
                sessionModels.Idioma = lang;
                sessionModels.Inicio = "Si";
                sessionModels.Estado = 0;
                sessionModels.Modulos = modulos;
                sessionModels.Subtitulos = subtitulos;
                sessionModels.Descripciones = descripciones;
                sessionModels.piedepaginacards = piedepaginacards;
                base.Session["SessionUser"] = sessionModels;
            }
            else
            {
                SessionModels sessionModels2 = new SessionModels
                {
                    IdUser = userEntityAccount.IdUser,
                    NameUser = userEntityAccount.NameUser + " " + userEntityAccount.SurNameUser,
                    LoginUser = userEntityAccount.LoginUser,
                    SedeCategoriaId = 1,
                    RolId = userEntityAccount.RolId,
                    Pais = pais,
                    Bandera = bandera,
                    Welcome = Welcome,
                    Invite = invite,
                    TextoPro1 = textopp1,
                    TextoPro2 = textopp2,
                    TextoPro3 = textopp3,
                    TextoPro4 = textopp4,
                    Idioma = lang,
                    Inicio = "Si",
                    Estado = 0,
                    Modulos = modulos,
                    Subtitulos = subtitulos,
                    Descripciones = descripciones,
                    piedepaginacards = piedepaginacards,
                };
                base.Session["SessionUser"] = sessionModels2;
                sedecategoriaid = 1;
                email = sessionModels2.Email;
                base.Session["CountryID"] = 1;
            }
           
            return Json(new { result = true, message = mess, url = Url.Action("LoginFacade", "Account") }, JsonRequestBehavior.AllowGet);
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
