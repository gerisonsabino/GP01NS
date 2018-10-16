using GP01NS.Classes.Servicos;
using GP01NS.Classes.Util;
using GP01NS.Classes.ViewModels.Entrar;
using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Controllers
{
    public class EntrarController : Controller
    {
        public ActionResult Index() 
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string Email, string Senha, string Esqueci)
        {
            if (string.IsNullOrEmpty(Esqueci))
            {
                try
                {
                    var u = Auth.Autenticar(Email, Senha, Session.SessionID);

                    if (u != null)
                    {
                        if (!u.Ativo)
                        {
                            ViewBag.Mensagem = "Usuário sem permissão de acesso ao sistema. Por favor, contate a administração.";
                        }
                        else
                        {
                            string id = string.Empty;

                            try
                            {
                                id = Criptografia.Descriptografar(Session["IDUsuario"].ToString());
                            }
                            catch { }

                            if (!string.IsNullOrEmpty(id))
                            {
                                base.Session.RemoveAll();
                                base.Session.Clear();
                                base.Session.Abandon();
                                base.Session["IDUsuario"] = string.Empty;
                            }

                            base.Session.Add("IDUsuario", Criptografia.Criptografar(u.ID.ToString()));

                            return Redirect("/inicio/buscar/");
                        }
                    }
                    else
                    {
                        ViewBag.Mensagem = "Usuário não cadastrado ou senha inválida.";
                    }
                }
                catch
                {
                    ViewBag.Mensagem = "Não foi possível estabelecer conexão com o servidor, por favor, tente novamente mais tarde.";
                }
            }
            else
            {
                EsqueciMinhaSenha(Esqueci);
            }

            return View();
        }

        private void EsqueciMinhaSenha(string email) 
        {
            try
            {
                var u = Auth.GetUsuarioByEmail(email);

                if (u != null)
                {
                    var req = new Requisicao(u, 1);

                    if (req.SaveChanges())
                        ViewBag.Sucesso = "Enviamos um e-mail para você com instruções de como redefinir sua senha.";
                    else
                        ViewBag.Mensagem = "Não foi possível estabelecer conexão com o servidor, por favor, tente novamente mais tarde.";
                }
                else
                {
                    ViewBag.Mensagem = "Nome de usuário/E-mail não encontrado. Por favor, tente novamente ou entre em contato conosco.";
                }
            }
            catch
            {
                ViewBag.Mensagem = "E-mail não encontrado. Por favor, tente novamente ou entre em contato conosco.";
            }
        }

        public ActionResult Cadastro() 
        {
            return View(new CadastroVM());
        }

        [HttpPost]
        public ActionResult Cadastro(CadastroVM model) 
        {
            if (ModelState.IsValid) 
            {
                if (model.ValidarEmail())
                {
                    if (model.ValidarNomeUsuario())
                    {
                        if (model.ValidarSenha())
                        {
                            if (model.SaveChanges())
                                ViewBag.Sucesso = "Cadastro efetuado, enviamos novas instruções para seu e-mail.";
                            else
                                ViewBag.Mensagem = "Não foi possível estabelecer conexão com o servidor, por favor, tente novamente mais tarde.";
                        }
                        else
                        {
                            ViewBag.Mensagem = "As senhas informadas não coincidem.";
                        }
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

        public ActionResult ConfirmarConta(string id) 
        {
            try 
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var req = db.requisicao.SingleOrDefault(x => x.Hash == id && x.Ativa);

                    if (req != null)
                    {
                        var u = req.usuario;

                        u.Confirmado = true;
                        req.Ativa = false;

                        db.ObjectStateManager.ChangeObjectState(req, System.Data.EntityState.Modified);
                        db.ObjectStateManager.ChangeObjectState(u, System.Data.EntityState.Modified);

                        db.SaveChanges();

                        ViewBag.Nome = u.Username;

                        return View();
                    }
                }
            }
            catch { }

            return Redirect("/entrar/"); 
        }

        public ActionResult RedefinirSenha(string id) 
        {
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    using (var db = new nosso_showEntities(Conexao.GetString()))
                    {
                        var data = DateTime.Now.AddHours(1);

                        var requisicao = db.requisicao.Single(x => x.Hash == id && x.TipoRequisicao == 1 && x.Vencimento <= data && x.Ativa);

                        return View(requisicao.usuario);
                    }
                }
                catch { }
            }

            return Redirect("/entrar/");
        }

        [HttpPost]
        public ActionResult RedefinirSenha(string id, string Senha, string Confirmacao) 
        {
            if (!string.IsNullOrEmpty(id))
            {
                try
                {
                    using (var db = new nosso_showEntities(Conexao.GetString()))
                    {
                        var requisicao = db.requisicao.Single(x => x.Hash == id && x.TipoRequisicao == 1 && x.Ativa); // 1 - Redefinir senha

                        try
                        {
                            var u = requisicao.usuario;

                            if (Senha == Confirmacao)
                            {
                                u.Senha = Criptografia.GetHash128(Senha);

                                db.ObjectStateManager.ChangeObjectState(u, System.Data.EntityState.Modified);
                                db.SaveChanges();

                                db.requisicao.DeleteObject(requisicao);
                                db.SaveChanges();

                                ViewBag.Sucesso = "Sua senha foi alterada, efetue o acesso para continuar."; ;
                            }
                            else
                            {
                                ViewBag.Mensagem = "As senhas informadas não coincidem.";
                            }

                            return View(u);
                        }
                        catch
                        {
                            ViewBag.Mensagem = "Sua requisição expirou ou é inválida.";
                        }
                    }
                }
                catch
                {
                    ViewBag.Mensagem = "Não foi possível conectar ao servidor. Tente novamente em alguns minutos.";
                }
            }
            else
            {
                ViewBag.Mensagem = "Sua requisição expirou ou é inválida.";
            }

            return Redirect("/entrar/");
        }
    }
}
