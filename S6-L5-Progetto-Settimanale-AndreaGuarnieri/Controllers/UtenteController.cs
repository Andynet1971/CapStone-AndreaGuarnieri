using Microsoft.AspNetCore.Mvc;
using CapStone_AndreaGuarnieri.Models;
using CapStone_AndreaGuarnieri.Models.Services;
using CapStone_AndreaGuarnieri.Models.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace CapStone_AndreaGuarnieri.Controllers
{
    public class UtenteController : Controller
    {
        private readonly UtenteService _utenteService;

        // Costruttore che inietta il servizio UtenteService
        public UtenteController(UtenteService utenteService)
        {
            _utenteService = utenteService;
        }

        // Metodo HTTP POST per creare un nuovo utente
        [HttpPost]
        public IActionResult Create(UtenteViewModel model)
        {
            // Verifica se il modello è valido
            if (ModelState.IsValid)
            {
                // Genera il sale per l'hashing della password
                string salt = GenerateSalt();
                // Calcola l'hash della password utilizzando il sale
                string hash = HashPassword(model.Password, salt);

                // Crea un nuovo oggetto Utente e imposta le sue proprietà
                var utente = new Utente
                {
                    Username = model.Username,
                    PasswordHash = hash,
                    Salt = salt
                };

                // Aggiunge il nuovo utente al database
                _utenteService.AddUtente(utente);

                // Reindirizza all'azione Index
                return RedirectToAction("Index");
            }

            // Se il modello non è valido, ritorna alla vista con il modello attuale
            return View(model);
        }

        // Metodo per generare il sale per l'hashing della password
        private string GenerateSalt()
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                return Convert.ToBase64String(saltBytes);
            }
        }

        // Metodo per calcolare l'hash della password utilizzando il sale
        private string HashPassword(string password, string salt)
        {
            using (var deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 10000))
            {
                byte[] hashBytes = deriveBytes.GetBytes(32);
                return Convert.ToBase64String(hashBytes);
            }
        }
    }
}
