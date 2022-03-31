namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for calculating distances between cells.
    /// </summary>
    public static class CellDistanceSearch
    {
        /// <summary>
        /// A construct for storing search data.
        /// </summary>
        private struct Data
        {
            /// <summary>
            /// The array of cells.
            /// </summary>
            public Array2D<Cell> Cells { get; }

            /// <summary>
            /// The arary of distances.
            /// </summary>
            public Array2D<int> Distances { get; }

            /// <summary>
            /// Initializes the data.
            /// </summary>
            /// <param name="cells">An array of cells.</param>
            public Data(Array2D<Cell> cells)
            {
                Cells = cells;
                Distances = new Array2D<int>(cells.Rows, cells.Columns);
                Distances.Fill(-1);
            }
        }

        /// <summary>
        /// Returns an array of distances from the specified index to each cell.
        /// Values of -1 indicate that the index does not exist.
        /// </summary>
        /// <param name="cells">An array of cells.</param>
        /// <param name="index">The index for which distances will be calculated.</param>
        public static Array2D<int> FindCellDistances(Array2D<Cell> cells, Vector2DInt index)
        {
            var data = new Data(cells);
            SearchCellDistances(data, index, 0);
            return data.Distances;
        }

        /// <summary>
        /// Performs a recursive crawl of the template cells to determine the distance to an index.
        /// </summary>
        /// <param name="index">The current index.</param>
        /// <param name="distance">The distance to the current index.</param>
        private static void SearchCellDistances(Data data, Vector2DInt index, int distance)
        {
            if (data.Cells.GetOrDefault(index.X, index.Y) == null)
                return;

            var currentDistance = data.Distances[index.X, index.Y];

            if (currentDistance >= 0 && currentDistance <= distance)
                return;

            data.Distances[index.X, index.Y] = distance;
            SearchCellDistances(data, new Vector2DInt(index.X - 1, index.Y), distance + 1);
            SearchCellDistances(data, new Vector2DInt(index.X, index.Y - 1), distance + 1);
            SearchCellDistances(data, new Vector2DInt(index.X, index.Y + 1), distance + 1);
            SearchCellDistances(data, new Vector2DInt(index.X + 1, index.Y), distance + 1);
        }
    }
}
