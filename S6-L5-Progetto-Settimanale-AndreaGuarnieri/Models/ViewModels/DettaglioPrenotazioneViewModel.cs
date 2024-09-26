using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class DettaglioPrenotazioneViewModel
    {
        // Dettagli Prenotazione
        public int ID { get; set; }
        public string ClienteID { get; set; }
        public int CameraID { get; set; }
        [BindNever]
        public List<Camera> CamereDisponibili { get; set; } = new List<Camera>();

        public DateTime DataPrenotazione { get; set; }
        public int NumeroProgressivo { get; set; }
        public int Anno { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime DataFine { get; set; }
        public decimal Caparra { get; set; }
        public string TipoSoggiorno { get; set; }
        public decimal PrezzoTotale { get; set; }
        public bool Confermata { get; set; }

        // Dettagli Cliente
        public string Cognome { get; set; }
        public string Nome { get; set; }
        public string Citta { get; set; }
        public string Provincia { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
        public string Cellulare { get; set; }

        // Servizi Aggiuntivi (Usiamo il tuo `ServizioAggiuntivoViewModel`)
        public List<ServizioAggiuntivoViewModel> ServiziAggiuntivi { get; set; } = new List<ServizioAggiuntivoViewModel>();
    }
}
