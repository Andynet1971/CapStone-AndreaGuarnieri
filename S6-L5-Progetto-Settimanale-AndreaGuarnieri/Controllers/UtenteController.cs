using Microsoft.AspNetCore.Mvc;
using CapStone_AndreaGuarnieri.Models;
using CapStone_AndreaGuarnieri.Models.Services;
using Microsoft.Extensions.Logging;
using System;
using CapStone_AndreaGuarnieri.Models.ViewModels;
using System.Security.Cryptography;
using System.Text;

namespace CapStone_AndreaGuarnieri.Controllers
{
    public class UtenteController : Controller
    {
        private readonly UtenteService _utenteService;
        private readonly ILogger<UtenteController> _logger;  // Logger iniettato

        public UtenteController(UtenteService utenteService, ILogger<UtenteController> logger)
        {
            _utenteService = utenteService;
            _logger = logger;
        }

        // Metodo per visualizzare la gestione utenti
        [HttpGet]
        public IActionResult GestisciUtenti()
        {
            var utenti = _utenteService.GetAllUtenti();
            return View(utenti); // Questo dovrebbe caricare la vista di default in /Views/Utente/GestisciUtenti.cshtml
        }

        // Metodo HTTP POST per creare un nuovo utente
        [HttpPost]
        public IActionResult Create(UtenteViewModel model)
        {
            if (ModelState.IsValid)
            {
                // Verifica che la password non sia null o vuota
                if (!string.IsNullOrWhiteSpace(model.Password))
                {
                    // Genera il sale per l'hashing della password
                    string salt = GenerateSalt();
                    // Calcola l'hash della password utilizzando il sale
                    string hash = HashPassword(model.Password, salt);

                    var utente = new Utente
                    {
                        Username = model.Username,
                        PasswordHash = hash,
                        Salt = salt,
                        Nome = model.Nome,
                        Cognome = model.Cognome,
                        Ruolo = model.Ruolo
                    };

                    _utenteService.AddUtente(utente);
                    return RedirectToAction("GestisciUtenti");
                }

                ModelState.AddModelError("", "La password non può essere vuota.");
            }
            return View(model);
        }

        // Metodo per aggiornare i dettagli di un utente
        [HttpPost]
        public IActionResult SalvaModificheUtente(Utente utente)
        {
            _logger.LogInformation("Tentativo di aggiornamento utente con ID: {ID}", utente.ID);
            _logger.LogInformation("Nome: {Nome}, Cognome: {Cognome}, Ruolo: {Ruolo}", utente.Nome, utente.Cognome, utente.Ruolo);

            // Rimuovi Salt e PasswordHash dal controllo di ModelState
            ModelState.Remove("Salt");
            ModelState.Remove("PasswordHash");

            if (!ModelState.IsValid)
            {
                foreach (var state in ModelState)
                {
                    foreach (var error in state.Value.Errors)
                    {
                        _logger.LogWarning("Errore nel campo {FieldName}: {ErrorMessage}", state.Key, error.ErrorMessage);
                    }
                }

                _logger.LogWarning("ModelState non valido per utente con ID: {ID}", utente.ID);
                return View("GestisciUtenti", _utenteService.GetAllUtenti());
            }

            // Ottieni i valori originali di Salt e PasswordHash per mantenere i valori esistenti
            var utenteEsistente = _utenteService.GetUtenteById(utente.ID);
            if (utenteEsistente != null)
            {
                _logger.LogInformation("Recupero di Salt e PasswordHash dall'utente esistente con ID: {ID}", utente.ID);

                // Mantieni Salt e PasswordHash esistenti
                utente.Salt = utenteEsistente.Salt;
                utente.PasswordHash = utenteEsistente.PasswordHash;

                // Aggiorna i dati che sono stati modificati (Nome, Cognome, Ruolo)
                utenteEsistente.Nome = utente.Nome;
                utenteEsistente.Cognome = utente.Cognome;
                utenteEsistente.Ruolo = utente.Ruolo;

                try
                {
                    _utenteService.UpdateUtente(utenteEsistente);
                    _logger.LogInformation("Utente con ID {ID} aggiornato con successo.", utente.ID);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Errore durante l'aggiornamento dell'utente con ID {ID}", utente.ID);
                    throw;
                }
            }
            else
            {
                _logger.LogWarning("Nessun utente trovato con ID: {ID}", utente.ID);
            }

            return RedirectToAction("GestisciUtenti");
        }


        // Metodo per cancellare un utente
        [HttpPost]
        public IActionResult DeleteUtente(int id)
        {
            var utente = _utenteService.GetUtenteById(id);
            if (utente == null)
            {
                _logger.LogWarning("Nessun utente trovato con ID: {ID}", id);  // Logging
                ModelState.AddModelError("", "L'utente non esiste.");
                return View("GestisciUtenti", _utenteService.GetAllUtenti());
            }

            try
            {
                _utenteService.DeleteUtente(id);
                _logger.LogInformation("Utente con ID {ID} eliminato con successo.", id);  // Logging
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Errore durante la cancellazione dell'utente con ID {ID}", id);  // Logging error
                throw;
            }

            return RedirectToAction("GestisciUtenti");
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

        // Metodo per calcolare l'hash della password
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
