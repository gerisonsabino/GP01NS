using GP01NS.Classes.Util;
using GP01NS.Classes.ViewModels;
using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace GP01NS.Controllers
{
    public class InicioController : BaseController
    {
        private UsuarioVM Usuario;

        public ActionResult Index() 
        {
            if (this.BaseUsuario != null)
            {
                this.Usuario = new UsuarioVM(this.BaseUsuario);
                ViewBag.Usuario = this.Usuario;
            }

            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase Arquivo) 
        {
            return View();
        }
    }
}
