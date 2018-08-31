using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Controllers
{
    public class EstabelecimentoController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Configuracoes()
        {
            return View();
        }
    }
}
