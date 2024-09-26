using System.ComponentModel.DataAnnotations;

namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class TariffaViewModel
    {
        public int ID { get; set; }

        [Required]
        [Display(Name = "Tipo Stagione")]
        public string TipoStagione { get; set; }

        [Required]
        [Display(Name = "Tipo Camera")]
        public string TipoCamera { get; set; }

        [Required]
        [Range(0, double.MaxValue, ErrorMessage = "La tariffa deve essere un valore positivo")]
        [Display(Name = "Tariffa Giornaliera")]
        public decimal TariffaGiornaliera { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data Inizio")]
        public DateTime DataInizio { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Data Fine")]
        public DateTime DataFine { get; set; }
    }
}

