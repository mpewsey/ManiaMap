using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class DoorConnection
    {
        [DataMember(Order = 1)]
        public Room FromRoom { get; private set; }

        [DataMember(Order = 2)]
        public Room ToRoom { get; private set; }

        [DataMember(Order = 3)]
        public Door FromDoor { get; private set; }

        [DataMember(Order = 4)]
        public Door ToDoor { get; private set; }

        [DataMember(Order = 5)]
        public Shaft Shaft { get; private set; }

        public DoorConnection(Room fromRoom, Room toRoom, Door fromDoor, Door toDoor, Shaft shaft = null)
        {
            FromRoom = fromRoom;
            ToRoom = toRoom;
            FromDoor = fromDoor;
            ToDoor = toDoor;
            Shaft = shaft;
        }

        public override string ToString()
        {
            var shaft = Shaft == null ? "None" : Shaft.ToString();
            return $"DoorConnection(FromRoom = {FromRoom}, ToRoom = {ToRoom}, FromDoor = {FromDoor}, ToDoor = {ToDoor}, Shaft = {shaft})";
        }
    }
}
