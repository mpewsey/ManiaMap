using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for creating groups of collectables.
    /// </summary>
    [DataContract]
    public class CollectableGroups
    {
        /// <summary>
        /// A dictionary of collectable ID's by group name.
        /// </summary>
        [DataMember(Order = 1)]
        public Dictionary<string, List<int>> Groups { get; private set; } = new Dictionary<string, List<int>>();

        public override string ToString()
        {
            return $"CollectableGroups(Groups.Count = {Groups.Count})";
        }

        /// <summary>
        /// Adds a collectable to a group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="collectable">The collectable ID.</param>
        public void Add(string group, int collectable)
        {
            if (!Groups.TryGetValue(group, out var list))
            {
                list = new List<int>();
                Groups.Add(group, list);
            }

            list.Add(collectable);
        }

        /// <summary>
        /// Adds collectables to a group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="collectables">The collectable ID's.</param>
        public void Add(string group, params int[] collectables)
        {
            if (!Groups.TryGetValue(group, out var list))
            {
                list = new List<int>();
                Groups.Add(group, list);
            }

            list.AddRange(collectables);
        }

        /// <summary>
        /// Adds collectables to a group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="collectables">The collectable ID's.</param>
        public void Add(string group, IEnumerable<int> collectables)
        {
            if (!Groups.TryGetValue(group, out var list))
            {
                list = new List<int>();
                Groups.Add(group, list);
            }

            list.AddRange(collectables);
        }

        /// <summary>
        /// Adds collectables to a group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="collectables">The collectable ID's.</param>
        public void Add(string group, params IEnumerable<int>[] collectables)
        {
            if (!Groups.TryGetValue(group, out var list))
            {
                list = new List<int>();
                Groups.Add(group, list);
            }

            foreach (var collection in collectables)
            {
                list.AddRange(collection);
            }
        }
    }
}
