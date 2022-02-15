namespace MPewsey.ManiaMap
{
    public class Configuration
    {
        public int X { get; }
        public int Y { get; }
        public Door FromDoor { get; }
        public Door ToDoor { get; }
        public EdgeDirection EdgeDirection { get; }

        public Configuration(int x, int y, Door from, Door to)
        {
            X = x;
            Y = y;
            FromDoor = from;
            ToDoor = to;
            EdgeDirection = Door.GetEdgeDirection(from.Type, to.Type);
        }

        public override string ToString()
        {
            return $"Configuration(X = {X}, Y = {Y}, FromDoor = {FromDoor}, ToDoor = {ToDoor})";
        }

        /// <summary>
        /// Returns true if the parameters match the configuration.
        /// </summary>
        public bool Matches(EdgeDirection direction)
        {
            return EdgeDirection == direction;
        }

        /// <summary>
        /// Returns true if the parameters match the configuration.
        /// </summary>
        public bool Matches(int x, int y, EdgeDirection direction)
        {
            return X == x && Y == y && EdgeDirection == direction;
        }
    }
}
