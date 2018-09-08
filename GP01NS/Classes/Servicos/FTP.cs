using GP01NS.Classes.Util;
using GP01NSLibrary;
using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;

namespace GP01NS.Classes.Servicos
{
    public class FTP
    {
        public int IDUsuario { get; set; }
        public DateTime Data { get; set; }
        private string Diretorio { get; set; }

        public FTP(int idUsuario, DateTime data)
        {
            this.IDUsuario = idUsuario;
            this.Data = data;
        }

        //public string UploadFile(HttpPostedFileBase arquivo)
        //{
        //    try
        //    {
        //        string wwwroot = "/site/wwwroot/cdn/";
        //        string dir = CriptografarDiretorio();

        //        string[] diretorios = Regex.Split(FtpGp01ns.ListDirectory(wwwroot), "\r\n");

        //        if (!diretorios.Contains(dir))
        //            FtpGp01ns.MakeDirectory(wwwroot + dir);

        //        var ext = arquivo.FileName.Split('.').Last();

        //        var dirArquivo = dir + "/" + this.Data.ToString("ssmmhhyyyyMMdd") + "." + ext;

        //        switch (ext)
        //        {
        //            case "jpg":
        //            case "jpeg":
        //            case "png":
        //                FtpGp01ns.UploadFile(wwwroot + dirArquivo, arquivo.InputStream);
        //                break;

        //            default:
        //                return string.Empty;
        //        }

        //        return "/cdn/" + dirArquivo;
        //    }
        //    catch (Exception e) { }

        //    return string.Empty;
        //}

        //private string CriptografarDiretorio()
        //{
        //    return string.Join(string.Empty, Criptografia.GetHash64(this.IDUsuario.ToString()).Split(Path.GetInvalidFileNameChars()));
        //}
    }
}