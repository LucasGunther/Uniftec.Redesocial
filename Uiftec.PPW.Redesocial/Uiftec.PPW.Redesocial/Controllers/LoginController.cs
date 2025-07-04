using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Hosting;
using Newtonsoft.Json;
using Uiftec.PPW.Redesocial.Models;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text;
using System.Net.Http;

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
        public async Task<IActionResult> Login(string username, string senha)
        {
            var client = new HttpClient(); // ou _httpClientFactory.CreateClient()

            var loginData = new
            {
                Username = username,
                Senha = senha
            };

            var content = new StringContent(
                JsonConvert.SerializeObject(loginData),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("https://suaapi.com/api/login", content); ////definir api

            if (response.IsSuccessStatusCode)
            {
                var responseBody = await response.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<UsuarioModel>(responseBody);

                HttpContext.Session.SetString("UsuarioLogadoId", usuario.ID.ToString());

                // (Opcional: salvar token JWT se a API enviar)
                // HttpContext.Session.SetString("AuthToken", token);

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
        public async Task<IActionResult> EsqueciSenha(string username)
        {
            var client = new HttpClient();

            var content = new StringContent(
                JsonConvert.SerializeObject(new { Username = username }),
                Encoding.UTF8,
                "application/json"
            );

            var response = await client.PostAsync("https://suaapi.com/api/usuarios/esqueci-senha", content);

            if (response.IsSuccessStatusCode)
            {
                var json = await response.Content.ReadAsStringAsync();
                var usuario = JsonConvert.DeserializeObject<UsuarioModel>(json);

                // Pode passar o ID ou token recebido da API para a view de redefinir
                return View("RedefinirSenha", usuario);
            }

            ViewBag.Erro = "Usuário não encontrado.";
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> RedefinirSenha(UsuarioModel model)
        {
            if (model == null || string.IsNullOrEmpty(model.Senha))
            {
                ViewBag.Erro = "Senha inválida.";
                return View("RedefinirSenha", model);
            }

            try
            {
                var client = new HttpClient();

                var content = new StringContent(
                    JsonConvert.SerializeObject(new
                    {
                        Id = model.ID,
                        NovaSenha = model.Senha
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                // Faça o PUT ou POST de acordo com a definição da API
                var response = await client.PutAsync("https://suaapi.com/api/usuarios/redefinir-senha", content);

                if (response.IsSuccessStatusCode)
                {
                    TempData["Mensagem"] = "Senha redefinida com sucesso!";
                    return RedirectToAction("Index");
                }

                ViewBag.Erro = "Erro ao redefinir senha.";
                return View("RedefinirSenha", model);
            }
            catch (Exception ex)
            {
                ViewBag.Erro = $"Erro ao conectar à API: {ex.Message}";
                return View("RedefinirSenha", model);
            }
        }


    }
}