using System;
using System.Security.Cryptography;
using System.Text;

namespace PasswordHasher
{
    class Program
    {
        static void Main(string[] args)
        {
            string passwordLuca = "1234";
            string passwordPaolo = "5678";

            string saltLuca, saltPaolo, hashLuca, hashPaolo;

            hashLuca = HashPassword(passwordLuca, out saltLuca);
            hashPaolo = HashPassword(passwordPaolo, out saltPaolo);

            Console.WriteLine("Luca:");
            Console.WriteLine("Hash: " + hashLuca);
            Console.WriteLine("Salt: " + saltLuca);

            Console.WriteLine("\nPaolo:");
            Console.WriteLine("Hash: " + hashPaolo);
            Console.WriteLine("Salt: " + saltPaolo);

            // Genera lo script SQL per inserire nel database
            string insertScript = GenerateInsertScript("Luca", hashLuca, saltLuca, "Paolo", hashPaolo, saltPaolo);
            Console.WriteLine("\nScript SQL:");
            Console.WriteLine(insertScript);
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

        public static string GenerateInsertScript(string username1, string hash1, string salt1, string username2, string hash2, string salt2)
        {
            return $@"
INSERT INTO [dbo].[Utenti] (Username, PasswordHash, Salt)
VALUES
('{username1}', '{hash1}', '{salt1}'),
('{username2}', '{hash2}', '{salt2}');
";
        }
    }
}
