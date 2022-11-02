using MPewsey.ManiaMap.Exceptions;
using MPewsey.ManiaMap.Serialization;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for creating groups of RoomTemplate.
    /// </summary>
    [DataContract(Namespace = XmlSerialization.Namespace)]
    public class TemplateGroups : ItemGroups<string, RoomTemplate>
    {
        public override string ToString()
        {
            return $"TemplateGroups(Groups.Count = {Groups.Count})";
        }

        /// <inheritdoc/>
        public override void Add(string group, RoomTemplate value)
        {
            ValidateGroupName(group);
            base.Add(group, value);
        }

        /// <inheritdoc/>
        public override void Add(string group, IEnumerable<RoomTemplate> values)
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
        /// Validates the template groups and raises any associated exceptions.
        /// </summary>
        public void Validate()
        {
            foreach (var template in GetAllItems())
            {
                template.Validate();
            }
        }

        /// <summary>
        /// Returns a new list of room templates for the specified group.
        /// </summary>
        /// <param name="group">The template group name.</param>
        public List<RoomTemplate> GetTemplates(string group)
        {
            var templates = new List<RoomTemplate>();

            if (!string.IsNullOrWhiteSpace(group) && Groups.TryGetValue(group, out var list))
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

            foreach (var from in GetAllItems())
            {
                foreach (var to in GetAllItems())
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
