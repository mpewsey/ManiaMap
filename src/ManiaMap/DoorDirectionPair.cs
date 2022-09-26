using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A door and door direction pair.
    /// </summary>
    [DataContract]
    public class DoorDirectionPair
    {
        /// <summary>
        /// The door direction.
        /// </summary>
        [DataMember(Order = 1)]
        public DoorDirection Direction { get; set; }

        /// <summary>
        /// The door.
        /// </summary>
        [DataMember(Order = 2)]
        public Door Door { get; set; }

        /// <summary>
        /// Initializes a new pair.
        /// </summary>
        /// <param name="direction">The door direction.</param>
        /// <param name="door">The door.</param>
        public DoorDirectionPair(DoorDirection direction, Door door)
        {
            Direction = direction;
            Door = door;
        }

        public override string ToString()
        {
            return $"DoorDirectionPair(Direction = {Direction}, Door = {Door})";
        }
    }
}
