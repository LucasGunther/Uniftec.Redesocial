using Microsoft.AspNetCore.Mvc;

namespace Uiftec.PPW.Redesocial.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Login(string username, string senha)
        {
            // Aqui você poderia validar o usuário/senha (exemplo simples)

            // Se login for válido, redireciona para a Home (Index)
            return RedirectToAction("Index", "Home");

            // Se login inválido (exemplo opcional)
            // return View("Login");  // Volta para a página de login
        }
    }
}
