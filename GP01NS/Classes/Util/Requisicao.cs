using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.Util
{
    public class Requisicao
    {
        public int IDTipo { get; set; }
        public usuario Usuario { get; set; }

        private MensagemEmail Mensagem;

        public Requisicao(usuario usuario, int idTipo)
        {
            this.Usuario = usuario;
            this.IDTipo = idTipo;
            this.Mensagem = new MensagemEmail();
        }

        public bool SaveChanges()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var req = new requisicao
                    {
                        Ativa = true,
                        Data = this.Mensagem.Data,
                        Hash = this.Mensagem.Hash,
                        IDUsuario = this.Usuario.ID,
                        Vencimento = DateTime.Now.AddHours(1),
                        TipoRequisicao = this.IDTipo,
                        TipoUsuario = this.Usuario.Tipo
                    };

                    db.requisicao.AddObject(req);
                    db.SaveChanges();

                    if (EnviarMensagem(req))
                    {
                        return true;
                    }
                }
            }
            catch { }

            return false;
        }

        private bool EnviarMensagem(requisicao req)
        {
            switch (req.TipoRequisicao)
            {
                case 1:
                    return Mensagem.MensagemRedefinirSenha(req);

                case 2:
                    return Mensagem.MensagemCadastro(req);
            }

            return false;
        }
    }
}