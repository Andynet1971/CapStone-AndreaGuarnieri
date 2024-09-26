using System.Data.SqlClient;
using CapStone_AndreaGuarnieri.Models;
using CapStone_AndreaGuarnieri.Models.Interfaces;
using CapStone_AndreaGuarnieri.Models.ViewModels;

namespace CapStone_AndreaGuarnieri.DataAccess
{
    public class PrenotazioneDataAccess : IPrenotazione
    {
        private readonly string _connectionString;
        private readonly ILogger<PrenotazioneDataAccess> _logger;

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

        public void UpdatePrenotazione(Prenotazione prenotazione)
        {
            var query = @"UPDATE Prenotazioni 
                  SET DataPrenotazione = @DataPrenotazione, 
                      NumeroProgressivo = @NumeroProgressivo, 
                      Anno = @Anno, 
                      DataInizio = @DataInizio, 
                      DataFine = @DataFine, 
                      Caparra = @Caparra, 
                      TipoSoggiorno = @TipoSoggiorno, 
                      PrezzoTotale = @PrezzoTotale, 
                      Confermata = @Confermata
                  WHERE ID = @ID";

            using (var connection = new SqlConnection(_connectionString))
            {
                using (var command = new SqlCommand(query, connection))
                {
                    // Aggiungi tutti i parametri senza CameraID
                    command.Parameters.AddWithValue("@ID", prenotazione.ID);
                    command.Parameters.AddWithValue("@DataPrenotazione", prenotazione.DataPrenotazione != DateTime.MinValue ? prenotazione.DataPrenotazione : DateTime.Now);
                    command.Parameters.AddWithValue("@NumeroProgressivo", prenotazione.NumeroProgressivo);
                    command.Parameters.AddWithValue("@Anno", prenotazione.Anno);
                    command.Parameters.AddWithValue("@DataInizio", prenotazione.DataInizio != DateTime.MinValue ? prenotazione.DataInizio : DateTime.Now);
                    command.Parameters.AddWithValue("@DataFine", prenotazione.DataFine != DateTime.MinValue ? prenotazione.DataFine : DateTime.Now);
                    command.Parameters.AddWithValue("@Caparra", prenotazione.Caparra);
                    command.Parameters.AddWithValue("@TipoSoggiorno", prenotazione.TipoSoggiorno);
                    command.Parameters.AddWithValue("@PrezzoTotale", prenotazione.PrezzoTotale);
                    command.Parameters.AddWithValue("@Confermata", prenotazione.Confermata);

                    // Esegui il comando di aggiornamento
                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
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
        public IEnumerable<PrenotazioneViewModel> GetAllPrenotazioni()
        {
            var prenotazioni = new List<PrenotazioneViewModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            SELECT p.ID, p.ClienteID, c.Cognome, c.Nome, p.CameraID, p.DataPrenotazione, 
                   p.DataInizio, p.DataFine, p.Caparra, p.TipoSoggiorno, p.PrezzoTotale, p.Confermata
            FROM Prenotazioni p
            INNER JOIN Clienti c ON p.ClienteID = c.CodiceFiscale";

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            prenotazioni.Add(new PrenotazioneViewModel
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                ClienteID = reader.GetString(reader.GetOrdinal("ClienteID")),
                                Cognome = reader.GetString(reader.GetOrdinal("Cognome")),  // Recupera il cognome
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),        // Recupera il nome
                                CameraID = reader.GetInt32(reader.GetOrdinal("CameraID")),
                                DataPrenotazione = reader.GetDateTime(reader.GetOrdinal("DataPrenotazione")),
                                DataInizio = reader.GetDateTime(reader.GetOrdinal("DataInizio")),
                                DataFine = reader.GetDateTime(reader.GetOrdinal("DataFine")),
                                Caparra = reader.GetDecimal(reader.GetOrdinal("Caparra")),
                                TipoSoggiorno = reader.GetString(reader.GetOrdinal("TipoSoggiorno")),
                                PrezzoTotale = reader.GetDecimal(reader.GetOrdinal("PrezzoTotale")),
                                Confermata = reader.GetBoolean(reader.GetOrdinal("Confermata"))
                            });
                        }
                    }
                }
            }

            return prenotazioni;
        }


        // Implementazione del metodo per ottenere le prenotazioni tramite Codice Fiscale
        public IEnumerable<Prenotazione> GetPrenotazioniByClienteId(string codiceFiscale)
        {
            var prenotazioni = new List<Prenotazione>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string sql = "SELECT * FROM Prenotazioni WHERE ClienteID = @codiceFiscale";
                var command = new SqlCommand(sql, connection);
                command.Parameters.AddWithValue("@codiceFiscale", codiceFiscale);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        prenotazioni.Add(new Prenotazione
                        {
                            DataInizio = reader.GetDateTime(reader.GetOrdinal("DataInizio")),
                            DataFine = reader.GetDateTime(reader.GetOrdinal("DataFine")),
                            // altre proprietà
                        });
                    }
                }
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
        // Metodo per ottenere la somma dei servizi aggiuntivi per una settimana specifica
        public decimal OttieniSommaServiziPerSettimana(int numeroSettimana)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand(
                    @"SELECT SUM(SA.Quantita) 
              FROM ServiziAggiuntivi SA
              JOIN Prenotazioni P ON SA.PrenotazioneID = P.ID
              WHERE DATEPART(WEEK, P.DataPrenotazione) = @NumeroSettimana", connection);

                command.Parameters.AddWithValue("@NumeroSettimana", numeroSettimana);

                connection.Open();
                var result = command.ExecuteScalar();
                var totaleServizi = result != DBNull.Value ? Convert.ToDecimal(result) : 0;

                // Log del risultato per verificare
                Console.WriteLine($"Settimana: {numeroSettimana}, Totale Servizi: {totaleServizi}");

                return totaleServizi;
            }
        }
        public IEnumerable<PrenotazioneViewModel> GetPrenotazioniConClienti()
        {
            var prenotazioni = new List<PrenotazioneViewModel>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"SELECT p.ID, p.ClienteID, c.Cognome, c.Nome, p.CameraID, p.DataInizio, p.DataFine, p.Caparra, p.TipoSoggiorno
                      FROM Prenotazioni p
                      INNER JOIN Clienti c ON p.ClienteID = c.CodiceFiscale";

                using (var command = new SqlCommand(query, connection))
                {
                    connection.Open();
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var prenotazione = new PrenotazioneViewModel
                            {
                                ID = reader.GetInt32(reader.GetOrdinal("ID")),
                                ClienteID = reader.GetString(reader.GetOrdinal("ClienteID")),
                                Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                                Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                CameraID = reader.GetInt32(reader.GetOrdinal("CameraID")),
                                DataInizio = reader.GetDateTime(reader.GetOrdinal("DataInizio")),
                                DataFine = reader.GetDateTime(reader.GetOrdinal("DataFine")),
                                Caparra = reader.GetDecimal(reader.GetOrdinal("Caparra")),
                                TipoSoggiorno = reader.GetString(reader.GetOrdinal("TipoSoggiorno"))
                            };
                            prenotazioni.Add(prenotazione);
                        }
                    }
                }
            }

            return prenotazioni;
        }
        public Prenotazione GetPrenotazioneById(int id)
        {
            Prenotazione prenotazione = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = @"
            SELECT p.ID, p.ClienteID, p.CameraID, p.DataPrenotazione, p.NumeroProgressivo, 
                   p.Anno, p.DataInizio, p.DataFine, p.Caparra, p.TipoSoggiorno, p.PrezzoTotale, p.Confermata,
                   c.Cognome, c.Nome, c.Citta, c.Provincia, c.Email, c.Telefono, c.Cellulare
            FROM Prenotazioni p
            INNER JOIN Clienti c ON p.ClienteID = c.CodiceFiscale
            WHERE p.ID = @ID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    connection.Open();

                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            prenotazione = new Prenotazione
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
                                PrezzoTotale = reader.GetDecimal(reader.GetOrdinal("PrezzoTotale")),
                                Confermata = reader.GetBoolean(reader.GetOrdinal("Confermata")),
                                Cliente = new Cliente
                                {
                                    Cognome = reader.GetString(reader.GetOrdinal("Cognome")),
                                    Nome = reader.GetString(reader.GetOrdinal("Nome")),
                                    Citta = reader.GetString(reader.GetOrdinal("Citta")),
                                    Provincia = reader.GetString(reader.GetOrdinal("Provincia")),
                                    Email = reader.GetString(reader.GetOrdinal("Email")),
                                    Telefono = reader.GetString(reader.GetOrdinal("Telefono")),
                                    Cellulare = reader.GetString(reader.GetOrdinal("Cellulare"))
                                }
                            };
                        }
                    }
                }
            }

            return prenotazione;
        }
        // Ottieni tutte le prenotazioni non confermate
        public IEnumerable<Prenotazione> GetPrenotazioniNonConfermate()
        {
            var prenotazioni = new List<Prenotazione>();

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Prenotazioni WHERE Confermata = 0";
                var command = new SqlCommand(query, connection);

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
                            NumeroProgressivo = reader.GetInt32(reader.GetOrdinal("NumeroProgressivo")),
                            DataInizio = reader.GetDateTime(reader.GetOrdinal("DataInizio")),
                            DataFine = reader.GetDateTime(reader.GetOrdinal("DataFine")),
                            Confermata = reader.GetBoolean(reader.GetOrdinal("Confermata"))
                        });
                    }
                }
            }
            return prenotazioni;
        }

        // Ottieni la prenotazione non confermata in base al CameraID
        public Prenotazione GetPrenotazioneNonConfermataByCameraId(int cameraID)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Prenotazioni WHERE Confermata = 0 AND CameraID = @CameraID";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CameraID", cameraID);

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
                            NumeroProgressivo = reader.GetInt32(reader.GetOrdinal("NumeroProgressivo")),
                            DataInizio = reader.GetDateTime(reader.GetOrdinal("DataInizio")),
                            DataFine = reader.GetDateTime(reader.GetOrdinal("DataFine")),
                            Confermata = reader.GetBoolean(reader.GetOrdinal("Confermata"))
                        };
                    }
                }
            }
            return null;
        }
    }
}
