using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;

namespace MPewsey.ManiaMap.Serialization
{
    /// <summary>
    /// Contains methods related to encryption and decryption.
    /// </summary>
    public static class Cryptography
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

        /// <summary>
        /// Encrypts the object to the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="graph">The object graph.</param>
        /// <param name="key">The secret key.</param>
        public static void EncryptToStream<T>(Stream stream, XmlObjectSerializer serializer, T graph, byte[] key)
        {
            using (var algorithm = Aes.Create())
            using (var encryptor = algorithm.CreateEncryptor(key, algorithm.IV))
            using (var crypto = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
            {
                stream.Write(algorithm.IV, 0, algorithm.IV.Length);
                serializer.WriteObject(crypto, graph);
            }
        }

        /// <summary>
        /// Decrypts the object from the stream.
        /// </summary>
        /// <param name="stream">The stream.</param>
        /// <param name="serializer">The serializer.</param>
        /// <param name="key">The secret key.</param>
        public static T DecryptFromStream<T>(Stream stream, XmlObjectSerializer serializer, byte[] key)
        {
            using (var algorithm = Aes.Create())
            {
                var iv = new byte[algorithm.IV.Length];
                stream.Read(iv, 0, iv.Length);

                using (var decryptor = algorithm.CreateDecryptor(key, iv))
                using (var crypto = new CryptoStream(stream, decryptor, CryptoStreamMode.Read))
                {
                    return (T)serializer.ReadObject(crypto);
                }
            }
        }
    }
}
