using MPewsey.ManiaMap.Exceptions;
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
        /// A dictionary of doors.
        /// </summary>
        [DataMember(Order = 1)]
        public Dictionary<DoorDirection, Door> Doors { get; set; } = new Dictionary<DoorDirection, Door>();

        /// <summary>
        /// A dictionary of collectable group names by location ID.
        /// </summary>
        [DataMember(Order = 2)]
        public Dictionary<int, string> CollectableSpots { get; set; } = new Dictionary<int, string>();

        /// <summary>
        /// The west door. Set to null if no door exists.
        /// </summary>
        [IgnoreDataMember]
        public Door WestDoor { get => GetDoor(DoorDirection.West); set => SetDoor(DoorDirection.West, value); }

        /// <summary>
        /// The north door. Set to null if no door exists.
        /// </summary>
        [IgnoreDataMember]
        public Door NorthDoor { get => GetDoor(DoorDirection.North); set => SetDoor(DoorDirection.North, value); }

        /// <summary>
        /// The east door. Set to null if no door exists.
        /// </summary>
        [IgnoreDataMember]
        public Door EastDoor { get => GetDoor(DoorDirection.East); set => SetDoor(DoorDirection.East, value); }

        /// <summary>
        /// The south door. Set to null if no door exists.
        /// </summary>
        [IgnoreDataMember]
        public Door SouthDoor { get => GetDoor(DoorDirection.South); set => SetDoor(DoorDirection.South, value); }

        /// <summary>
        /// The top door. Set to null if no door exists.
        /// </summary>
        [IgnoreDataMember]
        public Door TopDoor { get => GetDoor(DoorDirection.Top); set => SetDoor(DoorDirection.Top, value); }

        /// <summary>
        /// The bottom door. Set to null if no door exists.
        /// </summary>
        [IgnoreDataMember]
        public Door BottomDoor { get => GetDoor(DoorDirection.Bottom); set => SetDoor(DoorDirection.Bottom, value); }

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
            Doors = other.Doors.ToDictionary(x => x.Key, x => x.Value?.Copy());
            CollectableSpots = new Dictionary<int, string>(other.CollectableSpots);
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
        /// Returns the door corresponding to the direction. If the door does not exist, returns null.
        /// </summary>
        /// <param name="direction">The door direction.</param>
        private Door GetDoor(DoorDirection direction)
        {
            Doors.TryGetValue(direction, out Door door);
            return door;
        }

        /// <summary>
        /// Sets the door direction. If the door is null, removes the entry from the dictionary.
        /// </summary>
        /// <param name="direction">The door direction.</param>
        /// <param name="door">The door.</param>
        private void SetDoor(DoorDirection direction, Door door)
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
        /// Adds the collectable spot and returns the cell.
        /// </summary>
        /// <param name="id">The location ID, unique to the cell.</param>
        /// <param name="group">The collectable group name.</param>
        /// <exception cref="InvalidNameException">Raised if the group name is null or white space.</exception>
        /// <exception cref="DuplicateIdException">Raised if the location ID already exists.</exception>
        public Cell AddCollectableSpot(int id, string group)
        {
            if (string.IsNullOrWhiteSpace(group))
                throw new InvalidNameException($"Group name is null or white space.");

            if (CollectableSpots.ContainsKey(id))
                throw new DuplicateIdException($"Location ID already exists: {id}.");

            CollectableSpots.Add(id, group);
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
                CollectableSpots = new Dictionary<int, string>(CollectableSpots),
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
                CollectableSpots = new Dictionary<int, string>(CollectableSpots),
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
                CollectableSpots = new Dictionary<int, string>(CollectableSpots),
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
                CollectableSpots = new Dictionary<int, string>(CollectableSpots),
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
                CollectableSpots = new Dictionary<int, string>(CollectableSpots),
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
            if (Doors.Count != other.Doors.Count)
                return false;

            if (CollectableSpots.Count != other.CollectableSpots.Count)
                return false;

            foreach (var pair in Doors)
            {
                if (!other.Doors.TryGetValue(pair.Key, out Door value))
                    return false;
                if (!Door.ValuesAreEqual(pair.Value, value))
                    return false;
            }

            foreach (var pair in CollectableSpots)
            {
                if (!other.CollectableSpots.TryGetValue(pair.Key, out string value))
                    return false;
                if (pair.Value != value)
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