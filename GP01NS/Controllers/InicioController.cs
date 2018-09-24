using GP01NS.Classes.Servicos;
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

            var musico = new Classes.ViewModels.Musico.ContaVM();

            ViewBag.Generos = musico.GetGenerosMusicais();
            ViewBag.TipoHabilidades = musico.GetTipoHabilidades();
            ViewBag.Habilidades = musico.GetHabilidades();

            var estabelecimento = new Classes.ViewModels.Estabelecimento.ContaVM();

            ViewBag.Ambientacoes = estabelecimento.GetAmbientacoesList();

            return View();
        }

        [HttpGet]
        public ActionResult Resultados(string q, string e, string g, string a, string h)
        {
            if (this.BaseUsuario != null)
            {
                this.Usuario = new UsuarioVM(this.BaseUsuario);
                ViewBag.Usuario = this.Usuario;
            }

            if (!string.IsNullOrEmpty(q))
            {
                try
                {
                    ViewBag.JSON = Pesquisa.Pesquisar(Server.UrlDecode(q), Server.UrlDecode(e), Server.UrlDecode(g), Server.UrlDecode(a), Server.UrlDecode(h));
                    return View();
                }
                catch { }

            }

            return Redirect("/inicio/");
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

        public ActionResult Musico(string id)
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
                    var u = db.usuario.First(x => x.Username == id && x.Tipo == 4);

                    return View(new MusicoVM(u));
                }
            }
            catch { return Redirect("/inicio/"); }
        }

        [HttpPost]
        public ActionResult Seguir(int ID)
        {
            if (this.BaseUsuario != null)
            {
                this.Usuario = new UsuarioVM(this.BaseUsuario);
                ViewBag.Usuario = this.Usuario;

                this.Usuario.Seguir(ID);
            }

            return Redirect("");
        }
    }
}
