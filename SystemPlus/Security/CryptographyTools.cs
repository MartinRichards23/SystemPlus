using System;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SystemPlus.Security
{
    /// <summary>
    /// Tools relating 
    /// </summary>
    public static class CryptographyTools
    {
        /// <summary>
        /// Helper that generates a random key on each call.
        /// </summary>
        public static byte[] NewKey(int length = 32)
        {
            using RandomNumberGenerator rnd = RandomNumberGenerator.Create();

            byte[] key = new byte[length];
            rnd.GetBytes(key);

            return key;
        }

        public static string CreateToken(int size)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = RandomNumberGenerator.GetBytes(size);

            StringBuilder result = new(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }

            return result.ToString();
        }

        /// <summary>
        /// Gets a random byte array for use as a salt
        /// </summary>
        /// <param name="length"></param>
        /// <returns>Byte array of random salt data</returns>
        public static byte[] GenerateRandomSalt(int length)
        {
            byte[] salt = RandomNumberGenerator.GetBytes(length);

            return salt;
        }

        /// <summary>
        /// Generates the SHA256 salted hash of the data
        /// </summary>
        public static byte[] CreateSha256Hash(byte[] data, byte[] salt)
        {
            ArgumentNullException.ThrowIfNull(data);
            ArgumentNullException.ThrowIfNull(salt);

            byte[] combinedBytes = new byte[data.Length + salt.Length];

            for (int i = 0; i < data.Length; i++)
            {
                combinedBytes[i] = data[i];
            }

            for (int i = 0; i < salt.Length; i++)
            {
                combinedBytes[data.Length + i] = salt[i];
            }

            using HashAlgorithm algorithm = SHA256.Create();

            return algorithm.ComputeHash(combinedBytes);
        }

        /// <summary>
        /// Generates the SHA256 salted hash of the data
        /// </summary>
        public static string CreateSha256Hash(string plainText, byte[] salt)
        {
            byte[] data = Encoding.Unicode.GetBytes(plainText);
            byte[] hashData = CreateSha256Hash(data, salt);
            return Encoding.Unicode.GetString(hashData);
        }

        public static string GenerateCryptoKeyString(int length)
        {
            byte[] bytes = GenerateCryptoKeyByteArray(length);
            return BitConverter.ToString(bytes);
        }

        public static byte[] GenerateCryptoKeyByteArray(int length)
        {
            byte[] bytes = new byte[length];
            using RandomNumberGenerator rng = RandomNumberGenerator.Create();

            rng.GetBytes(bytes);

            return bytes;
        }

        /// <summary>
        /// Scores How strong a password is, 0 to 1
        /// </summary>
        public static double CheckStrength(string password)
        {
            if (password == null)
                throw new ArgumentNullException(nameof(password));

            double score = 1;

            if (password.Length < 1)
                return 0;

            if (password.Length >= 6)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.IsMatch(password, @"\d+"))
                score++;
            if (Regex.IsMatch(password, @"[a-z]"))
                score++;
            if (Regex.IsMatch(password, @"[A-Z]"))
                score++;
            if (Regex.IsMatch(password, @"[!@#\$%\^&\*\?_~\-\(\);\.\+:]+"))
                score++;

            return score;
        }

    }
}