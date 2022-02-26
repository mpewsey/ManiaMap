using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class LayoutNode
    {
        [DataMember(Order = 1)]
        public int Id { get; set; }

        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public int Z { get; set; }

        [DataMember(Order = 4)]
        public List<string> TemplateGroups { get; private set; } = new List<string>();

        [DataMember(Order = 5)]
        public Color Color { get; set; } = Color.MidnightBlue;

        public Uid RoomId { get => new Uid(Id); }

        public LayoutNode(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"LayoutNode(Id = {Id}, Name = {Name})";
        }

        /// <summary>
        /// Sets the Z value of the node and returns the node.
        /// </summary>
        public LayoutNode SetZ(int value)
        {
            Z = value;
            return this;
        }

        /// <summary>
        /// Sets the name of the node and returns the node.
        /// </summary>
        public LayoutNode SetName(string value)
        {
            Name = value;
            return this;
        }

        /// <summary>
        /// Sets the color of the node and returns the node.
        /// </summary>
        public LayoutNode SetColor(Color value)
        {
            Color = value;
            return this;
        }

        /// <summary>
        /// Adds template groups to the node and returns the node.
        /// </summary>
        public LayoutNode AddTemplateGroups(string group)
        {
            if (!TemplateGroups.Contains(group))
            {
                TemplateGroups.Add(group);
            }

            return this;
        }

        /// <summary>
        /// Adds template groups to the node and returns the node.
        /// </summary>
        public LayoutNode AddTemplateGroups(params string[] groups)
        {
            foreach (var group in groups)
            {
                AddTemplateGroups(group);
            }

            return this;
        }
    }
}
