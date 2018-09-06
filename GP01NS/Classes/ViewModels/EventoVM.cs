using GP01NS.Models;
using System;
using System.Collections.Generic;
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
            this.Titulo = this.Titulo;
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
    }

    internal class Horas
    {
        public int ID { get; set; }
        public string Hora { get; set; }
    }
}