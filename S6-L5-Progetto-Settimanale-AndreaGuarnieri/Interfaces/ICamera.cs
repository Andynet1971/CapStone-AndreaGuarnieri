using CapStone_AndreaGuarnieri.Models;
using System.Collections.Generic;

namespace CapStone_AndreaGuarnieri.Models.Interfaces
{
    public interface ICamera
    {
        IEnumerable<Camera> GetCamereDisponibili();
        Camera GetCamera(int id);
        // Add other methods if needed...
    }
}
