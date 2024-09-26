using CapStone_AndreaGuarnieri.Models.Interfaces;
using CapStone_AndreaGuarnieri.Models.ViewModels;

namespace CapStone_AndreaGuarnieri.Models.Services
{
    public class ServizioAggiuntivoService
    {
        private readonly IServizioAggiuntivo _servizioAggiuntivoDataAccess;

        // Costruttore che inizializza il data access per i servizi aggiuntivi
        public ServizioAggiuntivoService(IServizioAggiuntivo servizioAggiuntivoDataAccess)
        {
            _servizioAggiuntivoDataAccess = servizioAggiuntivoDataAccess;
        }

        // Metodo per aggiungere un nuovo servizio aggiuntivo
        public void AddServizioAggiuntivo(ServizioAggiuntivo servizioAggiuntivo)
        {
            // Chiama il metodo dal data access per aggiungere il servizio aggiuntivo
            _servizioAggiuntivoDataAccess.AddServizioAggiuntivo(servizioAggiuntivo);
        }

        // Metodo per ottenere tutti i servizi aggiuntivi in base all'ID della prenotazione
        public IEnumerable<ServizioAggiuntivo> GetServiziAggiuntiviByPrenotazioneId(int prenotazioneID)
        {
            // Chiama il metodo dal data access per ottenere i servizi aggiuntivi in base all'ID della prenotazione
            return _servizioAggiuntivoDataAccess.GetServiziAggiuntiviByPrenotazioneId(prenotazioneID);
        }

        // Metodo per ottenere tutti i servizi
        public IEnumerable<Servizio> GetAllServizi()
        {
            // Chiama il metodo dal data access per ottenere tutti i servizi
            return _servizioAggiuntivoDataAccess.GetAllServizi();
        }
        public void UpdateServizioAggiuntivo(ServizioAggiuntivoViewModel servizio)
        {
            var servizioAggiuntivo = new ServizioAggiuntivo
            {
                ID = servizio.ID,
                PrenotazioneID = servizio.PrenotazioneID,
                ServizioID = servizio.ServizioID,
                Data = servizio.Data,
                Quantita = servizio.Quantita
            };

            _servizioAggiuntivoDataAccess.UpdateServizioAggiuntivo(servizioAggiuntivo);
        }
        public void DeleteServizioAggiuntivo(int id)
        {
            _servizioAggiuntivoDataAccess.DeleteServizioAggiuntivo(id);
        }
    }
}
