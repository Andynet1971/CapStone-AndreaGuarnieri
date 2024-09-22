using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using CapStone_AndreaGuarnieri.Models.ViewModels;
using CapStone_AndreaGuarnieri.Models.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using CapStone_AndreaGuarnieri.Models;

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
        public IActionResult GestisciUtenti()
        {
            var utenti = _utenteService.GetAllUtenti();
            return View(utenti);
        }
        // GET: Visualizza il modulo di modifica
        [HttpGet]
        public IActionResult ModificaUtenteGet(int id)
        {
            var utente = _utenteService.GetUtenteById(id);
            if (utente == null)
            {
                return NotFound();
            }

            return View("ModificaUtente", utente); // Qui indichi esplicitamente il nome della vista
        }

        // POST: Salva i dati modificati
        [HttpPost]
        public IActionResult ModificaUtentePost(Utente utente) // Rinominato ModificaUtentePost
        {
            if (ModelState.IsValid)
            {
                _utenteService.UpdateUtente(utente);
                return RedirectToAction("GestisciUtenti");
            }

            return View(utente);
        }
        public IActionResult Test()
        {
            return Content("Funziona!");
        }

        [HttpPost]
        public IActionResult SalvaModificheUtenti(List<Utente> utenti)
        {
            // Rimuovi i campi Salt e PasswordHash dal ModelState per ogni utente
            foreach (var i in Enumerable.Range(0, utenti.Count))
            {
                ModelState.Remove($"utenti[{i}].PasswordHash");
                ModelState.Remove($"utenti[{i}].Salt");
            }

            if (!ModelState.IsValid)
            {
                // Log degli errori di validazione
                foreach (var error in ModelState.Values.SelectMany(v => v.Errors))
                {
                    Console.WriteLine(error.ErrorMessage);
                }

                // Restituisci la vista con i dati attuali se ci sono errori di validazione
                return View("GestisciUtenti", utenti);
            }

            foreach (var utente in utenti)
            {
                // Recupera l'utente esistente
                var utenteEsistente = _utenteService.GetUtenteById(utente.ID);
                if (utenteEsistente != null)
                {
                    // Mantieni i valori di Salt e PasswordHash dall'utente esistente
                    utente.PasswordHash = utenteEsistente.PasswordHash;
                    utente.Salt = utenteEsistente.Salt;

                    // Aggiorna l'utente
                    _utenteService.UpdateUtente(utente);
                }
            }

            // Reindirizza alla pagina principale (Home)
            return RedirectToAction("Index", "Home");
        }

    }
}
