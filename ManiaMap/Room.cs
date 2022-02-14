using System.Drawing;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract(IsReference = true)]
    public class Room
    {
        [DataMember(Order = 1)]
        public int Id { get; private set; }

        [DataMember(Order = 2)]
        public int X { get; private set; }

        [DataMember(Order = 3)]
        public int Y { get; private set; }

        [DataMember(Order = 4)]
        public RoomTemplate Template { get; private set; }

        [DataMember(Order = 5)]
        public Color Color { get; set; }

        public Room(int id, int x, int y, RoomTemplate template, Color? color = null)
        {
            Id = id;
            X = x;
            Y = y;
            Template = template;
            Color = color ?? Color.MidnightBlue;
        }

        public Room(LayoutNode node, int x, int y, RoomTemplate template)
        {
            Id = node.Id;
            Color = node.Color;
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
