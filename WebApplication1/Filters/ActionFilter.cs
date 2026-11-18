using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using LogicBo;
using WebApplication1.Models;
namespace WebApplication1.Filters
{

        public static class Permisos
        {
            /// <summary>
            /// True si el usuario solo puede consultar la opcion (controller/action). Sin sesion se asume solo lectura;
            /// una opcion que el usuario no tiene asignada tampoco es editable.
            /// </summary>
            public static bool EsSoloLectura(SessionModels session, string controller, string action)
            {
                if (session == null || session.Opciones == null)
                    return true;

                var propias = session.Opciones.Where(o => AccessAttribute.Coincide(o, controller, action)).ToList();
                return propias.Count == 0 || propias.All(o => o.SoloLectura);
            }
        }

        /// <summary>
        /// Para endpoints que modifican datos: rechaza a quien tenga la opcion en solo lectura,
        /// aunque llame a la URL directamente.
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class EscrituraAttribute : AuthorizeAttribute
        {
            private readonly string _controller;
            private readonly string _opcionAction;

            /// <param name="controller">Controller de la opcion que gobierna el permiso.</param>
            /// <param name="opcionAction">Action de la vista (opcion) a la que pertenece este guardado.</param>
            public EscrituraAttribute(string controller, string opcionAction)
            {
                _controller = controller;
                _opcionAction = opcionAction;
            }

            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                var session = httpContext.Session["SessionUser"] as SessionModels;
                return session != null && !Permisos.EsSoloLectura(session, _controller, _opcionAction);
            }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                if (!(filterContext.HttpContext.Session["SessionUser"] is SessionModels))
                {
                    base.HandleUnauthorizedRequest(filterContext);
                    return;
                }
                // Los guardados responden JSON {success,message}; se informa al usuario sin romper el cliente
                filterContext.Result = new JsonResult
                {
                    Data = new { success = false, result = false, message = "Su rol solo tiene permiso de consulta en esta opcion" },
                    JsonRequestBehavior = JsonRequestBehavior.AllowGet
                };
            }
        }

        /// <summary>
        /// Igual que Escritura, pero para las pantallas de alta/edicion (GET): en lugar de JSON
        /// muestra un aviso, ya que el resultado se inyecta como HTML en el contenedor principal.
        /// </summary>
        [AttributeUsage(AttributeTargets.Method)]
        public class EscrituraVistaAttribute : EscrituraAttribute
        {
            public EscrituraVistaAttribute(string controller, string opcionAction) : base(controller, opcionAction) { }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                if (!(filterContext.HttpContext.Session["SessionUser"] is SessionModels))
                {
                    base.HandleUnauthorizedRequest(filterContext);
                    return;
                }
                filterContext.Result = new ContentResult
                {
                    ContentType = "text/html",
                    Content = "<div class=\"ui icon warning message\" style=\"margin:20px\"><i class=\"lock icon\"></i>" +
                              "<div class=\"content\"><div class=\"header\">Modo consulta</div>" +
                              "<p>Su rol solo tiene permiso de consulta en esta opci&oacute;n.</p></div></div>"
                };
            }
        }

        /// <summary>
        /// Exige sesion activa y, si el controller/action esta registrado como Opcion,
        /// que alguno de los roles del usuario tenga asignada esa opcion.
        /// </summary>
        public class AccessAttribute : AuthorizeAttribute
        {
            protected override bool AuthorizeCore(HttpContextBase httpContext)
            {
                var session = httpContext.Session["SessionUser"] as SessionModels;
                if (session==null)
                    return false;

                var rd = httpContext.Request.RequestContext.RouteData;
                var controller = Convert.ToString(rd.Values["controller"]);
                var action = Convert.ToString(rd.Values["action"]);

                var protegidas = new SecurityBo().GetOpcionesProtegidas()
                    .Where(o => Coincide(o, controller, action)).ToList();
                if (protegidas.Count == 0)
                    return true;

                return session.Opciones != null &&
                       session.Opciones.Any(o => Coincide(o, controller, action));
            }

            internal static bool Coincide(SecurityBo.OpcionModel o, string controller, string action)
            {
                // Action vacio = todo el controller; admite lista separada por comas (la primera es la del menu)
                return string.Equals(o.Controller, controller, StringComparison.OrdinalIgnoreCase)
                    && (string.IsNullOrEmpty(o.Action)
                        || o.Action.Split(',').Any(a => string.Equals(a.Trim(), action, StringComparison.OrdinalIgnoreCase)));
            }

            protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
            {
                var autenticado = filterContext.HttpContext.Session["SessionUser"] is SessionModels;
                if (!autenticado)
                {
                    base.HandleUnauthorizedRequest(filterContext);
                    return;
                }
                filterContext.Result = new HttpStatusCodeResult(403, "No tiene permisos para acceder a esta opcion");
            }

        }

}
