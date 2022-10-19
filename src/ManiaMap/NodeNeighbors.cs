using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A container for a node ID and a list of neighbor node ID's.
    /// </summary>
    [DataContract]
    public class NodeNeighbors
    {
        /// <summary>
        /// The node ID.
        /// </summary>
        [DataMember(Order = 1)]
        public int Id { get; set; }

        /// <summary>
        /// A list of neighbor ID's.
        /// </summary>
        public List<int> Neighbors { get; set; }

        /// <summary>
        /// An enumerable of neighbor ID's.
        /// </summary>
        [DataMember(Order = 2)]
        protected IEnumerable<int> NeighborIds
        {
            get => Neighbors;
            set => Neighbors = new List<int>(value);
        }

        /// <summary>
        /// Initializes a new object.
        /// </summary>
        /// <param name="id">The node ID.</param>
        /// <param name="neighbors">A list of neighbor ID's.</param>
        public NodeNeighbors(int id, List<int> neighbors)
        {
            Id = id;
            Neighbors = neighbors;
        }
    }
}
