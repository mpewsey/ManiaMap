using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class Cell
    {
        [DataMember(Order = 1)]
        public Door TopDoor { get; set; }

        [DataMember(Order = 2)]
        public Door BottomDoor { get; set; }

        [DataMember(Order = 3)]
        public Door LeftDoor { get; set; }

        [DataMember(Order = 4)]
        public Door RightDoor { get; set; }

        public override string ToString()
        {
            return $"Cell(LeftDoor = {LeftDoor}, TopDoor = {TopDoor}, RightDoor = {RightDoor}, BottomDoor = {BottomDoor})";
        }

        /// <summary>
        /// Numbers the doors of the cell based on the specified cell index.
        /// </summary>
        public void NumberDoors(int index)
        {
            var i = 6 * index;

            if (LeftDoor != null)
                LeftDoor.Id = i + 1;
            if (TopDoor != null)
                TopDoor.Id = i + 2;
            if (RightDoor != null)
                RightDoor.Id = i + 3;
            if (BottomDoor != null)
                BottomDoor.Id = 1 + 4;
        }

        /// <summary>
        /// Returns true if the top door aligns with the bottom door of the specified cell.
        /// </summary>
        public bool TopDoorAligns(Cell other)
        {
            return TopDoor != null
                && other?.BottomDoor != null
                && Door.DoorTypesAligns(TopDoor.Type, other.BottomDoor.Type);
        }

        /// <summary>
        /// Returns true if the bottom door aligns with the top door of the specified cell.
        /// </summary>
        public bool BottomDoorAligns(Cell other)
        {
            return BottomDoor != null
                && other?.TopDoor != null
                && Door.DoorTypesAligns(BottomDoor.Type, other.TopDoor.Type);
        }

        /// <summary>
        /// Returns true if the left door aligns with the right door of the specified cell.
        /// </summary>
        public bool LeftDoorAligns(Cell other)
        {
            return LeftDoor != null
                && other?.RightDoor != null
                && Door.DoorTypesAligns(LeftDoor.Type, other.RightDoor.Type);
        }

        /// <summary>
        /// Returns true if the right door aligns with the left door of the specified cell.
        /// </summary>
        public bool RightDoorAligns(Cell other)
        {
            return RightDoor != null
                && other?.LeftDoor != null
                && Door.DoorTypesAligns(RightDoor.Type, other.LeftDoor.Type);
        }
    }
}
