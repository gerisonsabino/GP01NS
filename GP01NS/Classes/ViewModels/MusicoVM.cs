using GP01NS.Models;
using GP01NSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.ViewModels
{
    public class MusicoVM : UsuarioVM
    {
        public string NomeArtistico { get; set; }
        public string CPF { get; set; }
        public string Descricao { get; set; }

        private readonly usuario_musico Musico;

        public MusicoVM(usuario usuario) : base(usuario)
        {
            this.Musico = GetMusicoByID(this.ID);

            if (this.Musico != null)
            {
                this.NomeArtistico = this.Musico.NomeArtistico;
                this.CPF = this.Musico.CPF;
                this.Descricao = this.Musico.Descricao;
            }
            else
            {
                this.NomeArtistico = string.Empty;
                this.CPF = string.Empty;
                this.Descricao = string.Empty;
            }
        }

        public List<string> GetGenerosMusicais()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.usuario.First(x => x.ID == this.ID).genero_musical.Select(x => x.Descricao).ToList();
                }
            }
            catch { }

            return null;
        }

        public List<string> GetHabilidades()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                   return db.usuario.First(x => x.ID == this.ID).hab_musical.Select(x => x.Descricao).ToList();
                }
            }
            catch { }

            return null;
        }

        public string GetAgendaJSON()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var e = db.evento.ToList();

                    var eventos = new List<AgendaJSON>();

                    for (int i = 0; i < e.Count; i++)
                        eventos.Add(new AgendaJSON(e[i]));

                    return JsonConvert.SerializeObject(eventos);
                }
            }
            catch { }

            return string.Empty;
        }

        public string GetConvitesJSON()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var m = db.usuario_musico.Single(x => x.IDUsuario == this.ID);
                    var e = m.evento_musico.ToList();

                    var eventos = new List<ConviteJSON>();

                    for (int i = 0; i < e.Count; i++)
                        eventos.Add(new ConviteJSON(e[i]));

                    return JsonConvert.SerializeObject(eventos);
                }
            }
            catch { }

            return string.Empty;
        }

        private usuario_musico GetMusicoByID(int id) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.usuario_musico.FirstOrDefault(x => x.IDUsuario == id);
                }
            }
            catch
            {
                return null;
            }
        }

        internal class AgendaJSON
        {
            public int ID { get; set; }
            public string Estabelecimento { get; set; }
            public string Endereco { get; set; }
            public string Evento { get; set; }
            public string Data { get; set; }

            public AgendaJSON(evento evento)
            {
                this.ID = evento.ID;
                this.Estabelecimento = evento.usuario_estabelecimento.usuario.Nome;
                this.Data = evento.DataDe.ToShortDateString() + " - " + evento.DataAte.ToShortDateString();
                this.Evento = evento.Titulo;
                this.Endereco = new UsuarioVM(evento.usuario_estabelecimento.usuario).GetEnderecoString();
            }
        }

        internal class ConviteJSON
        {
            public int IDEvento { get; set; }
            public string Estabelecimento { get; set; }
            public string Evento { get; set; }
            public string Data { get; set; }
            public bool Confirmado { get; set; }
            public bool Recusado { get; set; }

            public ConviteJSON(evento_musico convite)
            {
                this.IDEvento = convite.IDEvento;
                this.Estabelecimento = convite.evento.usuario_estabelecimento.usuario.Nome;
                this.Data = convite.evento.DataDe.ToShortDateString() + " - " + convite.evento.DataAte.ToShortDateString();
                this.Evento = convite.evento.Titulo;
                this.Confirmado = convite.Confirmado;
                this.Recusado = convite.Recusado;
            }
        }
    }
}