using CapStone_AndreaGuarnieri.Models.ViewModels;
using System.Data.SqlClient;

namespace CapStone_AndreaGuarnieri.DataAccess
{
    public class TariffeDataAccess
    {
        private readonly string _connectionString;

        public TariffeDataAccess(string connectionString)
        {
            _connectionString = connectionString;
        }

        public IEnumerable<TariffaViewModel> GetAllTariffe()
        {
            var tariffe = new List<TariffaViewModel>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Tariffe";
                using (var command = new SqlCommand(query, connection))
                {
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tariffa = new TariffaViewModel
                            {
                                ID = (int)reader["ID"],
                                TipoStagione = reader["TipoStagione"].ToString(),
                                TipoCamera = reader["TipoCamera"].ToString(),
                                TariffaGiornaliera = (decimal)reader["TariffaGiornaliera"],
                                DataInizio = (DateTime)reader["DataInizio"],
                                DataFine = (DateTime)reader["DataFine"]
                            };
                            tariffe.Add(tariffa);
                        }
                    }
                }
            }
            return tariffe;
        }

        public TariffaViewModel GetTariffaById(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Tariffe WHERE ID = @ID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    using (var reader = command.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            return new TariffaViewModel
                            {
                                ID = (int)reader["ID"],
                                TipoStagione = reader["TipoStagione"].ToString(),
                                TipoCamera = reader["TipoCamera"].ToString(),
                                TariffaGiornaliera = (decimal)reader["TariffaGiornaliera"],
                                DataInizio = (DateTime)reader["DataInizio"],
                                DataFine = (DateTime)reader["DataFine"]
                            };
                        }
                    }
                }
            }
            return null;
        }

        public IEnumerable<TariffaViewModel> GetOverlappingTariffe(DateTime startDate, DateTime endDate)
        {
            var tariffe = new List<TariffaViewModel>();
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "SELECT * FROM Tariffe WHERE DataInizio <= @EndDate AND DataFine >= @StartDate";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@StartDate", startDate);
                    command.Parameters.AddWithValue("@EndDate", endDate);
                    using (var reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            var tariffa = new TariffaViewModel
                            {
                                ID = (int)reader["ID"],
                                TipoStagione = reader["TipoStagione"].ToString(),
                                TipoCamera = reader["TipoCamera"].ToString(),
                                TariffaGiornaliera = (decimal)reader["TariffaGiornaliera"],
                                DataInizio = (DateTime)reader["DataInizio"],
                                DataFine = (DateTime)reader["DataFine"]
                            };
                            tariffe.Add(tariffa);
                        }
                    }
                }
            }
            return tariffe;
        }

        public void UpdateTariffa(TariffaViewModel tariffa)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"UPDATE Tariffe 
                         SET TipoStagione = @TipoStagione, 
                             TipoCamera = @TipoCamera, 
                             TariffaGiornaliera = @TariffaGiornaliera, 
                             DataInizio = @DataInizio, 
                             DataFine = @DataFine 
                         WHERE ID = @ID";

                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", tariffa.ID);
                    command.Parameters.AddWithValue("@TipoStagione", tariffa.TipoStagione);
                    command.Parameters.AddWithValue("@TipoCamera", tariffa.TipoCamera);
                    command.Parameters.AddWithValue("@TariffaGiornaliera", tariffa.TariffaGiornaliera);
                    command.Parameters.AddWithValue("@DataInizio", tariffa.DataInizio);
                    command.Parameters.AddWithValue("@DataFine", tariffa.DataFine);
                    command.ExecuteNonQuery();
                }
            }
        }


        public void CreateTariffa(TariffaViewModel tariffa)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = @"INSERT INTO Tariffe (TipoStagione, TipoCamera, TariffaGiornaliera, DataInizio, DataFine) 
                             VALUES (@TipoStagione, @TipoCamera, @TariffaGiornaliera, @DataInizio, @DataFine)";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@TipoStagione", tariffa.TipoStagione);
                    command.Parameters.AddWithValue("@TipoCamera", tariffa.TipoCamera);
                    command.Parameters.AddWithValue("@TariffaGiornaliera", tariffa.TariffaGiornaliera);
                    command.Parameters.AddWithValue("@DataInizio", tariffa.DataInizio);
                    command.Parameters.AddWithValue("@DataFine", tariffa.DataFine);
                    command.ExecuteNonQuery();
                }
            }
        }

        public void DeleteTariffa(int id)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                connection.Open();
                string query = "DELETE FROM Tariffe WHERE ID = @ID";
                using (var command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@ID", id);
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
