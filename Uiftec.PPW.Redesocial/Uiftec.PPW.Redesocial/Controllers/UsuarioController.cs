using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using Uiftec.PPW.Redesocial.Models;

namespace Uiftec.PPW.Redesocial.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly HttpClient _httpClient;
        private readonly string _usuariosPath;

        public UsuarioController(IWebHostEnvironment env, IHttpClientFactory httpClientFactory)
        {
            _env = env;
            _httpClient = httpClientFactory.CreateClient();
            _usuariosPath = Path.Combine(_env.WebRootPath, "BaseTemp", "usuarios.json");
        }

        [HttpGet]
        public IActionResult Cadastrar()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Cadastrar(UsuarioModel usuario)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Erro = "Preencha todos os campos obrigatórios.";
                return View(usuario);
            }

            var usuarios = CarregarUsuarios();

            usuario.ID = Guid.NewGuid();
            usuarios.Add(usuario);
            SalvarUsuarios(usuarios);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Buscar(string termo)
        {
            var usuarios = CarregarUsuarios();

            ViewBag.Termo = termo;

            if (!string.IsNullOrWhiteSpace(termo))
            {
                termo = termo.ToLower();
                usuarios = usuarios
                    .Where(u =>
                        (!string.IsNullOrEmpty(u.Nome) && u.Nome.ToLower().Contains(termo)) ||
                        (!string.IsNullOrEmpty(u.Username) && u.Username.ToLower().Contains(termo)))
                    .ToList();
            }

            return View(usuarios);
        }

        [HttpGet("Usuario/Perfil/{id}")]
        public IActionResult Perfil(Guid id)
        {
            var usuarios = CarregarUsuarios();
            var usuario = usuarios.FirstOrDefault(u => u.ID == id);

            if (usuario == null)
                return NotFound("Usuário não encontrado.");

            return View(usuario);
        }

        // Consumir API de outro colega
        [HttpGet]
        public async Task<IActionResult> UsuariosColega()
        {
            var response = await _httpClient.GetAsync("http://localhost:5136/api/UsuariosFake"); // adicionar URL do colega

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(json);
                return View("Buscar", usuarios);
            }

            ViewBag.Erro = "Erro ao conectar com a API externa.";
            return View("Buscar", new List<UsuarioModel>());
        }
        [HttpGet]
        public async Task<IActionResult> ComentariosExternos()
        {
            var response = await _httpClient.GetAsync("http://localhost:5136/api/ComentariosFake"); // adicionar URL do colega

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var comentarios = JsonConvert.DeserializeObject<List<ComentarioModel>>(json);
                return View("ExibirComentarios", comentarios);
            }

            return View("ExibirComentarios", new List<ComentarioModel>());
        }

        [HttpGet]
        public async Task<IActionResult> StoriesExternos()
        {
            var response = await _httpClient.GetAsync("http://localhost:5136/api/StoriesFake"); // adicionar URL do colega

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var stories = JsonConvert.DeserializeObject<List<StoryModel>>(json);
                return View("ExibirStories", stories);
            }

            return View("ExibirStories", new List<StoryModel>());
        }


        // Métodos auxiliares
        private List<UsuarioModel> CarregarUsuarios()
        {
            if (!System.IO.File.Exists(_usuariosPath))
                return new List<UsuarioModel>();

            var json = System.IO.File.ReadAllText(_usuariosPath);
            return JsonConvert.DeserializeObject<List<UsuarioModel>>(json) ?? new List<UsuarioModel>();
        }

        private void SalvarUsuarios(List<UsuarioModel> usuarios)
        {
            var json = JsonConvert.SerializeObject(usuarios, Formatting.Indented);
            System.IO.File.WriteAllText(_usuariosPath, json);
        }
    }
}
