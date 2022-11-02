using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace MPewsey.ManiaMap.Serialization
{
    public static class Encryption
    {
        /// <summary>
        /// Returns the decrypted text for the file at the specified path.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="key">The secret key.</param>
        public static string DecryptTextFile(string path, byte[] key)
        {
            using (var stream = File.OpenRead(path))
            using (var algorithm = Aes.Create())
            {
                var iv = new byte[algorithm.IV.Length];
                stream.Read(iv, 0, iv.Length);

                using (var encryptor = algorithm.CreateDecryptor(key, iv))
                using (var crypto = new CryptoStream(stream, encryptor, CryptoStreamMode.Read))
                using (var reader = new StreamReader(crypto))
                {
                    return reader.ReadToEnd();
                }
            }
        }
    }
}
