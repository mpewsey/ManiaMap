using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class referencing a `Door` and its local position within a `Room`.
    /// </summary>
    [DataContract]
    public class DoorPosition
    {
        /// <summary>
        /// The local x coordinate.
        /// </summary>
        [DataMember(Order = 1)]
        public int X { get; private set; }

        /// <summary>
        /// The local y coordinate.
        /// </summary>
        [DataMember(Order = 2)]
        public int Y { get; private set; }

        /// <summary>
        /// The door direction.
        /// </summary>
        [DataMember(Order = 3)]
        public DoorDirection Direction { get; private set; }

        /// <summary>
        /// The referenced door.
        /// </summary>
        [DataMember(Order = 4)]
        public Door Door { get; set; }

        /// <summary>
        /// Initializes an object.
        /// </summary>
        /// <param name="x">The local x coordinate.</param>
        /// <param name="y">The local y coordinate.</param>
        /// <param name="direction">The door direction.</param>
        /// <param name="door">The referenced door.</param>
        public DoorPosition(int x, int y, DoorDirection direction, Door door)
        {
            X = x;
            Y = y;
            Direction = direction;
            Door = door;
        }

        public override string ToString()
        {
            return $"DoorPosition(X = {X}, Y = {Y}, Direction = {Direction}, Door = {Door})";
        }

        /// <summary>
        /// Returns true if the door matches the specified properties.
        /// </summary>
        /// <param name="x">The local x coordinate.</param>
        /// <param name="y">The local y coordinate.</param>
        /// <param name="direction">The door direction.</param>
        public bool Matches(int x, int y, DoorDirection direction)
        {
            return X == x && Y == y && Direction == direction;
        }
    }
}
