using System.Drawing;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class Room
    {
        [DataMember(Order = 0)]
        public Uid Id { get; private set; }

        [DataMember(Order = 1)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public int X { get; private set; }

        [DataMember(Order = 3)]
        public int Y { get; private set; }

        [DataMember(Order = 4)]
        public int Z { get; private set; }

        [DataMember(Order = 5)]
        public int Seed { get; set; }

        [DataMember(Order = 6)]
        public Color Color { get; set; }

        [DataMember(Order = 7)]
        public RoomTemplate Template { get; private set; }

        public Room(IRoomSource source, int x, int y, int seed, RoomTemplate template)
        {
            Id = source.RoomId;
            Name = source.Name;
            X = x;
            Y = y;
            Z = source.Z;
            Seed = seed;
            Color = source.Color;
            Template = template;
        }

        public override string ToString()
        {
            return $"Room(Id = {Id}, Name = {Name}, X = {X}, Y = {Y}, Z = {Z}, Template = {Template})";
        }
    }
}
