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
            this.Usuario = new UsuarioVM(this.BaseUsuario);

            if (this.Usuario.TipoUsuario == 2)
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {

                    if (!db.usuario_estabelecimento.Any(x => x.IDUsuario == this.Usuario.ID))
                        return Redirect("/estabelecimento/cadastro/");
                }
            }

            return View(Usuario);
        }
    }
}
