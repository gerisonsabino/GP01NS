using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.ViewModels
{
    public class MusicoVM : UsuarioVM
    {
        public string NomeArtistico { get; set; }
        public string CPF { get; set; }
        public string Descricao { get; set; }

        private readonly usuario_musico Musico;

        public MusicoVM(usuario usuario) : base(usuario)
        {
            this.Musico = GetMusicoByID(this.ID);

            if (this.Musico != null)
            {
                this.NomeArtistico = this.Musico.NomeArtistico;
                this.CPF = this.Musico.CPF;
                this.Descricao = this.Musico.Descricao;
            }
            else
            {
                this.NomeArtistico = string.Empty;
                this.CPF = string.Empty;
                this.Descricao = string.Empty;
            }
        }

        private usuario_musico GetMusicoByID(int id)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.usuario_musico.FirstOrDefault(x => x.IDUsuario == id);
                }
            }
            catch
            {
                return null;
            }
        }
    }
}