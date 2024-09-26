namespace CapStone_AndreaGuarnieri.Models.Interfaces
{
    public interface IStorico
    {
        // Metodi per il calcolo delle statistiche dello storico
        IEnumerable<Prenotazione> GetPrenotazioniPerDateRange(DateTime dataInizio, DateTime dataFine);
        decimal GetOccupazioneMedia(DateTime dataInizio, DateTime dataFine);
        double GetDurataMediaSoggiorni(DateTime dataInizio, DateTime dataFine);
        decimal GetIncassoTotalePerDateRange(DateTime dataInizio, DateTime dataFine);
        decimal GetPercentualeClientiConServiziAggiuntivi(DateTime dataInizio, DateTime dataFine);
        int GetNumeroStanzeTotali();
        decimal GetIncassoTotale(DateTime dataInizio, DateTime dataFine);
        decimal GetUtilizzoServiziAggiuntivi(DateTime dataInizio, DateTime dataFine);
        IEnumerable<Prenotazione> GetPrenotazioniPerData(DateTime data);
        decimal GetUtilizzoServiziAggiuntiviPerDateRange(DateTime dataInizio, DateTime dataFine);
        decimal GetIncassoPerData(DateTime data);
    }
}
