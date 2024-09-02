using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CapStone_AndreaGuarnieri.Controllers
{
    [Authorize]  // Applica l'autorizzazione a tutto il controller
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]  // Permette l'accesso alla pagina di errore senza autenticazione
        public IActionResult Error()
        {
            return View();
        }
    }
}
