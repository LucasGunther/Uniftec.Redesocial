using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uiftec.PPW.Redesocial.Models;

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
            if (!Request.Cookies.TryGetValue("UsuarioLogadoId", out var userId))
            {
                return RedirectToAction("Index", "Login");
            }

            var usuariosPath = Path.Combine(_env.WebRootPath, "BaseTemp", "usuarios.json");
            List<UsuarioModel> usuarios = new();

            if (System.IO.File.Exists(usuariosPath))
            {
                try
                {
                    var usuariosJson = System.IO.File.ReadAllText(usuariosPath);
                    usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(usuariosJson) ?? new();
                }
                catch { }
            }

            var usuarioAtivo = usuarios.FirstOrDefault(u => u.ID.ToString() == userId);

            if (usuarioAtivo == null)
            {
                try
                {
                    var client = _httpClientFactory.CreateClient();
                    var responseUser = await client.GetAsync($"http://usuario.neurosky.com.br/api/usuario/{userId}");

                    if (responseUser.IsSuccessStatusCode)
                    {
                        var userJson = await responseUser.Content.ReadAsStringAsync();
                        usuarioAtivo = JsonConvert.DeserializeObject<UsuarioModel>(userJson);

                        if (usuarioAtivo != null)
                        {
                            usuarios.Add(usuarioAtivo);
                            System.IO.File.WriteAllText(usuariosPath, JsonConvert.SerializeObject(usuarios, Formatting.Indented));
                        }
                    }
                }
                catch { }
            }

            if (usuarioAtivo == null)
                return RedirectToAction("Index", "Login");

            var clientHttp = _httpClientFactory.CreateClient();

            List<PostModel> posts = new();
            List<StoryModel> stories = new();

            try
            {
                var response = await clientHttp.GetAsync("http://feed.neurosky.com.br/api/feed/geral");
                if (response.IsSuccessStatusCode)
                {
                    var json = await response.Content.ReadAsStringAsync();
                    posts = JsonConvert.DeserializeObject<List<PostModel>>(json) ?? new();

                    // Adiciona curtidas e comentários a cada post
                    foreach (var post in posts)
                    {
                        // Curtidas
                        var curtidaResp = await clientHttp.GetAsync($"http://curtidas.neurosky.com.br/api/Curtidas/post/{post.Id}/count");
                        if (curtidaResp.IsSuccessStatusCode)
                        {
                            var curtidasCount = await curtidaResp.Content.ReadAsStringAsync();
                            post.LikeCount = int.TryParse(curtidasCount, out var lc) ? lc : 0;
                        }

                        // Comentários
                        var comentariosResp = await clientHttp.GetAsync($"http://history.neurosky.com.br/api/Comentarios/post/{post.Id}/count");
                        if (comentariosResp.IsSuccessStatusCode)
                        {
                            var comentariosCount = await comentariosResp.Content.ReadAsStringAsync();
                            post.CommentCount = int.TryParse(comentariosCount, out var cc) ? cc : 0;
                        }
                    }
                }
            }
            catch { }

            try
            {
                var storiesResponse = await clientHttp.GetAsync("http://stories.neurosky.com.br/api/stories");
                if (storiesResponse.IsSuccessStatusCode)
                {
                    var json = await storiesResponse.Content.ReadAsStringAsync();
                    stories = JsonConvert.DeserializeObject<List<StoryModel>>(json) ?? new();
                }
            }
            catch { }

            var viewModel = new FeedViewModel
            {
                Usuarios = usuarios,
                Postagens = posts,
                UsuarioAtivo = usuarioAtivo,
                NovoPost = new PostModel(),
                StoriesExternos = stories
            };

            return View(viewModel);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(string TextoPost)
        {
            if (!Request.Cookies.TryGetValue("UsuarioLogadoId", out var userId))
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
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("http://feed.neurosky.com.br/api/feed", jsonContent);

            if (response.IsSuccessStatusCode)
                TempData["Mensagem"] = "Post publicado com sucesso!";
            else
                TempData["Erro"] = "Erro ao publicar o post.";

            return RedirectToAction("Index");
        }

        // Métodos para Comentários

        [HttpGet]
        public async Task<IActionResult> ObterComentario(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"http://history.neurosky.com.br/api/Comentarios/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var comentario = JsonConvert.DeserializeObject<ComentarioModel>(json);
                return Ok(comentario);
            }

            return NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarComentario(Guid id, [FromBody] ComentarioModel comentario)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(comentario),
                Encoding.UTF8,
                "application/json");

            var response = await client.PutAsync($"http://history.neurosky.com.br/api/Comentarios/{id}", jsonContent);

            if (response.IsSuccessStatusCode)
                return Ok("Comentário atualizado com sucesso.");

            return BadRequest("Erro ao atualizar o comentário.");
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirComentario(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"http://history.neurosky.com.br/api/Comentarios/{id}");

            if (response.IsSuccessStatusCode)
                return Ok("Comentário excluído com sucesso.");

            return BadRequest("Erro ao excluir o comentário.");
        }

        [HttpGet]
        public async Task<IActionResult> ObterComentariosPorPost(Guid idPost)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"http://history.neurosky.com.br/api/Comentarios/post/{idPost}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var comentarios = JsonConvert.DeserializeObject<List<ComentarioModel>>(json);
                return Ok(comentarios);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CriarComentario([FromBody] ComentarioModel comentario)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(comentario),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("http://history.neurosky.com.br/api/Comentarios", jsonContent);

            if (response.IsSuccessStatusCode)
                return Ok("Comentário criado com sucesso.");

            return BadRequest("Erro ao criar o comentário.");
        }

        // Métodos para Curtidas

        [HttpGet]
        public async Task<IActionResult> ObterCurtida(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"http://curtidas.neurosky.com.br/api/Curtidas/{id}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var curtida = JsonConvert.DeserializeObject<CurtidaModel>(json);
                return Ok(curtida);
            }

            return NotFound();
        }

        [HttpPut]
        public async Task<IActionResult> AtualizarCurtida(Guid id, [FromBody] CurtidaModel curtida)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(curtida),
                Encoding.UTF8,
                "application/json");

            var response = await client.PutAsync($"http://curtidas.neurosky.com.br/api/Curtidas/{id}", jsonContent);

            if (response.IsSuccessStatusCode)
                return Ok("Curtida atualizada com sucesso.");

            return BadRequest("Erro ao atualizar a curtida.");
        }

        [HttpDelete]
        public async Task<IActionResult> ExcluirCurtida(Guid id)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.DeleteAsync($"http://curtidas.neurosky.com.br/api/Curtidas/{id}");

            if (response.IsSuccessStatusCode)
                return Ok("Curtida excluída com sucesso.");

            return BadRequest("Erro ao excluir a curtida.");
        }

        [HttpGet]
        public async Task<IActionResult> ObterCurtidasPorPost(Guid idPost)
        {
            var client = _httpClientFactory.CreateClient();
            var response = await client.GetAsync($"http://curtidas.neurosky.com.br/api/Curtidas/post/{idPost}");

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var curtidas = JsonConvert.DeserializeObject<List<CurtidaModel>>(json);
                return Ok(curtidas);
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CriarCurtida([FromBody] CurtidaModel curtida)
        {
            var client = _httpClientFactory.CreateClient();
            var jsonContent = new StringContent(
                JsonConvert.SerializeObject(curtida),
                Encoding.UTF8,
                "application/json");

            var response = await client.PostAsync("http://curtidas.neurosky.com.br/api/Curtidas", jsonContent);

            if (response.IsSuccessStatusCode)
                return Ok("Curtida criada com sucesso.");

            return BadRequest("Erro ao criar a curtida.");
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
