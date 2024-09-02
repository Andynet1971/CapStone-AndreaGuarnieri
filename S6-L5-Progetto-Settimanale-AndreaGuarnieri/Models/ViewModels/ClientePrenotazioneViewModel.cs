using System.ComponentModel.DataAnnotations;

namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class ClientePrenotazioneViewModel
    {
        // Cliente properties
        [Required]
        [Display(Name = "Codice Fiscale")]
        public string ClienteCodiceFiscale { get; set; }

        [Required]
        [Display(Name = "Cognome")]
        public string ClienteCognome { get; set; }

        [Required]
        [Display(Name = "Nome")]
        public string ClienteNome { get; set; }

        [Required]
        [Display(Name = "Città")]
        public string ClienteCitta { get; set; }

        [Required]
        [Display(Name = "Provincia")]
        public string ClienteProvincia { get; set; }

        [Required]
        [Display(Name = "Email")]
        public string ClienteEmail { get; set; }

        [Display(Name = "Telefono")]
        public string ClienteTelefono { get; set; }

        [Required]
        [Display(Name = "Cellulare")]
        public string ClienteCellulare { get; set; }

        // Prenotazione properties
        [Required]
        [Display(Name = "Camera")]
        public int CameraID { get; set; }

        [Required]
        [Display(Name = "Data Prenotazione")]
        public DateTime DataPrenotazione { get; set; }

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

        // Lista di camere disponibili
        [Display(Name = "Camere Disponibili")]
        public List<Camera> CamereDisponibili { get; set; } = new List<Camera>();
    }
}
