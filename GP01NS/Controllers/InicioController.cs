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

            return View();
        }

        [HttpGet]
        public ActionResult Index(string nom, string gen, string amb, string not, string tip)
        {
            //Pegando usuário caso logado
            if (this.BaseUsuario != null)
            {
                this.Usuario = new UsuarioVM(this.BaseUsuario);
                ViewBag.Usuario = this.Usuario;
            }

            try
            {
                string nome = nom;
                int idGenero, idAmbientacao, nota, tipo;

                idAmbientacao = idGenero = nota = tipo = int.MinValue;

                int.TryParse(amb, out idAmbientacao);
                int.TryParse(gen, out idGenero);
                int.TryParse(not, out nota);
                int.TryParse(tip, out tipo);

                if (tipo != 0)
                {
                    //Conexão com o banco de dados 
                    using (var db = new nosso_showEntities(Conexao.GetString()))
                    {
                        //var estabelecimentos = db.usuario_estabelecimento.Where(x =>
                        //    (!string.IsNullOrEmpty(nome) ? x.usuario.Nome.ToLower().Contains(nome.ToLower()) : true)
                        //    && (idAmbientacao > 0 ? x.ambientacao.ID == idAmbientacao : true)
                        //).ToList();

                        string s = Pesquisar.EventosJSON(nome, idAmbientacao);

                        //var musicos = db.usuario_musico.Where(x =>
                        //    (!string.IsNullOrEmpty(nome) ? x.NomeArtistico.ToLower().Contains(nome.ToLower()) : true)
                        //    && (idGenero > 0 ? x.usuario.genero_musical.Any(y => y.ID == idGenero) : true)
                        //).ToList();
                    }
                }
            }
            catch { }

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
