using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Uiftec.PPW.Redesocial.Models
{
    public class UsuarioModel
    {
        public Guid ID { get; set; }
        public string Nome { get; set; }
        public string senha { get; set; }
        public string Username { get; set; }
        public DateTime DataNascimento { get; set; }
        public string genero { get; set; }
        public string Descricao { get; set; }
        public int Publicacoes { get; set; }
        public int Seguidores { get; set;}
        public int Seguindo {  get; set;}
        public string FotoPerfil { get; set; }

    }
}
