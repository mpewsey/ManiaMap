using System.IO;
using System.Runtime.Serialization.Json;
using System.Text;

namespace MPewsey.ManiaMap.Serialization
{
    /// <summary>
    /// Contains methods for serializing objects to and from JSON.
    /// </summary>
    public static class JsonSerialization
    {
        /// <summary>
        /// Returns the JSON string for the object graph.
        /// </summary>
        /// <param name="graph">The object graph.</param>
        /// <param name="settings">The writer settings. Pretty print used if none specified.</param>
        public static string GetJsonString<T>(T graph, JsonWriterSettings settings = null)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));
            settings = settings ?? JsonWriterSettings.PrettyPrintSettings();

            using (var stream = new MemoryStream())
            {
                using (var writer = JsonReaderWriterFactory.CreateJsonWriter(stream,
                    settings.Encoding, false, settings.Indent, settings.IndentCharacters))
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
        /// Serializes the object as JSON to the specified file path.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="graph">The object graph.</param>
        public static void SaveJson<T>(string path, T graph)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = File.Create(path))
            {
                serializer.WriteObject(stream, graph);
            }
        }

        /// <summary>
        /// Loads a JSON object from the specified path.
        /// </summary>
        /// <param name="path">The file path.</param>
        public static T LoadJson<T>(string path)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = File.OpenRead(path))
            {
                return (T)serializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Loads a JSON object from a byte array.
        /// </summary>
        /// <param name="bytes">A byte array.</param>
        public static T LoadJson<T>(byte[] bytes)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = new MemoryStream(bytes))
            {
                return (T)serializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Loads a JSON object from a JSON string.
        /// </summary>
        /// <param name="json">The JSON string.</param>
        public static T LoadJsonString<T>(string json)
        {
            return LoadJson<T>(Encoding.UTF8.GetBytes(json));
        }

        /// <summary>
        /// Serializes the object as encrypted JSON to the specified path.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="graph">The object graph.</param>
        /// <param name="key">The secret key.</param>
        public static void SaveEncryptedJson<T>(string path, T graph, byte[] key)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = File.Create(path))
            {
                Cryptography.EncryptToStream(stream, serializer, graph, key);
            }
        }

        /// <summary>
        /// Loads the object from an encrypted JSON file.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="key">The secret key.</param>
        public static T LoadEncryptedJson<T>(string path, byte[] key)
        {
            var serializer = new DataContractJsonSerializer(typeof(T));

            using (var stream = File.OpenRead(path))
            {
                return Cryptography.DecryptFromStream<T>(stream, serializer, key);
            }
        }
    }
}
