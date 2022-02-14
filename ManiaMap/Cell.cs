using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class Cell
    {
        [DataMember(Order = 1)]
        public Door LeftDoor { get; set; }

        [DataMember(Order = 2)]
        public Door TopDoor { get; set; }

        [DataMember(Order = 3)]
        public Door RightDoor { get; set; }

        [DataMember(Order = 4)]
        public Door BottomDoor { get; set; }

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
                BottomDoor.Id = i + 4;
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

        /// <summary>
        /// Returns a new cell rotated clockwise 90 degrees.
        /// </summary>
        public Cell Rotated90()
        {
            var rotation = new Cell();

            if (TopDoor != null)
                rotation.RightDoor = new Door(TopDoor.Type);
            if (RightDoor != null)
                rotation.BottomDoor = new Door(RightDoor.Type);
            if (BottomDoor != null)
                rotation.LeftDoor = new Door(BottomDoor.Type);
            if (LeftDoor != null)
                rotation.TopDoor = new Door(LeftDoor.Type);

            return rotation;
        }

        /// <summary>
        /// Returns a new cell rotated 180 degrees.
        /// </summary>
        public Cell Rotated180()
        {
            var rotation = new Cell();

            if (TopDoor != null)
                rotation.BottomDoor = new Door(TopDoor.Type);
            if (BottomDoor != null)
                rotation.TopDoor = new Door(BottomDoor.Type);
            if (LeftDoor != null)
                rotation.RightDoor = new Door(LeftDoor.Type);
            if (RightDoor != null)
                rotation.LeftDoor = new Door(RightDoor.Type);

            return rotation;
        }

        /// <summary>
        /// Returns a new cell rotated clockwise 270 degrees.
        /// </summary>
        public Cell Rotated270()
        {
            var rotation = new Cell();

            if (TopDoor != null)
                rotation.LeftDoor = new Door(TopDoor.Type);
            if (RightDoor != null)
                rotation.TopDoor = new Door(RightDoor.Type);
            if (BottomDoor != null)
                rotation.RightDoor = new Door(BottomDoor.Type);
            if (LeftDoor != null)
                rotation.BottomDoor = new Door(LeftDoor.Type);

            return rotation;
        }

        /// <summary>
        /// Returns a new cell mirrored vertically, i.e. about the horizontal axis.
        /// </summary>
        public Cell MirroredVertically()
        {
            var mirror = new Cell();

            if (TopDoor != null)
                mirror.BottomDoor = new Door(TopDoor.Type);
            if (BottomDoor != null)
                mirror.TopDoor = new Door(BottomDoor.Type);
            if (LeftDoor != null)
                mirror.LeftDoor = new Door(LeftDoor.Type);
            if (RightDoor != null)
                mirror.RightDoor = new Door(RightDoor.Type);

            return mirror;
        }

        /// <summary>
        /// Returns a new cell mirrored horizontally, i.e. about the vertical axis.
        /// </summary>s
        public Cell MirroredHorizontally()
        {
            var mirror = new Cell();

            if (TopDoor != null)
                mirror.TopDoor = new Door(TopDoor.Type);
            if (BottomDoor != null)
                mirror.BottomDoor = new Door(BottomDoor.Type);
            if (LeftDoor != null)
                mirror.RightDoor = new Door(LeftDoor.Type);
            if (RightDoor != null)
                mirror.LeftDoor = new Door(RightDoor.Type);

            return mirror;
        }
    }
}
