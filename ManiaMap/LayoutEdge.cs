using System;
using System.Collections.Generic;
using System.Linq;

namespace ManiaMap
{
    public class LayoutEdge
    {
        public int FromNode { get; }
        public int ToNode { get; }
        public EdgeDirection Direction { get; set; }

        public LayoutEdge(int fromNode, int toNode, EdgeDirection direction = EdgeDirection.Both)
        {
            FromNode = fromNode;
            ToNode = toNode;
            Direction = direction;
        }

        public override string ToString()
        {
            return $"LayoutEdge(FromNode = {FromNode}, ToNode = {ToNode}, Direction = {Direction})";
        }

        /// <summary>
        /// Returns the short string for the edge.
        /// </summary>
        public string ToShortString()
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
    }
}
