using MPewsey.ManiaMap.Exceptions;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A node in a LayoutGraph.
    /// 
    /// Several chainable property setting methods are included to allow the creation
    /// of a node and setting of properties in a single line. For example, the following
    /// creates a node in a graph, then sets the name and adds a template group:
    /// 
    /// ```
    /// graph.AddNode(1).SetName("Node1").SetTemplateGroup("Default");
    /// ```
    /// </summary>
    [DataContract]
    public class LayoutNode : IRoomSource
    {
        /// <summary>
        /// The unique node ID.
        /// </summary>
        [DataMember(Order = 1)]
        public int Id { get; set; }

        /// <inheritdoc>
        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;

        /// <inheritdoc>
        [DataMember(Order = 3)]
        public int Z { get; set; }

        /// <inheritdoc>
        [DataMember(Order = 4)]
        public string TemplateGroup { get; set; } = "Default";

        /// <inheritdoc>
        [DataMember(Order = 5)]
        public Color4 Color { get; set; } = new Color4(25, 25, 112, 255);

        /// <inheritdoc>
        public Uid RoomId { get => new Uid(Id); }

        /// <summary>
        /// Initializes an empty node from ID.
        /// </summary>
        /// <param name="id">The node ID.</param>
        public LayoutNode(int id)
        {
            Id = id;
        }

        /// <summary>
        /// Initializes a copy of the node.
        /// </summary>
        /// <param name="other">The node to be copied.</param>
        private LayoutNode(LayoutNode other)
        {
            Id = other.Id;
            Name = other.Name;
            Z = other.Z;
            TemplateGroup = other.TemplateGroup;
            Color = other.Color;
        }

        public override string ToString()
        {
            return $"LayoutNode(Id = {Id}, Name = {Name})";
        }

        /// <summary>
        /// Returns a copy of the node.
        /// </summary>
        public LayoutNode Copy()
        {
            return new LayoutNode(this);
        }

        /// <summary>
        /// Validates the node and raises any applicable exceptions.
        /// </summary>
        /// <exception cref="NoTemplateGroupAssignedException">Raised if a valid template group is not assigned to the node.</exception>
        public void Validate()
        {
            if (string.IsNullOrWhiteSpace(TemplateGroup))
                throw new NoTemplateGroupAssignedException($"No template group assigned to node: {this}.");
        }

        /// <summary>
        /// Returns true if the node is valid.
        /// </summary>
        public bool IsValid()
        {
            return !string.IsNullOrWhiteSpace(TemplateGroup);
        }

        /// <summary>
        /// Sets the Z value of the node and returns the node.
        /// </summary>
        /// <param name="value">The z value.</param>
        public LayoutNode SetZ(int value)
        {
            Z = value;
            return this;
        }

        /// <summary>
        /// Sets the name of the node and returns the node.
        /// </summary>
        /// <param name="value">The name</param>
        public LayoutNode SetName(string value)
        {
            Name = value;
            return this;
        }

        /// <summary>
        /// Sets the color of the node and returns the node.
        /// </summary>
        /// <param name="value">The color.</param>
        public LayoutNode SetColor(Color4 value)
        {
            Color = value;
            return this;
        }

        /// <summary>
        /// Sets the template group and returns the node.
        /// </summary>
        /// <param name="value">The template group name.</param>
        public LayoutNode SetTemplateGroup(string value)
        {
            TemplateGroup = value;
            return this;
        }
    }
}
