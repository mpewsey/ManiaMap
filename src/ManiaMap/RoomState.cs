using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Stores the state of a Room in a Layout.
    /// </summary>
    [DataContract]
    public class RoomState
    {
        /// <summary>
        /// The ID of the corresponding room.
        /// </summary>
        [DataMember(Order = 1)]
        public Uid Id { get; private set; }

        /// <summary>
        /// A set of visible room cell indexes.
        /// </summary>
        [DataMember(Order = 2)]
        public HashSet<Vector2DInt> VisibleIndexes { get; private set; } = new HashSet<Vector2DInt>();

        /// <summary>
        /// A set of acquired collectable location ID's.
        /// </summary>
        [DataMember(Order = 3)]
        public HashSet<int> AcquiredCollectables { get; private set; } = new HashSet<int>();

        /// <summary>
        /// A set of flags that are set for a room.
        /// </summary>
        [DataMember(Order = 4)]
        public HashSet<int> Flags { get; private set; } = new HashSet<int>();

        /// <summary>
        /// Initializes from a room.
        /// </summary>
        /// <param name="room">The room.</param>
        public RoomState(Room room)
        {
            Id = room.Id;
        }
    }
}
