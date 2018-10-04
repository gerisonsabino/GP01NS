using GP01NS.Models;
using GP01NSLibrary;
using Newtonsoft.Json;
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
                return this.Endereco.Logradouro + ", " + this.Endereco.Numero + " - " + this.Endereco.Bairro + " - " + this.Endereco.Cidade + ", " + this.Endereco.UF + " - " + this.Endereco.CEP.Insert(5, "-");

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
                    return "http://nossoshow.gerison.net" + db.usuario.First(x => x.ID == this.ID).imagem.Last(x => x.TipoImagem == 1).Diretorio;
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
                    return "http://nossoshow.gerison.net" + db.usuario.First(x => x.ID == this.ID).imagem.Last(x => x.TipoImagem == 2).Diretorio;
                }
            }
            catch { return "/Imagens/Views/Estabelecimento/bg-estabelecimento.jpg"; }
        }

        public bool ToggleSeguir(int idUsuario)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.Single(x => x.ID == this.ID);
                    var s = db.usuario.Single(x => x.ID == idUsuario);

                    if (u.usuario2.Any(x => x.ID == idUsuario))
                        u.usuario2.Remove(s);
                    else
                        u.usuario2.Add(s);

                    db.ObjectStateManager.ChangeObjectState(u, System.Data.EntityState.Modified);
                    db.SaveChanges();

                    return true;
                }
            }
            catch { }

            return false;
        }

        public bool Seguindo(int idUsuario)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.Single(x => x.ID == this.ID);

                    return u.usuario2.Any(x => x.ID == idUsuario);
                }
            }
            catch { }

            return false;
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

        public string GetSeguindoJSON()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.Single(x => x.ID == this.ID);
                    var l = u.usuario2.ToList();

                    var estabelecimentos = l.Where(x => x.Tipo == 2).Select(x => x.usuario_estabelecimento.FirstOrDefault()).ToList();
                    //var eventos = l.Where(x => x.Tipo == 2).Select(x => x.usuario_estabelecimento.FirstOrDefault()).ToList();
                    var musicos = l.Where(x => x.Tipo == 4).Select(x => x.usuario_musico.FirstOrDefault()).ToList();

                    var seguindo = new List<SeguindoJSON>();

                    for (int i = 0; i < l.Count; i++)
                    {
                        SeguindoJSON s = new SeguindoJSON
                        {
                            Estabelecimentos = GetSeguindoEstabelecimentosJSON(estabelecimentos),
                            //Eventos = GetSeguindoEventosJSON(),
                            Musicos = this.GetSeguindoMusicosJSON(musicos)
                        };

                        seguindo.Add(s);
                    }

                    return JsonConvert.SerializeObject(seguindo);
                }
            }
            catch { }

            return string.Empty;
        }

        private string GetSeguindoMusicosJSON(List<usuario_musico> musicos)
        {
            var resultados = new List<Resultado>();

            for (int i = 0; i < musicos.Count; i++)
            {
                var m = musicos[i];

                Resultado r = new Resultado
                {
                    ID = m.IDUsuario,
                    Nome = m.NomeArtistico,
                    Username = m.usuario.Username,
                    Badges = m.usuario.genero_musical.Select(x => x.Descricao).ToList(),
                    Tipo = "Músico"
                };

                try
                {
                    r.Imagem = m.usuario.imagem.Last(x => x.TipoImagem == 1).Diretorio;
                }
                catch
                {
                    r.Imagem = "/Imagens/Views/user.svg";
                }

                resultados.Add(r);
            }

            return JsonConvert.SerializeObject(resultados);
        }

        private string GetSeguindoEstabelecimentosJSON(List<usuario_estabelecimento> estabelecimentos)
        {
            var resultados = new List<Resultado>();

            for (int i = 0; i < estabelecimentos.Count; i++)
            {
                var e = estabelecimentos[i];

                Resultado r = new Resultado
                {
                    ID = e.IDUsuario,
                    Nome = e.usuario.Nome,
                    Username = e.usuario.Username,
                    Tipo = "Estabelecimento"
                };

                try
                {
                    r.Imagem = e.usuario.imagem.Last(x => x.TipoImagem == 2).Diretorio;
                }
                catch
                {
                    r.Imagem = "/Imagens/Views/user.svg";
                }

                var usuario = new UsuarioVM(e.usuario);
                r.Endereco = usuario.GetEnderecoString();

                r.Badges = new List<string>();
                r.Badges.Add(e.ambientacao.Descricao);

                resultados.Add(r);
            }

            return JsonConvert.SerializeObject(resultados);
        }

        private string GetSeguindoEventosJSON(List<evento> eventos)
        {
            var resultados = new List<Resultado>();

            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    for (int i = 0; i < eventos.Count; i++)
                    {
                        var evento = eventos[i];

                        Resultado r = new Resultado
                        {
                            ID = evento.ID,
                            Nome = evento.Titulo,
                            Username = string.Empty,
                            Tipo = "Evento"
                        };

                        try
                        {
                            //   r.Imagem = evento.usuario.imagem.Last(x => x.TipoImagem == 1).Diretorio;/
                            r.Imagem = "/Imagens/Views/user.svg";
                        }
                        catch
                        {
                            r.Imagem = string.Empty;
                        }

                        var usuario = new UsuarioVM(evento.usuario_estabelecimento.usuario);
                        r.Endereco = usuario.GetEnderecoString();
                        resultados.Add(r);
                    }
                }
            }
            catch { }

            return JsonConvert.SerializeObject(resultados);
        }

        internal class SeguindoJSON
        {
            public string Estabelecimentos { get; set; }
            public string Eventos { get; set; }
            public string Musicos { get; set; }
        }

        internal class Resultado
        {
            public int ID { get; set; }
            public string Nome { get; set; }
            public string Tipo { get; set; }
            public string Username { get; set; }
            public string Endereco { get; set; }
            public string Imagem { get; set; }
            public List<string> Badges { get; set; }
        }
    }
}