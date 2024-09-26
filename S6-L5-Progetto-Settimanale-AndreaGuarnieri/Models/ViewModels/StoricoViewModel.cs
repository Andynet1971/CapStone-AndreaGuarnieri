namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class StoricoViewModel
    {
        public decimal OccupazioneMedia { get; set; }
        public double DurataMediaSoggiorni { get; set; }
        public decimal IncassoTotale { get; set; }
        public decimal UtilizzoServiziAggiuntivi { get; set; }
        public decimal PercentualeClientiConServiziAggiuntivi { get; set; }

        // Inizializza le liste come vuote per evitare null reference
        public List<DateTime> Date { get; set; } = new List<DateTime>();
        public string PeriodoSelezionato { get; set; } = string.Empty;
        public List<int> TassoOccupazione { get; set; } = new List<int>();
        public List<decimal> Incassi { get; set; } = new List<decimal>();
    }
}
