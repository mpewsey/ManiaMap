using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for creating groups of collectables.
    /// </summary>
    [DataContract]
    public class CollectableGroups : ItemGroups<string, int>
    {
        public override string ToString()
        {
            return $"CollectableGroups(Groups.Count = {Groups.Count})";
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
                    list.Add(new Collectable(pair.Key, id));
                }
            }

            return list;
        }
    }
}
