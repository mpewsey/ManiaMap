﻿using MPewsey.Common.Collections;
using MPewsey.ManiaMap.Exceptions;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A RoomTemplate cell element with references to door connections for that cell.
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
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
        /// A dictionary of doors.
        /// </summary>
        [DataMember(Order = 1)]
        public HashMap<DoorDirection, Door> Doors { get; set; } = new HashMap<DoorDirection, Door>();

        /// <summary>
        /// A list of feature names.
        /// </summary>
        [DataMember(Order = 3)]
        public List<string> Features { get; set; } = new List<string>();

        /// <summary>
        /// The west door. Set to null if no door exists.
        /// </summary>
        public Door WestDoor { get => GetDoor(DoorDirection.West); set => SetDoor(DoorDirection.West, value); }

        /// <summary>
        /// The north door. Set to null if no door exists.
        /// </summary>
        public Door NorthDoor { get => GetDoor(DoorDirection.North); set => SetDoor(DoorDirection.North, value); }

        /// <summary>
        /// The east door. Set to null if no door exists.
        /// </summary>
        public Door EastDoor { get => GetDoor(DoorDirection.East); set => SetDoor(DoorDirection.East, value); }

        /// <summary>
        /// The south door. Set to null if no door exists.
        /// </summary>
        public Door SouthDoor { get => GetDoor(DoorDirection.South); set => SetDoor(DoorDirection.South, value); }

        /// <summary>
        /// The top door. Set to null if no door exists.
        /// </summary>
        public Door TopDoor { get => GetDoor(DoorDirection.Top); set => SetDoor(DoorDirection.Top, value); }

        /// <summary>
        /// The bottom door. Set to null if no door exists.
        /// </summary>
        public Door BottomDoor { get => GetDoor(DoorDirection.Bottom); set => SetDoor(DoorDirection.Bottom, value); }

        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Doors = Doors ?? new HashMap<DoorDirection, Door>();
            Features = Features ?? new List<string>();
        }

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
        private Cell(Cell other)
        {
            Doors = other.Doors.ToDictionary(x => x.Key, x => x.Value?.Copy());
            Features = new List<string>(other.Features);
        }

        /// <summary>
        /// Returns a new copy of the cell.
        /// </summary>
        public Cell Copy()
        {
            return new Cell(this);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            var doors = Doors.OrderBy(x => x.Key).Select(x => $"{x.Key}{x.Value}");
            return $"Cell(Doors = [{string.Join(", ", doors)}], Features = [{string.Join(", ", Features)}])";
        }

        /// <summary>
        /// Returns the door corresponding to the direction. If the door does not exist, returns null.
        /// </summary>
        /// <param name="direction">The door direction.</param>
        public Door GetDoor(DoorDirection direction)
        {
            Doors.TryGetValue(direction, out Door door);
            return door;
        }

        /// <summary>
        /// Sets the door direction. If the door is null, removes the entry from the dictionary.
        /// </summary>
        /// <param name="direction">The door direction.</param>
        /// <param name="door">The door.</param>
        public void SetDoor(DoorDirection direction, Door door)
        {
            if (door == null)
                Doors.Remove(direction);
            else
                Doors[direction] = door;
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
        /// Adds the name to the feature list if it doesn't already exist. Returns the cell.
        /// </summary>
        /// <param name="name"></param>
        public Cell AddFeature(string name)
        {
            if (!Features.Contains(name))
                Features.Add(name);

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
                Features = new List<string>(Features),
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
                Features = new List<string>(Features),
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
                Features = new List<string>(Features),
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
                Features = new List<string>(Features),
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
                Features = new List<string>(Features),
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
            return DoorValuesAreEqual(other)
                && FeatureValuesAreEqual(other);
        }

        /// <summary>
        /// Returns true if the values of the doors of the two cells are equal.
        /// </summary>
        /// <param name="other">The other cell.</param>
        private bool DoorValuesAreEqual(Cell other)
        {
            if (Doors.Count != other.Doors.Count)
                return false;

            foreach (var pair in other.Doors)
            {
                if (!Doors.TryGetValue(pair.Key, out Door value) || !Door.ValuesAreEqual(pair.Value, value))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if the values of the features of the two cells are equal.
        /// </summary>
        /// <param name="other">The other cell.</param>
        private bool FeatureValuesAreEqual(Cell other)
        {
            if (Features.Count != other.Features.Count)
                return false;

            for (int i = 0; i < Features.Count; i++)
            {
                if (Features[i] != other.Features[i])
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if any door is not null and is assigned a type.
        /// </summary>
        public bool AnyDoorExists()
        {
            foreach (var door in Doors.Values)
            {
                if (door != null)
                    return true;
            }

            return false;
        }
    }
}