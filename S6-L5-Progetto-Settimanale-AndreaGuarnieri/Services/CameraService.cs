using CapStone_AndreaGuarnieri.Models.Interfaces;

namespace CapStone_AndreaGuarnieri.Models.Services
{
    public class CameraService
    {
        private readonly ICamera _cameraDataAccess;

        // Costruttore che inizializza il data access per le camere
        public CameraService(ICamera cameraDataAccess)
        {
            _cameraDataAccess = cameraDataAccess;
        }

        // Metodo per ottenere tutte le camere disponibili
        public IEnumerable<Camera> GetCamereDisponibili()
        {
            // Chiama il metodo dal data access per ottenere tutte le camere disponibili
            return _cameraDataAccess.GetCamereDisponibili();
        }

        // Metodo per ottenere una camera in base all'ID
        public Camera GetCamera(int id)
        {
            // Chiama il metodo dal data access per ottenere una camera specifica
            return _cameraDataAccess.GetCamera(id);
        }
    }
}
