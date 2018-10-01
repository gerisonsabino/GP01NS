using GP01NS.Models;
using GP01NSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Classes.ViewModels
{
    public class EventoVM
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime DataDe { get; set; }
        public DateTime DataAte { get; set; }
        public int HoraDe { get; set; }
        public int HoraAte { get; set; }
        public int MinutoDe { get; set; }
        public int MinutoAte { get; set; }

        public EstabelecimentoVM Estabelecimento;

        public EventoVM()
        {
            this.DataAte = DateTime.MinValue;
            this.DataDe = DateTime.MinValue;
            this.Descricao = string.Empty;
            this.HoraAte = 0;
            this.HoraDe = 0;
            this.ID = 0;
            this.MinutoAte = 0;
            this.MinutoDe = 0;
            this.Titulo = string.Empty;
        }

        public EventoVM(evento evento)
        {
            this.DataAte = evento.DataAte;
            this.DataDe = evento.DataDe;
            this.Descricao = evento.Descricao;
            this.HoraAte = evento.HoraAte;
            this.HoraDe = evento.HoraDe;
            this.ID = evento.ID;
            this.MinutoAte = evento.MinutoAte;
            this.MinutoDe = evento.MinutoDe;
            this.Titulo = evento.Titulo;

            this.Estabelecimento = new EstabelecimentoVM(this.GetEstabelecimentoByID(evento.IDUsuario));
        }

        public SelectList GetHoras(int hora)
        {
            var horas = new List<Horas>();

            for (int i = 0; i <= 23; i++)
            {
                var hr = new Horas
                {
                    ID = i,
                    Hora = i.ToString("D2")
                };

                horas.Add(hr);
            }

            return new SelectList(horas, "ID", "Hora", hora);
        }

        public SelectList GetMinutos(int minuto)
        {
            var minutos = new List<Horas>();

            for (int i = 0; i <= 59; i++)
            {
                var min = new Horas
                {
                    ID = i,
                    Hora = i.ToString("D2")
                };

                minutos.Add(min);
            }

            return new SelectList(minutos, "ID", "Hora", minuto);
        }

        public string GetHorarioString()
        {
            CultureInfo ci = new CultureInfo("pt-BR");
            DateTimeFormatInfo f = ci.DateTimeFormat;

            var dia = this.DataDe.Day;
            var mes = ci.TextInfo.ToTitleCase(f.GetMonthName(this.DataDe.Month));
            var ano = DateTime.Now.Year;
            var diasemana = ci.TextInfo.ToTitleCase(f.GetDayName(this.DataDe.DayOfWeek));

            if (this.DataDe == this.DataAte)
                return this.DataDe.Day + " de " + mes + " de " + this.DataDe.Year + " - das " + this.HoraDe.ToString("D2") + ":" + this.MinutoDe.ToString("D2") + "h às " + this.HoraAte.ToString("D2") + ":" + this.MinutoAte.ToString("D2") + "h";
            else if (this.DataDe != this.DataAte && this.DataDe.Month == this.DataAte.Month)
                return this.DataDe.Day.ToString("D2") + " a " + this.DataAte.Day.ToString("D2") + " de " + mes + " de " + this.DataAte.Year + " - das " + this.HoraDe.ToString("D2") + ":" + this.MinutoDe.ToString("D2") + "h às " + this.HoraAte.ToString("D2") + ":" + this.MinutoAte.ToString("D2") + "h";
            else
                return "De " + this.DataDe.ToShortDateString() + " até " + this.DataAte.ToShortDateString() + " - das " + this.HoraDe.ToString("D2") + ":" + this.MinutoDe.ToString("D2") + "h às " + this.HoraAte.ToString("D2") + ":" + this.MinutoAte.ToString("D2") + "h";
        }

        public string GetMusicos()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var em = db.evento_musico.Where(x => x.IDEvento == this.ID).ToList();

                    var musicos = new List<Atracao>();

                    for (int i = 0; i < em.Count; i++)
                    {
                        var mu = new MusicoVM(em[i].usuario_musico.usuario);

                        var a = new Atracao
                        {
                            Confirmado = em[i].Confirmado,
                            ID = mu.ID,
                            Imagem = mu.GetImagemPerfil(),
                            Nome = mu.NomeArtistico,
                            Recusado = em[i].Recusado,
                            Username = mu.Username
                        };

                        musicos.Add(a);
                    }

                    return JsonConvert.SerializeObject(musicos);
                }
            }
            catch { }

            return string.Empty;
        }

        public string PesquisarMusicos(string termo)
        {
            var resultados = new List<Atracao>();

            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var musicos = db.usuario_musico.Where(x =>
                        (!string.IsNullOrEmpty(termo) ? x.NomeArtistico.ToLower().Contains(termo.ToLower()) ||
                        x.usuario.Username.ToLower() == termo.Replace("@", string.Empty).ToLower() : false)
                    ).ToList();

                    musicos = musicos.Where(x =>
                        !x.evento_musico.Any(y => y.IDEvento == this.ID)
                    ).ToList();

                    for (int i = 0; i < musicos.Count; i++)
                    {
                        var usuario_musico = musicos[i];

                        Atracao a = new Atracao
                        {
                            ID = usuario_musico.IDUsuario,
                            Nome = usuario_musico.NomeArtistico,
                            Username = usuario_musico.usuario.Username,
                        };

                        try
                        {
                            a.Imagem = usuario_musico.usuario.imagem.Last(x => x.TipoImagem == 1).Diretorio;
                        }
                        catch
                        {
                            a.Imagem = "/Imagens/Views/user.svg";
                        }

                        resultados.Add(a);
                    }
                }
            }
            catch { }

            return JsonConvert.SerializeObject(resultados);
        }

        public bool Atracao(int idMusico)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var musico = db.usuario_musico.Single(x => x.IDUsuario == idMusico);
                    var evento = db.evento.Single(x => x.ID == this.ID);

                    if (evento.evento_musico.Any(x => x.IDMusico == musico.IDUsuario))
                    {
                        var e = evento.evento_musico.Single(x => x.IDMusico == musico.IDUsuario);

                        evento.evento_musico.Remove(e);
                    }
                    else
                    {
                        var e = new evento_musico
                        {
                            evento = evento,
                            usuario_musico = musico,
                            Confirmado = false,
                            Data = DateTime.Now,
                            Recusado = false
                        };

                        db.evento_musico.AddObject(e);
                    }

                    db.SaveChanges();

                    return true;
                }
            }
            catch { }

            return false;
        }

        public bool SaveChanges(EstabelecimentoVM estabelecimento) 
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario_estabelecimento.Single(x => x.IDUsuario == estabelecimento.ID);
                    var e = u.evento.SingleOrDefault(x => x.ID == this.ID);

                    if (e == null)
                    {
                        e = new evento
                        {
                            Ativo = true,
                            Publicado = false,
                            IDUsuario = u.IDUsuario,
                            CNPJ = u.CNPJ,
                            TipoUsuario = u.TipoUsuario,
                            Cadastro = DateTime.Now,
                        };
                    }

                    e.DataAte = this.DataAte;
                    e.DataDe = this.DataDe;
                    e.Descricao = this.Descricao;
                    e.HoraAte = this.HoraAte;
                    e.HoraDe = this.HoraDe;
                    e.MinutoAte = this.MinutoAte;
                    e.MinutoDe = this.MinutoDe;
                    e.Titulo = this.Titulo;

                    if (e.ID > 0)
                        db.ObjectStateManager.ChangeObjectState(e, System.Data.EntityState.Modified);
                    else
                        u.evento.Add(e);

                    db.SaveChanges();

                    return true;
                }
            }
            catch { }

            return false;
        }

        private usuario GetEstabelecimentoByID(int id)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.usuario.FirstOrDefault(x => x.ID == id);
                }
            }
            catch
            {
                return null;
            }
        }
    }

    internal class Horas
    {
        public int ID { get; set; }
        public string Hora { get; set; }
    }

    internal class Atracao
    {
        public int ID { get; set; }
        public string Imagem { get; set; }
        public string Nome { get; set; }
        public string Username { get; set; }
        public bool Confirmado { get; set; }
        public bool Recusado { get; set; }
    }
}