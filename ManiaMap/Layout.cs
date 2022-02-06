using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class Layout
    {
        public int Id { get; } = -1;
        public Dictionary<int, Room> Rooms { get; } = new();

        public Layout()
        {

        }

        public Layout(Layout baseLayout)
        {
            Id = baseLayout.Id + 1;
            Rooms = new(baseLayout.Rooms);
        }
    }
}
