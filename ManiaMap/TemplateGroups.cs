using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class TemplateGroups
    {
        public Dictionary<string, List<RoomTemplate>> Groups { get; } = new();

        /// <summary>
        /// Adds the template to the group.
        /// </summary>
        public void Add(string group, RoomTemplate template)
        {
            if (Groups.TryGetValue(group, out var templates))
            {
                templates = new();
                Groups.Add(group, templates);
            }

            templates.Add(template);
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
        public List<RoomTemplate> GetTemplates(List<string> groups)
        {
            var templates = new List<RoomTemplate>();

            foreach (var group in groups)
            {
                templates.AddRange(Groups[group]);
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
                    spaces[new(from, to)] = new(from, to);
                }
            }

            return spaces;
        }
    }
}
