using GP01NS.Classes.ViewModels;
using GP01NS.Models;
using GP01NSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.Servicos
{
    public static class Pesquisa
    {
        public static string Pesquisar(string termo, string entidades, string generos, string ambientacoes, string habilidades)
        {
            List<int> tipos, idGeneros, idHabilidades, idAmbientacoes;

            tipos = JsonConvert.DeserializeObject<List<int>>(!string.IsNullOrEmpty(entidades) ? entidades : "[]");
            idGeneros = JsonConvert.DeserializeObject<List<int>>(!string.IsNullOrEmpty(generos) ? generos : "[]");
            idHabilidades = JsonConvert.DeserializeObject<List<int>>(!string.IsNullOrEmpty(habilidades) ? habilidades : "[]");
            idAmbientacoes = JsonConvert.DeserializeObject<List<int>>(!string.IsNullOrEmpty(ambientacoes) ? ambientacoes : "[]");

            Resultados r = new Resultados();

            if (tipos.Any(x => x == 2 || x == 1))
                r.Estabelecimentos = EstabelecimentosJSON(termo, idAmbientacoes, tipos.Any(x => x == 1));
            else
                r.Estabelecimentos = string.Empty;

            if (tipos.Any(x => x == 3 || x == 1))
                r.Eventos = EventosJSON(termo, idAmbientacoes, tipos.Any(x => x == 1));
            else
                r.Eventos = string.Empty;

            if (tipos.Any(x => x == 4))
                r.Musicos = MusicosJSON(termo, idGeneros, idHabilidades);
            else
                r.Musicos = string.Empty;

            return JsonConvert.SerializeObject(r);
        }


        private static string EstabelecimentosJSON(string termo, List<int> ambientacoes, bool endereco)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var estabelecimentos = new List<usuario_estabelecimento>();

                    estabelecimentos.AddRange(db.usuario_estabelecimento.Where(x => (!string.IsNullOrEmpty(termo) ? x.usuario.Nome.ToLower().Contains(termo.ToLower()) : false)).ToList());

                    if (endereco)
                    {
                        var ids = estabelecimentos.Select(x => x.IDUsuario);
                        estabelecimentos.AddRange(db.usuario_estabelecimento.Where(x => !ids.Contains(x.IDUsuario) && x.usuario.endereco.Any(y => y.Logradouro.ToLower().Contains(termo.ToLower()) || y.Bairro.ToLower().Contains(termo.ToLower()) || y.Cidade.ToLower().Contains(termo.ToLower()) || y.UF.ToLower().Contains(termo.ToLower()) || y.CEP.Contains(termo))).ToList());
                    }

                    estabelecimentos = estabelecimentos.Where(x => (ambientacoes.Count > 0 ? ambientacoes.Contains(x.IDAmbientacao) : true)).ToList();

                    var resultados = new List<Resultado>();

                    for (int i = 0; i < estabelecimentos.Count; i++)
                    {
                        var e = estabelecimentos[i];

                        Resultado r = new Resultado
                        {
                            ID = e.IDUsuario,
                            Nome = e.usuario.Nome,
                            Username = e.usuario.Username
                        };

                        try
                        {
                            r.Imagem = e.usuario.imagem.Last(x => x.TipoImagem == 1).Diretorio;
                        }
                        catch
                        {
                            r.Imagem = "/Imagens/Views/user.svg";
                        }

                        if (endereco)
                        {
                            var usuario = new UsuarioVM(e.usuario);
                            r.Endereco = usuario.GetEnderecoString();
                        }

                        resultados.Add(r);
                    }

                    if (estabelecimentos.Count > 0)
                        return JsonConvert.SerializeObject(resultados);
                }
            }
            catch { }

            return string.Empty;
        }

        private static string EventosJSON(string termo, List<int> ambientacoes, bool endereco) 
        {
            try  
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var eventos = new List<evento>();

                    //Filtragem por NOME
                    eventos = db.evento.Where(x => (!string.IsNullOrEmpty(termo) ? x.Titulo.ToLower().Contains(termo.ToLower()) : false) && x.Publicado && x.Ativo).ToList();

                    //Filtragem por ENDEREÇO
                    if (endereco)
                    {
                        var ids = eventos.Select(x => x.IDUsuario);
                        eventos.AddRange(db.evento.Where(x => x.Publicado && x.Ativo && x.usuario_estabelecimento.usuario.endereco.Any(y => y.Logradouro.ToLower().Contains(termo.ToLower()) || y.Bairro.ToLower().Contains(termo.ToLower()) || y.Cidade.ToLower().Contains(termo.ToLower()) || y.UF.ToLower().Contains(termo.ToLower()) || y.CEP.Contains(termo))).ToList());
                    }

                    //Filtragem por AMBIENTAÇÃO
                    eventos = eventos.Where(x => (ambientacoes.Count > 0 ? ambientacoes.Contains(x.usuario_estabelecimento.IDAmbientacao) : true)).ToList();
                
                    var resultados = new List<Resultado>();

                    for (int i = 0; i < eventos.Count; i++)
                    {
                        var evento = eventos[i];

                        Resultado r = new Resultado
                        {
                            ID = evento.ID,
                            Nome = evento.Titulo,
                            Username = string.Empty
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

                        if (endereco)
                        {
                            var usuario = new UsuarioVM(evento.usuario_estabelecimento.usuario);

                            if (usuario.GetEnderecoString().ToLower().Contains(termo.ToLower()))
                            {
                                r.Endereco = usuario.GetEnderecoString();
                                resultados.Add(r);
                            }
                        }
                        else
                        {
                            resultados.Add(r);
                        }
                    }

                    if (eventos.Count > 0)
                        return JsonConvert.SerializeObject(resultados);
                }
            }
            catch { }

            return string.Empty;
        }

        private static string MusicosJSON(string termo, List<int> generos, List<int> habilidades)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var musicos = db.usuario_musico.Where(x =>
                        (!string.IsNullOrEmpty(termo) ? x.NomeArtistico.ToLower().Contains(termo.ToLower()) : true)
                            && (generos.Count > 0 ? x.usuario.genero_musical.Any(y => generos.Contains(y.ID)) : true)
                            && (habilidades.Count > 0 ? x.usuario.hab_musical.Any(y => habilidades.Contains(y.ID)) : true)
                        ).ToList();

                    var resultados = new List<Resultado>();

                    for (int i = 0; i < musicos.Count; i++) 
                    {
                        var usuario_musico = musicos[i];

                        Resultado r = new Resultado
                        {
                            ID = usuario_musico.IDUsuario,
                            Nome = usuario_musico.NomeArtistico,
                            Username = usuario_musico.usuario.Username
                        };

                        try
                        {
                            r.Imagem = usuario_musico.usuario.imagem.Last(x => x.TipoImagem == 1).Diretorio;
                        }
                        catch
                        {
                            r.Imagem = "/Imagens/Views/user.svg";
                        }

                        resultados.Add(r);
                    }

                    if (musicos.Count > 0)
                        return JsonConvert.SerializeObject(resultados);
                }
            }
            catch { }

            return string.Empty;
        }

        internal class Resultados 
        {
            public string Estabelecimentos { get; set; }
            public string Eventos { get; set; }
            public string Musicos { get; set; }
        }

        internal class Resultado 
        {
            public int ID { get; set; }
            public string Nome { get; set; }
            public string Username { get; set; }
            public string Endereco { get; set; }
            public string Imagem { get; set; }
        }
    }
}