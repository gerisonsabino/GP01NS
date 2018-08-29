using GP01NS.Classes.Servicos;
using GP01NS.Classes.Util;
using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Controllers
{
    public class BaseController : Controller
    {
        protected nosso_showEntities db { get; set; }
        protected usuario Usuario { get; set; }

        protected override void OnAuthorization(AuthorizationContext filterContext)
        {
            var controller = filterContext.RouteData.Values["controller"].ToString();
            var action = filterContext.RouteData.Values["action"].ToString();

            base.ViewBag.Controller = controller;
            base.ViewBag.Action = action;

            try
            {
                var ck = Request.Cookies["Sessao"];
                var ck1 = Request.Cookies["ID"];
                var ck2 = Request.Cookies["Tipo"];

                int id = int.MinValue, tipo = int.MinValue;

                int.TryParse(Criptografia.Descriptografar(ck1.Value), out id);
                int.TryParse(Criptografia.Descriptografar(ck2.Value), out tipo);

                string sessao = Criptografia.Descriptografar(ck.Value.ToString());

                var db_ = new nosso_showEntities(Conexao.GetString());

                var auth = db_.autenticacao.SingleOrDefault(x => x.IDUsuario == id && x.Tipo == tipo && x.Sessao == sessao);

                if (auth != null)
                {
                    ViewBag.TipoConta = tipo;

                    db = new nosso_showEntities(Conexao.GetString());

                    ViewBag.Conta = db.usuario.SingleOrDefault(x => x.ID == auth.IDUsuario);

                    base.OnAuthorization(filterContext);
                }
                else
                {
                    string url = System.Web.HttpUtility.UrlEncode(HttpContext.Request.RawUrl);

                    if (!string.IsNullOrEmpty(url))
                        filterContext.Result = RedirectPermanent("/entrar/?url=" + url);
                    else
                        filterContext.Result = RedirectPermanent("/entrar/");
                }
            }
            catch
            {
                string url = System.Web.HttpUtility.UrlEncode(HttpContext.Request.RawUrl);

                if (!string.IsNullOrEmpty(url))
                    filterContext.Result = RedirectPermanent("/entrar/?url=" + url);
                else
                    filterContext.Result = RedirectPermanent("/entrar/");
            }
        }

        private void Autenticar(usuario usuario) 
        {
            var sessao = Criptografia.Criptografar(Session.SessionID);
            var id = Criptografia.Criptografar(usuario.ID.ToString());
            var tipo = Criptografia.Criptografar(usuario.Tipo.ToString());

            var ck = new HttpCookie("Sessao");
            ck.HttpOnly = true;
            ck.Path = "/";
            ck.Value = sessao;

            var ck1 = new HttpCookie("ID");
            ck1.HttpOnly = true;
            ck1.Path = "/";
            ck1.Value = id;

            var ck2 = new HttpCookie("Tipo");
            ck2.HttpOnly = true;
            ck2.Path = "/";
            ck2.Value = tipo;

            using (var db = new nosso_showEntities(Conexao.GetString()))
            {
                var logs = db.autenticacao.Where(x => x.IDUsuario == usuario.ID).ToList();

                for (int i = 0; i < logs.Count; i++)
                    db.autenticacao.DeleteObject(logs[i]);

                var auth = new autenticacao
                {
                    Data = DateTime.Now,
                    IDUsuario = usuario.ID,
                    IP = Request.ServerVariables["REMOTE_ADDR"].ToString(),
                    Sessao = sessao,
                    Tipo = usuario.Tipo
                };

                db.autenticacao.AddObject(auth);
                db.SaveChanges();

                Response.Cookies.Add(ck);
                Response.Cookies.Add(ck1);
                Response.Cookies.Add(ck2);
            }
        }
    }
}
