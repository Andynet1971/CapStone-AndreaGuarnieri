namespace CapStone_AndreaGuarnieri.Models
{
    public class Tariffa
    {
        public int ID { get; set; }
        public string TipoStagione { get; set; }
        public string TipoCamera { get; set; } // Aggiungi questa proprietà
        public decimal TariffaGiornaliera { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
    }
}
