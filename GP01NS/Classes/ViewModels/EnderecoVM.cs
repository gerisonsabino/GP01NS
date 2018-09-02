using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Classes.ViewModels
{
    public class EnderecoVM
    {
        public int IDEndereco { get; set; }
        public int IDMunicipio { get; set; }
        public string Numero { get; set; }
        public string CEP { get; set; }
        public string Complemento { get; set; }
        public string Logradouro { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        public string Latitude { get; set; }
        public string Longitude { get; set; }
        public string UF { get; set; }

        public EnderecoVM(endereco endereco)
        {
            if (endereco != null)
            {
                this.Bairro = endereco.Bairro;
                this.CEP = endereco.CEP;
                this.Cidade = endereco.Cidade;
                this.Complemento = endereco.Complemento;
                this.IDEndereco = endereco.ID;
                this.IDMunicipio = endereco.IDMunicipio;
                this.Latitude = endereco.Latitude;
                this.Logradouro = endereco.Logradouro;
                this.Longitude = endereco.Longitude;
                this.Numero = endereco.Numero;
                this.UF = endereco.UF;
            }
        }

        public SelectList GetUFs()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var ufs = db.endereco_uf.ToList();

                    if (!string.IsNullOrEmpty(this.UF))
                        return new SelectList(ufs, "UF", "Descricao", this.UF);
                    else
                        return new SelectList(ufs, "UF", "Descricao");
                }
            }
            catch { }

            return null;
        }
    }
}