﻿using System.Drawing;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A node in a `LayoutGraph`.
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
        public string TemplateGroup { get; set; }

        /// <inheritdoc>
        [DataMember(Order = 5)]
        public Color Color { get; set; } = Color.MidnightBlue;

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

        public override string ToString()
        {
            return $"LayoutNode(Id = {Id}, Name = {Name})";
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
        public LayoutNode SetColor(Color value)
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