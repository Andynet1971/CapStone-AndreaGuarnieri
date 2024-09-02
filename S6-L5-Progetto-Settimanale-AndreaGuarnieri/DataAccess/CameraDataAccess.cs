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
            var camere = new List<Camera>();

            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand("SELECT * FROM Camere WHERE Disponibile = 1", connection);
                connection.Open();
                var reader = command.ExecuteReader();

                while (reader.Read())
                {
                    var camera = new Camera
                    {
                        Numero = reader.GetInt32(reader.GetOrdinal("Numero")),
                        Descrizione = reader.GetString(reader.GetOrdinal("Descrizione")),
                        Tipologia = reader.GetString(reader.GetOrdinal("Tipologia")),
                        TariffaGiornaliera = reader.GetDecimal(reader.GetOrdinal("TariffaGiornaliera")),
                        Disponibile = reader.GetBoolean(reader.GetOrdinal("Disponibile"))
                    };
                    camere.Add(camera);
                }
            }

            return camere;
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
    }
}
