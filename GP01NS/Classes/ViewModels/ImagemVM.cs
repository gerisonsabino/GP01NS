using GP01NS.Classes.Util;
using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GP01NS.Classes.ViewModels
{
    public class ImagemVM
    {
        public DateTime Data { get; set; }
        public string Diretorio { get; set; }
        public int Tipo { get; set; }
        public int IDUsuario { get; set; }
        public int IDEvento { get; set; }

        private HttpPostedFileBase Imagem;

        public ImagemVM(HttpPostedFileBase imagem, int idUsuario, int idEvento, int tipo)
        {
            this.Imagem = imagem;
            this.IDUsuario = idUsuario;
            this.IDEvento = idEvento;
            this.Data = DateTime.Now;
            this.Tipo = tipo;
            this.Diretorio = "/cdn/" + CriptografarDiretorio() + "/" + this.Data.ToString("ssmmhhyyyyMMdd") + "." + this.Imagem.FileName.Split('.').Last();
        }

        public bool Upload()
        {
            switch (this.Imagem.FileName.Split('.').Last())
            {
                case "jpg":
                case "jpeg":
                case "png":
                    if (this.SaveChanges())
                    {
                        try
                        {
                            string wwwroot = "/web/nossoshow";
                            string dir = CriptografarDiretorio();

                            string[] diretorios = Regex.Split(FTP.ListDirectory(wwwroot + "/cdn/"), "\r\n");

                            if (!diretorios.Contains(dir))
                                FTP.MakeDirectory(wwwroot + "/cdn/" + dir);

                            FTP.UploadFile(wwwroot + this.Diretorio, this.Imagem.InputStream);

                            return true;
                        }
                        catch (Exception e) { }
                    }
                    break;
            }

            return false;
        }

        private string CriptografarDiretorio() 
        {
            return string.Join(string.Empty, Criptografia.GetHash64(this.IDUsuario.ToString()));
        }

        private bool SaveChanges() 
        {
            try  
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    var u = db.usuario.First(x => x.ID == this.IDUsuario);
                    var t = db.imagem_tipo.First(x => x.ID == this.Tipo);

                    var img = new imagem
                    {
                        ID = this.Data.ToString("ssmmhhyyyyMMdd"),
                        Data = this.Data,
                        Diretorio = this.Diretorio,
                        TipoImagem = t.ID,
                    };

                    if (img.TipoImagem != 4)
                    {
                        u.imagem.Add(img);
                    }
                    else
                    {
                        var e = db.evento.Single(x => x.ID == this.IDEvento && x.IDUsuario == this.IDUsuario);
                        e.imagem.Add(img);
                    }

                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception e) { }

            return false;
        }
    }
}