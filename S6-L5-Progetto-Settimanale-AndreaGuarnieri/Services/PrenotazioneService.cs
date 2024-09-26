using CapStone_AndreaGuarnieri.Models.Interfaces;
using System.Globalization;
using Microsoft.Extensions.Logging;
using CapStone_AndreaGuarnieri.Models.ViewModels;

namespace CapStone_AndreaGuarnieri.Models.Services
{
    public class PrenotazioneService
    {
        private readonly IPrenotazione _prenotazioneDataAccess;
        private readonly IServizioAggiuntivo _servizioAggiuntivoDataAccess;
        private readonly ILogger<PrenotazioneService> _logger;

        // Costruttore che inizializza il data access per le prenotazioni e i servizi aggiuntivi
        public PrenotazioneService(IPrenotazione prenotazioneDataAccess, IServizioAggiuntivo servizioAggiuntivoDataAccess)
        {
            _prenotazioneDataAccess = prenotazioneDataAccess;
            _servizioAggiuntivoDataAccess = servizioAggiuntivoDataAccess;
        }

        // Metodo per ottenere tutte le prenotazioni
        public IEnumerable<PrenotazioneConCliente> GetAllPrenotazioni()
        {
            var prenotazioniViewModels = _prenotazioneDataAccess.GetAllPrenotazioni();

            // Mappare PrenotazioneViewModel a PrenotazioneConCliente
            var prenotazioniConCliente = prenotazioniViewModels.Select(pvm => new PrenotazioneConCliente
            {
                ID = pvm.ID,
                ClienteID = pvm.ClienteID,
                Cognome = pvm.Cognome,  // Include Cognome
                Nome = pvm.Nome,        // Include Nome
                CameraID = pvm.CameraID,
                DataPrenotazione = pvm.DataPrenotazione,
                NumeroProgressivo = pvm.NumeroProgressivo,
                Anno = pvm.Anno,
                DataInizio = pvm.DataInizio,
                DataFine = pvm.DataFine,
                Caparra = pvm.Caparra,
                TipoSoggiorno = pvm.TipoSoggiorno,
                PrezzoTotale = pvm.PrezzoTotale,
                Confermata = pvm.Confermata
            });

            return prenotazioniConCliente;
        }
        public void UpdatePrenotazione(DettaglioPrenotazioneViewModel model)
        {
            var prenotazione = new Prenotazione
            {
                ID = model.ID,
                ClienteID = model.ClienteID,
                CameraID = model.CameraID,
                DataPrenotazione = model.DataPrenotazione,
                NumeroProgressivo = model.NumeroProgressivo,
                Anno = model.Anno,
                DataInizio = model.DataInizio,
                DataFine = model.DataFine,
                Caparra = model.Caparra,
                TipoSoggiorno = model.TipoSoggiorno,
                PrezzoTotale = model.PrezzoTotale,
                Confermata = model.Confermata
            };

            _prenotazioneDataAccess.UpdatePrenotazione(prenotazione);
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

        // Metodo per ottenere le tariffe per un periodo e una camera specifica
        public List<Tariffa> GetTariffePerPeriodo(DateTime dataInizio, DateTime dataFine, int cameraId)
        {
            return _prenotazioneDataAccess.GetTariffePerPeriodo(dataInizio, dataFine, cameraId);
        }

        // Metodo per ottenere gli incassi per un intervallo di date
        public List<decimal> GetIncassi(List<DateTime> date)
        {
            var incassi = new List<decimal>();

            foreach (var data in date)
            {
                var incasso = _prenotazioneDataAccess.GetIncassoPerData(data);
                incassi.Add(incasso);
            }

            return incassi;
        }

        // Metodo per ottenere la somma settimanale dei servizi aggiuntivi
        public List<decimal> GetSommaServiziAggiuntivi(List<DateTime> date)
        {
            var sommeSettimanali = new List<decimal>();

            foreach (var settimana in date.GroupBy(d => CultureInfo.CurrentCulture.Calendar.GetWeekOfYear(d, CalendarWeekRule.FirstDay, DayOfWeek.Monday)))
            {
                decimal sommaServizi = _prenotazioneDataAccess.OttieniSommaServiziPerSettimana(settimana.Key);
                sommeSettimanali.Add(sommaServizi);
            }

            return sommeSettimanali;
        }
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
        // Metodo per ottenere le prenotazioni tramite ID del cliente
        public IEnumerable<Prenotazione> GetPrenotazioniByClienteId(string codiceFiscale)
        {
            return _prenotazioneDataAccess.GetPrenotazioniByClienteId(codiceFiscale);
        }
        public Prenotazione GetPrenotazioneById(int id)
        {
            return _prenotazioneDataAccess.GetPrenotazioneById(id);
        }
        public IEnumerable<Prenotazione> GetPrenotazioniNonConfermate()
        {
            return _prenotazioneDataAccess.GetPrenotazioniNonConfermate();
        }

        public Prenotazione GetPrenotazioneNonConfermataByCameraId(int cameraID)
        {
            return _prenotazioneDataAccess.GetPrenotazioneNonConfermataByCameraId(cameraID);
        }
    }
}
