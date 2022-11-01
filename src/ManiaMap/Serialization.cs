using System.IO;
using System.Runtime.Serialization;
using System.Security.Cryptography;
using System.Text;
using System.Xml;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Contains methods for serializing objects.
    /// </summary>
    public static class Serialization
    {
        /// <summary>
        /// The data contract namespace.
        /// </summary>
        public const string Namespace = "http://github.com/mpewsey/ManiaMap";

        /// <summary>
        /// Returns a new instance of XML writer settings for pretty printing.
        /// </summary>
        public static XmlWriterSettings PrettyXmlWriterSettings()
        {
            return new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\n",
            };
        }

        /// <summary>
        /// Returns the pretty XML string for the object.
        /// </summary>
        /// <param name="graph">The object for serialization.</param>
        /// <param name="settings">The XML writer settings.</param>
        public static string GetXmlString<T>(T graph, XmlWriterSettings settings)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var stream = new MemoryStream())
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.WriteObject(writer, graph);
                }

                stream.Seek(0, SeekOrigin.Begin);

                using (var reader = new StreamReader(stream))
                {
                    return reader.ReadToEnd();
                }
            }
        }

        /// <summary>
        /// Returns the pretty XML string for the object.
        /// </summary>
        /// <param name="graph">The object for serialization.</param>
        public static string GetPrettyXmlString<T>(T graph)
        {
            return GetXmlString(graph, PrettyXmlWriterSettings());
        }

        /// <summary>
        /// Saves the object to the file path using the DataContractSerializer.
        /// The saved file includes tabs and new lines.
        /// </summary>
        /// <param name="path">The save file path.</param>
        /// <param name="graph">The object for serialization.</param>
        public static void SavePrettyXml<T>(string path, T graph)
        {
            SaveXml(path, graph, PrettyXmlWriterSettings());
        }

        /// <summary>
        /// Saves the object to the file path using the DataContractSerializer.
        /// </summary>
        /// <param name="path">The save file path.</param>
        /// <param name="graph">The object for serialization.</param>
        public static void SaveXml<T>(string path, T graph)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var stream = File.Create(path))
            {
                serializer.WriteObject(stream, graph);
            }
        }

        /// <summary>
        /// Saves the object to the file path using the DataContractSerializer.
        /// </summary>
        /// <param name="path">The save file path.</param>
        /// <param name="graph">The object for serialization.</param>
        /// <param name="settings">The XML writer settings.</param>
        public static void SaveXml<T>(string path, T graph, XmlWriterSettings settings)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var stream = File.Create(path))
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.WriteObject(writer, graph);
                }
            }
        }

        /// <summary>
        /// Loads an object from a file path using the DataContractSerializer.
        /// </summary>
        /// <param name="path">The file path.</param>
        public static T LoadXml<T>(string path)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var stream = File.OpenRead(path))
            {
                return (T)serializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Loads an object from a byte array using the DataContractSerializer.
        /// </summary>
        /// <param name="bytes">The byte array.</param>
        public static T LoadXml<T>(byte[] bytes)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var stream = new MemoryStream(bytes))
            {
                return (T)serializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Loads an object from an XML string using the DataContractSerializer.
        /// </summary>
        /// <param name="xml">The XML string.</param>
        public static T LoadXmlString<T>(string xml)
        {
            return LoadXml<T>(Encoding.UTF8.GetBytes(xml));
        }

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
        /// Serializes and encrypts and object to the specified file.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="graph">The object.</param>
        /// <param name="key">The private key.</param>
        public static void SaveEncryptedXml<T>(string path, T graph, byte[] key)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var stream = File.Create(path))
            using (var algorithm = Aes.Create())
            using (var encryptor = algorithm.CreateEncryptor(key, algorithm.IV))
            using (var crypto = new CryptoStream(stream, encryptor, CryptoStreamMode.Write))
            {
                stream.Write(algorithm.IV, 0, algorithm.IV.Length);
                serializer.WriteObject(crypto, graph);
            }
        }

        /// <summary>
        /// Decrypts and deserializes an object from the specified file.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="graph">The object.</param>
        /// <param name="key">The private key.</param>
        public static T LoadEncryptedXml<T>(string path, byte[] key)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var stream = File.OpenRead(path))
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
