using GP01NS.Models;
using GP01NSLibrary;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.ViewModels
{
    public class AdministradorVM : UsuarioVM
    {
        public AdministradorVM() { }

        public AdministradorVM(usuario usuario) : base(usuario) { }

        public string GetEstabelecimentosJSON()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    List<usuario> usuarios = db.usuario.Where(x => x.Tipo == 2).OrderBy(x => x.ID).ToList();

                    var lista = new List<UsuarioJSON>();

                    for (int i = 0; i < usuarios.Count; i++)
                    {
                        var at = usuarios[i];

                        var u = new UsuarioJSON(at);

                        lista.Add(u);
                    }

                    return JsonConvert.SerializeObject(lista);
                }
            }
            catch { }

            return string.Empty;
        }

        public string GetFasJSON()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    List<usuario> usuarios = db.usuario.Where(x => x.Tipo == 3).OrderBy(x => x.ID).ToList();
                    var lista = new List<UsuarioJSON>();

                    for (int i = 0; i < usuarios.Count; i++)
                    {
                        var at = usuarios[i];

                        var u = new UsuarioJSON(at);

                        lista.Add(u);
                    }

                    return JsonConvert.SerializeObject(lista);
                }
            }
            catch { }

            return string.Empty;
        }

        public string GetMusicosJSON()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    List<usuario> usuarios = db.usuario.Where(x => x.Tipo == 4).OrderBy(x => x.ID).ToList();
                    var lista = new List<UsuarioJSON>();

                    for (int i = 0; i < usuarios.Count; i++)
                    {
                        var at = usuarios[i];

                        var u = new UsuarioJSON(at);

                        lista.Add(u);
                    }

                    return JsonConvert.SerializeObject(lista);
                }
            }
            catch { }

            return string.Empty;
        }

        internal class UsuarioJSON
        {
            public UsuarioJSON(usuario at)
            {
                this.Cadastro = at.Cadastro.ToShortDateString();
                this.Codigo = at.ID.ToString();
                this.Nome = at.Nome;
                this.Status = at.Ativo ? "Ativo" : "Inativo";
                this.Tipo = at.usuario_tipo.Descricao;
                this.Username = at.Username;
            }

            public string Cadastro { get; set; }
            public string Codigo { get; set; }
            public string Nome { get; set; }
            public string Status { get; set; }
            public string Tipo { get; set; }
            public string Username { get; set; }
        }
    }

}