using System;

namespace MPewsey.ManiaMap
{
    public class LayoutEdge
    {
        public int FromNode { get; private set; }
        public int ToNode { get; private set; }
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

        /// <summary>
        /// Reverses the nodes and direction of the edge.
        /// </summary>
        public void Reverse()
        {
            Direction = Door.ReverseEdgeDirection(Direction);
            (FromNode, ToNode) = (ToNode, FromNode);
        }
    }
}
