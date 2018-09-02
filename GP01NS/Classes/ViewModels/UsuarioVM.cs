using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.ViewModels
{
    public class UsuarioVM
    {
        public int ID { get; set; }
        public bool Ativo { get; set; }
        public bool Confirmado { get; set; }
        public DateTime Cadastro { get; set; }
        public DateTime Nascimento { get; set; }
        public string Username { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public string Telefone { get; set; }
        public int TipoUsuario { get; set; }

        public EnderecoVM Endereco { get; set; }

        protected usuario Usuario;

        public UsuarioVM() { }

        public UsuarioVM(usuario usuario)
        {
            this.Usuario = usuario;

            this.Ativo = usuario.Ativo;
            this.Cadastro = usuario.Cadastro;
            this.Confirmado = usuario.Confirmado;
            this.Email = usuario.Email;
            this.ID = usuario.ID;
            this.Nascimento = usuario.Nascimento;
            this.Nome = usuario.Nome;
            this.Telefone = usuario.Telefone;
            this.TipoUsuario = usuario.Tipo;
            this.Username = usuario.Username;

            this.Endereco = new EnderecoVM(this.GetEnderecoByIDUsuario(this.ID));
        }

        private endereco GetEnderecoByIDUsuario(int id)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.endereco.FirstOrDefault(x => x.IDUsuario == id);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}