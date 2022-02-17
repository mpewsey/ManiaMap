using System.Collections.Generic;
using System.Drawing;

namespace MPewsey.ManiaMap
{
    public class LayoutNode
    {
        public int Id { get; }
        public string Name { get; set; } = string.Empty;
        public int Z { get; set; }
        public List<string> TemplateGroups { get; } = new List<string>();
        public Color Color { get; set; } = Color.MidnightBlue;

        public LayoutNode(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"LayoutNode(Id = {Id}, Name = {Name})";
        }
    }
}
