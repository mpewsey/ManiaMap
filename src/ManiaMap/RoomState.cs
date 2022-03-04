using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class RoomState
    {
        [DataMember(Order = 1)]
        public Uid Id { get; private set; }

        [DataMember(Order = 2)]
        public Array2D<bool> Visibility { get; private set; }

        public RoomState(Room room)
        {
            Id = room.Id;
            Visibility = new Array2D<bool>(room.Template.Cells.Rows, room.Template.Cells.Columns);
        }
    }
}
