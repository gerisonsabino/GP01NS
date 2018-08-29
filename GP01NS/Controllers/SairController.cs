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
    public class SairController : Controller
    {
        public ActionResult Index()
        {
            return Sair();
        }

        internal RedirectResult Sair() 
        {
            var cookies = new List<HttpCookie>();

            cookies.Add(Request.Cookies["Sessao"]);
            cookies.Add(Request.Cookies["ID"]);
            cookies.Add(Request.Cookies["Tipo"]);

            Sair(cookies);

            return Redirect("/entrar");
        }

        private void Sair(List<HttpCookie> Cookies) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString())) 
                {
                    for (int i = 0; i < Cookies.Count; i++)
                    {
                        var cookie = Cookies[i];

                        cookie.Expires = DateTime.Now.AddDays(-1);
                        cookie.Value = string.Empty;

                        Response.Cookies.Add(cookie);
                    }

                    int ID = int.MinValue;
                    int.TryParse(Criptografia.Descriptografar(Cookies[1].Value), out ID);

                    var auths = db.autenticacao.Where(x => x.IDUsuario == ID).ToList();

                    for (int i = 1; i < auths.Count; i++)
                        db.autenticacao.DeleteObject(auths[i]);

                    db.SaveChanges();
                }
            }
            catch { }
        }
    }
}
