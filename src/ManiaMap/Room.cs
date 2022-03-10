using System.Drawing;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Represents a room in a `Layout`.
    /// </summary>
    [DataContract]
    public class Room
    {
        /// <summary>
        /// The room ID.
        /// </summary>
        [DataMember(Order = 0)]
        public Uid Id { get; private set; }

        /// <summary>
        /// The room name.
        /// </summary>
        [DataMember(Order = 1)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The position of the room.
        /// </summary>
        [DataMember(Order = 2)]
        public Vector3DInt Position { get; set; }

        /// <summary>
        /// The random seed of the room that may be used for room specific generation.
        /// </summary>
        [DataMember(Order = 5)]
        public int Seed { get; set; }

        /// <summary>
        /// The room color.
        /// </summary>
        [DataMember(Order = 6)]
        public Color Color { get; set; }

        /// <summary>
        /// The room template.
        /// </summary>
        [DataMember(Order = 7)]
        public RoomTemplate Template { get; private set; }

        /// <summary>
        /// Initializes a room from a room source.
        /// </summary>
        /// <param name="source">The room source.</param>
        /// <param name="position">The position in the layout.</param>
        /// <param name="seed">The random seed of the room that may be used for room specific generation.</param>
        /// <param name="template">The room template.</param>
        public Room(IRoomSource source, Vector2DInt position, int seed, RoomTemplate template)
        {
            Id = source.RoomId;
            Name = source.Name;
            Position = new Vector3DInt(position.X, position.Y, source.Z);
            Seed = seed;
            Color = source.Color;
            Template = template;
        }

        public override string ToString()
        {
            return $"Room(Id = {Id}, Name = {Name}, Position = {Position}, Template = {Template})";
        }
    }
}
