using System.Security.Cryptography;
using System.Text;

namespace Snipper_Snippets_API.Services
{
    public class EncryptionService
    {
        private readonly string _key;
        public EncryptionService(IConfiguration configuration)
        {
            _key = configuration["ENCRYPTION_KEY"] ??
                throw new InvalidOperationException("ENCRYPTION_KEY not found in configuration");
        }

        public string Encrypt(string plainText)
        {
            using var aes = Aes.Create();
            aes.Key = Convert.FromHexString(_key);
            aes.GenerateIV();

            using var encryptor = aes.CreateEncryptor();
            var plainBytes = Encoding.UTF8.GetBytes(plainText);
            var encryptedBytes = encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);

            return Convert.ToHexString(aes.IV) + ":" + Convert.ToHexString(encryptedBytes);
        }

        public string Decrypt(string encryptedText)
        {
            var parts = encryptedText.Split(':');
            if (parts.Length != 2)
                throw new ArgumentException("Invalid encrypted text format");

            using var aes = Aes.Create();
            aes.Key = Convert.FromHexString(_key);
            aes.IV = Convert.FromHexString(parts[0]);

            using var decryptor = aes.CreateDecryptor();
            var encryptedBytes = Convert.FromHexString(parts[1]);
            var decryptedBytes = decryptor.TransformFinalBlock(encryptedBytes, 0, encryptedBytes.Length);

            return Encoding.UTF8.GetString(decryptedBytes);
        }
    }
}