﻿using MPewsey.ManiaMap.Serialization;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap.Collections
{
    /// <summary>
    /// The base class for dictionaries with custom data contract serialization.
    /// </summary>
    [DataContract(Name = "BaseDataContractDictionary", Namespace = XmlSerialization.Namespace)]
    public abstract class BaseDataContractDictionary<TKey, TValue> : ICollection<KeyValuePair<TKey, TValue>>, IEnumerable<KeyValuePair<TKey, TValue>>, IEnumerable, IDictionary<TKey, TValue>, IReadOnlyCollection<KeyValuePair<TKey, TValue>>, IReadOnlyDictionary<TKey, TValue>, ICollection
    {
        /// <summary>
        /// The underlying dictionary.
        /// </summary>
        public Dictionary<TKey, TValue> Dictionary { get; protected set; } = new Dictionary<TKey, TValue>();

        public TValue this[TKey key] { get => ((IDictionary<TKey, TValue>)Dictionary)[key]; set => ((IDictionary<TKey, TValue>)Dictionary)[key] = value; }

        public int Count => ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).Count;

        public bool IsReadOnly => ((ICollection<KeyValuePair<TKey, TValue>>)Dictionary).IsReadOnly;

        public ICollection<TKey> Keys => ((IDictionary<TKey, TValue>)Dictionary).Keys;

        public ICollection<TValue> Values => ((IDictionary<TKey, TValue>)Dictionary).Values;

        public bool IsSynchronized => ((ICollection)Dictionary).IsSynchronized;

        public object SyncRoot => ((ICollection)Dictionary).SyncRoot;

        public bool IsFixedSize => ((IDictionary)Dictionary).IsFixedSize;

        IEnumerable<TKey> IReadOnlyDictionary<TKey, TValue>.Keys => ((IReadOnlyDictionary<TKey, TValue>)Dictionary).Keys;

        IEnumerable<TValue> IReadOnlyDictionary<TKey, TValue>.Values => ((IReadOnlyDictionary<TKey, TValue>)Dictionary).Values;

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
    }
}
