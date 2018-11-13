using GP01NS.Classes.Servicos;
using GP01NS.Classes.Util;
using GP01NS.Classes.ViewModels;
using GP01NS.Classes.ViewModels.Estabelecimento;
using GP01NS.Models;
using GP01NSLibrary;
using System.Collections.Specialized;
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

            using (var db = new nosso_showEntities(Conexao.GetString()))
            {
                var u = db.usuario.Single(x => x.ID == this.Estabelecimento.ID);

                if (u.usuario_estabelecimento.Count == 0)
                    return Redirect("/estabelecimento/conta/");

                if (u.endereco.Count == 0)
                    return Redirect("/estabelecimento/endereco/");
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
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            ViewBag.JSON = this.Estabelecimento.GetEventosJSON();

            return View();
        }

        public ActionResult Evento(string id)
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            var evento = new EventoVM();

            int idEvento = int.MinValue;
            int.TryParse(id, out idEvento);

            if (idEvento > 0)
            {
                var e = this.Estabelecimento.GetEventoByID(idEvento);

                if (e != null)
                    evento = new EventoVM(e);
            }

            ViewBag.Estabelecimento = this.Estabelecimento;

            return View(evento);
        }

        [HttpPost]
        public string PesquisarMusicos(string id, string q)
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            var evento = new EventoVM();

            int idEvento = int.MinValue;
            int.TryParse(id, out idEvento);

            if (idEvento > 0)
            {
                var e = this.Estabelecimento.GetEventoByID(idEvento);

                if (e != null)
                    evento = new EventoVM(e);
            }

            return evento.PesquisarMusicos(q);
        }

        [HttpPost]
        public string GetAtracoes(string id)
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            var evento = new EventoVM();

            int idEvento = int.MinValue;
            int.TryParse(id, out idEvento);

            if (idEvento > 0)
            {
                var e = this.Estabelecimento.GetEventoByID(idEvento);

                if (e != null)
                    evento = new EventoVM(e);
            }

            return evento.GetMusicos();
        }

        [HttpPost]
        public void ConviteMusico(string idE, string idM)
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            var evento = new EventoVM();

            int idMusico = int.MinValue;
            int.TryParse(idM, out idMusico);

            int idEvento = int.MinValue;
            int.TryParse(idE, out idEvento);

            if (idEvento > 0)
            {
                var e = this.Estabelecimento.GetEventoByID(idEvento);

                if (e != null)
                    evento = new EventoVM(e);

                evento.ToggleSetMusico(idMusico);
            }
        }

        [HttpPost]
        public ActionResult Evento(EventoVM model)
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

        [HttpPost]
        public ActionResult PublicarEvento(int IDEvento)
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            ViewBag.Estabelecimento = this.Estabelecimento;
            var evento = this.Estabelecimento.GetEventoByID(IDEvento);

            if (evento != null)
            {
                if (new EventoVM(evento).TogglePublicar())
                {
                    return Redirect("/inicio/evento/" + evento.ID);
                }
            }

            ViewBag.Erro = "Falha ao publicar o evento, por favor, tente novamente.";

            return Redirect("/estabelecimento/evento/" + IDEvento);
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

        public ActionResult RedesSociais()
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);
            
            return View(this.Estabelecimento.RedesSociais);
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult RedesSociais(RedesSociaisVM model)
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            if (model.SaveChanges(this.Estabelecimento))
            {
                ViewBag.Sucesso = "Os dados de redes sociais foram salvos.";
            }
            else
            {
                ViewBag.Erro = "Por favor, confira os dados informados e tente novamente.";
            }

            return View(model);
        }

        public ActionResult Impulsionar()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Checkout()
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            var pagamento = new Pagamento(this.BaseUsuario, 30.00m).GerarPagamento();

            NameValueCollection dados = new NameValueCollection
            {
                { "reference", pagamento.REF },
                { "itemDescription1", "Conta Premium - Nosso Show" },
                { "itemAmount1", pagamento.Valor.ToString().Replace(",", ".") },
                { "senderName", this.Estabelecimento.Nome },
                { "senderEmail", this.Estabelecimento.Email.Split('@')[0].ToString() + "@sandbox.pagseguro.com.br" }
            };

            if (false)
            //if (!string.IsNullOrEmpty(this.Estabelecimento.Telefone))
            {
                string telefone = new string(this.Estabelecimento.Telefone.Where(c => char.IsDigit(c)).ToArray());

                dados.Add("senderAreaCode", telefone.Substring(0, 2));
                dados.Add("senderPhone", telefone.Substring(2, telefone.Count() - 2));
            }

			string url = PagSeguro.Checkout(dados);

            return Redirect(url);
        }

        [HttpPost]
        public ActionResult UploadProfile(HttpPostedFileBase Arquivo, string Href)
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            new ImagemVM(Arquivo, this.Estabelecimento.ID, int.MinValue, 1).Upload();

            return Redirect(Href);
        }

        [HttpPost]
        public ActionResult UploadCover(HttpPostedFileBase Arquivo)
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            new ImagemVM(Arquivo, this.Estabelecimento.ID, int.MinValue, 2).Upload();

            return Redirect("/inicio/estabelecimento/" + this.Estabelecimento.Username);
        }

        [HttpPost]
        public ActionResult UploadImagem(HttpPostedFileBase Imagem)
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            new ImagemVM(Imagem, this.Estabelecimento.ID, int.MinValue, 3).Upload();

            return Redirect("/inicio/estabelecimento/" + this.Estabelecimento.Username);
        }

        [HttpPost]
        public ActionResult UploadBannerEvento(HttpPostedFileBase Arquivo, int IDEvento)
        {
            this.Estabelecimento = new EstabelecimentoVM(this.BaseUsuario);

            var e = this.Estabelecimento.GetEventoByID(IDEvento);

            if (e != null)
                new ImagemVM(Arquivo, this.Estabelecimento.ID, IDEvento, 4).Upload();

            return Redirect("/inicio/evento/" + IDEvento);
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
