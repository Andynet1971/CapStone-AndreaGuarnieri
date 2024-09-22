namespace CapStone_AndreaGuarnieri.Models.Interfaces
{
    public interface IPrenotazione
    {
        // Metodi esistenti
        Prenotazione GetPrenotazione(int id);
        void AddPrenotazione(Prenotazione prenotazione);
        IEnumerable<Prenotazione> GetAllPrenotazioni();
        IEnumerable<Prenotazione> GetPrenotazioniByCodiceFiscale(string codiceFiscale);
        Dictionary<string, int> GetTipologiaSoggiornoCounts();
        int GetNextProgressiveNumber();
        List<Tariffa> GetTariffePerPeriodo(DateTime dataInizio, DateTime dataFine, int cameraId);

        // Nuovi metodi per lo storico delle presenze
        IEnumerable<Prenotazione> GetPrenotazioniPerData(DateTime data);
        int GetNumeroStanzeTotali();
        // Metodo per ottenere gli incassi per una data specifica
        decimal GetIncassoPerData(DateTime data);
    }
}
