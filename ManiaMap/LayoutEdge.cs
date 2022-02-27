using System;
using System.Collections.Generic;
using System.Drawing;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class LayoutEdge : IRoomSource
    {
        [DataMember(Order = 1)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 2)]
        public int FromNode { get; private set; }

        [DataMember(Order = 3)]
        public int ToNode { get; private set; }

        [DataMember(Order = 4)]
        public EdgeDirection Direction { get; set; }

        [DataMember(Order = 5)]
        public int DoorCode { get; set; }

        [DataMember(Order = 6)]
        public int Z { get; set; }

        [DataMember(Order = 7)]
        public float RoomChance { get; set; }

        [DataMember(Order = 8)]
        public Color Color { get; set; } = Color.MidnightBlue;

        [DataMember(Order = 9)]
        public List<string> TemplateGroups { get; private set; } = new List<string>();

        public Uid RoomId { get => new Uid(FromNode, ToNode, 1); }

        public LayoutEdge(int fromNode, int toNode)
        {
            FromNode = fromNode;
            ToNode = toNode;
        }

        public override string ToString()
        {
            return $"LayoutEdge(Name = {Name}, FromNode = {FromNode}, ToNode = {ToNode}, Direction = {Direction}, DoorCode = {DoorCode})";
        }

        /// <summary>
        /// Returns true if the room chance is satisfied.
        /// </summary>
        public bool RoomChanceSatisfied(double value)
        {
            return RoomChance >= 1 || (RoomChance > 0 && value <= RoomChance);
        }

        /// <summary>
        /// Returns the symbol string for the edge.
        /// </summary>
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
                    throw new Exception($"Unhandled Edge Direction: {Direction}.");
            }
        }

        /// <summary>
        /// Sets the room chance for the edge and returns the edge.
        /// </summary>
        public LayoutEdge SetRoomChance(float chance)
        {
            RoomChance = chance;
            return this;
        }

        /// <summary>
        /// Sets the Z value of the edge and returns the edge.
        /// </summary>
        public LayoutEdge SetZ(int z)
        {
            Z = z;
            return this;
        }

        /// <summary>
        /// Sets the color of the edge and returns the edge.
        /// </summary>
        public LayoutEdge SetColor(Color color)
        {
            Color = color;
            return this;
        }

        /// <summary>
        /// Sets the direction of the edge and returns the edge.
        /// </summary>
        public LayoutEdge SetDirection(EdgeDirection direction)
        {
            Direction = direction;
            return this;
        }

        /// <summary>
        /// Sets the door code of the edge and returns the edge.
        /// </summary>
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
        public bool SharesNode(LayoutEdge other)
        {
            return FromNode == other.FromNode
                || FromNode == other.ToNode
                || ToNode == other.FromNode
                || ToNode == other.ToNode;
        }

        /// <summary>
        /// Adds template groups to the edge and returns the edge.
        /// </summary>
        public LayoutEdge AddTemplateGroups(string group)
        {
            if (!TemplateGroups.Contains(group))
            {
                TemplateGroups.Add(group);
            }

            return this;
        }

        /// <summary>
        /// Adds template groups to the edge and returns the edge.
        /// </summary>
        public LayoutEdge AddTemplateGroups(params string[] groups)
        {
            foreach (var group in groups)
            {
                AddTemplateGroups(group);
            }

            return this;
        }

        /// <summary>
        /// Returns the reverse of the specified direction.
        /// </summary>
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
