using MPewsey.Common.Collections;
using MPewsey.ManiaMap.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for creating groups of RoomTemplate.
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class TemplateGroups
    {
        /// <summary>
        /// A dictionary of templates by group name.
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        private DataContractDictionary<string, List<TemplateGroupsEntry>> Groups { get; set; } = new DataContractDictionary<string, List<TemplateGroupsEntry>>();

        /// <summary>
        /// A readonly dictionary of templates by group name.
        /// </summary>
        public IReadOnlyDictionary<string, List<TemplateGroupsEntry>> GroupsDictionary => Groups;

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            ConsolidateTemplates();
        }

        public override string ToString()
        {
            return $"TemplateGroups(Groups.Count = {Groups.Count})";
        }

        /// <summary>
        /// Consolidates duplicate templates into a single reference.
        /// </summary>
        private void ConsolidateTemplates()
        {
            var entries = GetAllEntries();

            for (int i = 0; i < entries.Count; i++)
            {
                for (int j = i + 1; j < entries.Count; j++)
                {
                    entries[j].ConsolidateTemplate(entries[i].Template);
                }
            }
        }

        /// <summary>
        /// Returns the group entries for the specified group name.
        /// </summary>
        /// <param name="group">The group name.</param>
        public List<TemplateGroupsEntry> GetGroup(string group)
        {
            ValidateGroupName(group);
            return Groups[group];
        }

        /// <summary>
        /// Adds a room template to the group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="template">The room template.</param>
        public void Add(string group, RoomTemplate template)
        {
            Add(group, new TemplateGroupsEntry(template));
        }

        /// <summary>
        /// Adds a template group entry to the group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="entry">The group entry.</param>
        public void Add(string group, TemplateGroupsEntry entry)
        {
            ValidateGroupName(group);

            if (!Groups.TryGetValue(group, out List<TemplateGroupsEntry> entries))
            {
                entries = new List<TemplateGroupsEntry>();
                Groups.Add(group, entries);
            }

            entries.Add(entry);
        }

        /// <summary>
        /// Adds a range of room templates to the group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="templates">An enumerable of templates.</param>
        public void Add(string group, IEnumerable<RoomTemplate> templates)
        {
            Add(group, templates.Select(x => new TemplateGroupsEntry(x)));
        }

        /// <summary>
        /// Adds a range of template group entries to the group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="entries">An enumerable of entries.</param>
        public void Add(string group, IEnumerable<TemplateGroupsEntry> entries)
        {
            ValidateGroupName(group);

            if (!Groups.TryGetValue(group, out List<TemplateGroupsEntry> collection))
            {
                collection = new List<TemplateGroupsEntry>();
                Groups.Add(group, collection);
            }

            collection.AddRange(entries);
        }

        /// <summary>
        /// Validates the specified group name and throws an exception if it is invalid.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <exception cref="InvalidNameException">Raised if the group name is invalid.</exception>
        private static void ValidateGroupName(string group)
        {
            if (string.IsNullOrWhiteSpace(group))
                throw new InvalidNameException($"Invalid group name: `{group}`.");
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
                        spaces.Add(pair, new ConfigurationSpace(from, to));
                }
            }

            return spaces;
        }

        /// <summary>
        /// Returns a list of all template group entries.
        /// </summary>
        public List<TemplateGroupsEntry> GetAllEntries()
        {
            var result = new List<TemplateGroupsEntry>();

            foreach (var pair in Groups.OrderBy(x => x.Key))
            {
                foreach (var entry in pair.Value)
                {
                    result.Add(entry);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns an enumerable of all templates in all groups.
        /// </summary>
        public IEnumerable<RoomTemplate> GetAllTemplates()
        {
            foreach (var pair in Groups.OrderBy(x => x.Key))
            {
                foreach (var entry in pair.Value)
                {
                    yield return entry.Template;
                }
            }
        }
    }
}
