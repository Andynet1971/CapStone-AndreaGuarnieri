using CapStone_AndreaGuarnieri.DataAccess;
using CapStone_AndreaGuarnieri.Models.Interfaces;
using CapStone_AndreaGuarnieri.Models.ViewModels;

namespace CapStone_AndreaGuarnieri.Services
{
    public class TariffeService : ITariffeService
    {
        private readonly TariffeDataAccess _tariffeDataAccess; 

        public TariffeService(TariffeDataAccess tariffeDataAccess) 
        {
            _tariffeDataAccess = tariffeDataAccess;
        }

        public IEnumerable<TariffaViewModel> GetAllTariffe()
        {
            return _tariffeDataAccess.GetAllTariffe();
        }

        public TariffaViewModel GetTariffaById(int id)
        {
            return _tariffeDataAccess.GetTariffaById(id);
        }

        public void UpdateTariffa(TariffaViewModel tariffa)
        {
            _tariffeDataAccess.UpdateTariffa(tariffa);
        }


        public void CreateTariffa(TariffaViewModel tariffa)
        {
            var overlappingTariffe = _tariffeDataAccess.GetOverlappingTariffe(tariffa.DataInizio, tariffa.DataFine);

            foreach (var existingTariffa in overlappingTariffe)
            {
                if (existingTariffa.DataInizio < tariffa.DataInizio)
                {
                    var prePeriod = new TariffaViewModel
                    {
                        TipoStagione = existingTariffa.TipoStagione,
                        TipoCamera = existingTariffa.TipoCamera,
                        TariffaGiornaliera = existingTariffa.TariffaGiornaliera,
                        DataInizio = existingTariffa.DataInizio,
                        DataFine = tariffa.DataInizio.AddDays(-1)
                    };
                    _tariffeDataAccess.CreateTariffa(prePeriod);
                }

                if (existingTariffa.DataFine > tariffa.DataFine)
                {
                    var postPeriod = new TariffaViewModel
                    {
                        TipoStagione = existingTariffa.TipoStagione,
                        TipoCamera = existingTariffa.TipoCamera,
                        TariffaGiornaliera = existingTariffa.TariffaGiornaliera,
                        DataInizio = tariffa.DataFine.AddDays(1),
                        DataFine = existingTariffa.DataFine
                    };
                    _tariffeDataAccess.CreateTariffa(postPeriod);
                }

                _tariffeDataAccess.DeleteTariffa(existingTariffa.ID);
            }

            _tariffeDataAccess.CreateTariffa(tariffa);
        }

        public void DeleteTariffa(int id)
        {
            _tariffeDataAccess.DeleteTariffa(id);
        }
        public IEnumerable<TariffaViewModel> GetOverlappingTariffe(DateTime startDate, DateTime endDate)
        {
            return _tariffeDataAccess.GetOverlappingTariffe(startDate, endDate);
        }
    }
}
