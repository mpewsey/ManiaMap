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
