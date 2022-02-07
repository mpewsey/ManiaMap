using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class LayoutGenerator
    {
        public int Seed { get; }
        public LayoutGraph Graph { get; }
        public TemplateGroups TemplateGroups { get; }

        public LayoutGenerator(int seed, LayoutGraph graph, TemplateGroups templateGroups)
        {
            Seed = seed;
            Graph = graph;
            TemplateGroups = templateGroups;
        }
    }
}
