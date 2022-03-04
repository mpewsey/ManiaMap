using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract(IsReference = true)]
    public class RoomTemplate
    {
        [DataMember(Order = 1)]
        public int Id { get; private set; }

        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;

        [DataMember(Order = 3)]
        public Array2D<Cell> Cells { get; private set; }

        public RoomTemplate(int id, string name, Array2D<Cell> cells)
        {
            Id = id;
            Name = name;
            Cells = cells;
        }

        public override string ToString()
        {
            return $"RoomTemplate(Id = {Id}, Name = {Name})";
        }

        /// <summary>
        /// Returns an array of this template plus all mirrored and rotated variations.
        /// </summary>
        public List<RoomTemplate> AllVariations()
        {
            var horzMirror = MirroredHorizontally();
            var vertMirror = MirroredVertically();
            var fullMirror = horzMirror.MirroredVertically();

            return new List<RoomTemplate>()
            {
                this,
                Rotated90(),
                Rotated180(),
                Rotated270(),
                horzMirror,
                horzMirror.Rotated90(),
                horzMirror.Rotated180(),
                horzMirror.Rotated270(),
                vertMirror,
                vertMirror.Rotated90(),
                vertMirror.Rotated180(),
                vertMirror.Rotated270(),
                fullMirror,
                fullMirror.Rotated90(),
                fullMirror.Rotated180(),
                fullMirror.Rotated270(),
            };
        }

        /// <summary>
        /// Returns a new list of this template plus all unique variations.
        /// </summary>
        public List<RoomTemplate> UniqueVariations()
        {
            var templates = AllVariations();
            var result = new List<RoomTemplate>();

            foreach (var template in templates)
            {
                if (!result.Any(x => x.CellsMatch(template)))
                {
                    result.Add(template);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns true if all cells in this template match the cells of another template.
        /// </summary>
        public bool CellsMatch(RoomTemplate other)
        {
            if (Cells.Rows != other.Cells.Rows || Cells.Columns != other.Cells.Columns)
                return false;

            for (int i = 0; i < Cells.Rows; i++)
            {
                for (int j = 0; j < Cells.Columns; j++)
                {
                    var x = Cells[i, j];
                    var y = other.Cells[i, j];

                    if ((x == null) != (y == null))
                        return false;

                    if (x != null && y != null && !x.Matches(y))
                        return false;
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a new room template rotated 90 degrees clockwise.
        /// </summary>
        public RoomTemplate Rotated90()
        {
            var cells = Cells.Rotated90();

            for (int i = 0; i < cells.Array.Length; i++)
            {
                cells.Array[i] = cells.Array[i]?.Rotated90();
            }

            return new RoomTemplate(Id, Name + "_Rotated90", cells);
        }

        /// <summary>
        /// Returns a new room template rotated 180 degrees.
        /// </summary>
        public RoomTemplate Rotated180()
        {
            var cells = Cells.Rotated180();

            for (int i = 0; i < cells.Array.Length; i++)
            {
                cells.Array[i] = cells.Array[i]?.Rotated180();
            }

            return new RoomTemplate(Id, Name + "_Rotated180", cells);
        }

        /// <summary>
        /// Returns a new room template rotated 270 degrees clockwise.
        /// </summary>
        public RoomTemplate Rotated270()
        {
            var cells = Cells.Rotated180();

            for (int i = 0; i < cells.Array.Length; i++)
            {
                cells.Array[i] = cells.Array[i]?.Rotated270();
            }

            return new RoomTemplate(Id, Name + "_Rotated270", cells);
        }

        /// <summary>
        /// Returns a new room template mirrored vertically, i.e. about the horizontal axis.
        /// </summary>
        public RoomTemplate MirroredVertically()
        {
            var cells = Cells.MirroredVertically();

            for (int i = 0; i < cells.Array.Length; i++)
            {
                cells.Array[i] = cells.Array[i]?.MirroredVertically();
            }

            return new RoomTemplate(Id, Name + "_MirroredVertically", cells);
        }

        /// <summary>
        /// Returns a new room template mirrored horizontally, i.e. about the vertical axis.
        /// </summary>
        public RoomTemplate MirroredHorizontally()
        {
            var cells = Cells.MirroredVertically();

            for (int i = 0; i < cells.Array.Length; i++)
            {
                cells.Array[i] = cells.Array[i]?.MirroredHorizontally();
            }

            return new RoomTemplate(Id, Name + "_MirroredHorizontally", cells);
        }

        /// <summary>
        /// Returns true if the template intersects the specified range.
        /// </summary>
        public bool Intersects(int xMin, int xMax, int yMin, int yMax)
        {
            var jStart = Math.Max(xMin, 0);
            var jStop = Math.Min(xMax + 1, Cells.Rows);

            if (jStart < jStop)
            {
                var iStart = Math.Max(yMin, 0);
                var iStop = Math.Min(yMax + 1, Cells.Columns);

                for (int i = iStart; i < iStop; i++)
                {
                    for (int j = jStart; j < jStop; j++)
                    {
                        if (Cells[i, j] != null)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns true if the room templates another template at the specified offset.
        /// </summary>
        public bool Intersects(RoomTemplate other, int dx, int dy)
        {
            var jStart = Math.Max(0, dy);
            var jStop = Math.Min(Cells.Columns, other.Cells.Columns + dy);

            if (jStart < jStop)
            {
                var iStart = Math.Max(0, dx);
                var iStop = Math.Min(Cells.Rows, other.Cells.Rows + dx);

                for (int i = iStart; i < iStop; i++)
                {
                    for (int j = jStart; j < jStop; j++)
                    {
                        if (Cells[i, j] != null && other.Cells[i - dx, j - dy] != null)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a list of doors aligning with a template at the specified offset.
        /// </summary>
        public List<DoorPair> AlignedDoors(RoomTemplate other, int dx, int dy)
        {
            var result = new List<DoorPair>();
            var jStart = Math.Max(0, dy - 1);
            var jStop = Math.Min(Cells.Columns, other.Cells.Columns + dy + 1);

            if (jStart >= jStop)
                return result;

            var iStart = Math.Max(0, dx - 1);
            var iStop = Math.Min(Cells.Rows, other.Cells.Rows + dx + 1);
            var intersects = Intersects(other, dx, dy);

            for (int i = iStart; i < iStop; i++)
            {
                for (int j = jStart; j < jStop; j++)
                {
                    var cell = Cells[i, j];

                    if (cell == null)
                        continue;

                    var x = i - dx;
                    var y = j - dy;

                    if (intersects)
                    {
                        var vert = other.Cells.GetOrDefault(x, y);

                        if (cell.TopDoorAligns(vert))
                        {
                            var door1 = new DoorPosition(i, j, DoorDirection.Top, cell.TopDoor);
                            var door2 = new DoorPosition(x, y, DoorDirection.Bottom, vert.BottomDoor);
                            result.Add(new DoorPair(door1, door2));
                        }

                        if (cell.BottomDoorAligns(vert))
                        {
                            var door1 = new DoorPosition(i, j, DoorDirection.Bottom, cell.BottomDoor);
                            var door2 = new DoorPosition(x, y, DoorDirection.Top, vert.TopDoor);
                            result.Add(new DoorPair(door1, door2));
                        }
                    }
                    else
                    {
                        var north = other.Cells.GetOrDefault(x - 1, y);
                        var south = other.Cells.GetOrDefault(x + 1, y);
                        var west = other.Cells.GetOrDefault(x, y - 1);
                        var east = other.Cells.GetOrDefault(x, y + 1);

                        if (cell.WestDoorAligns(west))
                        {
                            var door1 = new DoorPosition(i, j, DoorDirection.West, cell.WestDoor);
                            var door2 = new DoorPosition(x, y - 1, DoorDirection.East, west.EastDoor);
                            result.Add(new DoorPair(door1, door2));
                        }

                        if (cell.NorthDoorAligns(north))
                        {
                            var door1 = new DoorPosition(i, j, DoorDirection.North, cell.NorthDoor);
                            var door2 = new DoorPosition(x - 1, y, DoorDirection.South, north.SouthDoor);
                            result.Add(new DoorPair(door1, door2));
                        }

                        if (cell.EastDoorAligns(east))
                        {
                            var door1 = new DoorPosition(i, j, DoorDirection.East, cell.EastDoor);
                            var door2 = new DoorPosition(x, y + 1, DoorDirection.West, east.WestDoor);
                            result.Add(new DoorPair(door1, door2));
                        }

                        if (cell.SouthDoorAligns(south))
                        {
                            var door1 = new DoorPosition(i, j, DoorDirection.South, cell.SouthDoor);
                            var door2 = new DoorPosition(x + 1, y, DoorDirection.North, south.NorthDoor);
                            result.Add(new DoorPair(door1, door2));
                        }
                    }
                }
            }

            return result;
        }
    }
}
