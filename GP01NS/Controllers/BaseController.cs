using GP01NS.Classes.Servicos;
using GP01NS.Classes.Util;
using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;

namespace GP01NS.Controllers
{
    public class BaseController : Controller
    {
        protected usuario Usuario;

        protected override void OnActionExecuting(ActionExecutingContext filterContext) 
        {
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();

            if (controller.Equals("entrar", StringComparison.CurrentCultureIgnoreCase) && action.Equals("index", StringComparison.CurrentCultureIgnoreCase))
            {
                return;
            }

            string id = string.Empty;

            try
            {
                id = Criptografia.Descriptografar(Session["IDUsuario"].ToString());
            }
            catch { }

            if (((string.IsNullOrEmpty(id)) && (!Session.IsNewSession)) || (Session.IsNewSession))
            {
                Session.RemoveAll();
                Session.Clear();
                Session.Abandon();

                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary { { "controller", "entrar" }, { "action", "index" } });
            }
            else
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    this.Usuario = db.autenticacao.First(x => x.Sessao.Equals(this.Session.SessionID)).usuario;
                }
            }

            base.OnActionExecuting(filterContext);
        }
    }
}
