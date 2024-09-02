using CapStone_AndreaGuarnieri.Models.Interfaces;

namespace CapStone_AndreaGuarnieri.Models.Services
{
    public class PrenotazioneService
    {
        private readonly IPrenotazione _prenotazioneDataAccess;
        private readonly IServizioAggiuntivo _servizioAggiuntivoDataAccess;

        // Costruttore che inizializza il data access per le prenotazioni e i servizi aggiuntivi
        public PrenotazioneService(IPrenotazione prenotazioneDataAccess, IServizioAggiuntivo servizioAggiuntivoDataAccess)
        {
            _prenotazioneDataAccess = prenotazioneDataAccess;
            _servizioAggiuntivoDataAccess = servizioAggiuntivoDataAccess;
        }

        // Metodo per ottenere tutte le prenotazioni
        public IEnumerable<Prenotazione> GetAllPrenotazioni()
        {
            // Chiama il metodo dal data access per ottenere tutte le prenotazioni
            return _prenotazioneDataAccess.GetAllPrenotazioni();
        }

        // Metodo per ottenere una prenotazione in base all'ID
        public Prenotazione GetPrenotazione(int id)
        {
            // Chiama il metodo dal data access per ottenere una prenotazione specifica
            return _prenotazioneDataAccess.GetPrenotazione(id);
        }

        // Metodo per aggiungere una nuova prenotazione
        public void AddPrenotazione(Prenotazione prenotazione)
        {
            // Chiama il metodo dal data access per aggiungere la prenotazione
            _prenotazioneDataAccess.AddPrenotazione(prenotazione);
        }

        // Metodo per ottenere i servizi aggiuntivi in base all'ID della prenotazione
        public IEnumerable<ServizioAggiuntivo> GetServiziAggiuntivi(int prenotazioneID)
        {
            // Chiama il metodo dal data access per ottenere i servizi aggiuntivi di una specifica prenotazione
            return _servizioAggiuntivoDataAccess.GetServiziAggiuntiviByPrenotazioneId(prenotazioneID);
        }

        // Metodo per ottenere le prenotazioni in base al codice fiscale
        public IEnumerable<Prenotazione> GetPrenotazioniByCodiceFiscale(string codiceFiscale)
        {
            // Chiama il metodo dal data access per ottenere le prenotazioni in base al codice fiscale del cliente
            return _prenotazioneDataAccess.GetPrenotazioniByCodiceFiscale(codiceFiscale);
        }

        // Metodo per ottenere il conteggio delle diverse tipologie di soggiorno
        public Dictionary<string, int> GetTipologiaSoggiornoCounts()
        {
            // Chiama il metodo dal data access per ottenere il conteggio delle diverse tipologie di soggiorno
            return _prenotazioneDataAccess.GetTipologiaSoggiornoCounts();
        }

        // Metodo per ottenere il prossimo numero progressivo disponibile
        public int GetNextProgressiveNumber()
        {
            // Chiama il metodo dal data access per ottenere il prossimo numero progressivo disponibile
            return _prenotazioneDataAccess.GetNextProgressiveNumber();
        }
    }
}
