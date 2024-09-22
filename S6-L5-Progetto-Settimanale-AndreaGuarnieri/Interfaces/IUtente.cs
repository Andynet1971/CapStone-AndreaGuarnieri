using CapStone_AndreaGuarnieri.Models;

public interface IUtente
{
    Utente GetUtente(string username);
    Utente GetUtenteById(int id); // Aggiungi questo metodo
    void AddUtente(Utente utente);
    IEnumerable<Utente> GetAllUtenti();
    void UpdateUtente(Utente utente);
    void DeleteUtente(int id);
}
