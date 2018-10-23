using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.ViewModels
{
    public class RedesSociaisVM
    {
        public string Deezer { get; set; }
        public string Facebook { get; set; }
        public string GooglePlus { get; set; }
        public string Instagram { get; set; }
        public string SoundCloud { get; set; }
        public string Spotify { get; set; }
        public string Twitter { get; set; }
        public string Youtube { get; set; }
        public string Embed { get; set; }

        public RedesSociaisVM()
        {
            this.Deezer = string.Empty;
            this.Embed = string.Empty;
            this.Facebook = string.Empty;
            this.GooglePlus = string.Empty;
            this.Instagram = string.Empty;
            this.SoundCloud = string.Empty;
            this.Spotify = string.Empty;
            this.Twitter = string.Empty;
            this.Youtube = string.Empty;
        }

        public RedesSociaisVM(usuario_redes_sociais redes_sociais)
        {
            this.Deezer = redes_sociais.Deezer;
            this.Embed = redes_sociais.Embed;
            this.Facebook = redes_sociais.Facebook;
            this.GooglePlus = redes_sociais.GooglePlus;
            this.Instagram = redes_sociais.Instagram;
            this.SoundCloud = redes_sociais.SoundCloud;
            this.Spotify = redes_sociais.Spotify;
            this.Twitter = redes_sociais.Twitter;
            this.Youtube = redes_sociais.YouTube;
        }

        public bool SaveChanges(UsuarioVM usuario)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.SingleOrDefault(x => x.ID == usuario.ID);
                    var r = u.usuario_redes_sociais;

                    bool ex = r != null;

                    if (r == null)
                    {


                        r = new usuario_redes_sociais
                        {
                            IDUsuario = u.ID,
                            TipoUsuario = u.Tipo
                        };
                    }

                    r.Deezer = !string.IsNullOrEmpty(this.Deezer) ? this.Deezer : string.Empty;
                    r.Embed = !string.IsNullOrEmpty(this.Embed) ? this.Embed : string.Empty;
                    r.Facebook = !string.IsNullOrEmpty(this.Facebook) ? this.Facebook : string.Empty;
                    r.GooglePlus = !string.IsNullOrEmpty(this.GooglePlus) ? this.GooglePlus : string.Empty;
                    r.Instagram = !string.IsNullOrEmpty(this.Instagram) ? this.Instagram : string.Empty;
                    r.SoundCloud = !string.IsNullOrEmpty(this.SoundCloud) ? this.SoundCloud : string.Empty;
                    r.Spotify = !string.IsNullOrEmpty(this.Spotify) ? this.Spotify : string.Empty;
                    r.Twitter = !string.IsNullOrEmpty(this.Twitter) ? this.Twitter : string.Empty;
                    r.YouTube = !string.IsNullOrEmpty(this.Youtube) ? this.Youtube : string.Empty;

                    if (ex)
                        db.ObjectStateManager.ChangeObjectState(r, System.Data.EntityState.Modified);
                    else
                        db.usuario_redes_sociais.AddObject(r);

                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception e) { return false; }
        }
        
    }
}