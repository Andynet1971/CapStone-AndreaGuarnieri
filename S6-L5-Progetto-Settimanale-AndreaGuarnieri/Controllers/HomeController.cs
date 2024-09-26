using CapStone_AndreaGuarnieri.Models.Services; // Per CameraService
using CapStone_AndreaGuarnieri.Models; // Per Camera model
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CapStone_AndreaGuarnieri.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly CameraService _cameraService;

        // Costruttore per iniettare il CameraService
        public HomeController(CameraService cameraService)
        {
            _cameraService = cameraService;
        }

        public IActionResult Index()
        {
            // Supponiamo che tu voglia mostrare tutte le camere, indipendentemente dalla tipologia
            var camere = _cameraService.GetCamereDisponibili();

            if (camere == null || !camere.Any())
            {
                return NotFound("Nessuna camera disponibile.");
            }

            // Passa la lista delle camere alla vista
            return View(camere);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [AllowAnonymous]
        public IActionResult Error()
        {
            return View();
        }
    }
}
