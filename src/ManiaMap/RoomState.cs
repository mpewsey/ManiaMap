using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Stores the state of a `Room` in a `Layout`.
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
        /// An array of room cell visibilities. True values indicate the cells are visible.
        /// </summary>
        [DataMember(Order = 2)]
        public Array2D<bool> Visibility { get; private set; }

        /// <summary>
        /// A set of flags that are set for a room.
        /// </summary>
        [DataMember(Order = 3)]
        public HashSet<int> Flags { get; private set; } = new HashSet<int>();

        /// <summary>
        /// Initializes from a room.
        /// </summary>
        /// <param name="room">The room.</param>
        public RoomState(Room room)
        {
            Id = room.Id;
            Visibility = new Array2D<bool>(room.Template.Cells.Rows, room.Template.Cells.Columns);
        }
    }
}
