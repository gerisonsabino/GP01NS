﻿using GP01NS.Classes.Util;
using GP01NS.Classes.ViewModels;
using GP01NS.Classes.ViewModels.Musico;
using GP01NS.Models;
using GP01NSLibrary;
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
            this.Musico = new MusicoVM(this.BaseUsuario);

            using (var db = new nosso_showEntities(Conexao.GetString()))
            {
                var u = db.usuario.Single(x => x.ID == this.Musico.ID);

                if (u.usuario_musico.Count == 0)
                    return Redirect("/musico/conta/");

                if (u.endereco.Count == 0)
                    return Redirect("/musico/endereco/");
            }

            return View(this.Musico);
        }

        public ActionResult Conta()
        {
            this.Musico = new MusicoVM(this.BaseUsuario);

            var cadastro = new ContaVM(this.Musico);

            ViewBag.Generos = cadastro.GetGenerosMusicais();
            ViewBag.TipoHabilidades = cadastro.GetTipoHabilidades();
            ViewBag.Habilidades = cadastro.GetHabilidades();

            return View(cadastro);
        }

        public ActionResult Agenda()
        {
            return View();
        }

        public ActionResult Convites()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Conta(ContaVM model)
        {
            this.Musico = new MusicoVM(this.BaseUsuario);

            if (ModelState.IsValid)
            {
                if (model.ValidarEmail(this.Musico))
                {
                    if (model.ValidarNomeUsuario(this.Musico))
                    {
                        if (model.SaveChanges(this.Musico))
                            ViewBag.Sucesso = "Os dados de sua conta foram salvos.";
                        else
                            ViewBag.Erro = "Não foi possível estabelecer conexão com o servidor, por favor, tente novamente mais tarde.";
                    }
                    else
                    {
                        ViewBag.Erro = "O nome de usuário informado já está sendo utilizado.";
                    }
                }
                else
                {
                    ViewBag.Erro = "O endereço de e-mail informado já está sendo utilizado.";
                }
            }
            else
            {
                ViewBag.Erro = "Por favor, confira os dados informados e tente novamente.";
            }

            ViewBag.Generos = model.GetGenerosMusicais();
            ViewBag.TipoHabilidades = model.GetTipoHabilidades();
            ViewBag.Habilidades = model.GetHabilidades();

            return View(model);
        }

        public ActionResult Perfil()
        {
            this.Musico = new MusicoVM(this.BaseUsuario);

            return View(Musico);
        }

        [HttpPost]
        public ActionResult UploadProfile(HttpPostedFileBase Arquivo)
        {
            this.Musico = new MusicoVM(this.BaseUsuario);

            new ImagemVM(Arquivo, this.Musico.ID, 1).Upload();

            return Redirect("/inicio/musico/" + this.Musico.Username);
        }

        [HttpPost]
        public ActionResult UploadImagem(HttpPostedFileBase Imagem)
        {
            this.Musico = new MusicoVM(this.BaseUsuario);

            new ImagemVM(Imagem, this.Musico.ID, 3).Upload();

            return Redirect("/inicio/musico/" + this.Musico.Username);
        }

        public ActionResult Endereco()
        {
            this.Musico = new MusicoVM(this.BaseUsuario);

            var endereco = this.Musico.Endereco;

            return View(endereco);
        }

        [HttpPost]
        public ActionResult Endereco(EnderecoVM model)
        {
            this.Musico = new MusicoVM(this.BaseUsuario);

            if (model.SaveChanges(this.Musico))
            {
                ViewBag.Sucesso = "Os dados de endereço foram salvos.";
            }
            else
            {
                ViewBag.Erro = "Por favor, confira os dados informados e tente novamente.";
            }

            return View(model);
        }

        public ActionResult Sair()
        {
            try
            {
                string id = string.Empty;

                try
                {
                    id = Criptografia.Descriptografar(base.Session["IDUsuario"].ToString());
                }
                catch { }

                if (!string.IsNullOrEmpty(id))
                {
                    base.Session.RemoveAll();
                    base.Session.Clear();
                    base.Session.Abandon();
                    base.Session["IDUsuario"] = string.Empty;

                    using (var db = new nosso_showEntities(Conexao.GetString()))
                    {
                        int idUsuario = int.Parse(Criptografia.Descriptografar(id));

                        var auths = db.autenticacao.Where(x => x.IDUsuario == idUsuario && x.Sessao == Session.SessionID).ToList();

                        for (int i = 0; i < auths.Count; i++)
                            db.autenticacao.DeleteObject(auths[i]);

                        db.SaveChanges();
                    }
                }

            }
            catch { }

            return Redirect("/inicio/");
        }
    }
}
