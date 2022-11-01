using MPewsey.ManiaMap.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for creating groups of collectables.
    /// </summary>
    [DataContract(Namespace = Serialization.Namespace)]
    public class CollectableGroups : ItemGroups<string, int>
    {
        public override string ToString()
        {
            return $"CollectableGroups(Groups.Count = {Groups.Count})";
        }

        /// <inheritdoc/>
        public override void Add(string group, int value)
        {
            ValidateGroupName(group);
            base.Add(group, value);
        }

        /// <inheritdoc/>
        public override void Add(string group, IEnumerable<int> values)
        {
            ValidateGroupName(group);
            base.Add(group, values);
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
