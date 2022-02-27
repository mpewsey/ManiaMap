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
        public bool Matches(int z, int code, EdgeDirection direction)
        {
            return EdgeDirection == direction
                && FromDoor.Door.Code == code
                && ToDoor.Door.Code == code
                && Matches(z);
        }

        /// <summary>
        /// Returns true if the parameters match the configuration.
        /// </summary>
        public bool Matches(int x, int y, int z, int code, EdgeDirection direction)
        {
            return X == x
                && Y == y
                && EdgeDirection == direction
                && FromDoor.Door.Code == code
                && ToDoor.Door.Code == code
                && Matches(z);
        }
    }
}
