using MPewsey.Common.Collections;
using MPewsey.ManiaMap.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for creating groups of collectables.
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class CollectableGroups
    {
        /// <summary>
        /// A dictionary of collectables by group name.
        /// </summary>
        [DataMember(Order = 1)]
        private DataContractDictionary<string, List<int>> Groups { get; set; } = new DataContractDictionary<string, List<int>>();

        /// <summary>
        /// A readonly dictionary of collectables by group name.
        /// </summary>
        public IReadOnlyDictionary<string, List<int>> GroupsDictionary => Groups;

        /// <inheritdoc/>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Groups = Groups ?? new DataContractDictionary<string, List<int>>();
        }

        /// <inheritdoc/>
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
            ValidateGroupName(group);

            if (!Groups.TryGetValue(group, out List<int> collectables))
            {
                collectables = new List<int>();
                Groups.Add(group, collectables);
            }

            collectables.Add(collectable);
        }

        /// <summary>
        /// Adds a range of collectables to a group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="collectables">The collectable ID's.</param>
        public void Add(string group, IEnumerable<int> collectables)
        {
            ValidateGroupName(group);

            if (!Groups.TryGetValue(group, out List<int> entries))
            {
                entries = new List<int>();
                Groups.Add(group, entries);
            }

            entries.AddRange(collectables);
        }

        /// <summary>
        /// Validates the specified group name and throws an exception if it is invalid.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <exception cref="InvalidNameException">Raised if the group name is invalid.</exception>
        private static void ValidateGroupName(string group)
        {
            if (string.IsNullOrWhiteSpace(group))
                throw new InvalidNameException($"Invalid group name: {group}.");
        }

        /// <summary>
        /// Returns a new list of collectables.
        /// </summary>
        public List<Collectable> GetCollectables()
        {
            var list = new List<Collectable>();

            foreach (var pair in Groups.OrderBy(x => x.Key))
            {
                foreach (var id in pair.Value)
                {
                    list.Add(new Collectable(id, pair.Key));
                }
            }

            return list;
        }
    }
}
