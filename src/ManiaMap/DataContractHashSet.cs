using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A hash set that is data contract serializable.
    /// </summary>
    [DataContract(Name = "DataContractHashSet")]
    public class DataContractHashSet<T> : ICollection<T>, IEnumerable<T>, IEnumerable, IReadOnlyCollection<T>, ISet<T>
    {
        /// <summary>
        /// The underlying hash set.
        /// </summary>
        public HashSet<T> HashSet { get; private set; }

        /// <summary>
        /// An array of hash set entries.
        /// </summary>
        [DataMember(Order = 1, Name = "HashSet")]
        public T[] Array { get => GetArray(); set => HashSet = new HashSet<T>(value); }

        /// <summary>
        /// Initializes a new hash set.
        /// </summary>
        public DataContractHashSet()
        {
            HashSet = new HashSet<T>();
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

            int i = 0;
            var array = new T[HashSet.Count];

            foreach (var value in HashSet)
            {
                array[i++] = value;
            }

            return array;
        }

        public int Count => ((ICollection<T>)HashSet).Count;

        public bool IsReadOnly => ((ICollection<T>)HashSet).IsReadOnly;

        public void Add(T item)
        {
            ((ICollection<T>)HashSet).Add(item);
        }

        public void Clear()
        {
            ((ICollection<T>)HashSet).Clear();
        }

        public bool Contains(T item)
        {
            return ((ICollection<T>)HashSet).Contains(item);
        }

        public void CopyTo(T[] array, int arrayIndex)
        {
            ((ICollection<T>)HashSet).CopyTo(array, arrayIndex);
        }

        public void ExceptWith(IEnumerable<T> other)
        {
            ((ISet<T>)HashSet).ExceptWith(other);
        }

        public IEnumerator<T> GetEnumerator()
        {
            return ((IEnumerable<T>)HashSet).GetEnumerator();
        }

        public void IntersectWith(IEnumerable<T> other)
        {
            ((ISet<T>)HashSet).IntersectWith(other);
        }

        public bool IsProperSubsetOf(IEnumerable<T> other)
        {
            return ((ISet<T>)HashSet).IsProperSubsetOf(other);
        }

        public bool IsProperSupersetOf(IEnumerable<T> other)
        {
            return ((ISet<T>)HashSet).IsProperSupersetOf(other);
        }

        public bool IsSubsetOf(IEnumerable<T> other)
        {
            return ((ISet<T>)HashSet).IsSubsetOf(other);
        }

        public bool IsSupersetOf(IEnumerable<T> other)
        {
            return ((ISet<T>)HashSet).IsSupersetOf(other);
        }

        public bool Overlaps(IEnumerable<T> other)
        {
            return ((ISet<T>)HashSet).Overlaps(other);
        }

        public bool Remove(T item)
        {
            return ((ICollection<T>)HashSet).Remove(item);
        }

        public bool SetEquals(IEnumerable<T> other)
        {
            return ((ISet<T>)HashSet).SetEquals(other);
        }

        public void SymmetricExceptWith(IEnumerable<T> other)
        {
            ((ISet<T>)HashSet).SymmetricExceptWith(other);
        }

        public void UnionWith(IEnumerable<T> other)
        {
            ((ISet<T>)HashSet).UnionWith(other);
        }

        bool ISet<T>.Add(T item)
        {
            return ((ISet<T>)HashSet).Add(item);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)HashSet).GetEnumerator();
        }
    }
}
