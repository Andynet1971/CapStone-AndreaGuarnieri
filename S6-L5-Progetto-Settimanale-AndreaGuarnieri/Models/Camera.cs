using System.ComponentModel.DataAnnotations;

namespace CapStone_AndreaGuarnieri.Models
{
    public class Camera
    {
        [Key]
        public int Numero { get; set; }

        [Required]
        public string Descrizione { get; set; }

        [Required]
        public string Tipologia { get; set; }

        [Required]
        public decimal TariffaGiornaliera { get; set; }

        [Required]
        public bool Disponibile { get; set; }

        // Aggiungi la proprietà combinata NumeroDescrizione
        public string NumeroDescrizione
        {
            get
            {
                return $"{Numero} - {Descrizione}";
            }
        }
    }
}
