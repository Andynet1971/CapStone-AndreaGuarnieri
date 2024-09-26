using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using CapStone_AndreaGuarnieri.Models.ViewModels;
using CapStone_AndreaGuarnieri.Models.Services;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace CapStone_AndreaGuarnieri.Controllers
{
    public class AccountController : Controller
    {
        private readonly UtenteService _utenteService;

        public AccountController(UtenteService utenteService)
        {
            _utenteService = utenteService;
        }

        // Metodo per il login
        [AllowAnonymous]
        public IActionResult Login(string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid && _utenteService.VerifyPassword(model.Username, model.Password))
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, model.Username) };
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties { IsPersistent = model.RememberMe };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                return Redirect(model.ReturnUrl ?? "/");
            }

            ModelState.AddModelError("", "Username o password non validi");
            return View(model);
        }

        // Metodo per il logout
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
