using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class Room
    {
        public int Id { get; }
        public int X { get; }
        public int Y { get; }
        public RoomTemplate Template { get; }

        public Room(int id, int x, int y, RoomTemplate template)
        {
            Id = id;
            X = x;
            Y = y;
            Template = template;
        }

        public override string ToString()
        {
            return $"Room(Id = {Id}, X = {X}, Y = {Y}, Template = {Template})";
        }
    }
}
