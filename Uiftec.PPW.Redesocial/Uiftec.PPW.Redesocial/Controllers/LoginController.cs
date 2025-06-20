using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Uiftec.PPW.Redesocial.Models;
using System.IO;
using System.Linq;
using System.Collections.Generic;

namespace Uiftec.PPW.Redesocial.Controllers
{
    public class LoginController : Controller
    {
        private readonly IWebHostEnvironment _env;

        public LoginController(IWebHostEnvironment env)
        {
            _env = env;
        }

        public IActionResult Index()
        {
            if (TempData["Mensagem"] != null)
            {
                ViewBag.Mensagem = TempData["Mensagem"];
            }

            return View();
        }

        [HttpPost]
        public IActionResult Login(string username, string senha)
        {
            var path = Path.Combine(_env.WebRootPath, "BaseTemp", "usuarios.json");

            if (!System.IO.File.Exists(path))
                return View("Index");

            var json = System.IO.File.ReadAllText(path);
            var usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(json) ?? new List<UsuarioModel>();

            var usuario = usuarios.FirstOrDefault(u =>
                u.Username.ToLower() == username.ToLower() && u.Senha == senha);

            if (usuario != null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.Erro = "Usuário ou senha inválidos.";
            return View("Index");
        }

        [HttpGet]
        public IActionResult EsqueciSenha()
        {
            return View();
        }

        [HttpPost]
        public IActionResult EsqueciSenha(string username)
        {
            var path = Path.Combine(_env.WebRootPath, "BaseTemp", "usuarios.json");

            if (!System.IO.File.Exists(path))
            {
                ViewBag.Erro = "Base de usuários não encontrada.";
                return View();
            }

            var json = System.IO.File.ReadAllText(path);
            var usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(json) ?? new List<UsuarioModel>();

            var usuario = usuarios.FirstOrDefault(u => u.Username.ToLower() == username.ToLower());

            if (usuario == null)
            {
                ViewBag.Erro = "Usuário não encontrado.";
                return View();
            }

            // Redirecionar para redefinição de senha
            return View("RedefinirSenha", usuario);
        }

        [HttpPost]
        public IActionResult RedefinirSenha(UsuarioModel model)
        {
            var path = Path.Combine(_env.WebRootPath, "BaseTemp", "usuarios.json");

            var json = System.IO.File.ReadAllText(path);
            var usuarios = JsonConvert.DeserializeObject<List<UsuarioModel>>(json) ?? new List<UsuarioModel>();

            var usuario = usuarios.FirstOrDefault(u => u.ID == model.ID);

            if (usuario != null)
            {
                usuario.Senha = model.Senha;
                System.IO.File.WriteAllText(path, JsonConvert.SerializeObject(usuarios, Formatting.Indented));
                TempData["Mensagem"] = "Senha redefinida com sucesso!";
                return RedirectToAction("Index");
            }

            ViewBag.Erro = "Erro ao redefinir senha.";
            return View("RedefinirSenha", model);
        }
    }
}