using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A base class for implementing groups of items.
    /// </summary>
    /// <typeparam name="TKey">The group key type.</typeparam>
    /// <typeparam name="TValue">The item value type.</typeparam>
    [DataContract(Name = "ItemGroups")]
    public abstract class ItemGroups<TKey, TValue>
    {
        /// <summary>
        /// A dictionary of items by group.
        /// </summary>
        [DataMember(Order = 1)]
        protected DataContractDictionary<TKey, List<TValue>> Groups { get; set; } = new DataContractDictionary<TKey, List<TValue>>();

        /// <summary>
        /// Returns an enumerable of group key-value pairs.
        /// </summary>
        public IEnumerable<KeyValuePair<TKey, List<TValue>>> GetGroups()
        {
            return Groups;
        }

        /// <summary>
        /// Returns an enumerable of all group ID's.
        /// </summary>
        public IEnumerable<TKey> GetGroupIds()
        {
            return Groups.Keys;
        }

        /// <summary>
        /// Returns an enumerable of all group items.
        /// </summary>
        public IEnumerable<List<TValue>> GetGroupItems()
        {
            return Groups.Values;
        }

        /// <summary>
        /// Returns an enumerable of all items.
        /// </summary>
        public IEnumerable<TValue> GetAllItems()
        {
            foreach (var group in Groups.Values)
            {
                foreach (var item in group)
                {
                    yield return item;
                }
            }
        }

        /// <summary>
        /// Returns the list corresponding to the group. If the group does not exist,
        /// creates it and returns a new list.
        /// </summary>
        /// <param name="group">The group.</param>
        public List<TValue> Get(TKey group)
        {
            if (!Groups.TryGetValue(group, out var list))
            {
                list = new List<TValue>();
                Groups.Add(group, list);
            }

            return list;
        }

        /// <summary>
        /// Adds the value to the group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="template">The value to add.</param>
        public virtual void Add(TKey group, TValue value)
        {
            Get(group).Add(value);
        }

        /// <summary>
        /// Adds the values to the group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="values">An enumerable of values to add.</param>
        public virtual void Add(TKey group, IEnumerable<TValue> values)
        {
            Get(group).AddRange(values);
        }
    }
}
