using GP01NS.Classes.Util;
using GP01NS.Classes.ViewModels;
using GP01NS.Models;
using GP01NSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace GP01NS.Classes.Servicos
{
    public class Pagamento
    {
        public long ID { get; set; }
        public bool Aprovado { get; set; }
        public DateTime Data { get; set; }
        public DateTime DtPagamento { get; set; }
        public string REF { get; set; }
        public string Transacao { get; set; }
        public decimal Valor { get; set; }
        public DateTime Vencimento { get; set; }

        public readonly usuario Usuario;


        public Pagamento() { }

        public Pagamento(usuario_premium premium)
        {
            this.Aprovado = premium.Aprovado;
            this.Data = premium.Data;
            this.ID = premium.ID;
            this.DtPagamento = premium.Pagamento;
            this.REF = premium.REF;
            this.Transacao = premium.Transacao;
            this.Usuario = premium.usuario;
            this.Valor = premium.Valor;
            this.Vencimento = premium.Vencimento;
        }

        public Pagamento(usuario usuario, decimal valor)
        {
            this.Aprovado = false;
            this.Data = DateTime.MinValue;
            this.ID = int.MinValue;
            this.REF = string.Empty;
            this.Transacao = string.Empty;
            this.Usuario = usuario;
            this.Valor = valor;
            this.Vencimento = DateTime.MinValue;
        }

        public usuario_premium GerarPagamento() 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.First(x => x.ID == this.Usuario.ID);
                    var premium = new usuario_premium();

                    if (u.usuario_premium.Any(x => !x.Aprovado && string.IsNullOrEmpty(x.Transacao)))
                    {
                        premium = u.usuario_premium.First(x => !x.Aprovado && string.IsNullOrEmpty(x.Transacao));
                        premium.Data = DateTime.Now;

                        db.ObjectStateManager.ChangeObjectState(premium, System.Data.EntityState.Modified);
                    }
                    else
                    {
                        premium.Aprovado = false;
                        premium.Data = DateTime.Now;
                        premium.REF = Criptografia.Criptografar(this.Usuario.ID.ToString() + premium.Data.ToString("ddMMyyyyhhmmss") + this.Usuario.Tipo.ToString());
                        premium.IDUsuario = u.ID;
                        premium.Pagamento = DateTime.MinValue;
                        premium.TipoUsuario = u.Tipo;
                        premium.Transacao = string.Empty;
                        premium.Valor = this.Valor;
                        premium.Vencimento = DateTime.MinValue;

                        db.usuario_premium.AddObject(premium);
                    }

                    db.SaveChanges();

                    return premium;
                }
            }
            catch (Exception e) { }

            return null;
        }

        public static Pagamento AtualizarPagamento(XmlDocument xml)
        {
            try
            {
                string referencia = xml.GetElementsByTagName("reference")[0].InnerText.ToString().Replace("-", string.Empty);
                string code = xml.GetElementsByTagName("code")[0].InnerText.ToString().Replace("-", string.Empty);
                string status = xml.GetElementsByTagName("status")[0].InnerText.ToString();
                DateTime lastEventDate = DateTime.Parse(xml.GetElementsByTagName("lastEventDate")[0].InnerText.ToString());

                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var premium = db.usuario_premium.FirstOrDefault(x => x.REF.Equals(referencia));

                    if (premium != null)
                    {
                        if (!premium.Aprovado)
                        {
                            premium.Aprovado = status == "3";
                            premium.Pagamento = lastEventDate;

                            if (string.IsNullOrEmpty(premium.Transacao))
                                premium.Transacao = code;

                            premium.Vencimento = premium.Pagamento.AddDays(30);

                            db.ObjectStateManager.ChangeObjectState(premium, System.Data.EntityState.Modified);
                            db.SaveChanges();
                        }

                        return new Pagamento(premium);
                    }
                }
            }
            catch (Exception e) { }

            return null;
        }

        public static string GetPagamentosJSON()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var pagamentos = db.usuario_premium.Where(x => !string.IsNullOrEmpty(x.Transacao)).OrderByDescending(x => x.Pagamento).ToList();

                    List<PagamentoJSON> lista = new List<PagamentoJSON>();

                    foreach (var item in pagamentos)
                    {
                        lista.Add(new PagamentoJSON(item));
                    }

                    return JsonConvert.SerializeObject(lista);
                }
            }
            catch (Exception e) { }

            return string.Empty;
        }

        internal class PagamentoJSON
        {
            public string Status { get; set; }
            public string Data { get; set; }
            public string DtPagamento { get; set; }
            public string REF { get; set; }
            public string Transacao { get; set; }
            public string Valor { get; set; }
            public string Vencimento { get; set; }
            public int IDUsuario { get; set; }
            public string Usuario { get; set; }
            public string TipoUsuario { get; set; }

            public PagamentoJSON(usuario_premium premium)
            {
                this.Status = premium.Aprovado ? "Aprovado" : "Pendente";
                this.Data = premium.Data.ToShortDateString();
                this.DtPagamento = premium.Pagamento.ToShortDateString();
                this.REF = premium.REF;
                this.Transacao = premium.Transacao;
                this.Valor = premium.Valor.ToString("C");
                this.Vencimento = premium.Vencimento.ToShortDateString();
                this.Usuario = premium.usuario.Username;
                this.IDUsuario = premium.usuario.ID;
                this.TipoUsuario = premium.usuario.Tipo == 4 ? "Músico" : "Estabelecimento";
            }
        }
    }
}