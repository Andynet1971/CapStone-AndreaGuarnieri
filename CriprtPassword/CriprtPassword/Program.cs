using System;
using System.Security.Cryptography;
using System.Text;
using System.Data.SqlClient;

namespace PasswordHasher
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.Write("Inserisci il nome utente: ");
            string username = Console.ReadLine();

            Console.Write("Inserisci la password: ");
            string password = Console.ReadLine();

            string salt, hash;

            hash = HashPassword(password, out salt);

            Console.WriteLine("Hash: " + hash);
            Console.WriteLine("Salt: " + salt);

            // Inserisce i dati nel database
            InsertUserIntoDatabase(username, hash, salt);

            Console.WriteLine("\nDati inseriti con successo nel database.");
        }

        public static string HashPassword(string password, out string salt)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] saltBytes = new byte[16];
                rng.GetBytes(saltBytes);
                salt = Convert.ToBase64String(saltBytes);
            }

            using (var deriveBytes = new Rfc2898DeriveBytes(password, Encoding.UTF8.GetBytes(salt), 10000))
            {
                byte[] hashBytes = deriveBytes.GetBytes(32); // 32 bytes = 256 bits
                return Convert.ToBase64String(hashBytes);
            }
        }

        public static void InsertUserIntoDatabase(string username, string hash, string salt)
        {
            string connectionString = "Your_Connection_String_Here"; // Sostituisci con la tua stringa di connessione

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string query = @"
INSERT INTO [dbo].[Utenti] (Username, PasswordHash, Salt)
VALUES (@Username, @PasswordHash, @Salt);";

                using (SqlCommand command = new SqlCommand(query, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    command.Parameters.AddWithValue("@PasswordHash", hash);
                    command.Parameters.AddWithValue("@Salt", salt);

                    connection.Open();
                    command.ExecuteNonQuery();
                }
            }
        }
    }
}
