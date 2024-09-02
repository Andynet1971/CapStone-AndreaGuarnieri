namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class CheckoutViewModel
    {
        public int NumeroCamera { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public decimal Tariffa { get; set; }
        public List<ServizioAggiuntivo> ServiziAggiuntivi { get; set; }
        public decimal ImportoDaSaldare { get; set; }
    }
}
