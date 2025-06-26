using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Uiftec.PPW.Redesocial.Models;

namespace Uiftec.PPW.Redesocial.Controllers
{
    public class PublicacaoController : Controller
    {
        [HttpPost]
        public IActionResult Criar(PostModel post)
        {
            post.IdPost = Guid.NewGuid();
            post.DataPublicacao = DateTime.Now;

            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "BaseTemp", "posts.json");
            var posts = new List<PostModel>();

            if (System.IO.File.Exists(path))
            {
                var json = System.IO.File.ReadAllText(path);
                posts = JsonConvert.DeserializeObject<List<PostModel>>(json) ?? new List<PostModel>();
            }

            posts.Add(post);

            System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(posts, Formatting.Indented));
            return RedirectToAction("Index", "Home");
        }
    }
}
