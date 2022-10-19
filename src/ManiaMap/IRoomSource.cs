namespace MPewsey.ManiaMap
{
    /// <summary>
    /// An interface for objects that may be made into Room.
    /// </summary>
    public interface IRoomSource
    {
        /// <summary>
        /// The unique room ID.
        /// </summary>
        Uid RoomId { get; }

        /// <summary>
        /// The name of the room.
        /// </summary>
        string Name { get; set; }

        /// <summary>
        /// The color of the room background tiles.
        /// </summary>
        Color4 Color { get; set; }

        /// <summary>
        /// The z (layer) coordinate.
        /// </summary>
        int Z { get; set; }

        /// <summary>
        /// The template group name.
        /// </summary>
        string TemplateGroup { get; }
    }
}
