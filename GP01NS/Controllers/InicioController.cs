using GP01NS.Classes.Servicos;
using GP01NS.Classes.Util;
using GP01NS.Classes.ViewModels;
using GP01NS.Models;
using GP01NSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
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
        public string GetGeneros()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var lista = new List<ExpandoObject>();
                    var generos = db.genero_musical.ToList();

                    for (int i = 0; i < generos.Count; i++)
                    {
                        dynamic o = new ExpandoObject();
                        o.ID = generos[i].ID;
                        o.Descricao = generos[i].Descricao;

                        lista.Add(o);
                    }

                    return JsonConvert.SerializeObject(lista);
                }
            }
            catch { }

            return string.Empty;
        }

        [HttpPost]
        public string GetAmbientacoes()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var lista = new List<ExpandoObject>();
                    var ambs = db.ambientacao.ToList();

                    for (int i = 0; i < ambs.Count; i++)
                    {
                        dynamic o = new ExpandoObject();
                        o.ID = ambs[i].ID;
                        o.Descricao = ambs[i].Descricao;

                        lista.Add(o);
                    }

                    return JsonConvert.SerializeObject(lista);
                }
            }
            catch { }

            return string.Empty;
        }

        [HttpPost]
        public string GetTipoHabilidades()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var lista = new List<ExpandoObject>();
                    var tipos = db.hab_musical_tipo.ToList();

                    for (int i = 0; i < tipos.Count; i++)
                    {
                        dynamic o = new ExpandoObject();
                        o.ID = tipos[i].ID;
                        o.Descricao = tipos[i].Descricao;

                        lista.Add(o);
                    }

                    return JsonConvert.SerializeObject(lista);
                }
            }
            catch { }

            return string.Empty;
        }

        [HttpPost]
        public string GetHabilidades()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var lista = new List<ExpandoObject>();
                    var habis = db.hab_musical.ToList();

                    for (int i = 0; i < habis.Count; i++)
                    {
                        dynamic o = new ExpandoObject();
                        o.ID = habis[i].ID;
                        o.TipoHab = habis[i].TipoHab;
                        o.Descricao = habis[i].Descricao;

                        lista.Add(o);
                    }

                    return JsonConvert.SerializeObject(lista);
                }
            }
            catch { }

            return string.Empty;
        }

        [HttpPost]
        public string Pesquisar(string q, string e, string g, string a, string h)
        {
            if (!string.IsNullOrEmpty(q))
            {
                try
                {
                    return Pesquisa.Pesquisar(Server.UrlDecode(q), Server.UrlDecode(e), Server.UrlDecode(g), Server.UrlDecode(a), Server.UrlDecode(h));
                }
                catch { }
            }

            return string.Empty;
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
        public string ToggleSeguir(int ID)
        {
            if (this.BaseUsuario != null)
            {
                this.Usuario = new UsuarioVM(this.BaseUsuario);
                ViewBag.Usuario = this.Usuario;

                this.Usuario.ToggleSeguir(ID);

                return "ok";
            }

            return "erro";
        }
    }
}
