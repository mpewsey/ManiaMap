using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for searching for neighbors in a `Layout` up to a specified depth.
    /// </summary>
    public class LayoutClusterSearch
    {
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
        private HashSet<Uid> Marked { get; set; }

        /// <summary>
        /// Initializes a new layout cluster search.
        /// </summary>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        public LayoutClusterSearch(int maxDepth)
        {
            MaxDepth = maxDepth;
        }

        /// <summary>
        /// Returns an array of neighbors of the room up to the max depth.
        /// </summary>
        /// <param name="layout">The room layout.</param>
        /// <param name="room">The room ID.</param>
        public HashSet<Uid> FindCluster(Layout layout, Uid room)
        {
            Marked = new HashSet<Uid>();
            Neighbors = layout.RoomAdjacencies();
            SearchNeighbors(room, 0);
            return Marked;
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
