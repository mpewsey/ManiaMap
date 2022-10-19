using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A container for a variation group and its member ID's.
    /// </summary>
    [DataContract]
    public class NodeVariations
    {
        /// <summary>
        /// The variation group name.
        /// </summary>
        [DataMember(Order = 1)]
        public string GroupName { get; set; }

        /// <summary>
        /// A list of member ID's in the variation.
        /// </summary>
        [DataMember(Order = 2)]
        public List<int> Variations { get; set; }

        /// <summary>
        /// Initializes a new object.
        /// </summary>
        /// <param name="groupName">The variation group name.</param>
        /// <param name="variations">A list of member ID's in the variation.</param>
        public NodeVariations(string groupName, List<int> variations)
        {
            GroupName = groupName;
            Variations = variations;
        }
    }
}
