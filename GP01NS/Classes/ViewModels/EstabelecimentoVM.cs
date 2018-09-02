using GP01NS.Models;
using GP01NSLibrary;
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

        private usuario_estabelecimento Estabelecimento;

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
            }
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
                        e = new usuario_estabelecimento();

                    e.CNPJ = Regex.Replace(this.CNPJ, @"[^0-9]", string.Empty);
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
            catch (Exception e) { return false; }
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
}