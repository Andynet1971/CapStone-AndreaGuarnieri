using System.ComponentModel.DataAnnotations;

namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class SearchResultViewModel
    {
        [Required]
        public string CodiceFiscale { get; set; }

        [Required]
        public string Nome { get; set; }

        [Required]
        public string Cognome { get; set; }

        public List<PrenotazioneViewModel> Prenotazioni { get; set; }
    }
}
