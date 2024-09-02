using System.Security.Cryptography;
using System.Text;
using CapStone_AndreaGuarnieri.Models.Interfaces;

namespace CapStone_AndreaGuarnieri.Models.Services
{
    public class UtenteService
    {
        private readonly IUtente _utenteDataAccess;

        // Costruttore che inizializza il data access per l'utente
        public UtenteService(IUtente utenteDataAccess)
        {
            _utenteDataAccess = utenteDataAccess;
        }

        // Metodo per verificare la password dell'utente
        public bool VerifyPassword(string username, string password)
        {
            // Recupera l'utente dal data access
            var user = _utenteDataAccess.GetUtente(username);
            if (user == null)
            {
                return false; // Restituisce false se l'utente non esiste
            }

            // Usa Rfc2898DeriveBytes per derivare una chiave dalla password e dal salt dell'utente
            using (var deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(user.Salt), 10000))
            {
                byte[] hashBytes = deriveBytes.GetBytes(32); // Calcola l'hash della password
                string computedHash = Convert.ToBase64String(hashBytes); // Converte l'hash in stringa base64
                return computedHash == user.PasswordHash; // Confronta l'hash calcolato con l'hash memorizzato
            }
        }

        // Metodo per aggiungere un nuovo utente
        public void AddUtente(Utente utente)
        {
            // Aggiunge l'utente al data access
            _utenteDataAccess.AddUtente(utente);
        }
    }
}
