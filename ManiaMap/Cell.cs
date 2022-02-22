using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public struct Cell : IEquatable<Cell>
    {
        public static Cell Empty { get => new Cell { IsEmpty = true }; }

        [DataMember(Order = 0)]
        public bool IsEmpty { get; set; }

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
            return $"Cell(IsEmpty = {IsEmpty}, WestDoor = {WestDoor}, NorthDoor = {NorthDoor}, EastDoor = {EastDoor}, SouthDoor = {SouthDoor}, TopDoor = {TopDoor}, BottomDoor = {BottomDoor})";
        }

        /// <summary>
        /// Returns true if the top door aligns with the bottom door of the specified cell.
        /// </summary>
        public bool TopDoorAligns(Cell other)
        {
            return !IsEmpty
                && !other.IsEmpty
                && TopDoor.Aligns(other.BottomDoor);
        }

        /// <summary>
        /// Returns true if the bottom door aligns with the top door of the specified cell.
        /// </summary>
        public bool BottomDoorAligns(Cell other)
        {
            return !IsEmpty
                && !other.IsEmpty
                && BottomDoor.Aligns(other.TopDoor);
        }

        /// <summary>
        /// Returns true if the north door aligns with the south door of the specified cell.
        /// </summary>
        public bool NorthDoorAligns(Cell other)
        {
            return !IsEmpty
                && !other.IsEmpty
                && NorthDoor.Aligns(other.SouthDoor);
        }

        /// <summary>
        /// Returns true if the south door aligns with the north door of the specified cell.
        /// </summary>
        public bool SouthDoorAligns(Cell other)
        {
            return !IsEmpty
                && !other.IsEmpty
                && SouthDoor.Aligns(other.NorthDoor);
        }

        /// <summary>
        /// Returns true if the west door aligns with the east door of the specified cell.
        /// </summary>
        public bool WestDoorAligns(Cell other)
        {
            return !IsEmpty
                && !other.IsEmpty
                && WestDoor.Aligns(other.EastDoor);
        }

        /// <summary>
        /// Returns true if the east door aligns with the west door of the specified cell.
        /// </summary>
        public bool EastDoorAligns(Cell other)
        {
            return !IsEmpty
                && !other.IsEmpty
                && EastDoor.Aligns(other.WestDoor);
        }

        /// <summary>
        /// Returns a new cell rotated clockwise 90 degrees.
        /// </summary>
        public Cell Rotated90()
        {
            return new Cell
            {
                IsEmpty = IsEmpty,
                EastDoor = NorthDoor,
                SouthDoor = EastDoor,
                WestDoor = SouthDoor,
                NorthDoor = WestDoor,
                TopDoor = TopDoor,
                BottomDoor = BottomDoor,
            };
        }

        /// <summary>
        /// Returns a new cell rotated 180 degrees.
        /// </summary>
        public Cell Rotated180()
        {
            return new Cell
            {
                IsEmpty = IsEmpty,
                SouthDoor = NorthDoor,
                NorthDoor = SouthDoor,
                EastDoor = WestDoor,
                WestDoor = EastDoor,
                TopDoor = TopDoor,
                BottomDoor = BottomDoor,
            };
        }

        /// <summary>
        /// Returns a new cell rotated clockwise 270 degrees.
        /// </summary>
        public Cell Rotated270()
        {
            return new Cell
            {
                IsEmpty = IsEmpty,
                WestDoor = NorthDoor,
                NorthDoor = EastDoor,
                EastDoor = SouthDoor,
                SouthDoor = WestDoor,
                TopDoor = TopDoor,
                BottomDoor = BottomDoor,
            };
        }

        /// <summary>
        /// Returns a new cell mirrored vertically, i.e. about the horizontal axis.
        /// </summary>
        public Cell MirroredVertically()
        {
            return new Cell
            {
                IsEmpty = IsEmpty,
                SouthDoor = NorthDoor,
                NorthDoor = SouthDoor,
                WestDoor = WestDoor,
                EastDoor = EastDoor,
                TopDoor = TopDoor,
                BottomDoor = BottomDoor,
            };
        }

        /// <summary>
        /// Returns a new cell mirrored horizontally, i.e. about the vertical axis.
        /// </summary>s
        public Cell MirroredHorizontally()
        {
            return new Cell
            {
                IsEmpty = IsEmpty,
                NorthDoor = NorthDoor,
                SouthDoor = SouthDoor,
                WestDoor = EastDoor,
                EastDoor = WestDoor,
                TopDoor = TopDoor,
                BottomDoor = BottomDoor,
            };
        }

        public override bool Equals(object obj)
        {
            return obj is Cell cell && Equals(cell);
        }

        public bool Equals(Cell other)
        {
            return IsEmpty == other.IsEmpty &&
                   EqualityComparer<Door>.Default.Equals(WestDoor, other.WestDoor) &&
                   EqualityComparer<Door>.Default.Equals(NorthDoor, other.NorthDoor) &&
                   EqualityComparer<Door>.Default.Equals(EastDoor, other.EastDoor) &&
                   EqualityComparer<Door>.Default.Equals(SouthDoor, other.SouthDoor) &&
                   EqualityComparer<Door>.Default.Equals(TopDoor, other.TopDoor) &&
                   EqualityComparer<Door>.Default.Equals(BottomDoor, other.BottomDoor);
        }

        public override int GetHashCode()
        {
            int hashCode = -561712672;
            hashCode = hashCode * -1521134295 + IsEmpty.GetHashCode();
            hashCode = hashCode * -1521134295 + WestDoor.GetHashCode();
            hashCode = hashCode * -1521134295 + NorthDoor.GetHashCode();
            hashCode = hashCode * -1521134295 + EastDoor.GetHashCode();
            hashCode = hashCode * -1521134295 + SouthDoor.GetHashCode();
            hashCode = hashCode * -1521134295 + TopDoor.GetHashCode();
            hashCode = hashCode * -1521134295 + BottomDoor.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Cell left, Cell right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Cell left, Cell right)
        {
            return !(left == right);
        }
    }
}
