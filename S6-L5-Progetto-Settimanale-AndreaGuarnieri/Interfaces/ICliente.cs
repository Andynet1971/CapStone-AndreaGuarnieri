using CapStone_AndreaGuarnieri.Models;
using System.Collections.Generic;

namespace CapStone_AndreaGuarnieri.Models.Interfaces
{
    public interface ICliente
    {
        IEnumerable<Cliente> GetAllClienti();
        Cliente GetCliente(string codiceFiscale);
        void AddCliente(Cliente cliente);
    }
}
