using Microsoft.AspNetCore.Identity;
using static Uiftec.PPW.Redesocial.Models.DestaquesModel;


namespace Uiftec.PPW.Redesocial.Models
{
    public class FeedViewModel
    {
        public List<UsuarioModel> Usuarios { get; set; }
        public List<PostModel> Postagens { get; set; }
        public UsuarioModel UsuarioAtivo { get; set; }
        public List<DestaqueModel> Destaques { get; set; }
    }
}