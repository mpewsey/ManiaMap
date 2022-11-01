using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap.Collections
{
    /// <summary>
    /// A dictionary that is data contract serializable.
    /// </summary>
    [DataContract(Name = "DataContractDictionary")]
    public class DataContractDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection, IDictionary
    {
        /// <summary>
        /// The underlying dictionary.
        /// </summary>
        [IgnoreDataMember] // See KeyValueArray
        public Dictionary<TKey, TValue> Dictionary { get; private set; }

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
            Dictionary = new Dictionary<TKey, TValue>();
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

        public TValue this[TKey key] { get => ((IDictionary<TKey, TValue>)Dictionary)[key]; set => ((IDictionary<TKey, TValue>)Dictionary)[key] = value; }
        public object this[object key] { get => ((IDictionary)Dictionary)[key]; set => ((IDictionary)Dictionary)[key] = value; }

        public int Count => ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).IsReadOnly;

        public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)Dictionary).Keys;

        public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)Dictionary).Values;

        public bool IsSynchronized => ((ICollection)Dictionary).IsSynchronized;

        public object SyncRoot => ((ICollection)Dictionary).SyncRoot;

        public bool IsFixedSize => ((IDictionary)Dictionary).IsFixedSize;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IReadOnlyDictionary<TKey, TValue>)Dictionary).Keys;

        ICollection IDictionary.Keys => ((IDictionary)Dictionary).Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IReadOnlyDictionary<TKey, TValue>)Dictionary).Values;

        ICollection IDictionary.Values => ((IDictionary)Dictionary).Values;

        public void Add(KeyValuePair<TKey, TValue> item)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).Add(item);
        }

        public void Add(TKey key, TValue value)
        {
            ((IDictionary<TKey, TValue>)Dictionary).Add(key, value);
        }

        public void Add(object key, object value)
        {
            ((IDictionary)Dictionary).Add(key, value);
        }

        public void Clear()
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).Clear();
        }

        public bool Contains(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).Contains(item);
        }

        public bool Contains(object key)
        {
            return ((IDictionary)Dictionary).Contains(key);
        }

        public bool ContainsKey(TKey key)
        {
            return ((IDictionary<TKey, TValue>)Dictionary).ContainsKey(key);
        }

        public void CopyTo(KeyValuePair<TKey, TValue>[] array, int arrayIndex)
        {
            ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).CopyTo(array, arrayIndex);
        }

        public void CopyTo(Array array, int index)
        {
            ((ICollection)Dictionary).CopyTo(array, index);
        }

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator()
        {
            return ((IEnumerable<KeyValuePair<TKey, TValue>>)Dictionary).GetEnumerator();
        }

        public bool Remove(KeyValuePair<TKey, TValue> item)
        {
            return ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).Remove(item);
        }

        public bool Remove(TKey key)
        {
            return ((IDictionary<TKey, TValue>)Dictionary).Remove(key);
        }

        public void Remove(object key)
        {
            ((IDictionary)Dictionary).Remove(key);
        }

        public bool TryGetValue(TKey key, out TValue value)
        {
            return ((IDictionary<TKey, TValue>)Dictionary).TryGetValue(key, out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)Dictionary).GetEnumerator();
        }

        IDictionaryEnumerator IDictionary.GetEnumerator()
        {
            return ((IDictionary)Dictionary).GetEnumerator();
        }
    }
}
