using System.Security.Cryptography;

namespace neoStockMasterv2.Data.Services
{
    public static class PasswordHasher
    {
        private const string Prefix = "PBKDF2";
        private const int Iterations = 100_000;
        private const int SaltSize = 16;
        private const int HashSize = 32;

        public static string Hash(string password)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(SaltSize);
            byte[] hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, HashAlgorithmName.SHA256, HashSize);
            return $"{Prefix}${Iterations}${Convert.ToBase64String(salt)}${Convert.ToBase64String(hash)}";
        }

        public static bool IsHashed(string? stored) =>
            stored != null && stored.StartsWith(Prefix + "$");

        public static bool Verify(string password, string? stored)
        {
            if (string.IsNullOrEmpty(stored)) return false;

            // Eski düz metin kayıt (geçiş dönemi için)
            if (!IsHashed(stored)) return stored == password;

            string[] parts = stored.Split('$');
            if (parts.Length != 4) return false;

            int iterations = int.Parse(parts[1]);
            byte[] salt = Convert.FromBase64String(parts[2]);
            byte[] expected = Convert.FromBase64String(parts[3]);
            byte[] actual = Rfc2898DeriveBytes.Pbkdf2(password, salt, iterations, HashAlgorithmName.SHA256, expected.Length);

            return CryptographicOperations.FixedTimeEquals(actual, expected);
        }
    }
}