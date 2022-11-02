using MPewsey.ManiaMap.Collections;
using MPewsey.ManiaMap.Serialization;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Contains the states of a Layout.
    /// </summary>
    [DataContract(Namespace = XmlSerialization.Namespace)]
    public class LayoutState
    {
        /// <summary>
        /// The ID of the corresponding layout.
        /// </summary>
        [DataMember(Order = 0, IsRequired = true)]
        public int Id { get; private set; }

        /// <summary>
        /// A dictionary of room states by ID.
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public DataContractValueDictionary<Uid, RoomState> RoomStates { get; private set; } = new DataContractValueDictionary<Uid, RoomState>();

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
