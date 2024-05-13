using MPewsey.Common.Collections;
using MPewsey.Common.Mathematics;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Represents a door connection between two Room.
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class DoorConnection : IValueHashMapEntry<RoomPair>
    {
        /// <summary>
        /// The from room ID.
        /// </summary>
        [DataMember(Order = 1)]
        public Uid FromRoom { get; private set; }

        /// <summary>
        /// The to room ID.
        /// </summary>
        [DataMember(Order = 2)]
        public Uid ToRoom { get; private set; }

        /// <summary>
        /// The from door position.
        /// </summary>
        [DataMember(Order = 3)]
        public DoorPosition FromDoor { get; private set; }

        /// <summary>
        /// The to door position.
        /// </summary>
        [DataMember(Order = 4)]
        public DoorPosition ToDoor { get; private set; }

        /// <summary>
        /// The shaft (if any) connecting the two rooms.
        /// </summary>
        [DataMember(Order = 5)]
        public Box Shaft { get; private set; }

        /// <inheritdoc/>
        RoomPair IValueHashMapEntry<RoomPair>.Key => new RoomPair(FromRoom, ToRoom);

        /// <summary>
        /// The edge direction.
        /// </summary>
        public EdgeDirection EdgeDirection => Door.GetEdgeDirection(FromDoor.Door.Type, ToDoor.Door.Type);

        /// <summary>
        /// Initializes a door connection from two rooms, two door positions, and an optional shaft.
        /// </summary>
        /// <param name="fromRoom">The from room.</param>
        /// <param name="toRoom">The to room.</param>
        /// <param name="fromDoor">The from door position.</param>
        /// <param name="toDoor">The to door position.</param>
        /// <param name="shaft">The shaft connecting the two rooms.</param>
        public DoorConnection(Room fromRoom, Room toRoom, DoorPosition fromDoor, DoorPosition toDoor, Box shaft = null)
        {
            FromRoom = fromRoom.Id;
            ToRoom = toRoom.Id;
            FromDoor = fromDoor;
            ToDoor = toDoor;
            Shaft = shaft;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var shaft = Shaft?.ToString() ?? "None";
            return $"DoorConnection(FromRoom = {FromRoom}, ToRoom = {ToRoom}, FromDoor = {FromDoor}, ToDoor = {ToDoor}, Shaft = {shaft})";
        }

        /// <summary>
        /// Returns true if the connection includes the room door.
        /// </summary>
        /// <param name="roomId">The room ID.</param>
        /// <param name="position">The door position.</param>
        /// <param name="direction">The door direction.</param>
        public bool ContainsDoor(Uid roomId, Vector2DInt position, DoorDirection direction)
        {
            return (FromRoom == roomId && FromDoor.Matches(position, direction))
                || (ToRoom == roomId && ToDoor.Matches(position, direction));
        }

        /// <summary>
        /// Returns the door position corresponding to the room ID.
        /// Returns null if the door connection does not contain the room ID.
        /// </summary>
        /// <param name="roomId">The room ID.</param>
        public DoorPosition GetDoorPosition(Uid roomId)
        {
            if (roomId == FromRoom)
                return FromDoor;
            if (roomId == ToRoom)
                return ToDoor;
            return null;
        }

        /// <summary>
        /// Returns the ID of the connecting room, e.g. the "to room" if the "from room" ID is specified and vice versa.
        /// If the specified room ID does not exist in the connection, returns Uid(-1, -1, -1).
        /// </summary>
        /// <param name="roomId">The room ID.</param>
        public Uid GetConnectingRoom(Uid roomId)
        {
            if (roomId == FromRoom)
                return ToRoom;
            if (roomId == ToRoom)
                return FromRoom;
            return new Uid(-1, -1, -1);
        }

        /// <summary>
        /// Returns true if the door connections "from room" or "to room" matches the specified room ID.
        /// </summary>
        /// <param name="roomId">The room ID.</param>
        public bool ContainsRoom(Uid roomId)
        {
            return roomId == FromRoom || roomId == ToRoom;
        }
    }
}
