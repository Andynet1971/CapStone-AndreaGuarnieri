using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class ServizioAggiuntivoViewModel
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

        public List<Servizio> ServiziDisponibili { get; set; } = new List<Servizio>();

        // Proprietà aggiuntive per visualizzare i dettagli del servizio nella vista
        public string NomeServizio { get; set; }
        public decimal TariffaServizio { get; set; }
    }
}
