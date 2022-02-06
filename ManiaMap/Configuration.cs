using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class Configuration
    {
        public int X { get; }
        public int Y { get; }
        public Door FromDoor { get; }
        public Door ToDoor { get; }

        public Configuration(int x, int y, Door from, Door to)
        {
            X = x;
            Y = y;
            FromDoor = from;
            ToDoor = to;
        }

        public override string ToString()
        {
            return $"Configuration(X = {X}, Y = {Y}, FromDoor = {FromDoor}, ToDoor = {ToDoor})";
        }

        public EdgeDirection EdgeDirection()
        {
            return Door.GetEdgeDirection(FromDoor.Type, ToDoor.Type);
        }
    }
}
