using MPewsey.ManiaMap.Serialization;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap.Collections
{
    /// <summary>
    /// A hash set that is data contract serializable.
    /// </summary>
    [DataContract(Name = "DataContractHashSet", Namespace = XmlSerialization.Namespace)]
    public class DataContractHashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ISet<T>
    {
        /// <summary>
        /// The underlying hash set.
        /// </summary>
        public HashSet<T> HashSet { get; private set; } = new HashSet<T>();

        /// <summary>
        /// An array of hash set entries.
        /// </summary>
        [DataMember(Order = 1, Name = "HashSet", IsRequired = true)]
        public T[] Array { get => GetArray(); set => HashSet = new HashSet<T>(value); }

        /// <summary>
        /// Initializes a new hash set.
        /// </summary>
        public DataContractHashSet()
        {

        }

        /// <summary>
        /// Initializes a new hash set from a collection.
        /// </summary>
        /// <param name="collection">A collection of values.</param>
        public DataContractHashSet(IEnumerable<T> collection)
        {
            HashSet = new HashSet<T>(collection);
        }

        /// <summary>
        /// Creates a new data contract hash set with the specified hash set assigned.
        /// </summary>
        /// <param name="set">The hash set.</param>
        public static implicit operator DataContractHashSet<T>(HashSet<T> set)
        {
            return set == null ? null : new DataContractHashSet<T> { HashSet = set };
        }

        /// <summary>
        /// Returns a new array of hash set entries.
        /// </summary>
        private T[] GetArray()
        {
            if (HashSet.Count == 0)
                return System.Array.Empty<T>();

            var array = new T[HashSet.Count];
            CopyTo(array, 0);
            return array;
        }

        public int Count => HashSet.Count;

        public bool IsReadOnly => ((ICollection<T>)HashSet).IsReadOnly;

        void ICollection<T>.Add(T item)
        {
            ((ICollection<T>)HashSet).Add(item);
        }

        public void Clear()
        {
            HashSet.Clear();
        }

        public bool Contains(T item)
        {
            return HashSet.Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            HashSet.CopyTo(array, arrayIndex);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            HashSet.ExceptWith(other);
        }

        public HashSet<T>.Enumerator GetEnumerator()
        {
            return HashSet.GetEnumerator();
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return HashSet.GetEnumerator();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            HashSet.IntersectWith(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return HashSet.IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return HashSet.IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return HashSet.IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return HashSet.IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return HashSet.Overlaps(other);
        }

        public bool Remove(T item)
        {
            return HashSet.Remove(item);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return HashSet.SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            HashSet.SymmetricExceptWith(other);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            HashSet.UnionWith(other);
        }

        public bool Add(T item)
        {
            return HashSet.Add(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)HashSet).GetEnumerator();
        }
    }
}
