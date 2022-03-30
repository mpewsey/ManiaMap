using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for searching for neighbors in a `Layout` up to a specified depth.
    /// </summary>
    public static class LayoutClusterSearch
    {
        /// <summary>
        /// A construct for storing decomposer data.
        /// </summary>
        private class Data
        {
            /// <summary>
            /// The room layout.
            /// </summary>
            public Layout Layout { get; }

            /// <summary>
            /// The maximum depth for which neighbors will be returned.
            /// </summary>
            public int MaxDepth { get; }

            /// <summary>
            /// A dictionary of room neighbors by ID.
            /// </summary>
            public Dictionary<Uid, List<Uid>> Neighbors { get; }

            /// <summary>
            /// A set of all room ID's that have been visited.
            /// </summary>
            public HashSet<Uid> Marked { get; } = new HashSet<Uid>();

            /// <summary>
            /// Initializes the data.
            /// </summary>
            /// <param name="layout">The layout graph.</param>
            /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
            public Data(Layout layout, int maxDepth)
            {
                Layout = layout;
                MaxDepth = maxDepth;
                Neighbors = layout.RoomAdjacencies();
            }
        }

        /// <summary>
        /// Returns a list of neighbors of the room up to the max depth.
        /// </summary>
        /// <param name="layout">The room layout.</param>
        /// <param name="room">The room ID.</param>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        public static List<Uid> FindCluster(Layout layout, Uid room, int maxDepth)
        {
            var data = new Data(layout, maxDepth);
            SearchNeighbors(data, room, 0);
            return data.Marked.ToList();
        }

        /// <summary>
        /// Returns a dictionary of all clusters up to the max depth.
        /// </summary>
        /// <param name="layout">The room layout.</param>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        public static Dictionary<Uid, List<Uid>> FindClusters(Layout layout, int maxDepth)
        {
            var data = new Data(layout, maxDepth);
            var dict = new Dictionary<Uid, List<Uid>>(data.Layout.Rooms.Count);

            foreach (var room in data.Layout.Rooms.Keys)
            {
                data.Marked.Clear();
                SearchNeighbors(data, room, 0);
                dict.Add(room, data.Marked.ToList());
            }

            return dict;
        }

        /// <summary>
        /// Recursively searches for neighbors of the room.
        /// </summary>
        /// <param name="data">The decomposer data.</param>
        /// <param name="room">The room ID.</param>
        /// <param name="depth">The current depth.</param>
        private static void SearchNeighbors(Data data, Uid room, int depth)
        {
            if (depth <= data.MaxDepth && data.Marked.Add(room))
            {
                foreach (var neighbor in data.Neighbors[room])
                {
                    SearchNeighbors(data, neighbor, depth + 1);
                }
            }
        }
    }
}
