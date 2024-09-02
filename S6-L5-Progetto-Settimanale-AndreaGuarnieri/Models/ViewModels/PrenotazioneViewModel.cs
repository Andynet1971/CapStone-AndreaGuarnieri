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

        // Aggiungi la proprietà NumeroProgressivo
        public int NumeroProgressivo { get; set; }

        // Lista di camere disponibili
        public List<Camera> CamereDisponibili { get; set; }
    }
}
