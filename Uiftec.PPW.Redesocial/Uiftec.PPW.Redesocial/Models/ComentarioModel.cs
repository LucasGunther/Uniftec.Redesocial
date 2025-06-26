namespace Uiftec.PPW.Redesocial.Models
{
    public class ComentarioModel
    {
        public Guid Id { get; set; }
        public Guid IdPost { get; set; }
        public string NomeAutor { get; set; }
        public string Texto { get; set; }
        public DateTime DataComentario { get; set; }
    }
}
