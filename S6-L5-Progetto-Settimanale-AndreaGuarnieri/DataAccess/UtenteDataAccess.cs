using System.Data.SqlClient;
using CapStone_AndreaGuarnieri.Models;
using CapStone_AndreaGuarnieri.Models.Interfaces;

namespace CapStone_AndreaGuarnieri.DataAccess
{
    public class UtenteDataAccess : IUtente
    {
        private readonly string _connectionString;

        // Costruttore che accetta una stringa di connessione e verifica che non sia nulla
        public UtenteDataAccess(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // Metodo per ottenere un utente dal database in base al nome utente
        public Utente GetUtente(string username)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Utenti WHERE Username = @Username", connection);
                command.Parameters.AddWithValue("@Username", username);

                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Utente
                    {
                        ID = reader.GetInt32(reader.GetOrdinal("ID")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = reader.GetString(reader.GetOrdinal("Salt"))
                    };
                }

                return null;
            }
        }

        // Metodo per aggiungere un nuovo utente al database
        public void AddUtente(Utente utente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "INSERT INTO Utenti (Username, PasswordHash, Salt) " +
                    "VALUES (@Username, @PasswordHash, @Salt)", connection);

                command.Parameters.AddWithValue("@Username", utente.Username);
                command.Parameters.AddWithValue("@PasswordHash", utente.PasswordHash);
                command.Parameters.AddWithValue("@Salt", utente.Salt);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Metodo per ottenere tutti gli utenti dal database
        public IEnumerable<Utente> GetAllUtenti()
        {
            var utenti = new List<Utente>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Utenti", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var utente = new Utente
                    {
                        ID = reader.GetInt32(reader.GetOrdinal("ID")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = reader.GetString(reader.GetOrdinal("Salt"))
                    };
                    utenti.Add(utente);
                }
            }

            return utenti;
        }
    }
}
