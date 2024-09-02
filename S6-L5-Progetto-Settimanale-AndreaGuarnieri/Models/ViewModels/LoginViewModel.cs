namespace CapStone_AndreaGuarnieri.Models.ViewModels
{
    public class LoginViewModel
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public string ReturnUrl { get; set; }
        public bool RememberMe { get; set; } // Aggiungi questa proprietà
    }
}
