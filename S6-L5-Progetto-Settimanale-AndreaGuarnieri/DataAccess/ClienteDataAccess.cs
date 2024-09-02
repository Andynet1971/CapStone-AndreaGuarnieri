using System.Data.SqlClient;
using CapStone_AndreaGuarnieri.Models;
using CapStone_AndreaGuarnieri.Models.Interfaces;

namespace CapStone_AndreaGuarnieri.DataAccess
{
    public class ClienteDataAccess : ICliente
    {
        private readonly string _connectionString;

        // Costruttore che inizializza la stringa di connessione
        public ClienteDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Metodo per ottenere tutti i clienti
        public IEnumerable<Cliente> GetAllClienti()
        {
            var clienti = new List<Cliente>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Clienti", connection);
                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    clienti.Add(new Cliente
                    {
                        CodiceFiscale = reader.GetString(0),
                        Cognome = reader.GetString(1),
                        Nome = reader.GetString(2),
                        Citta = reader.GetString(3),
                        Provincia = reader.GetString(4),
                        Email = reader.GetString(5),
                        Telefono = reader.GetString(6),
                        Cellulare = reader.GetString(7)
                    });
                }
            }

            return clienti;
        }

        // Metodo per ottenere un cliente in base al codice fiscale
        public Cliente GetCliente(string codiceFiscale)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Clienti WHERE CodiceFiscale = @CodiceFiscale", connection);
                command.Parameters.AddWithValue("@CodiceFiscale", codiceFiscale);

                connection.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Cliente
                    {
                        CodiceFiscale = reader.GetString(0),
                        Cognome = reader.GetString(1),
                        Nome = reader.GetString(2),
                        Citta = reader.GetString(3),
                        Provincia = reader.GetString(4),
                        Email = reader.GetString(5),
                        Telefono = reader.GetString(6),
                        Cellulare = reader.GetString(7)
                    };
                }

                return null;
            }
        }

        // Metodo per aggiungere un nuovo cliente
        public void AddCliente(Cliente cliente)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "INSERT INTO Clienti (CodiceFiscale, Cognome, Nome, Citta, Provincia, Email, Telefono, Cellulare) " +
                    "VALUES (@CodiceFiscale, @Cognome, @Nome, @Citta, @Provincia, @Email, @Telefono, @Cellulare)", connection);

                command.Parameters.AddWithValue("@CodiceFiscale", cliente.CodiceFiscale);
                command.Parameters.AddWithValue("@Cognome", cliente.Cognome);
                command.Parameters.AddWithValue("@Nome", cliente.Nome);
                command.Parameters.AddWithValue("@Citta", cliente.Citta);
                command.Parameters.AddWithValue("@Provincia", cliente.Provincia);
                command.Parameters.AddWithValue("@Email", cliente.Email);
                command.Parameters.AddWithValue("@Telefono", cliente.Telefono);
                command.Parameters.AddWithValue("@Cellulare", cliente.Cellulare);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
