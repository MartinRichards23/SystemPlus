﻿using System;
using System.IO;
using System.Linq;
using System.Net.NetworkInformation;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace SystemPlus.Security
{
    public static class CryptoSecurity
    {
        public static string CreateToken(int size)
        {
            char[] chars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
            byte[] data = new byte[1];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(data);
                data = new byte[size];
                crypto.GetNonZeroBytes(data);
            }

            StringBuilder result = new StringBuilder(size);
            foreach (byte b in data)
            {
                result.Append(chars[b % (chars.Length)]);
            }

            return result.ToString();
        }

        public static string GetMacAddress()
        {
            NetworkInterface macAddr = NetworkInterface.GetAllNetworkInterfaces().FirstOrDefault(n => n.OperationalStatus == OperationalStatus.Up);

            if (macAddr == null)
                return null;

            PhysicalAddress pa = macAddr.GetPhysicalAddress();

            return pa.ToString();
        }

        /// <summary>
        /// Gets a random byte array for use as a salt
        /// </summary>
        /// <param name="length"></param>
        /// <returns>Byte array of random salt data</returns>
        public static byte[] GenerateRandomSalt(int length)
        {
            byte[] salt = new byte[length];

            using (RNGCryptoServiceProvider crypto = new RNGCryptoServiceProvider())
            {
                crypto.GetNonZeroBytes(salt);
            }

            return salt;
        }

        /// <summary>
        /// Generates the SHA256 salted hash of the data
        /// </summary>
        public static byte[] CreateSha256Hash(byte[] data, byte[] salt)
        {
            byte[] combinedBytes = new byte[data.Length + salt.Length];

            for (int i = 0; i < data.Length; i++)
            {
                combinedBytes[i] = data[i];
            }
            for (int i = 0; i < salt.Length; i++)
            {
                combinedBytes[data.Length + i] = salt[i];
            }

            using (HashAlgorithm algorithm = new SHA256Managed())
            {
                return algorithm.ComputeHash(combinedBytes);
            }
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

        public static string CalculateMd5Hash(string input)
        {
            return CalculateMd5Hash(input, Encoding.ASCII);
        }

        public static string CalculateMd5Hash(string input, Encoding encoding)
        {
            byte[] inputBytes = encoding.GetBytes(input);
            return CalculateMd5Hash(inputBytes);
        }

        public static string CalculateMd5Hash(byte[] input)
        {
            // step 1, calculate MD5 hash from input
            using (MD5 md5 = MD5.Create())
            {
                byte[] hash = md5.ComputeHash(input);

                // step 2, convert byte array to hex string
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < hash.Length; i++)
                {
                    sb.Append(hash[i].ToString("X2"));
                }

                return sb.ToString();
            }
        }

        [Obsolete]
        public static string CreateSha1Hash(string text, string salt) //old salt = "sdfdnm51159hjgng5df7fdj98gh"
        {
            // salt the password
            text += salt;

            byte[] buffer = Encoding.UTF8.GetBytes(text);

            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                string hash = BitConverter.ToString(sha1.ComputeHash(buffer)).Replace("-", "");
                return hash;
            }
        }

        public static byte[] CreateKeyedShaHash(byte[] key, string filePath)
        {
            using (HMACSHA1 hashAlg = new HMACSHA1(key))
            using (Stream file = new FileStream(filePath, FileMode.Open))
            {
                byte[] hash = hashAlg.ComputeHash(file);
                return hash;
            }
        }

        public static string GenerateCryptoKeyString(int length)
        {
            byte[] bytes = GenerateCryptoKeyByteArray(length);
            return BitConverter.ToString(bytes);
        }

        public static byte[] GenerateCryptoKeyByteArray(int length)
        {
            byte[] bytes = new byte[length];
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(bytes);
                return bytes;
            }
        }

        public enum PasswordScore
        {
            Blank = 0,
            VeryWeak = 1,
            Weak = 2,
            Medium = 3,
            Strong = 4,
            VeryStrong = 5
        }

        /// <summary>
        /// Scores How strong a password is, 0 to 1
        /// </summary>
        public static double CheckStrength(string password)
        {
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