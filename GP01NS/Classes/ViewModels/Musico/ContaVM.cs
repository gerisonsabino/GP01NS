using GP01NS.Models;
using GP01NSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GP01NS.Classes.ViewModels.Musico
{
    public class ContaVM 
    {
        public string Nome { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public DateTime Nascimento { get; set; }
        public string Telefone { get; set; }
        public string NomeArtistico { get; set; }
        public string CPF { get; set; }
        public string Descricao { get; set; }
        public string JsonGeneros { get; set; }
        public string JsonHabilidades { get; set; }

        private readonly MusicoVM Musico;

        public ContaVM() { }

        public ContaVM(MusicoVM musico) 
        {
            this.Musico = musico;

            this.Email = musico.Email;
            this.Nascimento = musico.Nascimento;
            this.Nome = musico.Nome;
            this.Telefone = musico.Telefone;
            this.Username = musico.Username;
            this.NomeArtistico = musico.NomeArtistico;
            this.CPF = musico.CPF;
            this.Descricao = musico.Descricao;
            this.JsonGeneros = this.GetJsonGeneros();
            this.JsonHabilidades = this.GetJsonHabilidades();
        }

        public bool ValidarEmail(MusicoVM musico) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return !db.usuario.Any(x => x.Email == this.Email && x.ID != musico.ID);
                }
            }
            catch { return true; }
        }

        public bool ValidarNomeUsuario(MusicoVM musico) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return !db.usuario.Any(x => x.Username.ToLower() == this.Username.ToLower() && x.ID != musico.ID);
                }
            }
            catch { return true; }
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

        private string GetJsonGeneros() 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var generos = db.usuario.First(x => x.ID == this.Musico.ID).genero_musical.Select(x => x.ID);

                    return JsonConvert.SerializeObject(generos);
                }
            }
            catch { }

            return null;
        }

        public List<hab_musical_tipo> GetTipoHabilidades() 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.hab_musical_tipo.ToList();
                }
            }
            catch { }

            return null;
        }

        private string GetJsonHabilidades() 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var habilidades = db.usuario.First(x => x.ID == this.Musico.ID).hab_musical.Select(x => x.ID);

                    return JsonConvert.SerializeObject(habilidades);
                }
            }
            catch { }

            return null;
        }

        public List<hab_musical> GetHabilidades() 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.hab_musical.ToList();
                }
            }
            catch { }

            return null;
        }

        public bool SaveChanges(MusicoVM musico) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.Single(x => x.ID == musico.ID);
                    var m = db.usuario_musico.SingleOrDefault(x => x.IDUsuario == musico.ID);

                    u.Email = this.Email;
                    u.Nascimento = this.Nascimento;
                    u.Nome = this.Nome;
                    u.Telefone = this.Telefone;
                    u.Username = this.Username;

                    if (m == null)
                    {
                        m = new usuario_musico
                        {
                            CPF = Regex.Replace(this.CPF, @"[^0-9]", string.Empty)
                        };
                    }

                    m.Descricao = this.Descricao;
                    m.IDUsuario = u.ID;
                    m.NomeArtistico = this.NomeArtistico;
                    m.TipoUsuario = u.Tipo;

                    this.SetJsonGeneros(u);
                    this.SetJsonHabilidades(u);

                    if (db.usuario_musico.Any(x => x.IDUsuario == u.ID))
                        db.ObjectStateManager.ChangeObjectState(m, System.Data.EntityState.Modified);
                    else
                        db.usuario_musico.AddObject(m);

                    db.SaveChanges();

                    db.ObjectStateManager.ChangeObjectState(u, System.Data.EntityState.Modified);
                    db.SaveChanges();

                    return true;
                }
            }
            catch { }

            return false;
        }

        private void SetJsonHabilidades(usuario usuario) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.Single(x => x.ID == usuario.ID);

                    var dbHabs = u.hab_musical.ToList();

                    for (int i = 0; i < dbHabs.Count; i++)
                    {
                        var hab = dbHabs[i];
                        u.hab_musical.Remove(hab);
                    }

                    var habIds = JsonConvert.DeserializeObject<List<int>>(this.JsonHabilidades);

                    for (int i = 0; i < habIds.Count; i++)
                    {
                        var hab = habIds[i];
                        var dbHab = db.hab_musical.Single(x => x.ID == hab);

                        u.hab_musical.Add(dbHab);
                    }

                    db.SaveChanges();
                }
            }
            catch { }
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
    }
}