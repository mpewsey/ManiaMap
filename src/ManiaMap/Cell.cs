using MPewsey.ManiaMap.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A RoomTemplate cell element with references to door connections for that cell.
    /// </summary>
    [DataContract]
    public class Cell
    {
        /// <summary>
        /// Returns a new empty cell (null).
        /// </summary>
        public static Cell Empty => null;

        /// <summary>
        /// Returns a new cell with no doors set.
        /// </summary>
        public static Cell New => new Cell();

        /// <summary>
        /// The west door. Set to null if no door exists.
        /// </summary>
        [DataMember(Order = 1)]
        public Door WestDoor { get; set; }

        /// <summary>
        /// The north door. Set to null if no door exists.
        /// </summary>
        [DataMember(Order = 2)]
        public Door NorthDoor { get; set; }

        /// <summary>
        /// The east door. Set to null if no door exists.
        /// </summary>
        [DataMember(Order = 3)]
        public Door EastDoor { get; set; }

        /// <summary>
        /// The south door. Set to null if no door exists.
        /// </summary>
        [DataMember(Order = 4)]
        public Door SouthDoor { get; set; }

        /// <summary>
        /// The top door. Set to null if no door exists.
        /// </summary>
        [DataMember(Order = 5)]
        public Door TopDoor { get; set; }

        /// <summary>
        /// The bottom door. Set to null if no door exists.
        /// </summary>
        [DataMember(Order = 6)]
        public Door BottomDoor { get; set; }

        /// <summary>
        /// The collectable group name.
        /// </summary>
        [DataMember(Order = 7)]
        public List<Collectable> CollectableGroups { get; private set; } = new List<Collectable>();

        /// <summary>
        /// Initializes a new cell.
        /// </summary>
        public Cell()
        {

        }

        /// <summary>
        /// Initializes a copy of a cell.
        /// </summary>
        /// <param name="other">The cell to copy.</param>
        public Cell(Cell other)
        {
            EastDoor = other.EastDoor?.Copy();
            SouthDoor = other.SouthDoor?.Copy();
            WestDoor = other.WestDoor?.Copy();
            NorthDoor = other.NorthDoor?.Copy();
            TopDoor = other.TopDoor?.Copy();
            BottomDoor = other.BottomDoor?.Copy();
            CollectableGroups = new List<Collectable>(other.CollectableGroups);
        }

        /// <summary>
        /// Returns a new copy of the cell.
        /// </summary>
        public Cell Copy()
        {
            return new Cell(this);
        }

        public override string ToString()
        {
            var west = WestDoor?.ToString() ?? "None";
            var north = NorthDoor?.ToString() ?? "None";
            var south = SouthDoor?.ToString() ?? "None";
            var east = EastDoor?.ToString() ?? "None";
            var top = TopDoor?.ToString() ?? "None";
            var bottom = BottomDoor?.ToString() ?? "None";
            return $"Cell(WestDoor = {west}, NorthDoor = {north}, EastDoor = {east}, SouthDoor = {south}, TopDoor = {top}, BottomDoor = {bottom})";
        }

        /// <summary>
        /// Sets the doors of the cell based on specified direction characters.
        /// Returns the cell.
        /// </summary>
        /// <param name="directions">
        /// A string with the directional characters to assign. The characters may be any case.
        /// 
        /// * 'N' = North
        /// * 'S' = South
        /// * 'E' = East
        /// * 'W' = West
        /// * 'T' = Top
        /// * 'B' = Bottom
        /// </param>
        /// <param name="door">The door to be copied and assigned to each location.</param>
        /// <exception cref="UnhandledCaseException">Raised if a character in the directions string is invalid.</exception>
        public Cell SetDoors(string directions, Door door)
        {
            foreach (var direction in directions)
            {
                switch (direction)
                {
                    case 'N':
                    case 'n':
                        NorthDoor = door?.Copy();
                        break;
                    case 'S':
                    case 's':
                        SouthDoor = door?.Copy();
                        break;
                    case 'W':
                    case 'w':
                        WestDoor = door?.Copy();
                        break;
                    case 'E':
                    case 'e':
                        EastDoor = door?.Copy();
                        break;
                    case 'B':
                    case 'b':
                        BottomDoor = door?.Copy();
                        break;
                    case 'T':
                    case 't':
                        TopDoor = door?.Copy();
                        break;
                    default:
                        throw new UnhandledCaseException($"Unhandled character: {direction}.");
                }
            }

            return this;
        }

        /// <summary>
        /// Adds the collectable group and returns the cell.
        /// </summary>
        /// <param name="locationId">The location ID, unique to the cell.</param>
        /// <param name="group">The collectable group name.</param>
        /// <exception cref="InvalidNameException">Raised if the group name is null or white space.</exception>
        /// <exception cref="DuplicateIdException">Raised if the location ID already exists.</exception>
        public Cell AddCollectableGroup(int locationId, string group)
        {
            if (string.IsNullOrWhiteSpace(group))
                throw new InvalidNameException($"Group name is null or white space.");

            var index = CollectableGroups.FindIndex(x => x.Id == locationId);

            if (index >= 0)
                throw new DuplicateIdException($"Location ID already exists: {locationId}.");

            CollectableGroups.Add(new Collectable(locationId, group));
            return this;
        }

        /// <summary>
        /// Returns true if the top door aligns with the bottom door of the specified cell.
        /// </summary>
        /// <param name="other">The other cell.</param>
        public bool TopDoorAligns(Cell other)
        {
            return TopDoor != null
                && other?.BottomDoor != null
                && TopDoor.Aligns(other.BottomDoor);
        }

        /// <summary>
        /// Returns true if the bottom door aligns with the top door of the specified cell.
        /// </summary>
        /// <param name="other">The other cell.</param>
        public bool BottomDoorAligns(Cell other)
        {
            return BottomDoor != null
                && other?.TopDoor != null
                && BottomDoor.Aligns(other.TopDoor);
        }

        /// <summary>
        /// Returns true if the north door aligns with the south door of the specified cell.
        /// </summary>
        /// <param name="other">The other cell.</param>
        public bool NorthDoorAligns(Cell other)
        {
            return NorthDoor != null
                && other?.SouthDoor != null
                && NorthDoor.Aligns(other.SouthDoor);
        }

        /// <summary>
        /// Returns true if the south door aligns with the north door of the specified cell.
        /// </summary>
        /// <param name="other">The other cell.</param>
        public bool SouthDoorAligns(Cell other)
        {
            return SouthDoor != null
                && other?.NorthDoor != null
                && SouthDoor.Aligns(other.NorthDoor);
        }

        /// <summary>
        /// Returns true if the west door aligns with the east door of the specified cell.
        /// </summary>
        /// <param name="other">The other cell.</param>
        public bool WestDoorAligns(Cell other)
        {
            return WestDoor != null
                && other?.EastDoor != null
                && WestDoor.Aligns(other.EastDoor);
        }

        /// <summary>
        /// Returns true if the east door aligns with the west door of the specified cell.
        /// </summary>
        /// <param name="other">The other cell.</param>
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
                CollectableGroups = new List<Collectable>(CollectableGroups),
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
                CollectableGroups = new List<Collectable>(CollectableGroups),
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
                CollectableGroups = new List<Collectable>(CollectableGroups),
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
                CollectableGroups = new List<Collectable>(CollectableGroups),
            };
        }

        /// <summary>
        /// Returns a new cell mirrored horizontally, i.e. about the vertical axis.
        /// </summary>
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
                CollectableGroups = new List<Collectable>(CollectableGroups),
            };
        }

        /// <summary>
        /// Returns true if the values of the two cells are equal.
        /// </summary>
        /// <param name="x">The first cell.</param>
        /// <param name="y">The second cell.</param>
        public static bool ValuesAreEqual(Cell x, Cell y)
        {
            if (x == y)
                return true;

            if (x == null || y == null)
                return false;

            return x.ValuesAreEqual(y);
        }

        /// <summary>
        /// Returns true if the values of the two cells are equal.
        /// </summary>
        /// <param name="other">The other cell.</param>
        public bool ValuesAreEqual(Cell other)
        {
            return Door.ValuesAreEqual(TopDoor, other.TopDoor)
                && Door.ValuesAreEqual(BottomDoor, other.BottomDoor)
                && Door.ValuesAreEqual(NorthDoor, other.NorthDoor)
                && Door.ValuesAreEqual(SouthDoor, other.SouthDoor)
                && Door.ValuesAreEqual(EastDoor, other.EastDoor)
                && Door.ValuesAreEqual(WestDoor, other.WestDoor)
                && CollectableGroups.SequenceEqual(other.CollectableGroups);
        }
    }
}