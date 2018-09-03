using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Controllers
{
    public class EventoController : Controller
    {
        //
        // GET: /Evento/

        public ActionResult Index()
        {
            return View();
        }

		// GET: /Evento/cadastro

		public ActionResult Cadastro()
		{
			return View();
		}

	}
}
