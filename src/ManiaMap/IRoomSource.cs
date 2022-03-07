using System.Collections.Generic;
using System.Drawing;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// An interface for objects that may be made into rooms.
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
        Color Color { get; set; }

        /// <summary>
        /// The z (layer) coordinate.
        /// </summary>
        int Z { get; set; }

        /// <summary>
        /// A list of template group names that may be used for the room.
        /// </summary>
        List<string> TemplateGroups { get; }
    }
}
