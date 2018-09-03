using GP01NS.Classes.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Controllers
{
    public class MusicoController : BaseController
    {
        private MusicoVM Musico;

        public ActionResult Index()
        {
            Musico = new MusicoVM(this.BaseUsuario);

            return View();
        }
    }
}
