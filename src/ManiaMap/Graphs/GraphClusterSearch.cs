using System.Collections.Generic;

namespace MPewsey.ManiaMap.Graphs
{
    /// <summary>
    /// A class for searching for neighbors in a graph up to a specified depth.
    /// </summary>
    /// <typeparam name="T">The type of the node ID.</typeparam>
    public class GraphClusterSearch<T>
    {
        /// <summary>
        /// The maximum depth for which neighbors will be returned.
        /// </summary>
        private int MaxDepth { get; set; }

        /// <summary>
        /// A set of all room ID's that have been visited.
        /// </summary>
        private HashSet<T> Marked { get; set; }

        /// <summary>
        /// A dictionary of room neighbors by ID.
        /// </summary>
        private Dictionary<T, List<T>> Neighbors { get; set; }

        /// <summary>
        /// Initializes the search buffers.
        /// </summary>
        /// <param name="neighbors">A dictionary of room neighbors.</param>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        private void Initialize(Dictionary<T, List<T>> neighbors, int maxDepth)
        {
            MaxDepth = maxDepth;
            Neighbors = neighbors;
            Marked = new HashSet<T>();
        }

        /// <summary>
        /// Returns a set of neighbors of the room up to the max depth.
        /// </summary>
        /// <param name="neighbors">A dictionary of room neighbors.</param>
        /// <param name="room">The room ID.</param>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        public HashSet<T> FindCluster(Dictionary<T, List<T>> neighbors, T room, int maxDepth)
        {
            Initialize(neighbors, maxDepth);
            SearchNeighbors(room, 0);
            return Marked;
        }

        /// <summary>
        /// Returns a dictionary of all clusters up to the max depth.
        /// </summary>
        /// <param name="neighbors">A dictionary of room neighbors.</param>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        public Dictionary<T, HashSet<T>> FindClusters(Dictionary<T, List<T>> neighbors, int maxDepth)
        {
            Initialize(neighbors, maxDepth);
            var dict = new Dictionary<T, HashSet<T>>(neighbors.Count);

            foreach (var room in neighbors.Keys)
            {
                Marked.Clear();
                SearchNeighbors(room, 0);
                dict.Add(room, new HashSet<T>(Marked));
            }

            return dict;
        }

        /// <summary>
        /// Recursively searches for neighbors of the room.
        /// </summary>
        /// <param name="room">The room ID.</param>
        /// <param name="depth">The current depth.</param>
        private void SearchNeighbors(T room, int depth)
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
