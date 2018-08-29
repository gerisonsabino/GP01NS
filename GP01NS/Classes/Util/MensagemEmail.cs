using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.Util
{
    public class MensagemEmail
    {
        private Email Email;
        private string Html;

        public DateTime Data { get; set; }
        public string Hash { get; set; }

        public MensagemEmail()
        {
            this.Data = DateTime.Now;
            this.Email = new Email();
            this.Html = File.ReadAllText(HttpContext.Current.Server.MapPath("~/HTML/email.html"));
            this.Hash = GerarHash();
        }

        private string Url() 
        {
            if (HttpContext.Current.Request.UserHostAddress == "::1" || HttpContext.Current.Request.UserHostAddress == "127.0.0.1" || HttpContext.Current.Request.UserHostAddress == "localhost")
                //Localhost
                return "http://localhost:" + HttpContext.Current.Request.Url.Port.ToString();
            else
                //Online
                return "https://gerisonsabino.azurewebsites.net";
        }

        private string GerarHash()
        {
            return Criptografia.GetHash64(DateTime.Now.ToString());
        }

        #region MENSAGENS DE E-MAIL
        public void MensagemCadastro(requisicao req)
        {
            string TITULO = "Olá, " + req.usuario.Nome.Split(' ')[0] + "!";
            string SUBTITULO = "Foi solicitada a criação de sua conta no Nosso Show.";

            string link = Url() + "/entrar/confirmar-conta/" + Hash;

            string MENSAGEM = string.Empty;

            MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
            MENSAGEM += "   Sua conta está quase pronta! Para confirmá-la, clique no link abaixo: ";
            MENSAGEM += "</p>";
            MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
            MENSAGEM += "   <a href='" + link + "' style='color:#006CD8 !important;text-decoration:none !important;font-size:16px !important;' target='_blank'>" + link + "</a>";
            MENSAGEM += "</p>";
            MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
            MENSAGEM += "   Este link de cadastro é válido somente até ser utilizado.";
            MENSAGEM += "</p>";
            MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>Caso não tenha solicitado a criação de conta, por favor, apenas ignore este e-mail.</p>";

            Html = Html.Replace("#TITULO", TITULO);
            Html = Html.Replace("#SUBTITULO", SUBTITULO);
            Html = Html.Replace("#MENSAGEM", MENSAGEM);

            Email.Enviar(req.usuario.Email, "Confirme sua conta - Nosso Show", Html);
        }

        public void MensagemRedefinirSenha(requisicao req)
        {
            string TITULO = "Olá, " + req.usuario.Nome.Split(' ')[0] + "!";
            string SUBTITULO = "Foi solicitada uma redefinição de senha para sua conta no Nosso Show.";

            string link = Url() + "/entrar/redefinir-senha/" + Hash;

            string MENSAGEM = string.Empty;

            MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
            MENSAGEM += "   Você pode redefinir sua senha clicando no link abaixo: ";
            MENSAGEM += "</p>";
            MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
            MENSAGEM += "   <a href='" + link + "' style='color:#006CD8 !important;text-decoration:none !important;font-size:16px !important;' target='_blank'>" + link + "</a>";
            MENSAGEM += "</p>";
            MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
            MENSAGEM += "   Este link para redefinição é válido somente até: " + req.Vencimento + ", ou até ser utilizado.";
            MENSAGEM += "</p>";
            MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>Caso não tenha solicitado uma redefinição de senha, por favor, apenas ignore este e-mail.</p>";

            Html = Html.Replace("#TITULO", TITULO);
            Html = Html.Replace("#SUBTITULO", SUBTITULO);
            Html = Html.Replace("#MENSAGEM", MENSAGEM);

            Email.Enviar(req.usuario.Email, "Redefina sua senha - Nosso Show", Html);
        }
        #endregion MENSAGENS DE E-MAIL
    }
}