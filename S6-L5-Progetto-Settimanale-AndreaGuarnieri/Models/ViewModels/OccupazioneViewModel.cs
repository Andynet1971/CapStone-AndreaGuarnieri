namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class OccupazioneViewModel
    {
        public List<DateTime> Date { get; set; }
        public List<int> TassoOccupazione { get; set; }
        public List<decimal> Incassi { get; set; }
        public string PeriodoSelezionato { get; set; }
    }
}
