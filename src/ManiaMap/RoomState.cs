using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Stores the state of a Room in a Layout.
    /// </summary>
    [DataContract]
    public class RoomState
    {
        /// <summary>
        /// The ID of the corresponding room.
        /// </summary>
        [DataMember(Order = 1)]
        public Uid Id { get; private set; }

        /// <summary>
        /// An array of room cell visibilities.
        /// </summary>
        [DataMember(Order = 2)]
        public Array2D<bool> VisibleCells { get; private set; }

        /// <summary>
        /// A set of acquired collectable location ID's.
        /// </summary>
        public HashSet<int> AcquiredCollectables { get; private set; } = new HashSet<int>();

        /// <summary>
        /// An enumerable of acquired collectable location ID's.
        /// </summary>
        [DataMember(Order = 3)]
        protected IEnumerable<int> AcquiredCollectableIds
        {
            get => AcquiredCollectables;
            set => AcquiredCollectables = new HashSet<int>(value);
        }

        /// <summary>
        /// A set of flags that are set for a room.
        /// </summary>
        public HashSet<int> Flags { get; private set; } = new HashSet<int>();

        /// <summary>
        /// An enumerable of flags that are set for the room.
        /// </summary>
        [DataMember(Order = 4)]
        protected IEnumerable<int> FlagIds
        {
            get => Flags;
            set => Flags = new HashSet<int>(value);
        }

        /// <summary>
        /// Initializes from a room.
        /// </summary>
        /// <param name="room">The room.</param>
        public RoomState(Room room)
        {
            Id = room.Id;
            VisibleCells = new Array2D<bool>(room.Template.Cells.Rows, room.Template.Cells.Columns);
        }

        /// <summary>
        /// Returns true if the specified index is within bounds and the cell is visible.
        /// </summary>
        /// <param name="index">The cell index.</param>
        public bool CellIsVisible(Vector2DInt index)
        {
            return CellIsVisible(index.X, index.Y);
        }

        /// <summary>
        /// Returns true if the specified index is within bounds and the cell is visible.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        public bool CellIsVisible(int row, int column)
        {
            return VisibleCells.GetOrDefault(row, column, false);
        }

        /// <summary>
        /// Sets the visibility of a cell. Returns true if the index is within bounds.
        /// </summary>
        /// <param name="index">The cell index.</param>
        /// <param name="value">The visibility.</param>
        public bool SetCellVisibility(Vector2DInt index, bool value)
        {
            return SetCellVisibility(index.X, index.Y, value);
        }

        /// <summary>
        /// Sets the visibility of a cell. Returns true if the index is within bounds.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        /// <param name="value">The visibility.</param>
        public bool SetCellVisibility(int row, int column, bool value)
        {
            if (VisibleCells.IndexExists(row, column))
            {
                VisibleCells[row, column] = value;
                return true;
            }

            return false;
        }
    }
}
