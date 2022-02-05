﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class LayoutNode
    {
        public int Id { get; }

        public LayoutNode(int id)
        {
            Id = id;
        }

        public override string ToString()
        {
            return $"LayoutNode(Id = {Id})";
        }
    }
}
