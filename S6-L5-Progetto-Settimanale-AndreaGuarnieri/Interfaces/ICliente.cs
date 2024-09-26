
namespace CapStone_AndreaGuarnieri.Models.Interfaces
{
    public interface ICliente
    {
        IEnumerable<Cliente> GetAllClienti();
        Cliente GetCliente(string codiceFiscale);
        void AddCliente(Cliente cliente);
        Cliente GetClienteByCodiceFiscaleOrCognome(string query);
        void UpdateCliente(Cliente cliente);
    }
}
