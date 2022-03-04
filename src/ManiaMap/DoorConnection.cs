using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Represents a door connection between two rooms.
    /// </summary>
    [DataContract]
    public class DoorConnection
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

        public override string ToString()
        {
            var shaft = Shaft?.ToString() ?? "None";
            return $"DoorConnection(FromRoom = {FromRoom}, ToRoom = {ToRoom}, FromDoor = {FromDoor}, ToDoor = {ToDoor}, Shaft = {shaft})";
        }
    }
}
