using System.Collections.Generic;

namespace CapStone_AndreaGuarnieri.Models.Interfaces
{
    public interface IPrenotazione
    {
        Prenotazione GetPrenotazione(int id);
        void AddPrenotazione(Prenotazione prenotazione);
        IEnumerable<Prenotazione> GetAllPrenotazioni();
        IEnumerable<Prenotazione> GetPrenotazioniByCodiceFiscale(string codiceFiscale);
        Dictionary<string, int> GetTipologiaSoggiornoCounts();
        int GetNextProgressiveNumber();
    }
}
