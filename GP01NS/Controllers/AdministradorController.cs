using GP01NS.Classes.Servicos;
using GP01NS.Classes.Util;
using GP01NS.Classes.ViewModels;
using GP01NS.Classes.ViewModels.Entrar;
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
    public class AdministradorController : BaseController
    {
        private AdministradorVM Administrador;

        public ActionResult Index()
        {
            this.Administrador = new AdministradorVM(this.BaseUsuario);

            return View();
        }

        public ActionResult CadastrarUsuario(string tipo = "fa")
        {
            ViewBag.Tipo = tipo;

            return View(new CadastroVM());
        }

        [HttpPost]
        public ActionResult CadastrarUsuario(CadastroVM model) 
        {
            if (ModelState.IsValid) 
            {
                if (model.ValidarEmail())
                {
                    if (model.ValidarNomeUsuario())
                    {
                        if (model.SaveChangesAdmin())
                            ViewBag.Sucesso = "Cadastro efetuado com sucesso.";
                        else
                            ViewBag.Mensagem = "Não foi possível estabelecer conexão com o servidor, por favor, tente novamente mais tarde.";
                    }
                    else
                    {
                        ViewBag.Mensagem = "O nome de usuário informado já está sendo utilizado.";
                    }
                }
                else
                {
                    ViewBag.Mensagem = "O e-mail informado está sendo utilizado por outro usuário.";
                }
            }
            else 
            {
                ViewBag.Mensagem = "Por favor, confira os dados informados e tente novamente.";
            }

            return View();
        }

        public ActionResult Fas()
        {
            this.Administrador = new AdministradorVM(this.BaseUsuario);

            return View(this.Administrador);
        }

        public ActionResult Estabelecimentos()
        {
            this.Administrador = new AdministradorVM(this.BaseUsuario);

            return View(this.Administrador);
        }

        public ActionResult Musicos()
        {
            this.Administrador = new AdministradorVM(this.BaseUsuario);

            return View(this.Administrador);
        }

        public ActionResult UsuariosDeTeste()
        {
            this.Administrador = new AdministradorVM(this.BaseUsuario);

            return View(this.Administrador);
        }

        public ActionResult Administradores()
        {
            this.Administrador = new AdministradorVM(this.BaseUsuario);

            return View(this.Administrador);
        }

        public ActionResult Pagamentos()
        {
            ViewBag.JSON = Pagamento.GetPagamentosJSON();

            return View();
        }

        public ActionResult Xml(string id)
        {
            return this.Content(PagSeguro.GetXmlTransacao(id).InnerXml, "text/xml");
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

            return Redirect("/inicio/");
        }
    }
}
