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

        public bool ValidarEmail(UsuarioVM usuario)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return !db.usuario.Any(x => x.Email == this.Email && x.ID != usuario.ID);
                }
            }
            catch { return true; }
        }

        public bool ValidarNomeUsuario(UsuarioVM usuario) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return !db.usuario.Any(x => x.Username.ToLower() == this.Username.ToLower() && x.ID != usuario.ID);
                }
            }
            catch { return true; }
        }

        public string GetEnderecoString()
        {
            if (this.Endereco != null)
                return  this.Endereco.Logradouro + ", " + this.Endereco.Numero + " - " + this.Endereco.Bairro + " - " + this.Endereco.Cidade + " - " + this.Endereco.UF + " - " + this.Endereco.CEP.Insert(5, "-");

            else
                return string.Empty;
        }

        public List<string> GetImagens()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.usuario.First(x => x.ID == this.ID).imagem.Where(x => x.TipoImagem == 3).Select(x => x.Diretorio).Take(4).ToList();
                }
            }
            catch {  }
            
            return null;
        }

        public string GetImagemPerfil()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return "https://gerisonsabino.azurewebsites.net" + db.usuario.First(x => x.ID == this.ID).imagem.Last(x => x.TipoImagem == 1).Diretorio;
                }
            }
            catch { return "#"; }
        }

        public string GetImagemCapa()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return "https://gerisonsabino.azurewebsites.net" + db.usuario.First(x => x.ID == this.ID).imagem.Last(x => x.TipoImagem == 2).Diretorio;
                }
            }
            catch { return "/Imagens/Views/Estabelecimento/bg-estabelecimento.jpg"; }
        }

        private endereco GetEnderecoByIDUsuario(int id) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.usuario.FirstOrDefault(x => x.ID == id).endereco.FirstOrDefault();
                }
            }
            catch
            {
                return null;
            }
        }
    }
}