using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace SystemPlus.Security
{
    /// <summary>
    /// Simple class for encrypting and decrypting string
    /// </summary>
    public static class StringEncryption
    {
        // This constant string is used as a "salt" value for the PasswordDeriveBytes function calls.
        // This size of the IV (in bytes) must = (keysize / 8).  Default keysize is 256, so the IV must be
        // 32 bytes long.  Using a 16 character string here gives us 32 bytes when converted to a byte array.
        const string initVector = "k85gnjk1qa08bzzp";

        // This constant is used to determine the keysize of the encryption algorithm.
        const int keysize = 256;

        /// <summary>
        /// Encrypt string
        /// </summary>
        /// <param name="plainText"></param>
        /// <param name="passPhrase"></param>
        /// <param name="salt">Must be at least 8 chars</param>
        /// <returns>Cipher text</returns>
        public static string Encrypt(string plainText, string passPhrase, string salt)
        {
            byte[] initVectorBytes = Encoding.UTF8.GetBytes(initVector);
            byte[] plainTextBytes = Encoding.UTF8.GetBytes(plainText);

            using (Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(passPhrase, Encoding.UTF8.GetBytes(salt)))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);

                using (RijndaelManaged symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC })
                using (ICryptoTransform encryptor = symmetricKey.CreateEncryptor(keyBytes, initVectorBytes))
                using (MemoryStream memoryStream = new MemoryStream())
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                {
                    cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                    cryptoStream.FlushFinalBlock();
                    byte[] cipherTextBytes = memoryStream.ToArray();
                    return Convert.ToBase64String(cipherTextBytes);
                }
            }
        }

        /// <summary>
        /// Decrypt string
        /// </summary>
        /// <param name="cipherText"></param>
        /// <param name="passPhrase"></param>
        /// <param name="salt">Must be at least 8 chars</param>
        /// <returns>Plain text</returns>
        public static string Decrypt(string cipherText, string passPhrase, string salt)
        {
            byte[] initVectorBytes = Encoding.ASCII.GetBytes(initVector);
            byte[] cipherTextBytes = Convert.FromBase64String(cipherText);

            using (Rfc2898DeriveBytes password = new Rfc2898DeriveBytes(passPhrase, Encoding.UTF8.GetBytes(salt)))
            {
                byte[] keyBytes = password.GetBytes(keysize / 8);

                using (RijndaelManaged symmetricKey = new RijndaelManaged { Mode = CipherMode.CBC })
                using (ICryptoTransform decryptor = symmetricKey.CreateDecryptor(keyBytes, initVectorBytes))
                using (MemoryStream memoryStream = new MemoryStream(cipherTextBytes))
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, decryptor, CryptoStreamMode.Read))
                {
                    byte[] plainTextBytes = new byte[cipherTextBytes.Length];
                    int decryptedByteCount = cryptoStream.Read(plainTextBytes, 0, plainTextBytes.Length);
                    return Encoding.UTF8.GetString(plainTextBytes, 0, decryptedByteCount);
                }
            }
        }
    }
}