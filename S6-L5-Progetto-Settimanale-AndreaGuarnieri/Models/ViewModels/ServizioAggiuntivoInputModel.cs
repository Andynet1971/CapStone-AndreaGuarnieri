using System.ComponentModel.DataAnnotations;

namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class ServizioAggiuntivoInputModel
    {
        [Required]
        public int PrenotazioneID { get; set; }

        [Required]
        public int ServizioID { get; set; }

        [Required]
        [DataType(DataType.Date)]
        public DateTime Data { get; set; }

        [Required]
        public int Quantita { get; set; }
    }
}
