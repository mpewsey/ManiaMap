using MPewsey.ManiaMap.Collections;
using MPewsey.ManiaMap.Exceptions;
using MPewsey.ManiaMap.Serialization;
using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Represents an edge connecting two nodes of a LayoutGraph.
    /// 
    /// Several chainable property setting methods are included to allow the creation
    /// of an edge and setting of properties in a single line. For example, the following
    /// creates a edge in a graph, then sets the name and adds a template group:
    /// 
    /// ```
    /// graph.AddEdge(1, 2).SetName("Edge1").SetTemplateGroup("Default");
    /// ```
    /// </summary>
    [DataContract(Namespace = XmlSerialization.Namespace)]
    public class LayoutEdge : IRoomSource, IKey<EdgeIndexes>
    {
        /// <inheritdoc/>
        [DataMember(Order = 1, IsRequired = true)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// The from node ID.
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public int FromNode { get; private set; }

        /// <summary>
        /// The to node ID.
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        public int ToNode { get; private set; }

        /// <summary>
        /// The edge direction.
        /// </summary>
        [DataMember(Order = 4, IsRequired = true)]
        public EdgeDirection Direction { get; set; }

        /// <summary>
        /// The matching door code.
        /// </summary>
        [DataMember(Order = 5, IsRequired = true)]
        public int DoorCode { get; set; }

        /// <inheritdoc/>
        [DataMember(Order = 6, IsRequired = true)]
        public int Z { get; set; }

        /// <summary>
        /// The chance that a room will be created from the edge. The value should be between 0 and 1.
        /// </summary>
        [DataMember(Order = 7, IsRequired = true)]
        public float RoomChance { get; set; }

        /// <summary>
        /// If true, in order to form a valid layout, a room must be added for this edge
        /// if the room chance is satisfied. Otherwise, the room may be skipped if adding
        /// an edge for the room fails.
        /// </summary>
        [DataMember(Order = 8, IsRequired = true)]
        public bool RequireRoom { get; set; }

        /// <inheritdoc/>
        [DataMember(Order = 9, IsRequired = true)]
        public Color4 Color { get; set; } = new Color4(25, 25, 112, 255);

        /// <inheritdoc/>
        [DataMember(Order = 10, IsRequired = true)]
        public string TemplateGroup { get; set; } = "Default";

        /// <inheritdoc/>
        public Uid RoomId { get => new Uid(FromNode, ToNode, 1); }

        /// <inheritdoc/>
        public EdgeIndexes Key => new EdgeIndexes(FromNode, ToNode);

        /// <summary>
        /// Initializes an edge with the from node and to node ID's.
        /// </summary>
        /// <param name="fromNode">The from node ID.</param>
        /// <param name="toNode">The to node ID.</param>
        public LayoutEdge(int fromNode, int toNode)
        {
            FromNode = fromNode;
            ToNode = toNode;
        }

        /// <summary>
        /// Initializes a copy of the edge.
        /// </summary>
        /// <param name="other">The edge to be copied.</param>
        private LayoutEdge(LayoutEdge other)
        {
            Name = other.Name;
            FromNode = other.FromNode;
            ToNode = other.ToNode;
            Direction = other.Direction;
            DoorCode = other.DoorCode;
            Z = other.Z;
            RoomChance = other.RoomChance;
            Color = other.Color;
            TemplateGroup = other.TemplateGroup;
        }

        public override string ToString()
        {
            return $"LayoutEdge(Name = {Name}, FromNode = {FromNode}, ToNode = {ToNode}, Direction = {Direction}, DoorCode = {DoorCode})";
        }

        /// <summary>
        /// Returns a copy of the edge.
        /// </summary>
        public LayoutEdge Copy()
        {
            return new LayoutEdge(this);
        }

        /// <summary>
        /// Sets the properties of this edge to that of the specified other edge,
        /// with the exception of the node ID's.
        /// </summary>
        /// <param name="other">The other edge.</param>
        public void SetProperties(LayoutEdge other)
        {
            Name = other.Name;
            Direction = other.Direction;
            DoorCode = other.DoorCode;
            Z = other.Z;
            RoomChance = other.RoomChance;
            Color = other.Color;
            TemplateGroup = other.TemplateGroup;
        }

        /// <summary>
        /// Validates the edge and raises any applicable exceptions.
        /// </summary>
        /// <exception cref="NoTemplateGroupAssignedException">Raised if the room chance is non-zero and a valid template group is not assigned.</exception>
        public void Validate()
        {
            if (RoomChance > 0 && string.IsNullOrWhiteSpace(TemplateGroup))
                throw new NoTemplateGroupAssignedException($"Template group is not assigned to edge with non-zero room chance: {this}.");
        }

        /// <summary>
        /// Returns true if the edge is valid.
        /// </summary>
        public bool IsValid()
        {
            return RoomChance <= 0 || !string.IsNullOrWhiteSpace(TemplateGroup);
        }

        /// <summary>
        /// Returns true if the room chance is satisfied.
        /// </summary>
        /// <param name="value">A random value between 0 and 1 to check against.</param>
        public bool RoomChanceSatisfied(double value)
        {
            return RoomChance >= 1 || (RoomChance > 0 && value <= RoomChance);
        }

        /// <summary>
        /// Returns the symbol string for the edge.
        /// 
        /// For instance, an edge with a forward flexible edge direction from node 1 to 2
        /// is symbolized as (1 => 2).
        /// 
        /// Edge directions are drawn as:
        /// 
        /// * "<=>" : Two way
        /// * "=>" and "<=" : One way flexible in the arrow direction.
        /// * "->" and "<-" : One way fixed in the arrow direction.
        /// 
        /// Basically, the "=", with its two lines, signals that the edge can go both directions
        /// given the conditions are met, whereas the "-", with its one line, signals that
        /// the edge only ever goes in one direction.
        /// </summary>
        /// <exception cref="UnhandledCaseException">Raised if the edge direction is not handled.</exception>
        public string ToSymbolString()
        {
            switch (Direction)
            {
                case EdgeDirection.Both:
                    return $"({FromNode} <=> {ToNode})";
                case EdgeDirection.ForwardFlexible:
                    return $"({FromNode} => {ToNode})";
                case EdgeDirection.ForwardFixed:
                    return $"({FromNode} -> {ToNode})";
                case EdgeDirection.ReverseFlexible:
                    return $"({FromNode} <= {ToNode})";
                case EdgeDirection.ReverseFixed:
                    return $"({FromNode} <- {ToNode})";
                default:
                    throw new UnhandledCaseException($"Unhandled Edge Direction: {Direction}.");
            }
        }

        /// <summary>
        /// Sets the name of the edge and returns the edge.
        /// </summary>
        /// <param name="name">The name.</param>
        public LayoutEdge SetName(string name)
        {
            Name = name;
            return this;
        }

        /// <summary>
        /// Sets the room chance for the edge and returns the edge.
        /// </summary>
        /// <param name="chance">The chance of creating a room from the edge, between 0 and 1.</param>
        public LayoutEdge SetRoomChance(float chance)
        {
            RoomChance = chance;
            return this;
        }

        /// <summary>
        /// Sets whether a room is required for the edge and returns the edge.
        /// </summary>
        /// <param name="value">Whether a room is required for the edge.</param>
        public LayoutEdge SetRequireRoom(bool value)
        {
            RequireRoom = value;
            return this;
        }

        /// <summary>
        /// Sets the Z value of the edge and returns the edge.
        /// </summary>
        /// <param name="z">The z value.</param>
        public LayoutEdge SetZ(int z)
        {
            Z = z;
            return this;
        }

        /// <summary>
        /// Sets the color of the edge and returns the edge.
        /// </summary>
        /// <param name="color">The color.</param>
        public LayoutEdge SetColor(Color4 color)
        {
            Color = color;
            return this;
        }

        /// <summary>
        /// Sets the direction of the edge and returns the edge.
        /// </summary>
        /// <param name="direction">The edge direction.</param>
        public LayoutEdge SetDirection(EdgeDirection direction)
        {
            Direction = direction;
            return this;
        }

        /// <summary>
        /// Sets the door code of the edge and returns the edge.
        /// </summary>
        /// <param name="doorCode">The matching door code.</param>
        public LayoutEdge SetDoorCode(int doorCode)
        {
            DoorCode = doorCode;
            return this;
        }

        /// <summary>
        /// Reverses the nodes and direction of the edge.
        /// </summary>
        public void Reverse()
        {
            Direction = ReverseEdgeDirection(Direction);
            (FromNode, ToNode) = (ToNode, FromNode);
        }

        /// <summary>
        /// Returns true if the edges shares a node with the specified edge.
        /// </summary>
        /// <param name="other">The other edge.</param>
        public bool SharesNode(LayoutEdge other)
        {
            return FromNode == other.FromNode
                || FromNode == other.ToNode
                || ToNode == other.FromNode
                || ToNode == other.ToNode;
        }

        /// <summary>
        /// Sets the template group and returns the edge.
        /// </summary>
        /// <param name="value">The template group name.</param>
        public LayoutEdge SetTemplateGroup(string value)
        {
            TemplateGroup = value;
            return this;
        }

        /// <summary>
        /// Returns the reverse of the specified direction.
        /// </summary>
        /// <param name="direction">The edge direction.</param>
        /// <exception cref="ArgumentException">Raises for an unhandled direction.</exception>
        public static EdgeDirection ReverseEdgeDirection(EdgeDirection direction)
        {
            switch (direction)
            {
                case EdgeDirection.Both:
                    return EdgeDirection.Both;
                case EdgeDirection.ForwardFlexible:
                    return EdgeDirection.ReverseFlexible;
                case EdgeDirection.ForwardFixed:
                    return EdgeDirection.ReverseFixed;
                case EdgeDirection.ReverseFlexible:
                    return EdgeDirection.ForwardFlexible;
                case EdgeDirection.ReverseFixed:
                    return EdgeDirection.ForwardFixed;
                default:
                    throw new ArgumentException($"Unhandled Edge Direction: {direction}.");
            }
        }
    }
}
