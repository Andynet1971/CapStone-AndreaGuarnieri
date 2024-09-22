namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class CheckoutViewModel
    {
        // Dettagli della prenotazione
        public int NumeroCamera { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public decimal Caparra { get; set; }

        // Tariffe per periodo
        public List<TariffaPeriodoViewModel> TariffePerPeriodo { get; set; } = new List<TariffaPeriodoViewModel>();

        // Servizi aggiuntivi
        public List<ServizioAggiuntivoViewModel> ServiziAggiuntivi { get; set; }

        // Totale importo da saldare
        public decimal ImportoDaSaldare { get; set; }
    }

    public class TariffaPeriodoViewModel
    {
        // Dettagli per ogni tariffa stagionale
        public string TipoStagione { get; set; }
        public decimal TariffaGiornaliera { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
    }
}
