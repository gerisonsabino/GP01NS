using GP01NS.Models;
using GP01NSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.ViewModels.Fa
{
    public class ContaVM
    {
        public string Nome { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Nascimento { get; set; }
        public string Telefone { get; set; }
        public string JsonGeneros { get; set; }
        public string JsonAmbientes { get; set; }

        private UsuarioVM Usuario;

        public ContaVM() { }

        public ContaVM(UsuarioVM usuario)
        {
            this.Usuario = usuario;

            this.Email = usuario.Email;
            this.Nascimento = usuario.Nascimento;
            this.Nome = usuario.Nome;
            this.Telefone = usuario.Telefone;
            this.Username = usuario.Username;
            this.JsonAmbientes = this.GetJsonAmbientes();
            this.JsonGeneros = this.GetJsonGeneros();
        }

        public bool ValidarEmail(UsuarioVM usuario) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return !db.usuario.Any(x => x.Email == this.Email && x.ID != usuario.ID);
                }
            }
            catch { return true; }
        }

        public bool ValidarNomeUsuario(UsuarioVM usuario) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return !db.usuario.Any(x => x.Username.ToLower() == this.Username.ToLower() && x.ID != usuario.ID);
                }
            }
            catch { return true; }
        }

        private string GetJsonGeneros()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var generos = db.usuario.First(x => x.ID == this.Usuario.ID).genero_musical.Select(x => x.ID);

                    return JsonConvert.SerializeObject(generos);
                }
            }
            catch { }

            return null;
        }

        private string GetJsonAmbientes()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var ambientacoes = db.usuario.First(x => x.ID == this.Usuario.ID).ambientacao.Select(x => x.ID);

                    return JsonConvert.SerializeObject(ambientacoes);
                }
            }
            catch { }

            return null;
        }

        public List<genero_musical> GetGenerosMusicais() 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.genero_musical.ToList();
                }
            }
            catch { }

            return null;
        }

        public List<ambientacao> GetAmbientacoes() 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.ambientacao.ToList();
                }
            }
            catch { }

            return null;
        }

        public bool SaveChanges(UsuarioVM usuario) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.Single(x => x.ID == usuario.ID);

                    u.Email = this.Email;
                    u.Nascimento = this.Nascimento;
                    u.Nome = this.Nome;
                    u.Telefone = this.Telefone;
                    u.Username = this.Username;

                    var ambs = JsonConvert.DeserializeObject<List<int>>(this.JsonAmbientes);

                    for (int i = 0; i < ambs.Count; i++)
                    {
                        var idAmb = ambs[i];
                        u.ambientacao.Add(db.ambientacao.First(x => x.ID == idAmb));
                    }

                    var gen = JsonConvert.DeserializeObject<List<int>>(this.JsonGeneros);

                    for (int i = 0; i < gen.Count; i++)
                    {
                        var idGen = gen[i];
                        u.genero_musical.Add(db.genero_musical.First(x => x.ID == idGen));
                    }

                    db.ObjectStateManager.ChangeObjectState(u, System.Data.EntityState.Modified);
                    db.SaveChanges();

                    return true;
                }
            }
            catch { }

            return false;
        }
    }
}