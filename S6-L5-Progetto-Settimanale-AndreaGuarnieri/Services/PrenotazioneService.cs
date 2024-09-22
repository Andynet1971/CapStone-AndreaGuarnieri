using CapStone_AndreaGuarnieri.Models.Interfaces;
using CapStone_AndreaGuarnieri.Models;
using System;
using System.Collections.Generic;

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
            return _prenotazioneDataAccess.GetAllPrenotazioni();
        }

        // Metodo per ottenere una prenotazione in base all'ID
        public Prenotazione GetPrenotazione(int id)
        {
            return _prenotazioneDataAccess.GetPrenotazione(id);
        }

        // Metodo per aggiungere una nuova prenotazione
        public void AddPrenotazione(Prenotazione prenotazione)
        {
            _prenotazioneDataAccess.AddPrenotazione(prenotazione);
        }

        // Metodo per ottenere i servizi aggiuntivi in base all'ID della prenotazione
        public IEnumerable<ServizioAggiuntivo> GetServiziAggiuntivi(int prenotazioneID)
        {
            return _servizioAggiuntivoDataAccess.GetServiziAggiuntiviByPrenotazioneId(prenotazioneID);
        }

        // Metodo per ottenere le prenotazioni in base al codice fiscale
        public IEnumerable<Prenotazione> GetPrenotazioniByCodiceFiscale(string codiceFiscale)
        {
            return _prenotazioneDataAccess.GetPrenotazioniByCodiceFiscale(codiceFiscale);
        }

        // Metodo per ottenere il conteggio delle diverse tipologie di soggiorno
        public Dictionary<string, int> GetTipologiaSoggiornoCounts()
        {
            return _prenotazioneDataAccess.GetTipologiaSoggiornoCounts();
        }

        // Metodo per ottenere il prossimo numero progressivo disponibile
        public int GetNextProgressiveNumber()
        {
            return _prenotazioneDataAccess.GetNextProgressiveNumber();
        }

        // Metodo aggiuntivo per ottenere le tariffe per un periodo e una camera specifica
        public List<Tariffa> GetTariffePerPeriodo(DateTime dataInizio, DateTime dataFine, int cameraId)
        {
            return _prenotazioneDataAccess.GetTariffePerPeriodo(dataInizio, dataFine, cameraId);
        }

        // Metodo per ottenere il tasso di occupazione per un intervallo di date
        public List<int> GetTassoOccupazione(List<DateTime> date)
        {
            var tassiOccupazione = new List<int>();

            foreach (var data in date)
            {
                // Ottieni le prenotazioni per una data specifica
                var prenotazioni = _prenotazioneDataAccess.GetPrenotazioniPerData(data);

                // Ottieni il numero totale di stanze
                var numeroStanzeTotali = _prenotazioneDataAccess.GetNumeroStanzeTotali();

                // Calcola il tasso di occupazione come percentuale
                int tasso = (prenotazioni.Count() * 100) / numeroStanzeTotali;
                tassiOccupazione.Add(tasso);
            }

            return tassiOccupazione;
        }
        // Metodo per ottenere gli incassi per un intervallo di date
        public List<decimal> GetIncassi(List<DateTime> date)
        {
            var incassi = new List<decimal>();

            foreach (var data in date)
            {
                var incasso = _prenotazioneDataAccess.GetIncassoPerData(data); // Dovrai implementare questo metodo nel data access
                incassi.Add(incasso);
            }

            return incassi;
        }
    }
}
