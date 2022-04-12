using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A base class for implementing groups of items.
    /// </summary>
    /// <typeparam name="TKey">The group key type.</typeparam>
    /// <typeparam name="TValue">The item value type.</typeparam>
    [DataContract]
    public abstract class ItemGroups<TKey, TValue>
    {
        /// <summary>
        /// A dictionary of items by group.
        /// </summary>
        [DataMember(Order = 1)]
        public Dictionary<TKey, List<TValue>> Groups { get; private set; } = new Dictionary<TKey, List<TValue>>();

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
        public void Add(TKey group, TValue value)
        {
            Get(group).Add(value);
        }

        /// <summary>
        /// Adds the values to the group.
        /// </summary>
        /// <param name="group">The group.</param>
        /// <param name="templates">An enumerable of values to add.</param>
        public void Add(TKey group, IEnumerable<TValue> templates)
        {
            Get(group).AddRange(templates);
        }
    }
}
