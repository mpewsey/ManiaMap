using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A dictionary that is data contract serializable.
    /// </summary>
    [DataContract(Name = "DataContractDictionary")]
    public class DataContractDictionary<TKey, TValue> : BaseDataContractDictionary<TKey, TValue>
    {
        /// <summary>
        /// An array of key value pairs.
        /// </summary>
        [DataMember(Order = 1, Name = "Dictionary")]
        public KeyValue<TKey, TValue>[] KeyValueArray { get => GetKeyValueArray(); set => SetDictionary(value); }

        /// <summary>
        /// Initializes a new empty dictionary.
        /// </summary>
        public DataContractDictionary()
        {

        }

        /// <summary>
        /// Initializes a new dictionary with the specified capacity.
        /// </summary>
        /// <param name="capacity">The capacity.</param>
        public DataContractDictionary(int capacity)
        {
            Dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        /// <summary>
        /// Initializes a copy of the specified dictionary.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        public DataContractDictionary(DataContractDictionary<TKey, TValue> dict)
        {
            Dictionary = new Dictionary<TKey, TValue>(dict.Dictionary);
        }

        /// <summary>
        /// Creates a new data contract dictionary instance with the specified dictionary assigned.
        /// </summary>
        /// <param name="dict">The dictionary.</param>
        public static implicit operator DataContractDictionary<TKey, TValue>(Dictionary<TKey, TValue> dict)
        {
            return dict == null ? null : new DataContractDictionary<TKey, TValue> { Dictionary = dict };
        }

        /// <summary>
        /// Returns a new array of key value pairs for the dictionary.
        /// </summary>
        private KeyValue<TKey, TValue>[] GetKeyValueArray()
        {
            if (Dictionary.Count == 0)
                return Array.Empty<KeyValue<TKey, TValue>>();

            int i = 0;
            var array = new KeyValue<TKey, TValue>[Dictionary.Count];

            foreach (var pair in Dictionary)
            {
                array[i++] = new KeyValue<TKey, TValue>(pair.Key, pair.Value);
            }

            return array;
        }

        /// <summary>
        /// Sets the dictionary from an array of key value pairs.
        /// </summary>
        /// <param name="array">An array of key value pairs.</param>
        private void SetDictionary(KeyValue<TKey, TValue>[] array)
        {
            Dictionary = new Dictionary<TKey, TValue>(array.Length);

            foreach (var pair in array)
            {
                Dictionary.Add(pair.Key, pair.Value);
            }
        }
    }
}
