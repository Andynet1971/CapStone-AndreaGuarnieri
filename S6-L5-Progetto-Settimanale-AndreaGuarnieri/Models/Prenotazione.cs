using System;
using System.ComponentModel.DataAnnotations;

namespace CapStone_AndreaGuarnieri.Models
{
    public class Prenotazione
    {
        public int ID { get; set; }

        [Required]
        public string ClienteID { get; set; }

        [Required]
        public int CameraID { get; set; }

        [Required]
        public DateTime DataPrenotazione { get; set; }

        [Required]
        public int NumeroProgressivo { get; set; }

        [Required]
        public int Anno { get; set; }

        [Required]
        public DateTime DataInizio { get; set; }

        [Required]
        public DateTime DataFine { get; set; }

        [Required]
        public decimal Caparra { get; set; }

        [Required]
        public string TipoSoggiorno { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "Il Prezzo Totale deve essere maggiore o uguale a 0.")]
        public decimal PrezzoTotale { get; set; } = 0; // Valore predefinito

        [Required]
        public bool Confermata { get; set; } = false; // Valore predefinito

        public Camera Camera { get; set; }
        public Cliente Cliente { get; set; }
        public ICollection<ServizioAggiuntivo> ServiziAggiuntivi { get; set; }
    }
}
