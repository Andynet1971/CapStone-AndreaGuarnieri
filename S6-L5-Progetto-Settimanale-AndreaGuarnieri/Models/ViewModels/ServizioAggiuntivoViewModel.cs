namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class ServizioAggiuntivoViewModel : ServizioAggiuntivoInputModel
    {
        // Proprietà aggiuntive per visualizzare i dettagli del servizio nella vista
        public int ID { get; set; }
        public string NomeServizio { get; set; }
        public decimal TariffaServizio { get; set; }
        public List<Servizio> ServiziDisponibili { get; set; } = new List<Servizio>();
    }
}
