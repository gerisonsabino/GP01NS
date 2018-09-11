using GP01NS.Classes.Util;
using GP01NS.Classes.ViewModels;
using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Controllers
{
    public class InicioController : BaseController
    {
        private UsuarioVM Usuario;

        public ActionResult Index() 
        {
            if (this.BaseUsuario != null)
            {
                this.Usuario = new UsuarioVM(this.BaseUsuario);
                ViewBag.Usuario = this.Usuario;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase Arquivo) 
        {
            return View();
        }

        public ActionResult Estabelecimento(string id)
        {
            if (this.BaseUsuario != null)
                this.Usuario = new UsuarioVM(this.BaseUsuario);
            else
                this.Usuario = null;

            ViewBag.Usuario = this.Usuario;

            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.First(x => x.Username == id && x.Tipo == 2);

                    return View(new EstabelecimentoVM(u));
                }
            }
            catch { return Redirect("/inicio/"); }

        }

        public ActionResult Evento(string id)
        {
            if (this.BaseUsuario != null)
                this.Usuario = new UsuarioVM(this.BaseUsuario);
            else
                this.Usuario = null;

            ViewBag.Usuario = this.Usuario;

            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    int idEvento = int.MinValue;
                    int.TryParse(id, out idEvento);

                    var e = db.evento.First(x => x.ID == idEvento /*&& x.Ativo && x.Publicado*/);

                    return View(new EventoVM(e));
                }
            }
            catch { return Redirect("/inicio/"); }
        }
    }
}
