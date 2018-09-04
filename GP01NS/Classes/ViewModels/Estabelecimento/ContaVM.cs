using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Classes.ViewModels.Estabelecimento
{
    public class ContaVM
    {
        public string Nome { get; set; }
        public string Username { get; set; }
        public string Email { get; set; }
        public string CNPJ { get; set; }
        public string Razao { get; set; }
        public string Descricao { get; set; }
        public int De { get; set; }
        public int Ate { get; set; }
        public int Das { get; set; }
        public int As { get; set; }
        public int IDAmbientacao { get; set; }

        public ContaVM() { }

        public ContaVM(EstabelecimentoVM estabelecimento)
        {
            if (estabelecimento != null)
            {
                this.Nome = estabelecimento.Nome;
                this.Username = estabelecimento.Username;
                this.Email = estabelecimento.Email;
                this.As = estabelecimento.As;
                this.Ate = estabelecimento.Ate;
                this.Das = estabelecimento.Das;
                this.De = estabelecimento.De;
                this.Descricao = estabelecimento.Descricao;
                this.CNPJ = estabelecimento.CNPJ;
                this.Razao = estabelecimento.Razao;
                this.IDAmbientacao = estabelecimento.IDAmbientacao;
            }
            else
            {
                this.Nome = string.Empty;
                this.Nome = string.Empty;
                this.Username = string.Empty;
                this.Email = string.Empty;
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

        public bool ValidarEmail(EstabelecimentoVM estabelecimento)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return !db.usuario.Any(x => x.Email == this.Email && x.ID != estabelecimento.ID);
                }
            }
            catch { return true; }
        }

        public bool ValidarNomeUsuario(EstabelecimentoVM estabelecimento)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return !db.usuario.Any(x => x.Username.ToLower() == this.Username.ToLower() && x.ID != estabelecimento.ID);
                }
            }
            catch { return true; }
        }

        public SelectList GetAmbientacoes()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var ambientacoes = db.ambientacao.ToList();

                    return new SelectList(ambientacoes, "ID", "Descricao", this.IDAmbientacao);
                }
            }
            catch { }

            return null;
        }

        public SelectList GetDias(int dia) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var dias = db.usuario_estabelecimento_dias.ToList();

                    return new SelectList(dias, "ID", "Descricao", dia);
                }
            }
            catch { }

            return null;
        }

        public SelectList GetHoras(int hora) 
        {
            var horas = new List<Horas>();

            for (int i = 0; i <= 23; i++) 
            {
                var hr = new Horas
                {
                    ID = i,
                    Hora = i.ToString("D2") + ":00"
                };

                horas.Add(hr);
            }

            return new SelectList(horas, "ID", "Hora", hora);
        }

        public bool SaveChanges(EstabelecimentoVM estabelecimento)
        {
            estabelecimento.As = this.As;
            estabelecimento.Ate = this.Ate;
            estabelecimento.CNPJ = this.CNPJ;
            estabelecimento.Das = this.Das;
            estabelecimento.De = this.De;
            estabelecimento.Descricao = this.Descricao;
            estabelecimento.Email = this.Email;
            estabelecimento.IDAmbientacao = this.IDAmbientacao;
            estabelecimento.Nome = this.Nome;
            estabelecimento.Razao = this.Razao;
            estabelecimento.Username = this.Username;

            return estabelecimento.SaveChanges();
        }

        internal class Horas 
        {
            public int ID { get; set; }
            public string Hora { get; set; }
        }
    }
}