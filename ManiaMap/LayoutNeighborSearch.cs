﻿using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap
{
    public class LayoutNeighborSearch
    {
        public Layout Layout { get; set; }
        public int MaxDepth { get; set; }
        private Dictionary<Uid, List<Uid>> Neighbors { get; set; }
        private HashSet<Uid> Marked { get; } = new HashSet<Uid>();

        public LayoutNeighborSearch(Layout layout, int maxDepth)
        {
            Layout = layout;
            MaxDepth = maxDepth;
        }

        /// <summary>
        /// Returns an array of neighbors of the room up to the max depth.
        /// </summary>
        public List<Uid> FindNeighbors(Uid room)
        {
            Marked.Clear();
            Neighbors = Layout.RoomAdjacencies();
            SearchNeighbors(room, 0);
            return Marked.ToList();
        }

        /// <summary>
        /// Recursively searches for neighbors of the room.
        /// </summary>
        private void SearchNeighbors(Uid room, int depth)
        {
            if (depth <= MaxDepth && Marked.Add(room))
            {
                foreach (var neighbor in Neighbors[room])
                {
                    SearchNeighbors(neighbor, depth + 1);
                }
            }
        }
    }
}