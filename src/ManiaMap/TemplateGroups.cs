using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for creating groups of RoomTemplate.
    /// </summary>
    [DataContract]
    public class TemplateGroups : ItemGroups<string, RoomTemplate>
    {
        public override string ToString()
        {
            return $"TemplateGroups(Groups.Count = {Groups.Count})";
        }

        /// <summary>
        /// Returns an enumerable of all room templates.
        /// </summary>
        public IEnumerable<RoomTemplate> AllTemplates()
        {
            foreach (var group in Groups.Values)
            {
                foreach (var template in group)
                {
                    yield return template;
                }
            }
        }

        /// <summary>
        /// Returns a new list of room templates for the specified group.
        /// </summary>
        /// <param name="group">The template group name.</param>
        public List<RoomTemplate> GetTemplates(string group)
        {
            var templates = new List<RoomTemplate>();

            if (!string.IsNullOrEmpty(group) && Groups.TryGetValue(group, out var list))
            {
                templates.AddRange(list);
            }

            return templates;
        }

        /// <summary>
        /// Returns a new dictionary of configuration spaces for all templates.
        /// </summary>
        public Dictionary<TemplatePair, ConfigurationSpace> GetConfigurationSpaces()
        {
            var spaces = new Dictionary<TemplatePair, ConfigurationSpace>();

            foreach (var from in AllTemplates())
            {
                foreach (var to in AllTemplates())
                {
                    var pair = new TemplatePair(from, to);

                    if (!spaces.ContainsKey(pair))
                    {
                        spaces.Add(pair, new ConfigurationSpace(from, to));
                    }
                }
            }

            return spaces;
        }
    }
}
