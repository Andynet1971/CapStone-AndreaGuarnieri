using System.ComponentModel.DataAnnotations;

namespace CapStone_AndreaGuarnieri.Models
{
    public class ServizioAggiuntivo
    {
        [Key]
        public int ID { get; set; }

        [Required]
        public int PrenotazioneID { get; set; }

        [Required]
        public int ServizioID { get; set; }

        [Required]
        public DateTime Data { get; set; }

        [Required]
        public int Quantita { get; set; }

        // Proprietà di navigazione
        public virtual Servizio Servizio { get; set; }
        public virtual Prenotazione Prenotazione { get; set; }
    }
}
