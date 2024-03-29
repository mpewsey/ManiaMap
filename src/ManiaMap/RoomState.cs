﻿using MPewsey.Common.Collections;
using MPewsey.Common.Mathematics;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Stores the state of a Room in a Layout.
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class RoomState : IValueHashMapEntry<Uid>
    {
        /// <summary>
        /// The ID of the corresponding room.
        /// </summary>
        [DataMember(Order = 1)]
        public Uid Id { get; private set; }

        /// <summary>
        /// If true, all cells of the room will be visible without the individual cells being marked visible.
        /// </summary>
        [DataMember(Order = 2)]
        public bool IsVisible { get; set; }

        /// <summary>
        /// An array of room cell visibilities.
        /// </summary>
        [DataMember(Order = 3)]
        public BitArray2D VisibleCells { get; private set; }

        /// <summary>
        /// A set of acquired collectable location ID's.
        /// </summary>
        [DataMember(Order = 4)]
        public Set<int> AcquiredCollectables { get; private set; } = new Set<int>();

        /// <summary>
        /// A set of flags that are set for a room.
        /// </summary>
        [DataMember(Order = 5)]
        public Set<int> Flags { get; private set; } = new Set<int>();

        /// <inheritdoc/>
        Uid IValueHashMapEntry<Uid>.Key => Id;

        /// <inheritdoc/>
        [OnDeserialized]
        private void OnDeserialized(StreamingContext context)
        {
            VisibleCells = VisibleCells ?? new BitArray2D();
            AcquiredCollectables = AcquiredCollectables ?? new Set<int>();
            Flags = Flags ?? new Set<int>();
        }

        /// <summary>
        /// Initializes from a room.
        /// </summary>
        /// <param name="room">The room.</param>
        public RoomState(Room room)
        {
            Id = room.Id;
            var cells = room.Template.Cells;
            VisibleCells = new BitArray2D(cells.Rows, cells.Columns);
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
