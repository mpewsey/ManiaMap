using System.IO;
using System.Runtime.Serialization;
using System.Xml;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Contains methods for serializing objects.
    /// </summary>
    public static class Serialization
    {
        /// <summary>
        /// Saves the object to the file path using the DataContractSerializer.
        /// The saved file includes tabs and new lines.
        /// </summary>
        /// <param name="path">The save file path.</param>
        /// <param name="graph">The object for serialization.</param>
        public static void SavePrettyXml<T>(string path, T graph)
        {
            var serializer = new DataContractSerializer(typeof(T));
            var settings = new XmlWriterSettings
            {
                Indent = true,
                IndentChars = "\t",
                NewLineChars = "\n",
            };

            using (var stream = File.Create(path))
            {
                using (var writer = XmlWriter.Create(stream, settings))
                {
                    serializer.WriteObject(writer, graph);
                }
            }
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
        /// <param name="bytes"></param>
        public static T LoadXml<T>(byte[] bytes)
        {
            var serializer = new DataContractSerializer(typeof(T));

            using (var stream = new MemoryStream(bytes))
            {
                return (T)serializer.ReadObject(stream);
            }
        }
    }
}
