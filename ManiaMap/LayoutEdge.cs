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

        public LayoutEdge(int fromNode, int toNode)
        {
            FromNode = fromNode;
            ToNode = toNode;
        }

        public override string ToString()
        {
            return $"LayoutEdge(FromNode = {FromNode}, ToNode = {ToNode})";
        }
    }
}
