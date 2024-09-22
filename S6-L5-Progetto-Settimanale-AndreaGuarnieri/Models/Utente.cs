namespace CapStone_AndreaGuarnieri.Models
{
    public class Utente
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string PasswordHash { get; set; }
        public string Salt { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Ruolo { get; set; }
    }
}
