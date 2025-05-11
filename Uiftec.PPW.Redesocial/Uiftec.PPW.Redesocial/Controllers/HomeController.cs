using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Uiftec.PPW.Redesocial.Models;
using Newtonsoft.Json;

namespace Uiftec.PPW.Redesocial.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;

        // Construtor CORRETO — injetando env
        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env)
        {
            _logger = logger;
            _env = env;
        }

        public IActionResult Index()
        {
            UsuarioModel usuarioativo = new UsuarioModel()
            {
                Nome = "Lucas Gunther",
                FotoPerfil = "~/Imagens/avatar6.JPG",
                Seguindo = 22,
                Seguidores = 200,
                Publicacoes = 20
            };

            string usuariosPath = Path.Combine(_env.ContentRootPath, "wwwroot", "BaseTemp", "usuarios.json");
            string postsPath = Path.Combine(_env.ContentRootPath, "wwwroot", "BaseTemp", "posts.json");

            // Ler arquivos
            var usuariosJson = System.IO.File.ReadAllText(usuariosPath);
            var postsJson = System.IO.File.ReadAllText(postsPath);

   
            var usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(usuariosJson) ?? new List<UsuarioModel>();
            var posts = JsonConvert.DeserializeObject<List<PostModel>>(postsJson) ?? new List<PostModel>();

            var viewModel = new FeedViewModel
            {
                Usuarios = usuarios,
                Postagens = posts,
                UsuarioAtivo = usuarioativo
            };
            

            return View(viewModel);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
