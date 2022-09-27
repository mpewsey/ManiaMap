using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A container for a collectable ID and group name.
    /// </summary>
    [DataContract]
    public class CollectableEntry
    {
        /// <summary>
        /// The ID.
        /// </summary>
        [DataMember(Order = 1)]
        public int LocationId { get; set; }

        /// <summary>
        /// The group name.
        /// </summary>
        [DataMember(Order = 2)]
        public string Group { get; set; }

        /// <summary>
        /// Initializes a new collectable.
        /// </summary>
        /// <param name="locationId">The ID.</param>
        /// <param name="group">The group name.</param>
        public CollectableEntry(int locationId, string group)
        {
            LocationId = locationId;
            Group = group;
        }

        public override string ToString()
        {
            return $"CollectableEntry(LocationId = {LocationId}, Group = {Group})";
        }
    }
}
