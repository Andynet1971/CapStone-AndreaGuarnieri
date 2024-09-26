namespace CapStone_AndreaGuarnieri.Models.Interfaces
{
    public interface ICamera
    {
        // Ottiene tutte le camere disponibili
        IEnumerable<Camera> GetCamereDisponibili();

        // Recupera una camera per ID
        Camera GetCameraById(int cameraID);

        // Imposta la disponibilità di una camera
        void SetDisponibile(int cameraID, bool disponibile);
        IEnumerable<Camera> GetCamereByTipologia(string tipologia);

    }
}
