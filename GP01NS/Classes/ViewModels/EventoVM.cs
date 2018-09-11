using GP01NS.Models;
using GP01NSLibrary;
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

                        e.endereco.Add(u.usuario.endereco.First());
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
            catch (Exception e) { }

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
}