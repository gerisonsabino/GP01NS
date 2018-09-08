using GP01NS.Classes.Util;
using GP01NS.Classes.ViewModels;
using GP01NS.Classes.ViewModels.Fa;
using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Controllers
{
    public class FaController : BaseController
    {
        UsuarioVM Usuario;

        public ActionResult Index()
        {
            this.Usuario = new UsuarioVM(this.BaseUsuario);

            using (var db = new nosso_showEntities(Conexao.GetString()))
            {
                var u = db.usuario.Single(x => x.ID == this.Usuario.ID);

                if (u.genero_musical.Count == 0)
                    return Redirect("/fa/conta/");

                if (u.endereco.Count == 0)
                    return Redirect("/fa/endereco/");
            }

            return View(this.Usuario);
        }

        public ActionResult Conta()
        {
            this.Usuario = new UsuarioVM(this.BaseUsuario);

            var cadastro = new ContaVM(this.Usuario);

            ViewBag.Generos = cadastro.GetGenerosMusicais();
            ViewBag.Ambientacoes = cadastro.GetAmbientacoes();

            return View(cadastro);
        }

        [HttpPost]
        public ActionResult Conta(ContaVM model)
        {
            this.Usuario = new UsuarioVM(this.BaseUsuario);

            if (ModelState.IsValid)
            {
                if (model.ValidarEmail(this.Usuario))
                {
                    if (model.ValidarNomeUsuario(this.Usuario))
                    {
                        if (model.SaveChanges(this.Usuario))
                            ViewBag.Sucesso = "Os dados de sua conta foram salvos.";
                        else
                            ViewBag.Erro = "Não foi possível estabelecer conexão com o servidor, por favor, tente novamente mais tarde.";
                    }
                    else
                    {
                        ViewBag.Erro = "O nome de usuário informado já está sendo utilizado.";
                    }
                }
                else
                {
                    ViewBag.Erro = "O endereço de e-mail informado já está sendo utilizado.";
                }
            }
            else
            {
                ViewBag.Erro = "Por favor, confira os dados informados e tente novamente.";
            }
            ViewBag.Generos = model.GetGenerosMusicais();
            ViewBag.Ambientacoes = model.GetAmbientacoes();
            return View(model);
        }

        public ActionResult Endereco()
        {
            this.Usuario = new UsuarioVM(this.BaseUsuario);

            var endereco = this.Usuario.Endereco;

            return View(endereco);
        }

        [HttpPost]
        public ActionResult Endereco(EnderecoVM model)
        {
            this.Usuario = new UsuarioVM(this.BaseUsuario);

            if (model.SaveChanges(this.Usuario))
            {
                ViewBag.Sucesso = "Os dados de endereço foram salvos.";
            }
            else
            {
                ViewBag.Erro = "Por favor, confira os dados informados e tente novamente.";
            }

            return View(model);
        }

        public ActionResult Sair()
        {
            try
            {
                string id = string.Empty;

                try
                {
                    id = Criptografia.Descriptografar(base.Session["IDUsuario"].ToString());
                }
                catch { }

                if (!string.IsNullOrEmpty(id))
                {
                    base.Session.RemoveAll();
                    base.Session.Clear();
                    base.Session.Abandon();
                    base.Session["IDUsuario"] = string.Empty;

                    using (var db = new nosso_showEntities(Conexao.GetString()))
                    {
                        int idUsuario = int.Parse(Criptografia.Descriptografar(id));

                        var auths = db.autenticacao.Where(x => x.IDUsuario == idUsuario && x.Sessao == Session.SessionID).ToList();

                        for (int i = 0; i < auths.Count; i++)
                            db.autenticacao.DeleteObject(auths[i]);

                        db.SaveChanges();
                    }
                }

            }
            catch { }

            return Redirect("/entrar");
        }
    }
}
