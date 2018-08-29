using GP01NS.Classes.Util;
using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.ViewModels.Entrar
{
    public class CadastroVM
    {
        public int ID { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Senha { get; set; }
        public string Confirmacao { get; set; }
        public int Tipo { get; set; }

        public CadastroVM()
        {
            this.ID = int.MinValue;
            this.Nome = string.Empty;
            this.Email = string.Empty;
            this.Senha = string.Empty;
            this.Confirmacao = string.Empty;
            this.Tipo = 2; //Fã
        }

        public bool ValidarEmail()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return !db.usuario.Any(x => x.Email == Email && x.Confirmado);
                }
            }
            catch { return true; }
        }

        public bool ValidarSenha()
        {
            return this.Senha == this.Confirmacao;
        }

        public bool SaveChanges()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var data = DateTime.Now;

                    var u = new usuario
                    {
                        Ativo = true,
                        Confirmado = false,
                        Cadastro = data,
                        Email = this.Email,
                        Nascimento = DateTime.MinValue,
                        Nome = this.Nome,
                        Senha = Criptografia.GetHash128(this.Senha),
                        Telefone = string.Empty,
                        Tipo = db.usuario_tipo.First(x => x.ID == this.Tipo).ID
                    };

                    db.usuario.AddObject(u);
                    db.SaveChanges();

                    var msg = new MensagemEmail();

                    var req = new requisicao
                    {
                        Ativa = true,
                        Data = msg.Data,
                        Hash = msg.Hash,
                        usuario = u,
                        Vencimento = DateTime.MinValue,
                        TipoRequisicao = 2 // 2 - Confirmação de Conta
                    };

                    db.requisicao.AddObject(req);

                    msg.MensagemCadastro(req);

                    db.SaveChanges();

                    return true;
                }
            }
            catch { return false; }
        }
    }
}