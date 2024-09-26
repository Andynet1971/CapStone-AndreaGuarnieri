using CapStone_AndreaGuarnieri.Models.ViewModels;

namespace CapStone_AndreaGuarnieri.Models.Interfaces
{
    public interface IPrenotazione
    {
        // Metodi per la gestione delle prenotazioni
        Prenotazione GetPrenotazione(int id);
        void AddPrenotazione(Prenotazione prenotazione);
        IEnumerable<PrenotazioneViewModel> GetAllPrenotazioni();
        Dictionary<string, int> GetTipologiaSoggiornoCounts();
        int GetNextProgressiveNumber();
        List<Tariffa> GetTariffePerPeriodo(DateTime dataInizio, DateTime dataFine, int cameraId);

        // Metodo per ottenere le prenotazioni per una data specifica
        IEnumerable<Prenotazione> GetPrenotazioniPerData(DateTime data);

        // Metodi per tariffe e servizi aggiuntivi
        decimal OttieniSommaServiziPerSettimana(int numeroSettimana);
        decimal GetIncassoPerData(DateTime data);
        int GetNumeroStanzeTotali();
        IEnumerable<Prenotazione> GetPrenotazioniByClienteId(string clienteId);
        Prenotazione GetPrenotazioneById(int id);
        void UpdatePrenotazione(Prenotazione prenotazione);
        IEnumerable<Prenotazione> GetPrenotazioniNonConfermate();
        Prenotazione GetPrenotazioneNonConfermataByCameraId(int cameraID);
    }
}
