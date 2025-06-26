namespace Uiftec.PPW.Redesocial.Models
{
    public class PostModel
    {
        
        public Guid IdPost { get; set; } = Guid.NewGuid(); 
        public Guid IdUsuarioPost { get; set; }
        public string TextoPost { get; set; }
        public string ImagemUrl { get; set; }
        public DateTime DataPublicacao { get; set; } = DateTime.Now;
        public int QuantidadeCurtidas { get; set; } = 0;
        public int QuantidadeComentarios { get; set; } = 0;
        public string NomeAutor { get; set; } 
        public string FotoPerfilAutor { get; set; }



    }

}
