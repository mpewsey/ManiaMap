using MPewsey.ManiaMap.Collections;
using MPewsey.ManiaMap.Exceptions;
using MPewsey.ManiaMap.Serialization;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for creating groups of RoomTemplate.
    /// </summary>
    [DataContract(Namespace = XmlSerialization.Namespace)]
    public class TemplateGroups
    {
        /// <summary>
        /// A dictionary of templates by group name.
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        private DataContractDictionary<string, List<RoomTemplate>> Groups { get; set; } = new DataContractDictionary<string, List<RoomTemplate>>();

        /// <summary>
        /// A readonly dictionary of templates by group name.
        /// </summary>
        public IReadOnlyDictionary<string, List<RoomTemplate>> GroupsDictionary => Groups;

        public override string ToString()
        {
            return $"TemplateGroups(Groups.Count = {Groups.Count})";
        }

        /// <summary>
        /// Adds a room template to the group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="template">The room template.</param>
        public void Add(string group, RoomTemplate template)
        {
            ValidateGroupName(group);

            if (!Groups.TryGetValue(group, out List<RoomTemplate> templates))
            {
                templates = new List<RoomTemplate>();
                Groups.Add(group, templates);
            }

            if (!templates.Contains(template))
                templates.Add(template);
        }

        /// <summary>
        /// Adds a range of room templates to the group.
        /// </summary>
        /// <param name="group">The </param>
        /// <param name="values"></param>
        public void Add(string group, IEnumerable<RoomTemplate> templates)
        {
            ValidateGroupName(group);

            if (!Groups.TryGetValue(group, out List<RoomTemplate> entries))
            {
                entries = new List<RoomTemplate>();
                Groups.Add(group, entries);
            }

            foreach (var template in templates)
            {
                if (!entries.Contains(template))
                    entries.Add(template);
            }
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
            foreach (var template in GetAllTemplates())
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

            foreach (var from in GetAllTemplates())
            {
                foreach (var to in GetAllTemplates())
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

        /// <summary>
        /// Returns an enumerable of all templates in all groups.
        /// </summary>
        public IEnumerable<RoomTemplate> GetAllTemplates()
        {
            foreach (var pair in Groups.OrderBy(x => x.Key))
            {
                foreach (var template in pair.Value)
                {
                    yield return template;
                }
            }
        }
    }
}
