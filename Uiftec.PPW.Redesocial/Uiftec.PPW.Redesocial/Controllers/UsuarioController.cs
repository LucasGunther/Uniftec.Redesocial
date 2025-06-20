using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Uiftec.PPW.Redesocial.Models;

namespace Uiftec.PPW.Redesocial.Controllers
{
    public class UsuarioController : Controller
    {
        private readonly IWebHostEnvironment _env;
        private readonly string _usuariosPath;

        public UsuarioController(IWebHostEnvironment env)
        {
            _env = env;
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

            usuario.ID = Guid.NewGuid(); // <- ajustado aqui

            usuarios.Add(usuario);
            SalvarUsuarios(usuarios);

            return RedirectToAction("Buscar");
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

        [HttpGet("Usuario/Perfil/{id}")]
        public IActionResult Perfil(Guid id)
        {
            var usuarios = CarregarUsuarios();

            var usuario = usuarios.FirstOrDefault(u => u.ID == id);

            if (usuario == null)
            {
                return NotFound("Usuário não encontrado.");
            }

            return View(usuario);
        }
    }
}