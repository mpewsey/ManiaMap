﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class Cell
    {
        public Door TopDoor { get; set; }
        public Door BottomDoor { get; set; }
        public Door LeftDoor { get; set; }
        public Door RightDoor { get; set; }

        public override string ToString()
        {
            return $"Cell(LeftDoor = {LeftDoor}, TopDoor = {TopDoor}, RightDoor = {RightDoor}, BottomDoor = {BottomDoor})";
        }

        public bool TopDoorAligns(Cell other)
        {
            return TopDoor != null
                && other?.BottomDoor != null
                && Door.DoorTypesAligns(TopDoor.Type, other.BottomDoor.Type);
        }

        public bool BottomDoorAligns(Cell other)
        {
            return BottomDoor != null
                && other?.TopDoor != null
                && Door.DoorTypesAligns(BottomDoor.Type, other.TopDoor.Type);
        }

        public bool LeftDoorAligns(Cell other)
        {
            return LeftDoor != null
                && other?.RightDoor != null
                && Door.DoorTypesAligns(LeftDoor.Type, other.RightDoor.Type);
        }

        public bool RightDoorAligns(Cell other)
        {
            return RightDoor != null
                && other?.LeftDoor != null
                && Door.DoorTypesAligns(RightDoor.Type, other.LeftDoor.Type);
        }
    }
}
