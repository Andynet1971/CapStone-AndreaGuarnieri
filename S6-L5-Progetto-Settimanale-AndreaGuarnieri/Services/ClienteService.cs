using CapStone_AndreaGuarnieri.Models.Interfaces;
using CapStone_AndreaGuarnieri.Models.ViewModels;

namespace CapStone_AndreaGuarnieri.Models.Services
{
    public class ClienteService
    {
        private readonly ICliente _clienteDataAccess;

        // Costruttore che inizializza il data access per i clienti
        public ClienteService(ICliente clienteDataAccess)
        {
            _clienteDataAccess = clienteDataAccess;
        }

        // Metodo per ottenere tutti i clienti
        public IEnumerable<Cliente> GetAllClienti()
        {
            // Chiama il metodo dal data access per ottenere tutti i clienti
            return _clienteDataAccess.GetAllClienti();
        }

        // Metodo per ottenere un cliente in base al codice fiscale
        public Cliente GetCliente(string codiceFiscale)
        {
            // Chiama il metodo dal data access per ottenere un cliente specifico
            return _clienteDataAccess.GetCliente(codiceFiscale);
        }

        // Metodo per aggiungere un nuovo cliente
        public void AddCliente(Cliente cliente)
        {
            // Chiama il metodo dal data access per aggiungere il cliente
            _clienteDataAccess.AddCliente(cliente);
        }
        public Cliente GetClienteByCodiceFiscaleOrCognome(string query)
        {
            return _clienteDataAccess.GetClienteByCodiceFiscaleOrCognome(query);
        }
        public void UpdateCliente(DettaglioPrenotazioneViewModel model)
        {
            var cliente = new Cliente
            {
                CodiceFiscale = model.ClienteID,
                Nome = model.Nome,
                Cognome = model.Cognome,
                Citta = model.Citta,
                Provincia = model.Provincia,
                Email = model.Email,
                Telefono = model.Telefono,
                Cellulare = model.Cellulare
            };

            _clienteDataAccess.UpdateCliente(cliente);
        }
    }
}
