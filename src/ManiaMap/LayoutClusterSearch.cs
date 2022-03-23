using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Contains methods for searching for neighbors in a `Layout` up to a specified depth.
    /// </summary>
    public static class LayoutClusterSearch
    {
        /// <summary>
        /// Returns an array of neighbors of the room up to the max depth.
        /// </summary>
        /// <param name="layout">The room layout.</param>
        /// <param name="room">The room ID.</param>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        public static List<Uid> FindCluster(Layout layout, Uid room, int maxDepth)
        {
            var marked = new HashSet<Uid>();
            var neighbors = layout.RoomAdjacencies();
            SearchNeighbors(room, 0, maxDepth, marked, neighbors);
            return marked.ToList();
        }

        /// <summary>
        /// Recursively searches for neighbors of the room.
        /// </summary>
        /// <param name="room">The room ID.</param>
        /// <param name="depth">The current depth.</param>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        /// <param name="marked">A set of all room ID's that have been visited.</param>
        /// <param name="neighbors">A dictionary of room neighbors by ID.</param>
        private static void SearchNeighbors(Uid room, int depth, int maxDepth,
            HashSet<Uid> marked, Dictionary<Uid, List<Uid>> neighbors)
        {
            if (depth <= maxDepth && marked.Add(room))
            {
                foreach (var neighbor in neighbors[room])
                {
                    SearchNeighbors(neighbor, depth + 1, maxDepth, marked, neighbors);
                }
            }
        }
    }
}
