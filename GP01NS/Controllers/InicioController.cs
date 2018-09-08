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
            return View();
        }

        [HttpPost]
        public ActionResult Index(HttpPostedFileBase Arquivo)
        {
            //var ftp = new Classes.Servicos.FTP();

            //var n = ftp.UploadFile(Arquivo);

            return View();
        }
    }
}
