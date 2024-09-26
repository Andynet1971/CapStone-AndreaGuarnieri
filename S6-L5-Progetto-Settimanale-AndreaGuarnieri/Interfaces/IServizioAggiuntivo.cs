using System.Collections.Generic;

namespace CapStone_AndreaGuarnieri.Models.Interfaces
{
    public interface IServizioAggiuntivo
    {
        void AddServizioAggiuntivo(ServizioAggiuntivo servizioAggiuntivo);
        IEnumerable<ServizioAggiuntivo> GetServiziAggiuntiviByPrenotazioneId(int prenotazioneID);
        IEnumerable<Servizio> GetAllServizi(); 
        void UpdateServizioAggiuntivo(ServizioAggiuntivo servizioAggiuntivo);
        void DeleteServizioAggiuntivo(int id);
    }
}
