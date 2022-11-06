using MPewsey.ManiaMap.Collections;
using MPewsey.ManiaMap.Exceptions;
using MPewsey.ManiaMap.Serialization;
using System;
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
        private DataContractDictionary<string, List<Entry>> Groups { get; set; } = new DataContractDictionary<string, List<Entry>>();

        /// <summary>
        /// A readonly dictionary of templates by group name.
        /// </summary>
        public IReadOnlyDictionary<string, List<Entry>> GroupsDictionary => Groups;

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
            var entries = GetAllEntries().ToList();

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
        public List<Entry> GetGroup(string group)
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
            Add(group, new Entry(template));
        }

        /// <summary>
        /// Adds a template group entry to the group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="entry">The group entry.</param>
        public void Add(string group, Entry entry)
        {
            ValidateGroupName(group);

            if (!Groups.TryGetValue(group, out List<Entry> entries))
            {
                entries = new List<Entry>();
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
            Add(group, templates.Select(x => new Entry(x)));
        }

        /// <summary>
        /// Adds a range of template group entries to the group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="entries">An enumerable of entries.</param>
        public void Add(string group, IEnumerable<Entry> entries)
        {
            ValidateGroupName(group);

            if (!Groups.TryGetValue(group, out List<Entry> collection))
            {
                collection = new List<Entry>();
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
        /// Returns an enumerable of all template group entries.
        /// </summary>
        public IEnumerable<Entry> GetAllEntries()
        {
            foreach (var pair in Groups.OrderBy(x => x.Key))
            {
                foreach (var entry in pair.Value)
                {
                    yield return entry;
                }
            }
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

        /// <summary>
        /// A template group entry, consisting of a RoomTemplate and usage constaints.
        /// </summary>
        [DataContract(Namespace = XmlSerialization.Namespace)]
        public class Entry
        {
            /// <summary>
            /// The room template.
            /// </summary>
            [DataMember(Order = 1)]
            public RoomTemplate Template { get; private set; }

            private int _minQuantity;
            /// <summary>
            /// The minimum number of uses for the entry.
            /// </summary>
            [DataMember(Order = 3)]
            public int MinQuantity
            {
                get => _minQuantity;
                set => _minQuantity = Math.Max(value, 0);
            }

            private int _maxQuantity = int.MaxValue;
            /// <summary>
            /// The maximum number of uses for the entry.
            /// </summary>
            [DataMember(Order = 4)]
            public int MaxQuantity
            {
                get => _maxQuantity;
                set => _maxQuantity = Math.Max(value, 0);
            }

            /// <summary>
            /// Intializes a new entry with no quantity contraints.
            /// </summary>
            /// <param name="template">The room template.</param>
            public Entry(RoomTemplate template)
            {
                Template = template;
            }

            /// <summary>
            /// Initializes a new entry with quantity constraints.
            /// </summary>
            /// <param name="template">The room template.</param>
            /// <param name="minQuantity">The minimum number of uses for the entry.</param>
            /// <param name="maxQuantity">The maximum number of uses for the entry.</param>
            public Entry(RoomTemplate template, int minQuantity, int maxQuantity = int.MaxValue)
            {
                Template = template;
                MinQuantity = minQuantity;
                MaxQuantity = maxQuantity;
            }

            /// <summary>
            /// Returns true if the constraint quantity is satisfied.
            /// </summary>
            /// <param name="count">The entry count.</param>
            public bool QuantitySatisfied(int count)
            {
                return count >= MinQuantity && count <= MaxQuantity;
            }

            /// <summary>
            /// If the specified room template has the same values as the current template,
            /// sets it as the current template.
            /// </summary>
            /// <param name="other">The specified room template.</param>
            public void ConsolidateTemplate(RoomTemplate other)
            {
                if (RoomTemplate.ValuesAreEqual(Template, other))
                    Template = other;
            }
        }
    }
}
