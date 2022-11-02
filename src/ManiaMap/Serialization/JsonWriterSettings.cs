using System.Text;

namespace MPewsey.ManiaMap.Serialization
{
    /// <summary>
    /// Contains settings for JSON writer formatting.
    /// </summary>
    public class JsonWriterSettings
    {
        /// <summary>
        /// The text encoding.
        /// </summary>
        public Encoding Encoding { get; set; } = Encoding.UTF8;

        /// <summary>
        /// If true, indenting wil be applied.
        /// </summary>
        public bool Indent { get; set; }

        /// <summary>
        /// The characters used for indents.
        /// </summary>
        public string IndentCharacters { get; set; } = string.Empty;

        /// <summary>
        /// Returns new settings for pretty printing.
        /// </summary>
        public static JsonWriterSettings PrettyPrintSettings()
        {
            return new JsonWriterSettings
            {
                Indent = true,
                IndentCharacters = "  ",
            };
        }
    }
}
