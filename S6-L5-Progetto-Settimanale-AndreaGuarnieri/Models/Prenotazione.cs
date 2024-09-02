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

        public Camera Camera { get; set; }
        public Cliente Cliente { get; set; }
    }
}
