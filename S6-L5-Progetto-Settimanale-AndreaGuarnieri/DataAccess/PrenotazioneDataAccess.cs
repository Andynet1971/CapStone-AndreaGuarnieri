using System;
using System.Collections.Generic;
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

        // Metodo per ottenere le prenotazioni per una data specifica
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
                        DataPrenotazione = reader.GetDateTime(reader.GetOrdinal("DataPrenotazione")),
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

        // Metodo per ottenere il numero totale di stanze dell'hotel
        public int GetNumeroStanzeTotali()
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT COUNT(*) FROM Camere", connection);

                connection.Open();
                return (int)command.ExecuteScalar();
            }
        }

        // Metodo per ottenere le tariffe per un determinato periodo e camera
        public List<Tariffa> GetTariffePerPeriodo(DateTime dataInizio, DateTime dataFine, int cameraId)
        {
            var tariffe = new List<Tariffa>();

            using (SqlConnection conn = new SqlConnection(_connectionString))
            {
                conn.Open();

                // Prima, ottieni la tipologia della camera dalla tabella 'Camere' usando il cameraId
                string cameraQuery = "SELECT Tipologia FROM Camere WHERE Numero = @CameraID";
                string tipoCamera = null;

                using (SqlCommand cmdCamera = new SqlCommand(cameraQuery, conn))
                {
                    cmdCamera.Parameters.AddWithValue("@CameraID", cameraId);
                    tipoCamera = cmdCamera.ExecuteScalar()?.ToString();
                }

                // Se è stata trovata la tipologia della camera, cerchiamo le tariffe corrispondenti
                if (!string.IsNullOrEmpty(tipoCamera))
                {
                    string query = @"
                SELECT ID, TipoStagione, TariffaGiornaliera, DataInizio, DataFine, TipoCamera
                FROM Tariffe
                WHERE DataInizio <= @DataFine 
                AND DataFine >= @DataInizio 
                AND TipoCamera = @TipoCamera";

                    using (SqlCommand cmd = new SqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("@DataInizio", dataInizio);
                        cmd.Parameters.AddWithValue("@DataFine", dataFine);
                        cmd.Parameters.AddWithValue("@TipoCamera", tipoCamera);

                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                var tariffa = new Tariffa
                                {
                                    ID = reader.GetInt32(0),
                                    TipoStagione = reader.GetString(1),
                                    TariffaGiornaliera = reader.GetDecimal(2),
                                    DataInizio = reader.GetDateTime(3),
                                    DataFine = reader.GetDateTime(4),
                                    TipoCamera = reader.GetString(5)
                                };

                                tariffe.Add(tariffa);
                            }
                        }
                    }
                }
            }

            return tariffe;
        }


        // Aggiungi qui il resto dei tuoi metodi come GetPrenotazione, AddPrenotazione, ecc.

        // Metodo per ottenere una prenotazione in base all'ID
        public Prenotazione GetPrenotazione(int id)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(
                    @"SELECT p.*, c.Numero, c.Descrizione, c.Tipologia, c.TariffaGiornaliera, c.Disponibile 
                      FROM Prenotazioni p 
                      JOIN Camere c ON p.CameraID = c.Numero 
                      WHERE p.ID = @ID", connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new Prenotazione
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
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log dell'eccezione (da implementare)
                throw new Exception("Errore durante il recupero della prenotazione", ex);
            }

            return null;
        }

        // Metodo per aggiungere una nuova prenotazione
        public void AddPrenotazione(Prenotazione prenotazione)
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand(
                    @"INSERT INTO Prenotazioni (ClienteID, CameraID, DataPrenotazione, NumeroProgressivo, Anno, DataInizio, DataFine, Caparra, TipoSoggiorno) 
                      VALUES (@ClienteID, @CameraID, @DataPrenotazione, @NumeroProgressivo, @Anno, @DataInizio, @DataFine, @Caparra, @TipoSoggiorno)", connection))
                {
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
            catch (Exception ex)
            {
                // Log dell'eccezione (da implementare)
                throw new Exception("Errore durante l'aggiunta della prenotazione", ex);
            }
        }

        // Metodo per ottenere tutte le prenotazioni
        public IEnumerable<Prenotazione> GetAllPrenotazioni()
        {
            var prenotazioni = new List<Prenotazione>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("SELECT * FROM Prenotazioni", connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prenotazioni.Add(new Prenotazione
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
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log dell'eccezione (da implementare)
                throw new Exception("Errore durante il recupero di tutte le prenotazioni", ex);
            }

            return prenotazioni;
        }

        // Metodo per ottenere le prenotazioni in base al codice fiscale
        public IEnumerable<Prenotazione> GetPrenotazioniByCodiceFiscale(string codiceFiscale)
        {
            var prenotazioni = new List<Prenotazione>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("SELECT * FROM Prenotazioni WHERE ClienteID = @ClienteID", connection))
                {
                    command.Parameters.AddWithValue("@ClienteID", codiceFiscale);

                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prenotazioni.Add(new Prenotazione
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
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log dell'eccezione (da implementare)
                throw new Exception("Errore durante il recupero delle prenotazioni per codice fiscale", ex);
            }

            return prenotazioni;
        }

        // Metodo per ottenere il conteggio dei diversi tipi di soggiorno
        public Dictionary<string, int> GetTipologiaSoggiornoCounts()
        {
            var counts = new Dictionary<string, int>();

            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("SELECT TipoSoggiorno, COUNT(*) AS Count FROM Prenotazioni GROUP BY TipoSoggiorno", connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            counts.Add(reader.GetString(reader.GetOrdinal("TipoSoggiorno")), reader.GetInt32(reader.GetOrdinal("Count")));
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // Log dell'eccezione (da implementare)
                throw new Exception("Errore durante il recupero del conteggio delle tipologie di soggiorno", ex);
            }

            return counts;
        }

        // Metodo per ottenere il prossimo numero progressivo disponibile
        public int GetNextProgressiveNumber()
        {
            try
            {
                using (var connection = new SqlConnection(_connectionString))
                using (var command = new SqlCommand("SELECT ISNULL(MAX(NumeroProgressivo), 0) + 1 FROM Prenotazioni", connection))
                {
                    connection.Open();
                    return (int)command.ExecuteScalar();
                }
            }
            catch (Exception ex)
            {
                // Log dell'eccezione (da implementare)
                throw new Exception("Errore durante il recupero del prossimo numero progressivo", ex);
            }
        }
        // Metodo per ottenere gli incassi per una data specifica
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

    }
}
