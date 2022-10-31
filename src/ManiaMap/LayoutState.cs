using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Contains the states of a Layout.
    /// </summary>
    [DataContract]
    public class LayoutState
    {
        /// <summary>
        /// The ID of the corresponding layout.
        /// </summary>
        [DataMember(Order = 0)]
        public int Id { get; private set; }

        /// <summary>
        /// A dictionary of room states by ID.
        /// </summary>
        public Dictionary<Uid, RoomState> RoomStates { get; private set; } = new Dictionary<Uid, RoomState>();

        /// <summary>
        /// An array of room states.
        /// </summary>
        [DataMember(Order = 1)]
        public RoomState[] RoomStatesArray
        {
            get => RoomStates.Values.ToArray();
            set => RoomStates = value.ToDictionary(x => x.Id, x => x);
        }

        /// <summary>
        /// Initializes an object from a layout.
        /// </summary>
        /// <param name="layout">The layout.</param>
        public LayoutState(Layout layout)
        {
            Id = layout.Id;
            RoomStates = layout.Rooms.Values.ToDictionary(x => x.Id, x => new RoomState(x));
        }
    }
}
