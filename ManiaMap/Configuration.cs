namespace MPewsey.ManiaMap
{
    public class Configuration
    {
        public int X { get; }
        public int Y { get; }
        public DoorPosition FromDoor { get; }
        public DoorPosition ToDoor { get; }
        public EdgeDirection EdgeDirection { get; }

        public Configuration(int x, int y, DoorPosition from, DoorPosition to)
        {
            X = x;
            Y = y;
            FromDoor = from;
            ToDoor = to;
            EdgeDirection = Door.GetEdgeDirection(from.Door.Type, to.Door.Type);
        }

        public override string ToString()
        {
            return $"Configuration(X = {X}, Y = {Y}, FromDoor = {FromDoor}, ToDoor = {ToDoor})";
        }

        /// <summary>
        /// Returns true if the parameters match the configuration.
        /// </summary>
        public bool Matches(int z)
        {
            if (z > 0)
            {
                return FromDoor.Direction == DoorDirection.Top
                    && ToDoor.Direction == DoorDirection.Bottom;
            }

            if (z < 0)
            {
                return FromDoor.Direction == DoorDirection.Bottom
                    && ToDoor.Direction == DoorDirection.Top;
            }

            return true;
        }

        /// <summary>
        /// Returns true if the parameters match the configuration.
        /// </summary>
        public bool Matches(int z, LayoutEdge edge)
        {
            return EdgeDirection == edge.Direction
                && FromDoor.Door.Code == edge.DoorCode
                && ToDoor.Door.Code == edge.DoorCode
                && Matches(z);
        }

        /// <summary>
        /// Returns true if the parameters match the configuration.
        /// </summary>
        public bool Matches(int x, int y, int z, LayoutEdge edge)
        {
            return X == x
                && Y == y
                && EdgeDirection == edge.Direction
                && FromDoor.Door.Code == edge.DoorCode
                && ToDoor.Door.Code == edge.DoorCode
                && Matches(z);
        }
    }
}
