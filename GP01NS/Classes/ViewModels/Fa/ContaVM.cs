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

                    this.SetJsonAmbientes(u);
                    this.SetJsonGeneros(u);

                    db.ObjectStateManager.ChangeObjectState(u, System.Data.EntityState.Modified);
                    db.SaveChanges();

                    return true;
                }
            }
            catch { }

            return false;
        }

        private void SetJsonGeneros(usuario usuario)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.Single(x => x.ID == usuario.ID);

                    var dbGens = u.genero_musical.ToList();

                    for (int i = 0; i < dbGens.Count; i++)
                    {
                        var gen = dbGens[i];
                        u.genero_musical.Remove(gen);
                    }

                    var genIds = JsonConvert.DeserializeObject<List<int>>(this.JsonGeneros);

                    for (int i = 0; i < genIds.Count; i++)
                    {
                        var gen = genIds[i];
                        var dbGen = db.genero_musical.Single(x => x.ID == gen);

                        u.genero_musical.Add(dbGen);
                    }

                    db.SaveChanges();
                }
            }
            catch { }
        }

        private void SetJsonAmbientes(usuario usuario)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.Single(x => x.ID == usuario.ID);

                    var dbAmbs = u.ambientacao.ToList();

                    for (int i = 0; i < dbAmbs.Count; i++)
                    {
                        var amb = dbAmbs[i];
                        u.ambientacao.Remove(amb);
                    }

                    var ambIds = JsonConvert.DeserializeObject<List<int>>(this.JsonAmbientes);

                    for (int i = 0; i < ambIds.Count; i++)
                    {
                        var amb = ambIds[i];
                        var dbAmb = db.ambientacao.Single(x => x.ID == amb);

                        u.ambientacao.Add(dbAmb);
                    }

                    db.SaveChanges();
                }
            }
            catch { }
        }
    }
}