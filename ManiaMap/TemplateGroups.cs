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

        public List<RoomTemplate> GetTemplates(List<string> groups)
        {
            var templates = new List<RoomTemplate>();

            foreach (var group in groups)
            {
                templates.AddRange(Groups[group]);
            }

            return templates;
        }

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
