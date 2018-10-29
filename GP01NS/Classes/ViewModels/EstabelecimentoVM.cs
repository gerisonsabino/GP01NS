using GP01NS.Models;
using GP01NSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Classes.ViewModels
{
    public class EstabelecimentoVM : UsuarioVM
    {
        public string CNPJ { get; set; }
        public string Razao { get; set; }
        public string Descricao { get; set; }
        public int De { get; set; }
        public int Ate { get; set; }
        public int Das { get; set; }
        public int As { get; set; }
        public int IDAmbientacao { get; set; }
        public RedesSociaisVM RedesSociais { get; set; }

        private readonly usuario_estabelecimento Estabelecimento;

        public EstabelecimentoVM(usuario usuario) : base(usuario)
        {
            this.Estabelecimento = this.GetEstabelecimentoByID(this.ID);

            if (this.Estabelecimento != null)
            {
                this.As = this.Estabelecimento.As;
                this.Ate = this.Estabelecimento.Ate;
                this.Das = this.Estabelecimento.Das;
                this.De = this.Estabelecimento.De;
                this.Descricao = this.Estabelecimento.Descricao;
                this.CNPJ = this.Estabelecimento.CNPJ;
                this.Razao = this.Estabelecimento.Razao;
                this.IDAmbientacao = this.Estabelecimento.IDAmbientacao;
                this.RedesSociais = this.GetRedesSociais();
            }
            else
            {
                this.As = 0;
                this.Ate = 1;
                this.Das = 0;
                this.De = 1;
                this.Descricao = string.Empty;
                this.CNPJ = string.Empty;
                this.Razao = string.Empty;
                this.IDAmbientacao = 1;
                this.RedesSociais = new RedesSociaisVM();
            }
        }

        public string GetEventosJSON()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario_estabelecimento.Single(x => x.IDUsuario == this.Estabelecimento.IDUsuario);
                    var e = u.evento.ToList();

                    var eventos = new List<EventoJSON>();

                    for (int i = 0; i < e.Count; i++)
                        eventos.Add(new EventoJSON(e[i]));

                    return JsonConvert.SerializeObject(eventos);
                }
            }
            catch { }

            return string.Empty;
        }

        public string GetFuncionamentoString()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var dias = db.usuario_estabelecimento_dias.ToList();

                    return dias.First(x => x.ID == this.De).Descricao.Substring(0, 3) + " à " + dias.First(x => x.ID == this.Ate).Descricao.Substring(0, 3) + " - das " + this.Das.ToString("D2") + "h às " + this.As.ToString("D2") + "h"; 
                }
            }
            catch { }

            return string.Empty;
        }

        public string GetAmbientacao()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.ambientacao.First(x => x.ID == this.IDAmbientacao).Descricao;
                }
            }
            catch { }

            return string.Empty;
        }

        public void ContarVisualizacao()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario_estabelecimento.First(x => x.IDUsuario == this.ID);

                    u.Visualizacoes += 1;

                    db.ObjectStateManager.ChangeObjectState(u, System.Data.EntityState.Modified);
                    db.SaveChanges();
                }
            }
            catch { }
        }

        private RedesSociaisVM GetRedesSociais()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.Single(x => x.ID == this.Usuario.ID);

                    if (u.usuario_redes_sociais != null)
                        return new RedesSociaisVM(u.usuario_redes_sociais);
                }
            }
            catch { }

            return null;
        }

        public bool SaveChanges() 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.Single(x => x.ID == this.Usuario.ID);
                    var e = db.usuario_estabelecimento.SingleOrDefault(x => x.IDUsuario == this.Usuario.ID);

                    u.Email = this.Email;
                    u.Nome = this.Nome;
                    u.Username = this.Username;

                    if (e == null)
                    {
                        e = new usuario_estabelecimento();
                        e.CNPJ = Regex.Replace(this.CNPJ, @"[^0-9]", string.Empty);
                    }

                    e.De = this.De;
                    e.Ate = this.Ate;
                    e.Das = this.Das;
                    e.As = this.As;
                    e.Descricao = this.Descricao;
                    e.IDAmbientacao = this.IDAmbientacao;
                    e.IDUsuario = this.Usuario.ID;
                    e.Razao = this.Razao;
                    e.TipoUsuario = this.Usuario.Tipo;

                    if (db.usuario_estabelecimento.Any(x => x.IDUsuario == u.ID))
                        db.ObjectStateManager.ChangeObjectState(e, System.Data.EntityState.Modified);
                    else
                        db.usuario_estabelecimento.AddObject(e);

                    db.SaveChanges();

                    db.ObjectStateManager.ChangeObjectState(u, System.Data.EntityState.Modified);
                    db.SaveChanges();

                    return true;
                }
            }
            catch { return false; }
        }

        public evento GetEventoByID(int id)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.usuario_estabelecimento.FirstOrDefault(x => x.IDUsuario == this.ID).evento.FirstOrDefault(x => x.ID == id);
                }
            }
            catch
            {
                return null;
            }
        }

        private usuario_estabelecimento GetEstabelecimentoByID(int id)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.usuario_estabelecimento.FirstOrDefault(x => x.IDUsuario == id);
                }
            }
            catch
            {
                return null;
            }
        }
    }

    internal class EventoJSON 
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Data { get; set; }
        public string Status { get; set; }

        public EventoJSON(evento evento)
        {
            this.Data = evento.DataDe.ToShortDateString() + " - " + evento.DataAte.ToShortDateString();
            this.ID = evento.ID;
            this.Status = evento.Publicado ? "Publicado" : "Não Publicado";
            this.Titulo = evento.Titulo;
        }
    }
}