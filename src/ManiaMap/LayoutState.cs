using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class LayoutState
    {
        [DataMember(Order = 0)]
        public int Id { get; private set; }

        [DataMember(Order = 1)]
        public Dictionary<Uid, RoomState> RoomStates { get; private set; } = new Dictionary<Uid, RoomState>();

        public LayoutState(Layout layout)
        {
            Id = layout.Id;
            RoomStates = layout.Rooms.Values.ToDictionary(x => x.Id, x => new RoomState(x));
        }
    }
}
