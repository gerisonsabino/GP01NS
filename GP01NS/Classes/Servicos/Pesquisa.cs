using GP01NS.Classes.ViewModels;
using GP01NS.Models;
using GP01NSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;

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


            List<Resultado> r = new List<Resultado>();

            if (tipos.Any(x => x == 2 || (x == 1 && tipos.Count == 1)))
                r.AddRange(Estabelecimentos(termo, idAmbientacoes, tipos.Any(x => x == 1)));

            if (tipos.Any(x => x == 3 || (x == 1 && tipos.Count == 1)))
                r.AddRange(Eventos(termo, idAmbientacoes, tipos.Any(x => x == 1)));

            if (tipos.Any(x => x == 4))
                r.AddRange(Musicos(termo, idGeneros, idHabilidades));

            r = r.GroupBy(x => x.ID).Select(y => y.First()).ToList();

            r = r.OrderBy(x => x.Nome).ToList();

            return JsonConvert.SerializeObject(r);
        }

        public static string Sugestoes(int page)
        {
            List<Resultado> r = new List<Resultado>();

            r.AddRange(EstabelecimentosSugestoes(page));
            r.AddRange(EventosSugestoes(page));
            r.AddRange(MusicosSugestoes(page));

            return JsonConvert.SerializeObject(r);
        }

        private static List<Resultado> Estabelecimentos(string termo, List<int> ambientacoes, bool endereco)
        {
            var resultados = new List<Resultado>();

            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var estabelecimentos = new List<usuario_estabelecimento>();

                    estabelecimentos.AddRange(db.usuario_estabelecimento.Where(x => (!string.IsNullOrEmpty(termo) ? x.usuario.Nome.ToLower().Contains(termo.ToLower()) || x.usuario.Username.ToLower() == termo.Replace("@", string.Empty).ToLower() : false)).ToList());

                    if (endereco)
                    {
                        var ids = estabelecimentos.Select(x => x.IDUsuario);
                        estabelecimentos.AddRange(db.usuario_estabelecimento.Where(x => !ids.Contains(x.IDUsuario) && x.usuario.endereco.Any(y => y.Logradouro.ToLower().Contains(termo.ToLower()) || y.Bairro.ToLower().Contains(termo.ToLower()) || y.Cidade.ToLower().Contains(termo.ToLower()) || y.UF.ToLower().Contains(termo.ToLower()) || y.CEP.Contains(termo.Replace("-", string.Empty)))).ToList());
                    }

                    estabelecimentos = estabelecimentos.Where(x => (ambientacoes.Count > 0 ? ambientacoes.Contains(x.IDAmbientacao) : true)).ToList();


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
                            r.Imagem = e.usuario.imagem.Last(x => x.TipoImagem == 1).Diretorio;
                        }
                        catch
                        {
                            r.Imagem = "/Imagens/Views/user.svg";
                        }

                        if (e.usuario.endereco.Any())
                        {
                            r.Endereco = new EnderecoVM(e.usuario.endereco.FirstOrDefault()).ToString();
                        }

                        //var usuario = new UsuarioVM(e.usuario);
                        //r.Endereco = usuario.GetEnderecoString();

                        r.Badges = new List<string>
                        {
                            e.ambientacao.Descricao
                        };

                        resultados.Add(r);
                    }
                }
            }
            catch { }

            return resultados;
        }

        private static List<Resultado> Eventos(string termo, List<int> ambientacoes, bool endereco) 
        {
            var resultados = new List<Resultado>();

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
                        eventos.AddRange(db.evento.Where(x => !ids.Contains(x.IDUsuario) && x.Publicado && x.Ativo && x.usuario_estabelecimento.usuario.endereco.Any(y => y.Logradouro.ToLower().Contains(termo.ToLower()) || y.Bairro.ToLower().Contains(termo.ToLower()) || y.Cidade.ToLower().Contains(termo.ToLower()) || y.UF.ToLower().Contains(termo.ToLower()) || y.CEP.Contains(termo.Replace("-", string.Empty)))).ToList());
                    }

                    //Filtragem por AMBIENTAÇÃO
                    eventos = eventos.Where(x => (ambientacoes.Count > 0 ? ambientacoes.Contains(x.usuario_estabelecimento.IDAmbientacao) : true)).ToList();

                    for (int i = 0; i < eventos.Count; i++)
                    {
                        var evento = eventos[i];
                        var u = evento.usuario_estabelecimento;

                        Resultado r = new Resultado
                        {
                            ID = evento.ID,
                            Nome = evento.Titulo,
                            Username = u.usuario.Username,
                            Tipo = "Evento"
                        };

                        try
                        {
                            r.Imagem = evento.imagem.Last(x => x.TipoImagem == 4).Diretorio;
                        }
                        catch
                        {
                            r.Imagem = "/Imagens/Views/user.svg";
                        }

                        if (evento.usuario_estabelecimento.usuario.endereco.Any())
                        {
                            r.Endereco = new EnderecoVM(evento.usuario_estabelecimento.usuario.endereco.FirstOrDefault()).ToString();
                        }

                        //var usuario = new UsuarioVM(evento.usuario_estabelecimento.usuario);
                        //r.Endereco = usuario.GetEnderecoString();

                        resultados.Add(r);
                    }
                }
            }
            catch { }

            return resultados;
        }

        private static List<Resultado> Musicos(string termo, List<int> generos, List<int> habilidades)
        {
            var resultados = new List<Resultado>();

            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var musicos = db.usuario_musico.Where(x =>
                        (!string.IsNullOrEmpty(termo) ? x.NomeArtistico.ToLower().Contains(termo.ToLower()) || x.usuario.Username.ToLower() == termo.Replace("@", string.Empty).ToLower() : false)
                            && (generos.Count > 0 ? x.usuario.genero_musical.Any(y => generos.Contains(y.ID)) : true)
                            && (habilidades.Count > 0 ? x.usuario.hab_musical.Any(y => habilidades.Contains(y.ID)) : true)
                        ).ToList();


                    for (int i = 0; i < musicos.Count; i++) 
                    {
                        var usuario_musico = musicos[i];

                        Resultado r = new Resultado
                        {
                            ID = usuario_musico.IDUsuario,
                            Nome = usuario_musico.NomeArtistico,
                            Username = usuario_musico.usuario.Username,
                            Badges = usuario_musico.usuario.genero_musical.Select(x => x.Descricao).ToList(),
                            Tipo = "Músico"
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
                }
            }
            catch { }

            return resultados;
        }

        private static List<Resultado> EstabelecimentosSugestoes(int page)
        {
            var resultados = new List<Resultado>();

            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var estabelecimentos = db.usuario_estabelecimento.Where(x => x.usuario.Ativo).OrderBy(x => x.IDUsuario).Skip(4 * page).Take(4).ToList();

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
                            r.Imagem = "http://www.nossoshow.gerison.net" + e.usuario.imagem.Last(x => x.TipoImagem == 1).Diretorio;
                        }
                        catch
                        {
                            r.Imagem = "http://www.nossoshow.gerison.net/Imagens/Views/user.svg";
                        }

                        try
                        {
                            var endereco = e.usuario.endereco.First();
                            r.Endereco = new EnderecoVM(endereco).ToString();
                        }
                        catch
                        {
                            r.Endereco = string.Empty;
                        }

                        try
                        {
                            r.Nota = (int)e.usuario.usuario_avalia_usuario1.Average(x => x.Nota);
                        }
                        catch { r.Nota = 0; }

                        r.Badges = new List<string>
                        {
                            e.ambientacao.Descricao
                        };

                        resultados.Add(r);
                    }
                }
            }
            catch (Exception e) { }

            return resultados;
        }

        private static List<Resultado> MusicosSugestoes(int page)
        {
            var resultados = new List<Resultado>();

            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var musicos = db.usuario_musico.Where(x => x.usuario.Ativo).OrderBy(x => x.IDUsuario).Skip(4 * page).Take(4).ToList();

                    for (int i = 0; i < musicos.Count; i++)
                    {
                        var m = musicos[i];

                        Resultado r = new Resultado
                        {
                            ID = m.IDUsuario,
                            Nome = m.NomeArtistico,
                            Username = m.usuario.Username,
                            Badges = m.usuario.genero_musical.Select(x => x.Descricao).ToList(),
                            Endereco = string.Empty,
                            Tipo = "Músico"
                        };

                        try
                        {
                            r.Imagem = "http://www.nossoshow.gerison.net" + m.usuario.imagem.Last(x => x.TipoImagem == 1).Diretorio;
                        }
                        catch
                        {
                            r.Imagem = "http://www.nossoshow.gerison.net/Imagens/Views/user.svg";
                        }

                        try
                        {
                            r.Nota = (int)m.usuario.usuario_avalia_usuario1.Average(x => x.Nota);
                        }
                        catch { r.Nota = 0; }

                        resultados.Add(r);
                    }
                }
            }
            catch (Exception e) { }

            return resultados;
        }

        private static List<Resultado> EventosSugestoes(int page)
        {
            var resultados = new List<Resultado>();

            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var eventos = db.evento.Where(x => x.usuario_estabelecimento.usuario.Ativo).OrderBy(x => x.IDUsuario).Skip(4 * page).Take(4).ToList();

                    for (int i = 0; i < eventos.Count; i++)
                    {
                        var evento = eventos[i];
                        var u = evento.usuario_estabelecimento;

                        Resultado r = new Resultado
                        {
                            ID = evento.ID,
                            Nome = evento.Titulo,
                            Username = u.usuario.Username,
                            Tipo = "Evento"
                        };

                        try
                        {
                            r.Imagem = "http://www.nossoshow.gerison.net" + evento.imagem.Last(x => x.TipoImagem == 4).Diretorio;
                        }
                        catch
                        {
                            r.Imagem = "http://www.nossoshow.gerison.net/Imagens/Views/user.svg";
                        }

                        try
                        {
                            var endereco = u.usuario.endereco.First();
                            r.Endereco = new EnderecoVM(endereco).ToString();
                        }
                        catch
                        {
                            r.Endereco = string.Empty;
                        }

                        resultados.Add(r);
                    }
                }
            }
            catch (Exception e) { }

            return resultados;
        }

        internal class Resultado 
        {
            public int ID { get; set; }
            public int Nota { get; set; }
            public string Nome { get; set; }
            public string Tipo { get; set; }
            public string Username { get; set; }
            public string Endereco { get; set; }
            public string Imagem { get; set; }
            public List<string> Badges { get; set; }
        }
    }
}