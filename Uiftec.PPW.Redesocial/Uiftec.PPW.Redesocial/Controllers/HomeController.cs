using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Uiftec.PPW.Redesocial.Models;
using Newtonsoft.Json;
using static Uiftec.PPW.Redesocial.Models.StoryModel;

namespace Uiftec.PPW.Redesocial.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IWebHostEnvironment _env;
        private readonly IHttpClientFactory _httpClientFactory;

        public HomeController(ILogger<HomeController> logger, IWebHostEnvironment env, IHttpClientFactory httpClientFactory)
        {
            _logger = logger;
            _env = env;
            _httpClientFactory = httpClientFactory;
        }

        public async Task<IActionResult> Index()
        {
            string userId = HttpContext.Session.GetString("UsuarioLogadoId");

            if (string.IsNullOrEmpty(userId))
            {
                return RedirectToAction("Index", "Login");
            }

            string usuariosPath = Path.Combine(_env.ContentRootPath, "wwwroot", "BaseTemp", "usuarios.json");
            var usuariosJson = System.IO.File.ReadAllText(usuariosPath);
            var usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(usuariosJson) ?? new List<UsuarioModel>();

            var usuarioativo = usuarios.FirstOrDefault(u => u.ID.ToString() == userId);

            if (usuarioativo == null)
            {
                return RedirectToAction("Index", "Login");
            }

            var client = _httpClientFactory.CreateClient();
            List<PostModel> posts = new();

            var response = await client.GetAsync($"https://seuservidor/api/feed/geral"); ///ajustar api
            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                posts = JsonConvert.DeserializeObject<List<PostModel>>(json) ?? new List<PostModel>();
            }

            // ✅ CHAMADA API DE STORIES (separada)
            List<StoryModel> stories = new();
            var storiesResponse = await client.GetAsync("https://api.seuservidor.com/api/stories"); ///ajustar api
            if (storiesResponse.IsSuccessStatusCode)
            {
                var json = await storiesResponse.Content.ReadAsStringAsync();
                stories = JsonConvert.DeserializeObject<List<StoryModel>>(json) ?? new List<StoryModel>();
            }

            // Monta ViewModel final
            var viewModel = new FeedViewModel
            {
                Usuarios = usuarios,
                Postagens = posts,
                UsuarioAtivo = usuarioativo,
                NovoPost = new PostModel(),
                StoriesExternos = stories
            };


            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(string TextoPost)
        {
            string userId = HttpContext.Session.GetString("UsuarioLogadoId");

            if (string.IsNullOrEmpty(userId))
                return RedirectToAction("Index", "Login");

            var novoPost = new PostModel
            {
                Id = Guid.NewGuid(),
                UserId = Guid.Parse(userId),
                TextPreview = TextoPost,
                date = DateTime.Now,
                LikeCount = 0,
                CommentCount = 0
            };

            var client = _httpClientFactory.CreateClient();

            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(novoPost),
                System.Text.Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("https://seuservidor/api/feed", jsonContent);

            if (response.IsSuccessStatusCode)
            {
                TempData["Mensagem"] = "Post publicado com sucesso!";
            }
            else
            {
                TempData["Erro"] = "Erro ao publicar o post.";
            }

            return RedirectToAction("Index");
        }



        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }

}
