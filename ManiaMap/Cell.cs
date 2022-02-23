using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class Cell
    {
        public static Cell Empty { get => null; }

        [DataMember(Order = 1)]
        public Door WestDoor { get; set; }

        [DataMember(Order = 2)]
        public Door NorthDoor { get; set; }

        [DataMember(Order = 3)]
        public Door EastDoor { get; set; }

        [DataMember(Order = 4)]
        public Door SouthDoor { get; set; }

        [DataMember(Order = 5)]
        public Door TopDoor { get; set; }

        [DataMember(Order = 6)]
        public Door BottomDoor { get; set; }

        public override string ToString()
        {
            var west = WestDoor == null ? "None" : WestDoor.ToString();
            var north = NorthDoor == null ? "None" : NorthDoor.ToString();
            var south = SouthDoor == null ? "None" : SouthDoor.ToString();
            var east = EastDoor == null ? "None" : EastDoor.ToString();
            var top = TopDoor == null ? "None" : TopDoor.ToString();
            var bottom = BottomDoor == null ? "None" : BottomDoor.ToString();
            return $"Cell(WestDoor = {west}, NorthDoor = {north}, EastDoor = {east}, SouthDoor = {south}, TopDoor = {top}, BottomDoor = {bottom})";
        }

        /// <summary>
        /// Returns true if the top door aligns with the bottom door of the specified cell.
        /// </summary>
        public bool TopDoorAligns(Cell other)
        {
            return TopDoor != null
                && other?.BottomDoor != null
                && TopDoor.Aligns(other.BottomDoor);
        }

        /// <summary>
        /// Returns true if the bottom door aligns with the top door of the specified cell.
        /// </summary>
        public bool BottomDoorAligns(Cell other)
        {
            return BottomDoor != null
                && other?.TopDoor != null
                && BottomDoor.Aligns(other.TopDoor);
        }

        /// <summary>
        /// Returns true if the north door aligns with the south door of the specified cell.
        /// </summary>
        public bool NorthDoorAligns(Cell other)
        {
            return NorthDoor != null
                && other?.SouthDoor != null
                && NorthDoor.Aligns(other.SouthDoor);
        }

        /// <summary>
        /// Returns true if the south door aligns with the north door of the specified cell.
        /// </summary>
        public bool SouthDoorAligns(Cell other)
        {
            return SouthDoor != null
                && other?.NorthDoor != null
                && SouthDoor.Aligns(other.NorthDoor);
        }

        /// <summary>
        /// Returns true if the west door aligns with the east door of the specified cell.
        /// </summary>
        public bool WestDoorAligns(Cell other)
        {
            return WestDoor != null
                && other?.EastDoor != null
                && WestDoor.Aligns(other.EastDoor);
        }

        /// <summary>
        /// Returns true if the east door aligns with the west door of the specified cell.
        /// </summary>
        public bool EastDoorAligns(Cell other)
        {
            return EastDoor != null
                && other?.WestDoor != null
                && EastDoor.Aligns(other.WestDoor);
        }

        /// <summary>
        /// Returns a new cell rotated clockwise 90 degrees.
        /// </summary>
        public Cell Rotated90()
        {
            return new Cell
            {
                EastDoor = NorthDoor?.Copy(),
                SouthDoor = EastDoor?.Copy(),
                WestDoor = SouthDoor?.Copy(),
                NorthDoor = WestDoor?.Copy(),
                TopDoor = TopDoor?.Copy(),
                BottomDoor = BottomDoor?.Copy(),
            };
        }

        /// <summary>
        /// Returns a new cell rotated 180 degrees.
        /// </summary>
        public Cell Rotated180()
        {
            return new Cell
            {
                SouthDoor = NorthDoor?.Copy(),
                NorthDoor = SouthDoor?.Copy(),
                EastDoor = WestDoor?.Copy(),
                WestDoor = EastDoor?.Copy(),
                TopDoor = TopDoor?.Copy(),
                BottomDoor = BottomDoor?.Copy(),
            };
        }

        /// <summary>
        /// Returns a new cell rotated clockwise 270 degrees.
        /// </summary>
        public Cell Rotated270()
        {
            return new Cell
            {
                WestDoor = NorthDoor?.Copy(),
                NorthDoor = EastDoor?.Copy(),
                EastDoor = SouthDoor?.Copy(),
                SouthDoor = WestDoor?.Copy(),
                TopDoor = TopDoor?.Copy(),
                BottomDoor = BottomDoor?.Copy(),
            };
        }

        /// <summary>
        /// Returns a new cell mirrored vertically, i.e. about the horizontal axis.
        /// </summary>
        public Cell MirroredVertically()
        {
            return new Cell
            {
                SouthDoor = NorthDoor?.Copy(),
                NorthDoor = SouthDoor?.Copy(),
                WestDoor = WestDoor?.Copy(),
                EastDoor = EastDoor?.Copy(),
                TopDoor = TopDoor?.Copy(),
                BottomDoor = BottomDoor?.Copy(),
            };
        }

        /// <summary>
        /// Returns a new cell mirrored horizontally, i.e. about the vertical axis.
        /// </summary>s
        public Cell MirroredHorizontally()
        {
            return new Cell
            {
                NorthDoor = NorthDoor?.Copy(),
                SouthDoor = SouthDoor?.Copy(),
                WestDoor = EastDoor?.Copy(),
                EastDoor = WestDoor?.Copy(),
                TopDoor = TopDoor?.Copy(),
                BottomDoor = BottomDoor?.Copy(),
            };
        }

        /// <summary>
        /// Returns true if the values of the two cells match.
        /// </summary>
        public bool Matches(Cell other)
        {
            if ((TopDoor == null) != (other.TopDoor == null))
                return false;
            if ((BottomDoor == null) != (other.BottomDoor == null))
                return false;
            if ((NorthDoor == null) != (other.NorthDoor == null))
                return false;
            if ((SouthDoor == null) != (other.SouthDoor == null))
                return false;
            if ((EastDoor == null) != (other.EastDoor == null))
                return false;
            if ((WestDoor == null) != (other.WestDoor == null))
                return false;
            
            if (TopDoor != null && other.TopDoor != null && !TopDoor.Matches(other.TopDoor))
                return false;
            if (BottomDoor != null && other.BottomDoor != null && !BottomDoor.Matches(other.BottomDoor))
                return false;
            if (NorthDoor != null && other.NorthDoor != null && !NorthDoor.Matches(other.NorthDoor))
                return false;
            if (SouthDoor != null && other.SouthDoor != null && !SouthDoor.Matches(other.SouthDoor))
                return false;
            if (EastDoor != null && other.EastDoor != null && !EastDoor.Matches(other.EastDoor))
                return false;
            if (WestDoor != null && other.WestDoor != null && !WestDoor.Matches(other.WestDoor))
                return false;

            return true;
        }
    }
}