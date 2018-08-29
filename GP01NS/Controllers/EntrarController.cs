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
            if (!string.IsNullOrEmpty(Esqueci))
            {
                try
                {
                    using (var db = new nosso_showEntities(Conexao.GetString()))
                    {
                        var u = new Auth().GetUsuario(Esqueci);

                        if (u != null)
                        {
                            var msg = new MensagemEmail();

                            var req = new requisicao
                            {
                                Ativa = true,
                                Data = msg.Data,
                                Hash = msg.Hash,
                                IDUsuario = u.ID,
                                Vencimento = DateTime.Now.AddHours(1),
                                TipoRequisicao = 1, // 1 - Redefinição de senha
                                TipoUsuario = u.Tipo
                            };

                            db.requisicao.AddObject(req);

                            msg.MensagemRedefinirSenha(req);

                            db.SaveChanges();

                            ViewBag.Sucesso = "Enviamos um e-mail para você com instruções de como redefinir sua senha.";
                        }
                        else
                        {
                            ViewBag.Mensagem = "E-mail não encontrado. Por favor, tente novamente ou entre em contato conosco.";
                        }
                    }
                }
                catch
                {
                    ViewBag.Mensagem = "E-mail não encontrado. Por favor, tente novamente ou entre em contato conosco.";
                }

                return View();
            }
            else
            {
                try
                {
                    var u = new Auth().GetUsuario(Email, Senha);

                    if (u != null)
                    {
                        if (!u.Ativo)
                        {
                            ViewBag.Mensagem = "Usuário sem permissão de acesso ao sistema. Por favor, contate a administração.";
                        }
                        else
                        {
                            Autenticar(u);

                            var url = Request.QueryString["url"];

                            if (!string.IsNullOrEmpty(url))
                                return Redirect(url);
                            else
                                return Redirect("/inicio/");
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

                return View();
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
                    ViewBag.Mensagem = "O endereço de e-mail informado já está sendo utilizado.";
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

                        ViewBag.Nome = u.Nome;
                        ViewBag.Mensagem = "Sua conta foi confirmada, efetue o acesso para continuar com o cadastro.";
                        ViewBag.Button = "CONTINUAR";
                    }
                    else
                    {
                        ViewBag.Nome = "Visitante";
                        ViewBag.Mensagem = "Este link não é mais válido, para continuar você deve entrar com seu acesso.";
                        ViewBag.Button = "ENTRAR";
                    }

                    return View();
                }
            }
            catch { return Redirect("/entrar/"); }
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
                    Sessao = Session.SessionID,
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
