using MPewsey.Common.Collections;
using MPewsey.Common.Mathematics;
using MPewsey.Common.Random;
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
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class RoomTemplate : IDataContractValueDictionaryValue<int>
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
        /// A dictionary of collectable spots by ID.
        /// </summary>
        [DataMember(Order = 4)]
        public DataContractDictionary<int, CollectableSpot> CollectableSpots { get; private set; }

        /// <inheritdoc/>
        int IDataContractValueDictionaryValue<int>.Key => Id;

        /// <inheritdoc/>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            Cells = Cells ?? new Array2D<Cell>();
            CollectableSpots = CollectableSpots ?? new DataContractDictionary<int, CollectableSpot>();
        }

        /// <summary>
        /// Initializes a room template.
        /// </summary>
        /// <param name="id">The unique ID.</param>
        /// <param name="name">The template name.</param>
        /// <param name="cells">An array of cells in the template.</param>
        /// <param name="collectableSpots">A dictionary of collectable spots by ID.</param>
        public RoomTemplate(int id, string name, Array2D<Cell> cells,
            DataContractDictionary<int, CollectableSpot> collectableSpots = null)
        {
            Id = id;
            Name = name;
            Cells = cells;
            CollectableSpots = collectableSpots ?? new DataContractDictionary<int, CollectableSpot>();
        }

        /// <summary>
        /// Returns a copy of the room template.
        /// </summary>
        /// <param name="other">The room template to copy.</param>
        private RoomTemplate(RoomTemplate other)
        {
            Id = other.Id;
            Name = other.Name;
            Cells = CopyCells(other.Cells);
            CollectableSpots = other.CollectableSpots.ToDictionary(x => x.Key, x => x.Value.Copy());
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"RoomTemplate(Id = {Id}, Name = {Name})";
        }

        /// <summary>
        /// Returns a deep copy of a cells array.
        /// </summary>
        /// <param name="cells">The cells array to copy.</param>
        public static Array2D<Cell> CopyCells(Array2D<Cell> cells)
        {
            var result = new Array2D<Cell>(cells);

            for (int i = 0; i < result.Array.Length; i++)
            {
                result.Array[i] = result.Array[i]?.Copy();
            }

            return result;
        }

        /// <summary>
        /// Returns true if the values of the room templates are equal.
        /// </summary>
        /// <param name="x">The first template.</param>
        /// <param name="y">The second template.</param>
        public static bool ValuesAreEqual(RoomTemplate x, RoomTemplate y)
        {
            if (x == y)
                return true;

            if (x == null || y == null)
                return false;

            return x.ValuesAreEqual(y);
        }

        /// <summary>
        /// Returns true if the values of the room template are equal to the specified template.
        /// </summary>
        /// <param name="other">The other room template.</param>
        public bool ValuesAreEqual(RoomTemplate other)
        {
            return Id == other.Id
                && Name == other.Name
                && CellValuesAreEqual(other)
                && CollectableSpotValuesAreEqual(other);
        }

        /// <summary>
        /// Validates the template and raises any associated exceptions.
        /// </summary>
        public void Validate()
        {
            ValidateIsFullyConnected();
            ValidateAnyDoorExists();
            ValidateCollectableSpots();
        }

        /// <summary>
        /// Validates that the cells of the template are fully connected and throws an exception if not.
        /// </summary>
        /// <exception cref="CellsNotFullyConnectedException">Raised if the cells are not fully connected.</exception>
        private void ValidateIsFullyConnected()
        {
            if (!IsFullyConnected())
                throw new CellsNotFullyConnectedException($"Cells are not fully connected: {this}.");
        }

        /// <summary>
        /// Validates that any doors exist in the template and throws an exception if not.
        /// </summary>
        /// <exception cref="NoDoorsExistException">Raised if no typed door is assigned to the template.</exception>
        private void ValidateAnyDoorExists()
        {
            if (!AnyDoorExists())
                throw new NoDoorsExistException($"No doors exist in template: {this}.");
        }

        /// <summary>
        /// Checks that all collectable spots are valid.
        /// </summary>
        private void ValidateCollectableSpots()
        {
            foreach (var spot in CollectableSpots.Values)
            {
                spot.Validate(Cells);
            }
        }

        /// <summary>
        /// Returns true if the room template is valid.
        /// </summary>
        public bool IsValid()
        {
            return IsFullyConnected()
                && AnyDoorExists()
                && CollectableSpotsAreValid();
        }

        /// <summary>
        /// Returns true if all collectable spots are valid.
        /// </summary>
        private bool CollectableSpotsAreValid()
        {
            foreach (var spot in CollectableSpots.Values)
            {
                if (!spot.IsValid(Cells))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns true if a door is not null and is assigned a type
        /// anywhere in the template.
        /// </summary>
        public bool AnyDoorExists()
        {
            foreach (var cell in Cells.Array)
            {
                if (cell != null && cell.AnyDoorExists())
                    return true;
            }

            return false;
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
            return new Array2DDistanceSearch<Cell>().FindDistances(Cells, index.X, index.Y);
        }

        /// <summary>
        /// Adds a collectable spot to the template and returns the template.
        /// </summary>
        /// <param name="id">The collectable location ID.</param>
        /// <param name="position">The collectable index.</param>
        /// <param name="group">The collectable group.</param>
        public RoomTemplate AddCollectableSpot(int id, Vector2DInt position, string group)
        {
            return AddCollectableSpot(id, new CollectableSpot(position, group));
        }

        /// <summary>
        /// Adds a collectable spot to the template and returns the template.
        /// </summary>
        /// <param name="id">The collectable location ID.</param>
        /// <param name="spot">The collectable spot.</param>
        /// <exception cref="IndexOutOfRangeException">Raised if the collectable spot location is outside the bounds of the room.</exception>
        /// <exception cref="InvalidNameException">Raised if the collectable spot name is null or whitespace.</exception>
        public RoomTemplate AddCollectableSpot(int id, CollectableSpot spot)
        {
            spot.Validate(Cells);
            CollectableSpots.Add(id, spot);
            return this;
        }

        /// <summary>
        /// Returns an array of this template plus all mirrored and rotated variations.
        /// </summary>
        public List<RoomTemplate> AllVariations()
        {
            var random = new RandomSeed(Id);
            var horzMirror = MirroredHorizontally(random.Next());
            var vertMirror = MirroredVertically(random.Next());
            var fullMirror = horzMirror.MirroredVertically(random.Next());

            return new List<RoomTemplate>()
            {
                Copy(),
                Rotated90(random.Next()),
                Rotated180(random.Next()),
                Rotated270(random.Next()),
                horzMirror,
                horzMirror.Rotated90(random.Next()),
                horzMirror.Rotated180(random.Next()),
                horzMirror.Rotated270(random.Next()),
                vertMirror,
                vertMirror.Rotated90(random.Next()),
                vertMirror.Rotated180(random.Next()),
                vertMirror.Rotated270(random.Next()),
                fullMirror,
                fullMirror.Rotated90(random.Next()),
                fullMirror.Rotated180(random.Next()),
                fullMirror.Rotated270(random.Next()),
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
                if (!result.Any(x => x.IsEquivalentTo(template)))
                    result.Add(template);
            }

            return result;
        }

        /// <summary>
        /// Returns true if the cell and collectable spot values are equal to the specified template.
        /// </summary>
        /// <param name="other">The other template.</param>
        public bool IsEquivalentTo(RoomTemplate other)
        {
            return CellValuesAreEqual(other)
                && CollectableSpotValuesAreEqual(other);
        }

        /// <summary>
        /// Returns true if all cells in this template match the cells of another template.
        /// </summary>
        /// <param name="other">The other room template.</param>
        public bool CellValuesAreEqual(RoomTemplate other)
        {
            return Cells.ValuesAreEqual(other.Cells, Cell.ValuesAreEqual);
        }

        /// <summary>
        /// Returns true if all collectable spots in this template match the spots of another template.
        /// </summary>
        /// <param name="other">The other room template.</param>
        private bool CollectableSpotValuesAreEqual(RoomTemplate other)
        {
            if (CollectableSpots.Count != other.CollectableSpots.Count)
                return false;

            foreach (var pair in other.CollectableSpots)
            {
                if (!CollectableSpots.TryGetValue(pair.Key, out var spot) || !CollectableSpot.ValuesAreEqual(pair.Value, spot))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Returns a copy of the room template.
        /// </summary>
        public RoomTemplate Copy()
        {
            return new RoomTemplate(this);
        }

        /// <summary>
        /// Returns a new room template rotated 90 degrees clockwise.
        /// </summary>
        public RoomTemplate Rotated90(int id)
        {
            return new RoomTemplate(id, Name + "::Rotated90", CellsRotated90(), CollectableSpotsRotated90());
        }

        /// <summary>
        /// Returns an array of cells rotated 90 degress clockwise.
        /// </summary>
        private Array2D<Cell> CellsRotated90()
        {
            var cells = Cells.Rotated90();

            for (int i = 0; i < cells.Array.Length; i++)
            {
                cells.Array[i] = cells.Array[i]?.Rotated90();
            }

            return cells;
        }

        /// <summary>
        /// Returns a dictionary of collectable spots rotated 90 degrees clockwise.
        /// </summary>
        private DataContractDictionary<int, CollectableSpot> CollectableSpotsRotated90()
        {
            var spots = CollectableSpots.ToDictionary(x => x.Key, x => x.Value.Copy());

            foreach (var spot in spots.Values)
            {
                spot.Position = Cells.IndexRotated90(spot.Position);
            }

            return spots;
        }

        /// <summary>
        /// Returns a new room template rotated 180 degrees.
        /// </summary>
        public RoomTemplate Rotated180(int id)
        {
            return new RoomTemplate(id, Name + "::Rotated180", CellsRotated180(), CollectableSpotsRotated180());
        }

        /// <summary>
        /// Returns an array of cells rotated 180 degress.
        /// </summary>
        private Array2D<Cell> CellsRotated180()
        {
            var cells = Cells.Rotated180();

            for (int i = 0; i < cells.Array.Length; i++)
            {
                cells.Array[i] = cells.Array[i]?.Rotated180();
            }

            return cells;
        }

        /// <summary>
        /// Returns a dictionary of collectable spots rotated 180 degrees.
        /// </summary>
        private DataContractDictionary<int, CollectableSpot> CollectableSpotsRotated180()
        {
            var spots = CollectableSpots.ToDictionary(x => x.Key, x => x.Value.Copy());

            foreach (var spot in spots.Values)
            {
                spot.Position = Cells.IndexRotated180(spot.Position);
            }

            return spots;
        }

        /// <summary>
        /// Returns a new room template rotated 270 degrees clockwise.
        /// </summary>
        public RoomTemplate Rotated270(int id)
        {
            return new RoomTemplate(id, Name + "::Rotated270", CellsRotated270(), CollectableSpotsRotated270());
        }

        /// <summary>
        /// Returns an array of cells rotated 270 degress clockwise.
        /// </summary>
        private Array2D<Cell> CellsRotated270()
        {
            var cells = Cells.Rotated270();

            for (int i = 0; i < cells.Array.Length; i++)
            {
                cells.Array[i] = cells.Array[i]?.Rotated270();
            }

            return cells;
        }

        /// <summary>
        /// Returns a dictionary of collectable spots rotated 270 degrees clockwise.
        /// </summary>
        private DataContractDictionary<int, CollectableSpot> CollectableSpotsRotated270()
        {
            var spots = CollectableSpots.ToDictionary(x => x.Key, x => x.Value.Copy());

            foreach (var spot in spots.Values)
            {
                spot.Position = Cells.IndexRotated270(spot.Position);
            }

            return spots;
        }

        /// <summary>
        /// Returns a new room template mirrored vertically, i.e. about the horizontal axis.
        /// </summary>
        public RoomTemplate MirroredVertically(int id)
        {
            return new RoomTemplate(id, Name + "::MirroredVertically", CellsMirroredVertically(), CollectableSpotsMirroredVertically());
        }

        /// <summary>
        /// Returns an array of cells mirrored vertically.
        /// </summary>
        private Array2D<Cell> CellsMirroredVertically()
        {
            var cells = Cells.MirroredVertically();

            for (int i = 0; i < cells.Array.Length; i++)
            {
                cells.Array[i] = cells.Array[i]?.MirroredVertically();
            }

            return cells;
        }

        /// <summary>
        /// Returns a dictionary of collectable spots mirrored vertically.
        /// </summary>
        private DataContractDictionary<int, CollectableSpot> CollectableSpotsMirroredVertically()
        {
            var spots = CollectableSpots.ToDictionary(x => x.Key, x => x.Value.Copy());

            foreach (var spot in spots.Values)
            {
                spot.Position = Cells.IndexMirroredVertically(spot.Position);
            }

            return spots;
        }

        /// <summary>
        /// Returns a new room template mirrored horizontally, i.e. about the vertical axis.
        /// </summary>
        public RoomTemplate MirroredHorizontally(int id)
        {
            return new RoomTemplate(id, Name + "::MirroredHorizontally", CellsMirroredHorizontally(), CollectableSpotsMirroredHorizontally());
        }

        /// <summary>
        /// Returns an array of cells mirrored horizontally.
        /// </summary>
        private Array2D<Cell> CellsMirroredHorizontally()
        {
            var cells = Cells.MirroredHorizontally();

            for (int i = 0; i < cells.Array.Length; i++)
            {
                cells.Array[i] = cells.Array[i]?.MirroredHorizontally();
            }

            return cells;
        }

        /// <summary>
        /// Returns a dictionary of collectable spots mirrored horizontally.
        /// </summary>
        private DataContractDictionary<int, CollectableSpot> CollectableSpotsMirroredHorizontally()
        {
            var spots = CollectableSpots.ToDictionary(x => x.Key, x => x.Value.Copy());

            foreach (var spot in spots.Values)
            {
                spot.Position = Cells.IndexMirroredHorizontally(spot.Position);
            }

            return spots;
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
                            return true;
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
                            return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a list of doors aligning with a template at the specified offset
        /// when the templates are not in the same plane.
        /// </summary>
        /// <param name="other">The other room template.</param>
        /// <param name="offset">The offset of the other template from this one.</param>
        private List<DoorPair> ShaftAlignedDoors(RoomTemplate other, Vector2DInt offset)
        {
            var result = new List<DoorPair>();
            var jStart = Math.Max(0, offset.Y - 1);
            var jStop = Math.Min(Cells.Columns, other.Cells.Columns + offset.Y + 1);

            if (jStart >= jStop)
                return result;

            var iStart = Math.Max(0, offset.X - 1);
            var iStop = Math.Min(Cells.Rows, other.Cells.Rows + offset.X + 1);

            for (int i = iStart; i < iStop; i++)
            {
                for (int j = jStart; j < jStop; j++)
                {
                    var cell = Cells[i, j];

                    if (cell == null)
                        continue;

                    var index1 = new Vector2DInt(i, j);
                    var index2 = index1 - offset;
                    var vert = other.Cells.GetOrDefault(index2.X, index2.Y);

                    if (cell.TopDoorAligns(vert))
                    {
                        var door1 = new DoorPosition(index1, DoorDirection.Top, cell.TopDoor);
                        var door2 = new DoorPosition(index2, DoorDirection.Bottom, vert.BottomDoor);
                        result.Add(new DoorPair(door1, door2));
                    }

                    if (cell.BottomDoorAligns(vert))
                    {
                        var door1 = new DoorPosition(index1, DoorDirection.Bottom, cell.BottomDoor);
                        var door2 = new DoorPosition(index2, DoorDirection.Top, vert.TopDoor);
                        result.Add(new DoorPair(door1, door2));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a list of doors aligning with a template at the specified offset
        /// when the templates are in the same plane.
        /// </summary>
        /// <param name="other">The other room template.</param>
        /// <param name="offset">The offset of the other template from this one.</param>
        private List<DoorPair> PlaneAlignedDoors(RoomTemplate other, Vector2DInt offset)
        {
            var result = new List<DoorPair>();
            var jStart = Math.Max(0, offset.Y - 1);
            var jStop = Math.Min(Cells.Columns, other.Cells.Columns + offset.Y + 1);

            if (jStart >= jStop)
                return result;

            var iStart = Math.Max(0, offset.X - 1);
            var iStop = Math.Min(Cells.Rows, other.Cells.Rows + offset.X + 1);

            for (int i = iStart; i < iStop; i++)
            {
                for (int j = jStart; j < jStop; j++)
                {
                    var cell = Cells[i, j];

                    if (cell == null)
                        continue;

                    var index = new Vector2DInt(i, j);
                    var x = i - offset.X;
                    var y = j - offset.Y;

                    var west = other.Cells.GetOrDefault(x, y - 1);
                    var north = other.Cells.GetOrDefault(x - 1, y);
                    var east = other.Cells.GetOrDefault(x, y + 1);
                    var south = other.Cells.GetOrDefault(x + 1, y);

                    if (cell.WestDoorAligns(west))
                    {
                        var door1 = new DoorPosition(index, DoorDirection.West, cell.WestDoor);
                        var door2 = new DoorPosition(new Vector2DInt(x, y - 1), DoorDirection.East, west.EastDoor);
                        result.Add(new DoorPair(door1, door2));
                    }

                    if (cell.NorthDoorAligns(north))
                    {
                        var door1 = new DoorPosition(index, DoorDirection.North, cell.NorthDoor);
                        var door2 = new DoorPosition(new Vector2DInt(x - 1, y), DoorDirection.South, north.SouthDoor);
                        result.Add(new DoorPair(door1, door2));
                    }

                    if (cell.EastDoorAligns(east))
                    {
                        var door1 = new DoorPosition(index, DoorDirection.East, cell.EastDoor);
                        var door2 = new DoorPosition(new Vector2DInt(x, y + 1), DoorDirection.West, east.WestDoor);
                        result.Add(new DoorPair(door1, door2));
                    }

                    if (cell.SouthDoorAligns(south))
                    {
                        var door1 = new DoorPosition(index, DoorDirection.South, cell.SouthDoor);
                        var door2 = new DoorPosition(new Vector2DInt(x + 1, y), DoorDirection.North, south.NorthDoor);
                        result.Add(new DoorPair(door1, door2));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Returns a list of doors aligning with a template at the specified offset.
        /// </summary>
        /// <param name="other">The other room template.</param>
        /// <param name="offset">The offset of the other template from this one.</param>
        public List<DoorPair> AlignedDoors(RoomTemplate other, Vector2DInt offset)
        {
            if (Intersects(other, offset))
                return ShaftAlignedDoors(other, offset);

            return PlaneAlignedDoors(other, offset);
        }
    }
}
