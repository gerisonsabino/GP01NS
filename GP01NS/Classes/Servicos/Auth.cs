using GP01NS.Classes.Util;
using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.Servicos
{
    public static class Auth
    {
        public static usuario Autenticar(string email, string senha, string sessao) 
        {
            try
            {
                var usuario = GetUsuario(email, senha);

                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var auth = new autenticacao
                    {
                        Data = DateTime.Now,
                        IDUsuario = usuario.ID,
                        IP = System.Web.HttpContext.Current.Request.UserHostAddress,
                        Sessao = sessao,
                        Tipo = usuario.Tipo
                    };

                    db.autenticacao.AddObject(auth);
                    db.SaveChanges();
                }

                return usuario;
            }
            catch { }

            return null;
        }

        public static usuario GetUsuarioByEmail(string email)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        var u = new usuario();

                        return db.usuario.Single(x => x.Email == email || x.Username == email);
                    }
                }
            }
            catch { }

            return null;
        }

        private static usuario GetUsuario(string email, string senha) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(senha))
                    {
                        var u = new usuario();

                        u = db.usuario.Single(x => x.Email == email || x.Username == email);

                        if (Criptografia.ValidarHash128(senha, u.Senha))
                            return u;
                    }
                }
            }
            catch { }

            return null;
        }
    }
}