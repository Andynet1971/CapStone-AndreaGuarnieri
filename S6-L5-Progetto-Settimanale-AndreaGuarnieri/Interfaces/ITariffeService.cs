using CapStone_AndreaGuarnieri.Models.ViewModels;
using System.Collections.Generic;

namespace CapStone_AndreaGuarnieri.Models.Interfaces
{
    public interface ITariffeService
    {
        IEnumerable<TariffaViewModel> GetAllTariffe();
        TariffaViewModel GetTariffaById(int id);
        IEnumerable<TariffaViewModel> GetOverlappingTariffe(DateTime startDate, DateTime endDate);
        void UpdateTariffa(TariffaViewModel tariffa);
        void CreateTariffa(TariffaViewModel tariffa);
        void DeleteTariffa(int id);
    }
}
