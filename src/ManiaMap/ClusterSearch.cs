using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for searching for neighbors in a graph up to a specified depth.
    /// </summary>
    public class ClusterSearch
    {
        /// <summary>
        /// The maximum depth for which neighbors will be returned.
        /// </summary>
        private int MaxDepth { get; set; }

        /// <summary>
        /// A set of all room ID's that have been visited.
        /// </summary>
        private HashSet<Uid> Marked { get; set; }

        /// <summary>
        /// A dictionary of room neighbors by ID.
        /// </summary>
        private Dictionary<Uid, List<Uid>> Neighbors { get; set; }

        /// <summary>
        /// Returns a set of neighbors of the room up to the max depth.
        /// </summary>
        /// <param name="neighbors">A dictionary of room neighbors.</param>
        /// <param name="room">The room ID.</param>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        public HashSet<Uid> FindCluster(Dictionary<Uid, List<Uid>> neighbors, Uid room, int maxDepth)
        {
            MaxDepth = maxDepth;
            Neighbors = neighbors;
            Marked = new HashSet<Uid>();
            SearchNeighbors(room, 0);
            return Marked;
        }

        /// <summary>
        /// Returns a dictionary of all clusters up to the max depth.
        /// </summary>
        /// <param name="neighbors">A dictionary of room neighbors.</param>
        /// <param name="maxDepth">The maximum depth for which neighbors will be returned.</param>
        public Dictionary<Uid, HashSet<Uid>> FindClusters(Dictionary<Uid, List<Uid>> neighbors, int maxDepth)
        {
            MaxDepth = maxDepth;
            Neighbors = neighbors;
            Marked = new HashSet<Uid>();
            var dict = new Dictionary<Uid, HashSet<Uid>>(neighbors.Count);

            foreach (var room in neighbors.Keys)
            {
                Marked.Clear();
                SearchNeighbors(room, 0);
                dict.Add(room, new HashSet<Uid>(Marked));
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
