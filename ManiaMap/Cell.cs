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

        public override string ToString()
        {
            return $"Cell(WestDoor = {WestDoor}, NorthDoor = {NorthDoor}, EastDoor = {EastDoor}, SouthDoor = {SouthDoor})";
        }

        /// <summary>
        /// Numbers the doors of the cell based on the specified cell index.
        /// </summary>
        public void NumberDoors(int index)
        {
            var i = 6 * index;

            if (WestDoor != null)
                WestDoor.Id = i + 1;
            if (NorthDoor != null)
                NorthDoor.Id = i + 2;
            if (EastDoor != null)
                EastDoor.Id = i + 3;
            if (SouthDoor != null)
                SouthDoor.Id = i + 4;
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
            var rotation = new Cell();

            if (NorthDoor != null)
                rotation.EastDoor = new Door(NorthDoor.Type);
            if (EastDoor != null)
                rotation.SouthDoor = new Door(EastDoor.Type);
            if (SouthDoor != null)
                rotation.WestDoor = new Door(SouthDoor.Type);
            if (WestDoor != null)
                rotation.NorthDoor = new Door(WestDoor.Type);

            return rotation;
        }

        /// <summary>
        /// Returns a new cell rotated 180 degrees.
        /// </summary>
        public Cell Rotated180()
        {
            var rotation = new Cell();

            if (NorthDoor != null)
                rotation.SouthDoor = new Door(NorthDoor.Type);
            if (SouthDoor != null)
                rotation.NorthDoor = new Door(SouthDoor.Type);
            if (WestDoor != null)
                rotation.EastDoor = new Door(WestDoor.Type);
            if (EastDoor != null)
                rotation.WestDoor = new Door(EastDoor.Type);

            return rotation;
        }

        /// <summary>
        /// Returns a new cell rotated clockwise 270 degrees.
        /// </summary>
        public Cell Rotated270()
        {
            var rotation = new Cell();

            if (NorthDoor != null)
                rotation.WestDoor = new Door(NorthDoor.Type);
            if (EastDoor != null)
                rotation.NorthDoor = new Door(EastDoor.Type);
            if (SouthDoor != null)
                rotation.EastDoor = new Door(SouthDoor.Type);
            if (WestDoor != null)
                rotation.SouthDoor = new Door(WestDoor.Type);

            return rotation;
        }

        /// <summary>
        /// Returns a new cell mirrored vertically, i.e. about the horizontal axis.
        /// </summary>
        public Cell MirroredVertically()
        {
            var mirror = new Cell();

            if (NorthDoor != null)
                mirror.SouthDoor = new Door(NorthDoor.Type);
            if (SouthDoor != null)
                mirror.NorthDoor = new Door(SouthDoor.Type);
            if (WestDoor != null)
                mirror.WestDoor = new Door(WestDoor.Type);
            if (EastDoor != null)
                mirror.EastDoor = new Door(EastDoor.Type);

            return mirror;
        }

        /// <summary>
        /// Returns a new cell mirrored horizontally, i.e. about the vertical axis.
        /// </summary>s
        public Cell MirroredHorizontally()
        {
            var mirror = new Cell();

            if (NorthDoor != null)
                mirror.NorthDoor = new Door(NorthDoor.Type);
            if (SouthDoor != null)
                mirror.SouthDoor = new Door(SouthDoor.Type);
            if (WestDoor != null)
                mirror.EastDoor = new Door(WestDoor.Type);
            if (EastDoor != null)
                mirror.WestDoor = new Door(EastDoor.Type);

            return mirror;
        }
    }
}
