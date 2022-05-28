using MPewsey.ManiaMap.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Contains information for the geometry and properties of a Room in a Layout.
    /// </summary>
    [DataContract(IsReference = true)]
    public class RoomTemplate
    {
        /// <summary>
        /// The unique ID.
        /// </summary>
        [DataMember(Order = 1)]
        public int Id { get; private set; }

        /// <summary>
        /// The template name.
        /// </summary>
        [DataMember(Order = 2)]
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// An array of cells in the template. The shape and contents of this array define the geometry
        /// and properties of the room.
        /// </summary>
        [DataMember(Order = 3)]
        public Array2D<Cell> Cells { get; private set; }

        /// <summary>
        /// Initializes a room template.
        /// </summary>
        /// <param name="id">The unique ID.</param>
        /// <param name="name">The template name.</param>
        /// <param name="cells">An array of cells in the template.</param>
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
        /// Validates the template and raises any associated exceptions.
        /// </summary>
        /// <exception cref="CellsNotFullyConnectedException">Raised if the cells are not fully connected.</exception>
        public void Validate()
        {
            if (!IsFullyConnected())
                throw new CellsNotFullyConnectedException($"Cells are not fully connected: {this}.");
        }

        /// <summary>
        /// Returns true if the cells in the template are fully connected.
        /// </summary>
        public bool IsFullyConnected()
        {
            var index = Cells.FindIndex(x => x != null);

            if (index.X < 0 || index.Y < 0)
                return true;

            var cellCount = Cells.Array.Count(x => x != null);
            var distances = FindCellDistances(index);
            var connectedCount = distances.Array.Count(x => x >= 0);
            return cellCount == connectedCount;
        }

        /// <summary>
        /// Returns an array of distances from the specified index to each cell of the template.
        /// Values of -1 indicate that the index does not exist.
        /// </summary>
        /// <param name="index">The index for which distances will be calculated.</param>
        public Array2D<int> FindCellDistances(Vector2DInt index)
        {
            return new CellDistanceSearch().FindCellDistances(Cells, index);
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
                Copy(),
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
                if (!result.Any(x => x.CellValuesAreEqual(template)))
                {
                    result.Add(template);
                }
            }

            return result;
        }

        /// <summary>
        /// Returns true if all cells in this template match the cells of another template.
        /// </summary>
        /// <param name="other">The other room template.</param>
        public bool CellValuesAreEqual(RoomTemplate other)
        {
            if (Cells.Rows != other.Cells.Rows || Cells.Columns != other.Cells.Columns)
                return false;

            for (int i = 0; i < Cells.Rows; i++)
            {
                for (int j = 0; j < Cells.Columns; j++)
                {
                    if (!Cell.ValuesAreEqual(Cells[i, j], other.Cells[i, j]))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Returns a copy of the room template.
        /// </summary>
        public RoomTemplate Copy()
        {
            var cells = new Array2D<Cell>(Cells.Rows, Cells.Columns);

            for (int i = 0; i < cells.Array.Length; i++)
            {
                cells.Array[i] = Cells.Array[i]?.Copy();
            }

            return new RoomTemplate(Id, Name, cells);
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
        /// <param name="min">The minimum values of the range.</param>
        /// <param name="max">The maximum values of the range.</param>
        public bool Intersects(Vector2DInt min, Vector2DInt max)
        {
            var jStart = Math.Max(min.X, 0);
            var jStop = Math.Min(max.X + 1, Cells.Rows);

            if (jStart < jStop)
            {
                var iStart = Math.Max(min.Y, 0);
                var iStop = Math.Min(max.Y + 1, Cells.Columns);

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
        /// <param name="other">The other room template.</param>
        /// <param name="offset">The offset of the other template from this one.</param>
        public bool Intersects(RoomTemplate other, Vector2DInt offset)
        {
            var jStart = Math.Max(0, offset.Y);
            var jStop = Math.Min(Cells.Columns, other.Cells.Columns + offset.Y);

            if (jStart < jStop)
            {
                var iStart = Math.Max(0, offset.X);
                var iStop = Math.Min(Cells.Rows, other.Cells.Rows + offset.X);

                for (int i = iStart; i < iStop; i++)
                {
                    for (int j = jStart; j < jStop; j++)
                    {
                        if (Cells[i, j] != null && other.Cells[i - offset.X, j - offset.Y] != null)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns an enumerable of doors aligning with a template at the specified offset.
        /// </summary>
        /// <param name="other">The other room template.</param>
        /// <param name="offset">The offset of the other template from this one.</param>
        public IEnumerable<DoorPair> AlignedDoors(RoomTemplate other, Vector2DInt offset)
        {
            var jStart = Math.Max(0, offset.Y - 1);
            var jStop = Math.Min(Cells.Columns, other.Cells.Columns + offset.Y + 1);

            if (jStart >= jStop)
                yield break;

            var iStart = Math.Max(0, offset.X - 1);
            var iStop = Math.Min(Cells.Rows, other.Cells.Rows + offset.X + 1);
            var intersects = Intersects(other, offset);

            for (int i = iStart; i < iStop; i++)
            {
                for (int j = jStart; j < jStop; j++)
                {
                    var cell = Cells[i, j];

                    if (cell == null)
                        continue;

                    var x = i - offset.X;
                    var y = j - offset.Y;

                    if (intersects)
                    {
                        var top = other.Cells.GetOrDefault(x, y);

                        if (cell.TopDoorAligns(top))
                        {
                            var door1 = new DoorPosition(new Vector2DInt(i, j), DoorDirection.Top, cell.TopDoor);
                            var door2 = new DoorPosition(new Vector2DInt(x, y), DoorDirection.Bottom, top.BottomDoor);
                            yield return new DoorPair(door1, door2);
                        }

                        var bottom = other.Cells.GetOrDefault(x, y);

                        if (cell.BottomDoorAligns(bottom))
                        {
                            var door1 = new DoorPosition(new Vector2DInt(i, j), DoorDirection.Bottom, cell.BottomDoor);
                            var door2 = new DoorPosition(new Vector2DInt(x, y), DoorDirection.Top, bottom.TopDoor);
                            yield return new DoorPair(door1, door2);
                        }
                    }
                    else
                    {
                        var west = other.Cells.GetOrDefault(x, y - 1);

                        if (cell.WestDoorAligns(west))
                        {
                            var door1 = new DoorPosition(new Vector2DInt(i, j), DoorDirection.West, cell.WestDoor);
                            var door2 = new DoorPosition(new Vector2DInt(x, y - 1), DoorDirection.East, west.EastDoor);
                            yield return new DoorPair(door1, door2);
                        }

                        var north = other.Cells.GetOrDefault(x - 1, y);

                        if (cell.NorthDoorAligns(north))
                        {
                            var door1 = new DoorPosition(new Vector2DInt(i, j), DoorDirection.North, cell.NorthDoor);
                            var door2 = new DoorPosition(new Vector2DInt(x - 1, y), DoorDirection.South, north.SouthDoor);
                            yield return new DoorPair(door1, door2);
                        }

                        var east = other.Cells.GetOrDefault(x, y + 1);

                        if (cell.EastDoorAligns(east))
                        {
                            var door1 = new DoorPosition(new Vector2DInt(i, j), DoorDirection.East, cell.EastDoor);
                            var door2 = new DoorPosition(new Vector2DInt(x, y + 1), DoorDirection.West, east.WestDoor);
                            yield return new DoorPair(door1, door2);
                        }

                        var south = other.Cells.GetOrDefault(x + 1, y);

                        if (cell.SouthDoorAligns(south))
                        {
                            var door1 = new DoorPosition(new Vector2DInt(i, j), DoorDirection.South, cell.SouthDoor);
                            var door2 = new DoorPosition(new Vector2DInt(x + 1, y), DoorDirection.North, south.NorthDoor);
                            yield return new DoorPair(door1, door2);
                        }
                    }
                }
            }
        }
    }
}
