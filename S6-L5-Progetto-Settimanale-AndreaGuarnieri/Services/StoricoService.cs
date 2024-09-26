using CapStone_AndreaGuarnieri.Models.Interfaces;

namespace CapStone_AndreaGuarnieri.Models.Services
{
    public class StoricoService
    {
        private readonly IStorico _storicoDataAccess;

        public StoricoService(IStorico storicoDataAccess)
        {
            _storicoDataAccess = storicoDataAccess;
        }

        public decimal GetOccupazioneMedia(DateTime dataInizio, DateTime dataFine)
        {
            // Ottieni l'elenco dei giorni tra dataInizio e dataFine
            var giorniPeriodo = (dataFine - dataInizio).TotalDays;

            // Ottieni il numero di settimane nel periodo
            var numeroSettimane = Math.Ceiling(giorniPeriodo / 7);

            // Calcola l'occupazione media come al solito
            var occupazioneTotale = _storicoDataAccess.GetOccupazioneMedia(dataInizio, dataFine);

            // Divide l'occupazione totale per il numero di settimane
            return occupazioneTotale / (decimal)numeroSettimane;
        }


        public double GetDurataMediaSoggiorni(DateTime dataInizio, DateTime dataFine)
        {
            return _storicoDataAccess.GetDurataMediaSoggiorni(dataInizio, dataFine);
        }

        public decimal GetIncassoTotale(DateTime dataInizio, DateTime dataFine)
        {
            return _storicoDataAccess.GetIncassoTotale(dataInizio, dataFine);
        }

        public decimal GetUtilizzoServiziAggiuntivi(DateTime dataInizio, DateTime dataFine)
        {
            return _storicoDataAccess.GetUtilizzoServiziAggiuntivi(dataInizio, dataFine);
        }

        public decimal GetPercentualeClientiConServiziAggiuntivi(DateTime dataInizio, DateTime dataFine)
        {
            return _storicoDataAccess.GetPercentualeClientiConServiziAggiuntivi(dataInizio, dataFine);
        }
        public List<int> GetTassoOccupazione(List<DateTime> date)
        {
            var tassiOccupazione = new List<int>();

            foreach (var data in date)
            {
                var prenotazioni = _storicoDataAccess.GetPrenotazioniPerData(data);
                var numeroStanzeTotali = _storicoDataAccess.GetNumeroStanzeTotali();
                int tasso = (prenotazioni.Count() * 100) / numeroStanzeTotali;
                tassiOccupazione.Add(tasso);
            }

            return tassiOccupazione;
        }

        public List<decimal> GetIncassi(List<DateTime> date)
        {
            var incassi = new List<decimal>();

            foreach (var data in date)
            {
                var incasso = _storicoDataAccess.GetIncassoPerData(data);
                incassi.Add(incasso);
            }

            return incassi;
        }

        public decimal CalcolaOccupazioneMedia(List<DateTime> date)
        {
            var tassiOccupazione = GetTassoOccupazione(date);
            return tassiOccupazione.Count > 0 ? (decimal)tassiOccupazione.Average() : 0;
        }

        public decimal CalcolaDurataMediaSoggiorni(List<Prenotazione> prenotazioni)
        {
            var durate = prenotazioni.Select(p => (p.DataFine - p.DataInizio).Days);
            return durate.Count() > 0 ? (decimal)durate.Average() : 0;
        }

        public decimal CalcolaPercentualeClientiUsaServiziAggiuntivi(List<Prenotazione> prenotazioni)
        {
            int clientiConServizi = prenotazioni.Count(p => p.ServiziAggiuntivi.Count() > 0);
            return prenotazioni.Count() > 0 ? (decimal)clientiConServizi / prenotazioni.Count() * 100 : 0;
        }

        public decimal CalcolaIncassoTotalePeriodo(List<Prenotazione> prenotazioni)
        {
            return prenotazioni.Sum(p => p.PrezzoTotale);
        }

        public int CalcolaUtilizzoServiziAggiuntivi(List<Prenotazione> prenotazioni)
        {
            return prenotazioni.Sum(p => p.ServiziAggiuntivi.Sum(s => s.Quantita));
        }
    }
}
