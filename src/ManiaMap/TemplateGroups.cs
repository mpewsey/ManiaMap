using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for creating groups of `RoomTemplate`.
    /// </summary>
    [DataContract]
    public class TemplateGroups
    {
        /// <summary>
        /// A dictionary of room templates by group string.
        /// </summary>
        [DataMember(Order = 1)]
        public Dictionary<string, List<RoomTemplate>> Groups { get; private set; } = new Dictionary<string, List<RoomTemplate>>();

        public override string ToString()
        {
            return $"TemplateGroups(Groups.Count = {Groups.Count})";
        }

        /// <summary>
        /// Saves the template groups to the file path using the `DataContractSerializer`.
        /// </summary>
        /// <param name="path">The save file path.</param>
        public void SaveXml(string path)
        {
            var serializer = new DataContractSerializer(GetType());

            using (var stream = File.Create(path))
            {
                serializer.WriteObject(stream, this);
            }
        }

        /// <summary>
        /// Loads the template groups from the file path using the `DataContractSerializer`.
        /// </summary>
        /// <param name="path">The file path.</param>
        public static TemplateGroups LoadXml(string path)
        {
            var serializer = new DataContractSerializer(typeof(TemplateGroups));

            using (var stream = File.OpenRead(path))
            {
                return (TemplateGroups)serializer.ReadObject(stream);
            }
        }

        /// <summary>
        /// Adds the template to the group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="template">The room template to add.</param>
        public void Add(string group, RoomTemplate template)
        {
            if (!Groups.TryGetValue(group, out var templates))
            {
                templates = new List<RoomTemplate>();
                Groups.Add(group, templates);
            }

            templates.Add(template);
        }

        /// <summary>
        /// Adds the templates to the group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="templates">The room templates to add.</param>
        public void Add(string group, params RoomTemplate[] templates)
        {
            foreach (var template in templates)
            {
                Add(group, template);
            }
        }

        /// <summary>
        /// Adds the templates to the group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="templates">A list of room templates to add.</param>
        public void Add(string group, List<RoomTemplate> templates)
        {
            if (!Groups.TryGetValue(group, out var list))
            {
                list = new List<RoomTemplate>();
                Groups.Add(group, list);
            }

            list.AddRange(templates);
        }

        /// <summary>
        /// Adds the templates to the group.
        /// </summary>
        /// <param name="group">The group name.</param>
        /// <param name="templates">The lists of room templates to add.</param>
        public void Add(string group, params List<RoomTemplate>[] templates)
        {
            foreach (var collection in templates)
            {
                Add(group, collection);
            }
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
