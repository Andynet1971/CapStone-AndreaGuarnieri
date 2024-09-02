using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using CapStone_AndreaGuarnieri.Models.ViewModels;
using CapStone_AndreaGuarnieri.Models.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CapStone_AndreaGuarnieri
    .Controllers
{
    public class AccountController : Controller
    {
        private readonly UtenteService _utenteService;

        // Costruttore che inietta il servizio UtenteService
        public AccountController(UtenteService utenteService)
        {
            _utenteService = utenteService;
        }

        // Metodo per visualizzare la pagina di login
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        // Metodo POST per effettuare il login
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            // Verifica se il modello è valido
            if (ModelState.IsValid)
            {
                // Verifica le credenziali dell'utente
                if (_utenteService.VerifyPassword(model.Username, model.Password))
                {
                    // Crea i claim per l'utente autenticato
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, model.Username)
                    };

                    var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    // Configura le proprietà dell'autenticazione
                    var authProperties = new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = model.RememberMe ? DateTimeOffset.UtcNow.AddMinutes(30) : (DateTimeOffset?)null
                    };

                    // Effettua il login dell'utente
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                    // Reindirizza l'utente alla pagina di ritorno o alla home page
                    return Redirect(model.ReturnUrl ?? "/");
                }

                // Aggiunge un messaggio di errore se le credenziali non sono valide
                ModelState.AddModelError("", "Username o password non validi");
                // Reindirizza alla vista LoginFailed in caso di errore di login
                return RedirectToAction("LoginFailed");
            }

            // Passa l'URL di ritorno alla vista
            ViewData["ReturnUrl"] = model.ReturnUrl;
            return View(model);
        }

        // Metodo per visualizzare la pagina di login fallito
        [AllowAnonymous]
        public IActionResult LoginFailed()
        {
            return View();
        }

        // Metodo POST per effettuare il logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            // Effettua il logout dell'utente
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
