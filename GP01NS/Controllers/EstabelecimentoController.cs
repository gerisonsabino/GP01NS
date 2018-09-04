using GP01NS.Classes.Util;
using GP01NS.Classes.ViewModels;
using GP01NS.Classes.ViewModels.Estabelecimento;
using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Controllers
{
    public class EstabelecimentoController : BaseController
    {
        private EstabelecimentoVM Estabelecimento;

        public ActionResult Index()
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            if (this.Estabelecimento.TipoUsuario == 2)
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    if (!db.usuario_estabelecimento.Any(x => x.IDUsuario == this.Estabelecimento.ID))
                        return Redirect("/estabelecimento/conta/");

                    if (!db.endereco.Any(x => x.IDUsuario == this.Estabelecimento.ID))
                        return Redirect("/estabelecimento/endereco/");
                }
            }

            return View(Estabelecimento);
        }

        public ActionResult Conta()
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            var cadastro = new ContaVM(this.Estabelecimento);

            return View(cadastro);
        }

        [HttpPost]
        public ActionResult Conta(ContaVM model) 
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            if (ModelState.IsValid)
            {
                if (model.ValidarEmail(this.Estabelecimento))
                {
                    if (model.ValidarNomeUsuario(this.Estabelecimento))
                    {
                        if (model.SaveChanges(this.Estabelecimento))
                            ViewBag.Sucesso = "Os dados foram salvos.";
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

            return View(model);
        }

        public ActionResult Eventos()
        {
            return View();
        }

        public ActionResult Evento()
        {
            return View();
        }

        public ActionResult Endereco()
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            ViewBag.Estabelecimento = this.Estabelecimento;

            var endereco = this.Estabelecimento.Endereco;

            return View(endereco);
        }

        [HttpPost]
        public ActionResult Endereco(EnderecoVM model)
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            ViewBag.Estabelecimento = this.Estabelecimento;

            if (model.SaveChanges(this.Estabelecimento))
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
