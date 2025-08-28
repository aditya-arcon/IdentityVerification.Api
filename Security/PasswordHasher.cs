using System.Security.Cryptography;

namespace IdentityVerification.Api.Security
{
    public interface IPasswordHasher
    {
        void CreateHash(string password, out string hashBase64, out string saltBase64);
        bool Verify(string password, string storedHashBase64, string storedSaltBase64);
    }

    public class Pbkdf2PasswordHasher : IPasswordHasher
    {
        private const int SaltSize = 16;            // 128-bit
        private const int KeySize = 32;             // 256-bit
        private const int Iterations = 100_000;     // adjust as needed
        private static readonly HashAlgorithmName Algo = HashAlgorithmName.SHA256;

        public void CreateHash(string password, out string hashBase64, out string saltBase64)
        {
            using var rng = RandomNumberGenerator.Create();
            var salt = new byte[SaltSize];
            rng.GetBytes(salt);

            var key = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algo, KeySize);
            hashBase64 = Convert.ToBase64String(key);
            saltBase64 = Convert.ToBase64String(salt);
        }

        public bool Verify(string password, string storedHashBase64, string storedSaltBase64)
        {
            if (string.IsNullOrEmpty(storedHashBase64) || string.IsNullOrEmpty(storedSaltBase64))
                return false;

            var salt = Convert.FromBase64String(storedSaltBase64);
            var keyToCheck = Rfc2898DeriveBytes.Pbkdf2(password, salt, Iterations, Algo, KeySize);
            var expected = Convert.FromBase64String(storedHashBase64);
            // constant-time comparison
            return CryptographicOperations.FixedTimeEquals(keyToCheck, expected);
        }
    }
}
