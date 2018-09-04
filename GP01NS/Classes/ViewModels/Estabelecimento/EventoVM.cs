using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Classes.ViewModels.Estabelecimento
{
    public class EventoVM
    {
        public int ID { get; set; }
        public string Titulo { get; set; }
        public string Descricao { get; set; }
        public DateTime Data { get; set; }
        public int Hora { get; set; }
        public int Minuto { get; set; }
        public int Duracao { get; set; }
    }
}