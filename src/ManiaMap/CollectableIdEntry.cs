using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A container for a collectable ID and location ID.
    /// </summary>
    [DataContract]
    public class CollectableIdEntry
    {
        /// <summary>
        /// The location ID.
        /// </summary>
        [DataMember(Order = 1)]
        public int LocationId { get; set; }

        /// <summary>
        /// The collectable ID.
        /// </summary>
        [DataMember(Order = 2)]
        public int CollectableId { get; set; }

        /// <summary>
        /// Initializes a new collectable entry.
        /// </summary>
        /// <param name="locationId">The location ID.</param>
        /// <param name="collectableId">The collectable ID.</param>
        public CollectableIdEntry(int locationId, int collectableId)
        {
            LocationId = locationId;
            CollectableId = collectableId;
        }

        public override string ToString()
        {
            return $"CollectableIdEntry(LocationId = {LocationId}, CollectableId = {CollectableId})";
        }
    }
}
