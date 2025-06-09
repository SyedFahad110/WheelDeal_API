using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace BookPlazaAPI.AppClasses
{
    public class GeneralClass
    {
        private static readonly byte[] Key = new byte[32];
        private static readonly byte[] IV = new byte[16];
        private static readonly byte[] IV2 = { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07, 0x08, 0x09, 0x10, 0x11, 0x12, 0x13, 0x14, 0x15, 0x16 };
        static string passphrase = "1rkIGgBkML3U";
        private const string SecretKey = "sP1iMyY40xe1fISbJ772sqU1p4EOttyf";
        private static readonly Random Random = new Random();
        private const string Characters = "G2YG3Fv23dGREWC27za1LhMhLwjzsOj52LDx5Om6jlTOr6a8waCTTkQb006Ku3Gz";

        private static byte[] GenerateSecureKey(int keySize)
        {
            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] key = new byte[keySize];
                rng.GetBytes(key);
                return key;
            }
        }
        public static async Task<string> DecryptAsync(byte[] encrypted)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(passphrase);
            aes.IV = IV2;
            using MemoryStream input = new(encrypted);
            using CryptoStream cryptoStream = new(input, aes.CreateDecryptor(), CryptoStreamMode.Read);
            using MemoryStream output = new();
            await cryptoStream.CopyToAsync(output);
            return Encoding.Unicode.GetString(output.ToArray());
        }
        public static async Task<byte[]> EncryptAsync(string clearText)
        {
            using Aes aes = Aes.Create();
            aes.Key = DeriveKeyFromPassword(passphrase);
            aes.IV = IV2;
            using MemoryStream output = new();
            using CryptoStream cryptoStream = new(output, aes.CreateEncryptor(), CryptoStreamMode.Write);
            await cryptoStream.WriteAsync(Encoding.Unicode.GetBytes(clearText));
            await cryptoStream.FlushFinalBlockAsync();
            return output.ToArray();
        }
        private static byte[] DeriveKeyFromPassword(string password)
        {
            var emptySalt = Array.Empty<byte>();
            var iterations = 1000;
            var desiredKeyLength = 16; // 16 bytes equal 128 bits.
            var hashMethod = HashAlgorithmName.SHA384;
            return Rfc2898DeriveBytes.Pbkdf2(Encoding.Unicode.GetBytes(password),
                                             emptySalt,
                                             iterations,
                                             hashMethod,
                                             desiredKeyLength);
        }

        public static byte[] HexStringToByteArray(string hexString)
        {
            // Remove any non-hexadecimal characters (e.g., dashes)
            string cleanedHexString = hexString.Replace("-", "");

            // Ensure the cleaned hex string has an even number of characters
            if (cleanedHexString.Length % 2 != 0)
            {
                throw new ArgumentException("Invalid hex string length.");
            }

            // Convert the cleaned hex string to a byte array
            byte[] byteArray = new byte[cleanedHexString.Length / 2];
            for (int i = 0; i < byteArray.Length; i++)
            {
                byteArray[i] = Convert.ToByte(cleanedHexString.Substring(i * 2, 2), 16);
            }

            return byteArray;
        }
        public static string GenerateJwtToken(string userName, string email, string userType)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                new Claim(ClaimTypes.Name, userName),
                new Claim(ClaimTypes.Email, email)
                }),
                Expires = DateTime.UtcNow.AddHours(12),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        public class EmailSettings
        {
            public string? Host { get; set; }
            public int Port { get; set; }
            public string? Username { get; set; }
            public string? Password { get; set; }
            public string? FromEmail { get; set; }
            public string? FromName { get; set; }
        }

    }
}
