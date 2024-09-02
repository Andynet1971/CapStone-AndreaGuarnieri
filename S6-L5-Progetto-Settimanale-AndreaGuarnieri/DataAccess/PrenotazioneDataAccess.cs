using System.Data.SqlClient;
using CapStone_AndreaGuarnieri.Models;
using CapStone_AndreaGuarnieri.Models.Interfaces;

namespace CapStone_AndreaGuarnieri.DataAccess
{
    public class PrenotazioneDataAccess : IPrenotazione
    {
        private readonly string _connectionString;

        // Costruttore che inizializza la stringa di connessione
        public PrenotazioneDataAccess(string connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        // Metodo per ottenere una prenotazione in base all'ID
        public Prenotazione GetPrenotazione(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "SELECT p.*, c.Numero, c.Descrizione, c.Tipologia, c.TariffaGiornaliera, c.Disponibile " +
                    "FROM Prenotazioni p " +
                    "JOIN Camere c ON p.CameraID = c.Numero " +
                    "WHERE p.ID = @ID", connection);
                command.Parameters.AddWithValue("@ID", id);

                connection.Open();
                var reader = command.ExecuteReader();
                if (reader.Read())
                {
                    var prenotazione = new Prenotazione
                    {
                        ID = reader.GetInt32(reader.GetOrdinal("ID")),
                        ClienteID = reader.GetString(reader.GetOrdinal("ClienteID")),
                        CameraID = reader.GetInt32(reader.GetOrdinal("CameraID")),
                        DataPrenotazione = reader.GetDateTime(reader.GetOrdinal("DataPrenotazione")),
                        NumeroProgressivo = reader.GetInt32(reader.GetOrdinal("NumeroProgressivo")),
                        Anno = reader.GetInt32(reader.GetOrdinal("Anno")),
                        DataInizio = reader.GetDateTime(reader.GetOrdinal("DataInizio")),
                        DataFine = reader.GetDateTime(reader.GetOrdinal("DataFine")),
                        Caparra = reader.GetDecimal(reader.GetOrdinal("Caparra")),
                        TipoSoggiorno = reader.GetString(reader.GetOrdinal("TipoSoggiorno")),
                        Camera = new Camera
                        {
                            Numero = reader.GetInt32(reader.GetOrdinal("Numero")),
                            Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                            Tipologia = reader.GetString(reader.GetOrdinal("Tipologia")),
                            TariffaGiornaliera = reader.GetDecimal(reader.GetOrdinal("TariffaGiornaliera")),
                            Disponibile = reader.GetBoolean(reader.GetOrdinal("Disponibile"))
                        }
                    };
                    return prenotazione;
                }

                return null;
            }
        }

        // Metodo per aggiungere una nuova prenotazione
        public void AddPrenotazione(Prenotazione prenotazione)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    "INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno) " +
                    "VALUES (@ClienteID, @CameraID, @DataPrenotazione, @NumeroProgressivo, @Anno, @DataInizio, @DataFine, @Caparra, @TipoSoggiorno)", connection);

                command.Parameters.AddWithValue("@ClienteID", prenotazione.ClienteID);
                command.Parameters.AddWithValue("@CameraID", prenotazione.CameraID);
                command.Parameters.AddWithValue("@DataPrenotazione", prenotazione.DataPrenotazione);
                command.Parameters.AddWithValue("@NumeroProgressivo", prenotazione.NumeroProgressivo);
                command.Parameters.AddWithValue("@Anno", prenotazione.Anno);
                command.Parameters.AddWithValue("@DataInizio", prenotazione.DataInizio);
                command.Parameters.AddWithValue("@DataFine", prenotazione.DataFine);
                command.Parameters.AddWithValue("@Caparra", prenotazione.Caparra);
                command.Parameters.AddWithValue("@TipoSoggiorno", prenotazione.TipoSoggiorno);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }

        // Metodo per ottenere tutte le prenotazioni
        public IEnumerable<Prenotazione> GetAllPrenotazioni()
        {
            var prenotazioni = new List<Prenotazione>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Prenotazioni", connection);
                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var prenotazione = new Prenotazione
                    {
                        ID = reader.GetInt32(reader.GetOrdinal("ID")),
                        ClienteID = reader.GetString(reader.GetOrdinal("ClienteID")),
                        CameraID = reader.GetInt32(reader.GetOrdinal("CameraID")),
                        DataPrenotazione = reader.GetDateTime(reader.GetOrdinal("DataPrenotazione")),
                        NumeroProgressivo = reader.GetInt32(reader.GetOrdinal("NumeroProgressivo")),
                        Anno = reader.GetInt32(reader.GetOrdinal("Anno")),
                        DataInizio = reader.GetDateTime(reader.GetOrdinal("DataInizio")),
                        DataFine = reader.GetDateTime(reader.GetOrdinal("DataFine")),
                        Caparra = reader.GetDecimal(reader.GetOrdinal("Caparra")),
                        TipoSoggiorno = reader.GetString(reader.GetOrdinal("TipoSoggiorno"))
                    };
                    prenotazioni.Add(prenotazione);
                }
            }

            return prenotazioni;
        }

        // Metodo per ottenere le prenotazioni in base al codice fiscale
        public IEnumerable<Prenotazione> GetPrenotazioniByCodiceFiscale(string codiceFiscale)
        {
            var prenotazioni = new List<Prenotazione>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Prenotazioni WHERE ClienteID = @ClienteID", connection);
                command.Parameters.AddWithValue("@ClienteID", codiceFiscale);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    var prenotazione = new Prenotazione
                    {
                        ID = reader.GetInt32(reader.GetOrdinal("ID")),
                        ClienteID = reader.GetString(reader.GetOrdinal("ClienteID")),
                        CameraID = reader.GetInt32(reader.GetOrdinal("CameraID")),
                        DataPrenotazione = reader.GetDateTime(reader.GetOrdinal("DataPrenotazione")),
                        NumeroProgressivo = reader.GetInt32(reader.GetOrdinal("NumeroProgressivo")),
                        Anno = reader.GetInt32(reader.GetOrdinal("Anno")),
                        DataInizio = reader.GetDateTime(reader.GetOrdinal("DataInizio")),
                        DataFine = reader.GetDateTime(reader.GetOrdinal("DataFine")),
                        Caparra = reader.GetDecimal(reader.GetOrdinal("Caparra")),
                        TipoSoggiorno = reader.GetString(reader.GetOrdinal("TipoSoggiorno"))
                    };
                    prenotazioni.Add(prenotazione);
                }
            }

            return prenotazioni;
        }

        // Metodo per ottenere il conteggio dei diversi tipi di soggiorno
        public Dictionary<string, int> GetTipologiaSoggiornoCounts()
        {
            var counts = new Dictionary<string, int>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT TipoSoggiorno, COUNT(*) AS Count FROM Prenotazioni GROUP BY TipoSoggiorno", connection);

                connection.Open();
                var reader = command.ExecuteReader();
                while (reader.Read())
                {
                    counts.Add(reader.GetString(reader.GetOrdinal("TipoSoggiorno")), reader.GetInt32(reader.GetOrdinal("Count")));
                }
            }

            return counts;
        }

        // Metodo per ottenere il prossimo numero progressivo disponibile
        public int GetNextProgressiveNumber()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT ISNULL(MAX(NumeroProgressivo), 0) + 1 FROM Prenotazioni", connection);

                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }
    }
}
