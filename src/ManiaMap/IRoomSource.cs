using System.Collections.Generic;

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
        string Name { get; }

        /// <summary>
        /// The color of the room background tiles.
        /// </summary>
        Color4 Color { get; }

        /// <summary>
        /// The z (layer) coordinate.
        /// </summary>
        int Z { get; }

        /// <summary>
        /// The template group name.
        /// </summary>
        string TemplateGroup { get; }

        /// <summary>
        /// A list of tags.
        /// </summary>
        List<string> Tags { get; }
    }
}
