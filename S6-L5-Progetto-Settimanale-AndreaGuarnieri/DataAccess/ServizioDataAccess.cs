using System.Data.SqlClient;
using CapStone_AndreaGuarnieri.Models;
using CapStone_AndreaGuarnieri.Models.Interfaces;

namespace CapStone_AndreaGuarnieri.DataAccess
{
    public class ServizioDataAccess : IServizio
    {
        private readonly string _connectionString;

        // Costruttore che accetta una stringa di connessione e verifica che non sia nulla
        public ServizioDataAccess(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // Metodo per ottenere tutti i servizi dal database
        public IEnumerable<Servizio> GetAllServizi()
        {
            var servizi = new List<Servizio>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Servizi", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var servizio = new Servizio
                    {
                        ID = reader.GetInt32(reader.GetOrdinal("ID")),
                        Nome = reader.GetString(reader.GetOrdinal("Nome")),
                        Tariffa = reader.GetDecimal(reader.GetOrdinal("Tariffa"))
                    };
                    servizi.Add(servizio);
                }
            }

            return servizi;
        }
        public Servizio GetServizioById(int id)
        {
            Servizio servizio = null;

            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT ID, Nome, Tariffa FROM Servizi WHERE ID = @ID";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@ID", id);

                connection.Open();

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        servizio = new Servizio
                        {
                            ID = reader.GetInt32(0),
                            Nome = reader.GetString(1),
                            Tariffa = reader.GetDecimal(2)
                        };
                    }
                }
            }

            return servizio;
        }
    }
}
