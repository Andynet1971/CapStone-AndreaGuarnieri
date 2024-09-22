using System.ComponentModel.DataAnnotations;

namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class PrenotazioneViewModel
    {
        public int ID { get; set; }
        public string ClienteID { get; set; }

        [Required]
        [Display(Name = "Numero Camera")]
        public int CameraID { get; set; }

        [Required]
        [Display(Name = "Data Prenotazione")]
        public DateTime DataPrenotazione { get; set; }

        [Required]
        [Display(Name = "Anno")]
        public int Anno { get; set; }

        [Required]
        [Display(Name = "Data Inizio")]
        public DateTime DataInizio { get; set; }

        [Required]
        [Display(Name = "Data Fine")]
        public DateTime DataFine { get; set; }

        [Required]
        [Display(Name = "Caparra")]
        public decimal Caparra { get; set; }

        [Required]
        [Display(Name = "Tipo Soggiorno")]
        public string TipoSoggiorno { get; set; }

        public int NumeroProgressivo { get; set; }

        [Required]
        [Display(Name = "Prezzo Totale")]
        public decimal PrezzoTotale { get; set; } // Campo per il prezzo totale

        [Required]
        [Display(Name = "Confermata")]
        public bool Confermata { get; set; } = false; // Campo booleano, impostato a false per default

        // Lista di camere disponibili
        public List<Camera> CamereDisponibili { get; set; }
    }
}
