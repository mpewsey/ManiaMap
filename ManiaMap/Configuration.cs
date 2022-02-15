namespace MPewsey.ManiaMap
{
    public class Configuration
    {
        public int X { get; }
        public int Y { get; }
        public Door FromDoor { get; }
        public Door ToDoor { get; }
        public EdgeDirection EdgeDirection { get; }

        public Configuration(int dx, int dy, Door from, Door to)
        {
            X = dx;
            Y = dy;
            FromDoor = from;
            ToDoor = to;
            EdgeDirection = Door.GetEdgeDirection(from.Type, to.Type);
        }

        public override string ToString()
        {
            return $"Configuration(X = {X}, Y = {Y}, FromDoor = {FromDoor}, ToDoor = {ToDoor})";
        }
    }
}
