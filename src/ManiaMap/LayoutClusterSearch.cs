using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for searching for neighbors in a `Layout` up to a specified depth.
    /// </summary>
    public class LayoutClusterSearch
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
        /// Initializes a new layout cluster search.
        /// </summary>
        /// <param name="layout">The room layout.</param>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        public LayoutClusterSearch(Layout layout, int maxDepth)
        {
            Layout = layout;
            MaxDepth = maxDepth;
        }

        /// <summary>
        /// Returns a list of neighbors of the room up to the max depth.
        /// </summary>
        /// <param name="room">The room ID.</param>
        public List<Uid> FindCluster(Uid room)
        {
            Neighbors = Layout.RoomAdjacencies();
            SearchNeighbors(room, 0);
            var result = Marked.ToList();
            Marked.Clear();
            return result;
        }

        /// <summary>
        /// Returns a dictionary of all clusters up to the max depth.
        /// </summary>
        public Dictionary<Uid, List<Uid>> FindClusters()
        {
            Neighbors = Layout.RoomAdjacencies();
            var dict = new Dictionary<Uid, List<Uid>>(Layout.Rooms.Count);

            foreach (var room in Layout.Rooms.Keys)
            {
                SearchNeighbors(room, 0);
                dict.Add(room, Marked.ToList());
                Marked.Clear();
            }

            return dict;
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
