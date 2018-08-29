using GP01NS.Classes.Util;
using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.Servicos
{
    public class Auth
    {
        public DateTime Data { get; set; }
        public int IDUsuario { get; set; }
        public string IP { get; set; }
        public string Sessao { get; set; }
        public int TipoUsuario { get; set; }

        public Auth()
        {

        }

        public Auth(usuario usuario, string sessionID) 
        {
            this.Data = DateTime.Now;
            this.IDUsuario = usuario.ID;
            this.IP = System.Web.HttpContext.Current.Request.UserHostAddress;
            this.Sessao = sessionID;
            this.TipoUsuario = usuario.Tipo;
        }

        public usuario GetUsuario(string email)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    if (!string.IsNullOrEmpty(email))
                    {
                        var u = new usuario();

                        return db.usuario.Single(x => x.Email == email);
                    }
                }
            }
            catch { }

            return null;
        }

        public usuario GetUsuario(string email, string senha)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    if (!string.IsNullOrEmpty(email) && !string.IsNullOrEmpty(senha))
                    {
                        var u = new usuario();

                        u = db.usuario.Single(x => x.Email == email);

                        if (Criptografia.ValidarHash128(senha, u.Senha))
                            return u;
                    }
                }
            }
            catch {  }

            return null;
        }

        public bool SaveChanges() 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var log = new autenticacao
                    {
                        Data = this.Data,
                        IDUsuario = this.IDUsuario,
                        IP = this.IP,
                        Sessao = this.Sessao,
                        Tipo = this.TipoUsuario
                    };

                    db.autenticacao.AddObject(log);
                    db.SaveChanges();

                    return true;
                }
            }
            catch {  }

            return false;
        }
    }
}