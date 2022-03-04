using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class DoorPosition
    {
        [DataMember(Order = 1)]
        public int X { get; private set; }

        [DataMember(Order = 2)]
        public int Y { get; private set; }

        [DataMember(Order = 3)]
        public DoorDirection Direction { get; private set; }

        [DataMember(Order = 4)]
        public Door Door { get; set; }

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
        public bool Matches(int x, int y, DoorDirection direction)
        {
            return X == x && Y == y && Direction == direction;
        }
    }
}
