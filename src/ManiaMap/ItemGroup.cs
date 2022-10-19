using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// An item group consisting of a group ID and a list of items.
    /// </summary>
    /// <typeparam name="TKey">The group key type.</typeparam>
    /// <typeparam name="TValue">The item value type.</typeparam>
    [DataContract]
    public class ItemGroup<TKey, TValue>
    {
        /// <summary>
        /// The group ID.
        /// </summary>
        [DataMember(Order = 1)]
        public TKey GroupId { get; set; }

        /// <summary>
        /// A list of items.
        /// </summary>
        [DataMember(Order = 2)]
        public List<TValue> Items { get; set; }

        /// <summary>
        /// Initializes a new item group.
        /// </summary>
        /// <param name="groupId">The group ID.</param>
        /// <param name="items">A list of items.</param>
        public ItemGroup(TKey groupId, List<TValue> items)
        {
            GroupId = groupId;
            Items = items;
        }
    }
}
