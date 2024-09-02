using System.Data.SqlClient;
using CapStone_AndreaGuarnieri.Models;
using CapStone_AndreaGuarnieri.Models.Interfaces;

namespace CapStone_AndreaGuarnieri.DataAccess
{
    public class ServizioAggiuntivoDataAccess : IServizioAggiuntivo
    {
        private readonly string _connectionString;

        // Costruttore che accetta una stringa di connessione e verifica che non sia nulla
        public ServizioAggiuntivoDataAccess(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // Metodo per aggiungere un nuovo servizio aggiuntivo nel database
        public void AddServizioAggiuntivo(ServizioAggiuntivo servizioAggiuntivo)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "INSERT INTO ServiziAggiuntivi (PrenotazioneID, ServizioID, Data, Quantita) " +
                    "VALUES (@PrenotazioneID, @ServizioID, @Data, @Quantita)", connection);

                command.Parameters.AddWithValue("@PrenotazioneID", servizioAggiuntivo.PrenotazioneID);
                command.Parameters.AddWithValue("@ServizioID", servizioAggiuntivo.ServizioID);
                command.Parameters.AddWithValue("@Data", servizioAggiuntivo.Data);
                command.Parameters.AddWithValue("@Quantita", servizioAggiuntivo.Quantita);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Metodo per ottenere i servizi aggiuntivi associati a una prenotazione specifica
        public IEnumerable<ServizioAggiuntivo> GetServiziAggiuntiviByPrenotazioneId(int prenotazioneID)
        {
            var serviziAggiuntivi = new List<ServizioAggiuntivo>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "SELECT sa.*, s.Nome, s.Tariffa " +
                    "FROM ServiziAggiuntivi sa " +
                    "JOIN Servizi s ON sa.ServizioID = s.ID " +
                    "WHERE sa.PrenotazioneID = @PrenotazioneID", connection);

                command.Parameters.AddWithValue("@PrenotazioneID", prenotazioneID);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var servizioAggiuntivo = new ServizioAggiuntivo
                    {
                        ID = reader.GetInt32(reader.GetOrdinal("ID")),
                        PrenotazioneID = reader.GetInt32(reader.GetOrdinal("PrenotazioneID")),
                        ServizioID = reader.GetInt32(reader.GetOrdinal("ServizioID")),
                        Data = reader.GetDateTime(reader.GetOrdinal("Data")),
                        Quantita = reader.GetInt32(reader.GetOrdinal("Quantita")),
                        Servizio = new Servizio
                        {
                            ID = reader.GetInt32(reader.GetOrdinal("ServizioID")),
                            Nome = reader.GetString(reader.GetOrdinal("Nome")),
                            Tariffa = reader.GetDecimal(reader.GetOrdinal("Tariffa"))
                        }
                    };
                    serviziAggiuntivi.Add(servizioAggiuntivo);
                }
            }

            return serviziAggiuntivi;
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
    }
}
