using Microsoft.AspNetCore.Identity;


namespace Uiftec.PPW.Redesocial.Models
{
    public class FeedViewModel
    {
        public List<UsuarioModel> Usuarios { get; set; }
        public List<PostModel> Postagens { get; set; }
        public UsuarioModel UsuarioAtivo { get; set; }
    }
}