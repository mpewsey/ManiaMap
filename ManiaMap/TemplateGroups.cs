using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    public class TemplateGroups
    {
        public Dictionary<string, List<RoomTemplate>> Groups { get; } = new Dictionary<string, List<RoomTemplate>>();

        public override string ToString()
        {
            return $"TemplateGroups(Groups.Count = {Groups.Count})";
        }

        /// <summary>
        /// Adds the template to the group.
        /// </summary>
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
        public void Add(string group, IEnumerable<RoomTemplate> templates)
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
        public void Add(string group, params IEnumerable<RoomTemplate>[] templates)
        {
            foreach (var collection in templates)
            {
                Add(group, collection);
            }
        }

        /// <summary>
        /// Returns an enumerable of all templates.
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
        /// Returns a new list of room templates for the specified groups.
        /// </summary>
        public List<RoomTemplate> GetTemplates(IEnumerable<string> groups)
        {
            var templates = new List<RoomTemplate>();

            foreach (var group in groups)
            {
                if (Groups.TryGetValue(group, out var list))
                {
                    templates.AddRange(list);
                }
            }

            if (templates.Count == 0)
                throw new Exception($"No templates exists in groups: {string.Join(", ", groups)}.");

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
                    spaces[new TemplatePair(from, to)] = new ConfigurationSpace(from, to);
                }
            }

            return spaces;
        }
    }
}
