using GP01NS.Classes.Util;
using GP01NS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Controllers
{
    public class InicioController : BaseController
    {
        public ActionResult Index()
        {
            var usuario = base.Usuario;

            return View(Usuario);
        }
    }
}
