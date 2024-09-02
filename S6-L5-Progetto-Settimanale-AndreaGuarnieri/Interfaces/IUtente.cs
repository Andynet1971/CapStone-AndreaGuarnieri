
namespace CapStone_AndreaGuarnieri.Models.Interfaces
{
    public interface IUtente
    {
        Utente GetUtente(string username);
        void AddUtente(Utente utente);
    }
}
