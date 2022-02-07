using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
