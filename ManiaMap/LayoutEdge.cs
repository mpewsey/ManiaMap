using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class LayoutEdge
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
            Direction = Door.ReverseEdgeDirection(Direction);
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
    }
}
