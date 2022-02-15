using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class Cell
    {
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
            return $"Cell(WestDoor = {WestDoor}, NorthDoor = {NorthDoor}, EastDoor = {EastDoor}, SouthDoor = {SouthDoor}, TopDoor = {TopDoor}, BottomDoor = {BottomDoor})";
        }

        /// <summary>
        /// Sets the position properties of the doors assigned to the cells.
        /// </summary>
        public void SetDoorProperties(int x, int y)
        {
            WestDoor?.SetProperties(x, y, DoorDirection.West);
            EastDoor?.SetProperties(x, y, DoorDirection.East);
            NorthDoor?.SetProperties(x, y, DoorDirection.North);
            SouthDoor?.SetProperties(x, y, DoorDirection.South);
            TopDoor?.SetProperties(x, y, DoorDirection.Top);
            BottomDoor?.SetProperties(x, y, DoorDirection.Bottom);
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
        /// Returns true if the north door aligns with the south door of the specified cell.
        /// </summary>
        public bool NorthDoorAligns(Cell other)
        {
            return NorthDoor != null
                && other?.SouthDoor != null
                && Door.DoorTypesAligns(NorthDoor.Type, other.SouthDoor.Type);
        }

        /// <summary>
        /// Returns true if the south door aligns with the north door of the specified cell.
        /// </summary>
        public bool SouthDoorAligns(Cell other)
        {
            return SouthDoor != null
                && other?.NorthDoor != null
                && Door.DoorTypesAligns(SouthDoor.Type, other.NorthDoor.Type);
        }

        /// <summary>
        /// Returns true if the west door aligns with the east door of the specified cell.
        /// </summary>
        public bool WestDoorAligns(Cell other)
        {
            return WestDoor != null
                && other?.EastDoor != null
                && Door.DoorTypesAligns(WestDoor.Type, other.EastDoor.Type);
        }

        /// <summary>
        /// Returns true if the east door aligns with the west door of the specified cell.
        /// </summary>
        public bool EastDoorAligns(Cell other)
        {
            return EastDoor != null
                && other?.WestDoor != null
                && Door.DoorTypesAligns(EastDoor.Type, other.WestDoor.Type);
        }

        /// <summary>
        /// Returns a new cell rotated clockwise 90 degrees.
        /// </summary>
        public Cell Rotated90()
        {
            return new Cell
            {
                EastDoor = NorthDoor?.CopyType(),
                SouthDoor = EastDoor?.CopyType(),
                WestDoor = SouthDoor?.CopyType(),
                NorthDoor = WestDoor?.CopyType(),
                TopDoor = TopDoor?.CopyType(),
                BottomDoor = BottomDoor?.CopyType(),
            };
        }

        /// <summary>
        /// Returns a new cell rotated 180 degrees.
        /// </summary>
        public Cell Rotated180()
        {
            return new Cell
            {
                SouthDoor = NorthDoor?.CopyType(),
                NorthDoor = SouthDoor?.CopyType(),
                EastDoor = WestDoor?.CopyType(),
                WestDoor = EastDoor?.CopyType(),
                TopDoor = TopDoor?.CopyType(),
                BottomDoor = BottomDoor?.CopyType(),
            };
        }

        /// <summary>
        /// Returns a new cell rotated clockwise 270 degrees.
        /// </summary>
        public Cell Rotated270()
        {
            return new Cell
            {
                WestDoor = NorthDoor?.CopyType(),
                NorthDoor = EastDoor?.CopyType(),
                EastDoor = SouthDoor?.CopyType(),
                SouthDoor = WestDoor?.CopyType(),
                TopDoor = TopDoor?.CopyType(),
                BottomDoor = BottomDoor?.CopyType(),
            };
        }

        /// <summary>
        /// Returns a new cell mirrored vertically, i.e. about the horizontal axis.
        /// </summary>
        public Cell MirroredVertically()
        {
            return new Cell
            {
                SouthDoor = NorthDoor?.CopyType(),
                NorthDoor = SouthDoor?.CopyType(),
                WestDoor = WestDoor?.CopyType(),
                EastDoor = EastDoor?.CopyType(),
                TopDoor = TopDoor?.CopyType(),
                BottomDoor = BottomDoor?.CopyType(),
            };
        }

        /// <summary>
        /// Returns a new cell mirrored horizontally, i.e. about the vertical axis.
        /// </summary>s
        public Cell MirroredHorizontally()
        {
            return new Cell
            {
                NorthDoor = NorthDoor?.CopyType(),
                SouthDoor = SouthDoor?.CopyType(),
                WestDoor = EastDoor?.CopyType(),
                EastDoor = WestDoor?.CopyType(),
                TopDoor = TopDoor?.CopyType(),
                BottomDoor = BottomDoor?.CopyType(),
            };
        }
    }
}
