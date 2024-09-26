using System.Data.SqlClient;
using CapStone_AndreaGuarnieri.Models.Interfaces;
using CapStone_AndreaGuarnieri.Models;

namespace CapStone_AndreaGuarnieri.DataAccess
{
    public class StoricoDataAccess : IStorico
    {
        private readonly string _connectionString;

        public StoricoDataAccess(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        public decimal GetOccupazioneMedia(DateTime dataInizio, DateTime dataFine)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT (COUNT(*) * 100.0 / (SELECT COUNT(*) FROM Camere)) AS PercentualeOccupazione
                      FROM Prenotazioni
                      WHERE DataInizio >= @DataInizio AND DataFine <= @DataFine";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DataInizio", dataInizio);
                command.Parameters.AddWithValue("@DataFine", dataFine);

                connection.Open();
                var result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }


        public double GetDurataMediaSoggiorni(DateTime dataInizio, DateTime dataFine)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT AVG(DATEDIFF(day, DataInizio, DataFine))
                              FROM Prenotazioni
                              WHERE DataInizio >= @DataInizio AND DataFine <= @DataFine";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DataInizio", dataInizio);
                command.Parameters.AddWithValue("@DataFine", dataFine);

                connection.Open();
                var result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDouble(result) : 0;
            }
        }

        public decimal GetIncassoTotale(DateTime dataInizio, DateTime dataFine)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT SUM(PrezzoTotale)
                      FROM Prenotazioni
                      WHERE DataInizio >= @DataInizio AND DataFine <= @DataFine";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DataInizio", dataInizio);
                command.Parameters.AddWithValue("@DataFine", dataFine);

                connection.Open();
                var result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }


        public decimal GetUtilizzoServiziAggiuntivi(DateTime dataInizio, DateTime dataFine)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT SUM(SA.Quantita)
                              FROM ServiziAggiuntivi SA
                              JOIN Prenotazioni P ON SA.PrenotazioneID = P.ID
                              WHERE P.DataInizio >= @DataInizio AND P.DataFine <= @DataFine";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DataInizio", dataInizio);
                command.Parameters.AddWithValue("@DataFine", dataFine);

                connection.Open();
                var result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        public decimal GetPercentualeClientiConServiziAggiuntivi(DateTime dataInizio, DateTime dataFine)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT CAST(COUNT(DISTINCT P.ID) AS DECIMAL(18, 2)) / 
                                     (SELECT COUNT(*) FROM Prenotazioni P WHERE P.DataInizio >= @DataInizio AND P.DataFine <= @DataFine)
                              FROM Prenotazioni P
                              JOIN ServiziAggiuntivi SA ON P.ID = SA.PrenotazioneID
                              WHERE P.DataInizio >= @DataInizio AND P.DataFine <= @DataFine";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DataInizio", dataInizio);
                command.Parameters.AddWithValue("@DataFine", dataFine);

                connection.Open();
                var result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) * 100 : 0;
            }
        }

        // Metodo per ottenere le prenotazioni in un range di date
        public IEnumerable<Prenotazione> GetPrenotazioniPerDateRange(DateTime dataInizio, DateTime dataFine)
        {
            var prenotazioni = new List<Prenotazione>();
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Prenotazioni WHERE DataInizio <= @DataFine AND DataFine >= @DataInizio", connection);
                command.Parameters.AddWithValue("@DataInizio", dataInizio);
                command.Parameters.AddWithValue("@DataFine", dataFine);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var prenotazione = new Prenotazione
                    {
                        ID = reader.GetInt32(reader.GetOrdinal("ID")),
                        ClienteID = reader.GetString(reader.GetOrdinal("ClienteID")),
                        CameraID = reader.GetInt32(reader.GetOrdinal("CameraID")),
                        DataInizio = reader.GetDateTime(reader.GetOrdinal("DataInizio")),
                        DataFine = reader.GetDateTime(reader.GetOrdinal("DataFine")),
                        PrezzoTotale = reader.GetDecimal(reader.GetOrdinal("PrezzoTotale")),
                    };
                    prenotazioni.Add(prenotazione);
                }
            }
            return prenotazioni;
        }

        // Metodo per ottenere la somma dei servizi aggiuntivi in un range di settimane
        public decimal OttieniSommaServiziPerSettimana(int numeroSettimana)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(@"SELECT SUM(SA.Quantita) 
                                          FROM ServiziAggiuntivi SA
                                          JOIN Prenotazioni P ON SA.PrenotazioneID = P.ID
                                          WHERE DATEPART(WEEK, P.DataPrenotazione) = @NumeroSettimana", connection);
                command.Parameters.AddWithValue("@NumeroSettimana", numeroSettimana);

                connection.Open();
                var result = command.ExecuteScalar();
                return result != DBNull.Value ? Convert.ToDecimal(result) : 0;
            }
        }

        // Metodo per ottenere gli incassi totali per un range di date
        public decimal GetIncassoPerData(DateTime data)
        {
            decimal incasso = 0;
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT SUM(PrezzoTotale) FROM Prenotazioni WHERE @Data BETWEEN DataInizio AND DataFine", connection);
                command.Parameters.AddWithValue("@Data", data);

                connection.Open();
                var result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    incasso = Convert.ToDecimal(result);
                }
            }
            return incasso;
        }
        // Metodo per ottenere l'incasso totale per un intervallo di date
        public decimal GetIncassoTotalePerDateRange(DateTime dataInizio, DateTime dataFine)
        {
            decimal incassoTotale = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT SUM(PrezzoTotale) 
                              FROM Prenotazioni 
                              WHERE DataInizio >= @DataInizio AND DataFine <= @DataFine";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DataInizio", dataInizio);
                command.Parameters.AddWithValue("@DataFine", dataFine);

                connection.Open();
                var result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    incassoTotale = Convert.ToDecimal(result);
                }
            }

            return incassoTotale;
        }

        // Metodo per ottenere l'utilizzo dei servizi aggiuntivi per un intervallo di date
        public decimal GetUtilizzoServiziAggiuntiviPerDateRange(DateTime dataInizio, DateTime dataFine)
        {
            decimal totaleServiziAggiuntivi = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT SUM(Quantita) 
                      FROM ServiziAggiuntivi SA
                      JOIN Prenotazioni P ON SA.PrenotazioneID = P.ID
                      WHERE P.DataInizio >= @DataInizio AND P.DataFine <= @DataFine";

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@DataInizio", dataInizio);
                command.Parameters.AddWithValue("@DataFine", dataFine);

                connection.Open();
                var result = command.ExecuteScalar();
                if (result != DBNull.Value)
                {
                    totaleServiziAggiuntivi = Convert.ToDecimal(result);
                }
            }

            return totaleServiziAggiuntivi;
        }


        // Metodo per ottenere il numero totale di stanze
        public int GetNumeroStanzeTotali()
        {
            int numeroStanzeTotali = 0;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT COUNT(*) FROM Camere";
                var command = new SqlCommand(query, connection);

                connection.Open();
                numeroStanzeTotali = (int)command.ExecuteScalar();
            }

            return numeroStanzeTotali;
        }
        public IEnumerable<Prenotazione> GetPrenotazioniPerData(DateTime data)
        {
            var prenotazioni = new List<Prenotazione>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Prenotazioni WHERE @Data BETWEEN DataInizio AND DataFine", connection);
                command.Parameters.AddWithValue("@Data", data);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var prenotazione = new Prenotazione
                    {
                        ID = reader.GetInt32(reader.GetOrdinal("ID")),
                        ClienteID = reader.GetString(reader.GetOrdinal("ClienteID")),
                        CameraID = reader.GetInt32(reader.GetOrdinal("CameraID")),
                        DataInizio = reader.GetDateTime(reader.GetOrdinal("DataInizio")),
                        DataFine = reader.GetDateTime(reader.GetOrdinal("DataFine")),
                        PrezzoTotale = reader.GetDecimal(reader.GetOrdinal("PrezzoTotale"))
                    };
                    prenotazioni.Add(prenotazione);
                }
            }

            return prenotazioni;
        }

    }
}
