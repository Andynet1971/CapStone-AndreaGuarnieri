namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class UtenteViewModel
    {
        public int ID { get; set; }
        public string Username { get; set; }
        public string Nome { get; set; }
        public string Cognome { get; set; }
        public string Ruolo { get; set; }

        // Aggiungi la proprietà per la password in chiaro (solo nel ViewModel)
        public string Password { get; set; }
    }

}
