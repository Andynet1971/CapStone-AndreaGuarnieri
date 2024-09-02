using System.ComponentModel.DataAnnotations;

namespace CapStone_AndreaGuarnieri.Models
{
    public class Cliente
    {
        [Required]
        [StringLength(16)]
        public string CodiceFiscale { get; set; }

        [Required]
        [StringLength(50)]
        public string Cognome { get; set; }

        [Required]
        [StringLength(50)]
        public string Nome { get; set; }

        [Required]
        [StringLength(50)]
        public string Citta { get; set; }

        [Required]
        [StringLength(50)]
        public string Provincia { get; set; }

        [Required]
        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(20)]
        public string Telefono { get; set; }

        [Required]
        [StringLength(20)]
        public string Cellulare { get; set; }
    }
}
