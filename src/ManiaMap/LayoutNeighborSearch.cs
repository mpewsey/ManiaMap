using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for searching for neighbors in a `Layout` up to a specified depth.
    /// </summary>
    public class LayoutNeighborSearch
    {
        /// <summary>
        /// The room layout.
        /// </summary>
        public Layout Layout { get; set; }

        /// <summary>
        /// The maximum depth for which neighbors will be returned.
        /// </summary>
        public int MaxDepth { get; set; }

        /// <summary>
        /// A dictionary of room neighbors by ID.
        /// </summary>
        private Dictionary<Uid, List<Uid>> Neighbors { get; set; }

        /// <summary>
        /// A set of all room ID's that have been visited.
        /// </summary>
        private HashSet<Uid> Marked { get; } = new HashSet<Uid>();

        /// <summary>
        /// Initializes the object.
        /// </summary>
        /// <param name="layout">The room layout.</param>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        public LayoutNeighborSearch(Layout layout, int maxDepth)
        {
            Layout = layout;
            MaxDepth = maxDepth;
        }

        /// <summary>
        /// Returns an array of neighbors of the room up to the max depth.
        /// </summary>
        /// <param name="room">The room ID.</param>
        public List<Uid> FindNeighbors(Uid room)
        {
            Marked.Clear();
            Neighbors = Layout.RoomAdjacencies();
            SearchNeighbors(room, 0);
            return Marked.ToList();
        }

        /// <summary>
        /// Recursively searches for neighbors of the room.
        /// </summary>
        /// <param name="room">The room ID.</param>
        /// <param name="depth">The current depth.</param>
        private void SearchNeighbors(Uid room, int depth)
        {
            if (depth <= MaxDepth && Marked.Add(room))
            {
                foreach (var neighbor in Neighbors[room])
                {
                    SearchNeighbors(neighbor, depth + 1);
                }
            }
        }
    }
}
