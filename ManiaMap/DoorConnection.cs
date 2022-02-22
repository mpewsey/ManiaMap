using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class DoorConnection
    {
        [DataMember(Order = 1)]
        public int FromRoom { get; private set; }

        [DataMember(Order = 2)]
        public int ToRoom { get; private set; }

        [DataMember(Order = 3)]
        public DoorPosition FromDoor { get; private set; }

        [DataMember(Order = 4)]
        public DoorPosition ToDoor { get; private set; }

        [DataMember(Order = 5)]
        public Box Shaft { get; private set; }

        public DoorConnection(int fromRoom, int toRoom, DoorPosition fromDoor, DoorPosition toDoor, Box shaft = null)
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
