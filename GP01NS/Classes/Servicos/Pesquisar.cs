using GP01NS.Models;
using GP01NSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.Servicos
{
    public static class Pesquisar
    {
        public static string EventosJSON(string nome, int idAmbientacao) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var eventos = db.evento.Where(x =>
                        (!string.IsNullOrEmpty(nome) ? x.Titulo.ToLower().Contains(nome.ToLower()) : true)
                        && (idAmbientacao > 0 ? x.usuario_estabelecimento.ambientacao.ID == idAmbientacao : true)
                    ).ToList();

                    var resultados = new List<Resultado>();

                    for (int i = 0; i < eventos.Count; i++)
                    {
                        var evento = eventos[i];

                        Resultado r = new Resultado
                        {
                            ID = evento.ID,
                            Descricao = evento.Titulo,
                            Tipo = "Evento"
                        };

                        resultados.Add(r);
                    }

                    return JsonConvert.SerializeObject(resultados);
                }
            }
            catch { }

            return string.Empty;
        }

        public static string EstabelecimentosJSON(string nome, int idAmbientacao)
        {
            //var estabelecimentos = db.usuario_estabelecimento.Where(x =>
            //    (!string.IsNullOrEmpty(nome) ? x.usuario.Nome.ToLower().Contains(nome.ToLower()) : true)
            //    && (idAmbientacao > 0 ? x.ambientacao.ID == idAmbientacao : true)
            //).ToList();

            return string.Empty;
        }

        public static string MusicosJSON(string nome, int idGenero)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var musicos = db.usuario_musico.Where(x =>
                        (!string.IsNullOrEmpty(nome) ? x.NomeArtistico.ToLower().Contains(nome.ToLower()) : true)
                        && (idGenero > 0 ? x.usuario.genero_musical.Any(y => y.ID == idGenero) : true)
                        ).ToList();

                    var resultados = new List<Resultado>();

                    for (int i = 0; i < musicos.Count; i++)
                    {
                        var usuario_musico = musicos[i];

                        Resultado r = new Resultado
                        {
                            ID = usuario_musico.IDUsuario,
                            Descricao = usuario_musico.NomeArtistico,
                            Tipo = "Músico"
                        };

                        resultados.Add(r);
                    }

                    return JsonConvert.SerializeObject(resultados);
                }
            }
            catch { }

            return string.Empty;
        }

        internal class Resultado
        {
            public int ID { get; set; }
            public string Descricao { get; set; }
            public string Tipo { get; set; }
        }
    }
}