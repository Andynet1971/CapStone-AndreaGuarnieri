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
                        Salt = reader.GetString(reader.GetOrdinal("Salt")),
                        Nome = reader.GetString(reader.GetOrdinal("Nome")),
                        Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                        Ruolo = reader.GetString(reader.GetOrdinal("Ruolo"))
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
                    "INSERT INTO Utenti (Username, PasswordHash, Salt, Nome, Cognome, Ruolo) " +
                    "VALUES (@Username, @PasswordHash, @Salt, @Nome, @Cognome, @Ruolo)", connection);

                command.Parameters.AddWithValue("@Username", utente.Username);
                command.Parameters.AddWithValue("@PasswordHash", utente.PasswordHash);
                command.Parameters.AddWithValue("@Salt", utente.Salt);
                command.Parameters.AddWithValue("@Nome", utente.Nome);
                command.Parameters.AddWithValue("@Cognome", utente.Cognome);
                command.Parameters.AddWithValue("@Ruolo", utente.Ruolo);

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
                        Salt = reader.GetString(reader.GetOrdinal("Salt")),
                        Nome = reader.GetString(reader.GetOrdinal("Nome")),
                        Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                        Ruolo = reader.GetString(reader.GetOrdinal("Ruolo"))
                    };
                    utenti.Add(utente);
                }
            }

            return utenti;
        }
        public void UpdateUtente(Utente utente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "UPDATE Utenti SET Username = @Username, Nome = @Nome, Cognome = @Cognome, Ruolo = @Ruolo WHERE ID = @ID", connection);

                command.Parameters.AddWithValue("@Username", utente.Username);
                command.Parameters.AddWithValue("@Nome", utente.Nome);
                command.Parameters.AddWithValue("@Cognome", utente.Cognome);
                command.Parameters.AddWithValue("@Ruolo", utente.Ruolo);
                command.Parameters.AddWithValue("@ID", utente.ID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public void DeleteUtente(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("DELETE FROM Utenti WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public Utente GetUtenteById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Utenti WHERE ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);

                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    return new Utente
                    {
                        ID = reader.GetInt32(reader.GetOrdinal("ID")),
                        Username = reader.GetString(reader.GetOrdinal("Username")),
                        Nome = reader.GetString(reader.GetOrdinal("Nome")),
                        Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                        Ruolo = reader.GetString(reader.GetOrdinal("Ruolo")),
                        PasswordHash = reader.GetString(reader.GetOrdinal("PasswordHash")),
                        Salt = reader.GetString(reader.GetOrdinal("Salt"))
                    };
                }

                return null;
            }
        }
    }
}
