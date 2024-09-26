using System.Data.SqlClient;
using CapStone_AndreaGuarnieri.Models;
using CapStone_AndreaGuarnieri.Models.Interfaces;

namespace CapStone_AndreaGuarnieri.DataAccess
{
    public class CameraDataAccess : ICamera
    {
        private readonly string _connectionString;

        // Costruttore che inizializza la stringa di connessione
        public CameraDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        // Metodo per ottenere tutte le camere disponibili
        public IEnumerable<Camera> GetCamereDisponibili()
        {
            var camereDisponibili = new List<Camera>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Camere WHERE Disponibile = 1";
                var command = new SqlCommand(query, connection);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        camereDisponibili.Add(new Camera
                        {
                            Numero = reader.GetInt32(reader.GetOrdinal("Numero")),
                            Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                            Tipologia = reader.GetString(reader.GetOrdinal("Tipologia")),
                            TariffaGiornaliera = reader.GetDecimal(reader.GetOrdinal("TariffaGiornaliera")),
                            Disponibile = reader.GetBoolean(reader.GetOrdinal("Disponibile"))
                        });
                    }
                }
            }

            return camereDisponibili;
        }

        // Metodo per ottenere una camera in base al numero (ID)
        public Camera GetCamera(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Camere WHERE Numero = @Numero", connection);
                command.Parameters.AddWithValue("@Numero", id);

                connection.Open();
                var reader = command.ExecuteReader();

                if (reader.Read())
                {
                    return new Camera
                    {
                        Numero = reader.GetInt32(reader.GetOrdinal("Numero")),
                        Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                        Tipologia = reader.GetString(reader.GetOrdinal("Tipologia")),
                        TariffaGiornaliera = reader.GetDecimal(reader.GetOrdinal("TariffaGiornaliera")),
                        Disponibile = reader.GetBoolean(reader.GetOrdinal("Disponibile"))
                    };
                }
            }

            return null;
        }
        public Camera GetCameraById(int cameraID)
        {
            Camera camera = null;

            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "SELECT * FROM Camere WHERE Numero = @CameraID";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@CameraID", cameraID);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        camera = new Camera
                        {
                            Numero = reader.GetInt32(reader.GetOrdinal("Numero")),
                            Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                            Tipologia = reader.GetString(reader.GetOrdinal("Tipologia")),
                            TariffaGiornaliera = reader.GetDecimal(reader.GetOrdinal("TariffaGiornaliera")),
                            Disponibile = reader.GetBoolean(reader.GetOrdinal("Disponibile"))
                        };
                    }
                }
            }

            return camera;
        }

        public void SetDisponibile(int cameraID, bool disponibile)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                string query = "UPDATE Camere SET Disponibile = @Disponibile WHERE Numero = @CameraID";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Disponibile", disponibile);
                command.Parameters.AddWithValue("@CameraID", cameraID);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
        public IEnumerable<Camera> GetCamereByTipologia(string tipologia)
        {
            var camereByTipologia = new List<Camera>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var query = "SELECT * FROM Camere WHERE Tipologia = @Tipologia";
                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Tipologia", tipologia);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        camereByTipologia.Add(new Camera
                        {
                            Numero = reader.GetInt32(reader.GetOrdinal("Numero")),
                            Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                            Tipologia = reader.GetString(reader.GetOrdinal("Tipologia")),
                            TariffaGiornaliera = reader.GetDecimal(reader.GetOrdinal("TariffaGiornaliera")),
                            Disponibile = reader.GetBoolean(reader.GetOrdinal("Disponibile"))
                        });
                    }
                }
            }

            return camereByTipologia;
        }

    }
}
