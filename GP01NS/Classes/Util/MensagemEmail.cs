﻿using GP01NS.Classes.ViewModels;
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
                return "http://nossoshow.gerison.net";
        }

        private string GerarHash()
        {
            return Criptografia.GetHash64(DateTime.Now.ToString());
        }

        #region MENSAGENS DE E-MAIL
        public bool Cadastro(requisicao req)
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

            try
            {
                Email.Enviar(req.usuario.Email, "Confirme sua conta - Nosso Show", Html);

                return true;
            }
            catch { }

            return false;
        }

        public bool RedefinirSenha(requisicao req)
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

            try
            {
                Email.Enviar(req.usuario.Email, "Redefina sua senha - Nosso Show", Html);

                return true;
            }
            catch { }

            return false;
        }

        public bool RespostaConvite(evento_musico c)
        {
            string TITULO = string.Empty;
            string SUBTITULO = string.Empty;
            string MENSAGEM = string.Empty;

            if (c.Confirmado)
            {
                TITULO = "Atração confirmada!";
                SUBTITULO = c.usuario_musico.NomeArtistico + ", acaba de confirmar presença ao seu evento no Nosso Show.";

                MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
                MENSAGEM += "   Ótima notícia! Seu evento " + c.evento.Titulo.ToUpper() + " agora conta com a presença confirmada de " + c.usuario_musico.NomeArtistico.ToUpper() + "."; 
                MENSAGEM += "</p>";
                MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
                MENSAGEM += "   A partir de agora, é possível publicar seu evento para que todos possam saber. Basta acessar o Nosso Show!";
                MENSAGEM += "</p>";
                MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
                MENSAGEM += "   <a href='http://nossoshow.gerison.net/inicio/evento/" + c.evento.ID + "'>http://nossoshow.gerison.net/inicio/evento/" + c.evento.ID + "</a>";
                MENSAGEM += "</p>";
            }
            else
            {
                TITULO = "Seu convite foi recusado!";
                SUBTITULO = c.usuario_musico.NomeArtistico + ", não confirmou presença ao seu evento no Nosso Show.";

                MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
                MENSAGEM += "   Que pena! Parece que seu evento " + c.evento.Titulo.ToUpper() + " não poderá contar com a presença de " + c.usuario_musico.NomeArtistico.ToUpper() + ".";
                MENSAGEM += "</p>";
                MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
                MENSAGEM += "   A publicação de seu evento pode ter sido removida caso esteja sem atrações confirmadas. Do contrário, tudo continua como estava. Acesse o Nosso Show para conferir!";
                MENSAGEM += "</p>";
                MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
                MENSAGEM += "   <a href='http://nossoshow.gerison.net/entrar/'>http://nossoshow.gerison.net/entrar/</a>";
                MENSAGEM += "</p>";
            }

            Html = Html.Replace("#TITULO", TITULO);
            Html = Html.Replace("#SUBTITULO", SUBTITULO);
            Html = Html.Replace("#MENSAGEM", MENSAGEM);

            try
            {
                Email.Enviar(c.evento.usuario_estabelecimento.usuario.Email, "Resposta ao convite - Nosso Show", Html);

                return true;
            }
            catch { }

            return false;
        }

        public bool Convite(EventoVM evento, MusicoVM musico)
        {
            string TITULO = "Olá, " + musico.NomeArtistico + "!";
            string SUBTITULO = "Você foi convidado para realizar um evento no Nosso Show.";

            string MENSAGEM = string.Empty;

            MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
            MENSAGEM += "   Veja os detalhes do Evento: ";
            MENSAGEM += "</p>";
            MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>";
            MENSAGEM += "<b>Título: </b>" + evento.Titulo + "<br /><br />";
            MENSAGEM += "<b>Horário: </b>" + evento.GetHorarioString() + "<br /><br />";
            MENSAGEM += "<b>Endereço: </b>" + evento.Estabelecimento.GetEnderecoString() + "<br /><br />";
            MENSAGEM += "<b>Sobre o evento</b><br /><br />" + evento.Descricao + "<br /><br />";
            MENSAGEM += "</p>";
            MENSAGEM += "<p style='font-size:17px;font-weight:500;margin:0;padding:0.5em 0;'>Para confirmar/cancelar a sua presença, acesse o site.</p>";

            Html = Html.Replace("#TITULO", TITULO);
            Html = Html.Replace("#SUBTITULO", SUBTITULO);
            Html = Html.Replace("#MENSAGEM", MENSAGEM);

            try
            {
                Email.Enviar(musico.Email, "Convite para realização de Evento - Nosso Show", Html);

                return true;
            }
            catch { }

            return false;
        }
        #endregion MENSAGENS DE E-MAIL
    }
}