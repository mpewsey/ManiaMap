using MPewsey.ManiaMap.Serialization;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap.Collections
{
    /// <summary>
    /// A serializable key value pair.
    /// </summary>
    [DataContract(Name = "KeyValue", Namespace = XmlSerialization.Namespace)]
    public struct KeyValue<TKey, TValue>
    {
        /// <summary>
        /// The key.
        /// </summary>
        [DataMember(Order = 1, Name = "Key", IsRequired = true)]
        public TKey Key { get; private set; }

        /// <summary>
        /// The value.
        /// </summary>
        [DataMember(Order = 2, Name = "Value", IsRequired = true)]
        public TValue Value { get; private set; }

        /// <summary>
        /// Initializes a new key value pair.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public KeyValue(TKey key, TValue value)
        {
            Key = key;
            Value = value;
        }

        public override string ToString()
        {
            return $"KeyValue(Key = {Key}, Value = {Value})";
        }
    }
}
