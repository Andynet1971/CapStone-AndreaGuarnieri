using System.Collections.Generic;

namespace CapStone_AndreaGuarnieri.Models.Interfaces
{
    public interface IServizio
    {
        IEnumerable<Servizio> GetAllServizi();
    }
}
