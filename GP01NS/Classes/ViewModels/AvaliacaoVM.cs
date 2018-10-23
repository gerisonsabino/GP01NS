using GP01NS.Models;
using GP01NSLibrary;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace GP01NS.Classes.ViewModels
{
    public class AvaliacaoVM
    {
        public int Nota { get; set; }
        public string Comentario { get; set; }
        public int IDAvaliado { get; set; }
        public int TipoAvaliado { get; set; }
        public int IDElogio { get; set; }

        private readonly usuario_avalia_usuario Avaliacao;

        public AvaliacaoVM() { }

        public AvaliacaoVM(UsuarioVM usuario, usuario avaliado)
        {
            this.Avaliacao = this.GetAvaliacao(usuario.ID, avaliado.ID);

            if (Avaliacao != null)
            {
                this.Comentario = this.Avaliacao.Comentario;
                this.IDAvaliado = this.Avaliacao.IDAvaliado;
                this.IDElogio = this.Avaliacao.IDElogio;
                this.Nota = this.Avaliacao.Nota;
                this.TipoAvaliado = this.Avaliacao.TipoAvaliado;
            }
            else
            {
                this.Comentario = string.Empty;
                this.IDAvaliado = avaliado.ID;
                this.IDElogio = 11;
                this.Nota = 1;
                this.TipoAvaliado = avaliado.Tipo;
            }
        }

        private usuario_avalia_usuario GetAvaliacao(int idUsuario, int idAvaliado)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.usuario_avalia_usuario.First(x => x.IDAvaliado == idAvaliado && x.IDUsuario == idUsuario);
                }
            }
            catch { }

            return null;
        }

        public List<usuario_avaliacao_elogio> GetElogios()
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    return db.usuario_avaliacao_elogio.Where(x => x.Tipo == this.TipoAvaliado).ToList();
                }
            }
            catch { }

            return null;
        }

        public bool SaveChanges(UsuarioVM usuario)
        {
            try
            {
                using (var db = new nosso_showEntities(Conexao.GetString()))
                {
                    bool ex = true;
                    var av = db.usuario_avalia_usuario.FirstOrDefault(x => x.IDAvaliado == this.IDAvaliado && x.IDUsuario == usuario.ID);

                    if (av == null)
                    {
                        ex = false;
                        av = new usuario_avalia_usuario
                        {
                            IDAvaliado = this.IDAvaliado,
                            IDUsuario = usuario.ID,
                            TipoUsuario = usuario.TipoUsuario,
                            TipoAvaliado = this.TipoAvaliado
                        };
                    }

                    av.Comentario = this.Comentario;
                    av.Data = DateTime.Now;
                    av.IDElogio = this.IDElogio;
                    av.Nota = this.Nota;

                    if (ex)
                        db.ObjectStateManager.ChangeObjectState(av, System.Data.EntityState.Modified);
                    else
                        db.usuario_avalia_usuario.AddObject(av);

                    db.SaveChanges();

                    return true;
                }
            }
            catch { }

            return false;
        }
    }
}