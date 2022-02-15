namespace MPewsey.ManiaMap
{
    public class Configuration
    {
        public int DX { get; }
        public int DY { get; }
        public Door FromDoor { get; }
        public Door ToDoor { get; }
        public EdgeDirection EdgeDirection { get; }

        public Configuration(int dx, int dy, Door from, Door to)
        {
            DX = dx;
            DY = dy;
            FromDoor = from;
            ToDoor = to;
            EdgeDirection = Door.GetEdgeDirection(from.Type, to.Type);
        }

        public override string ToString()
        {
            return $"Configuration(DX = {DX}, DY = {DY}, FromDoor = {FromDoor}, ToDoor = {ToDoor})";
        }
    }
}
