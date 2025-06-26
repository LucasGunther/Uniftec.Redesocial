using Microsoft.AspNetCore.Identity;
using static Uiftec.PPW.Redesocial.Models.StoryModel;


namespace Uiftec.PPW.Redesocial.Models
{
    public class FeedViewModel
    {
        public List<UsuarioModel> Usuarios { get; set; }
        public List<PostModel> Postagens { get; set; }
        public UsuarioModel UsuarioAtivo { get; set; }
        public PostModel NovoPost { get; set; }
        public List<StoryModel> StoriesExternos { get; set; }


    }
}