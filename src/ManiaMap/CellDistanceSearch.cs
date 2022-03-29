namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for calculating distances between cells.
    /// </summary>
    public class CellDistanceSearch
    {
        /// <summary>
        /// The array of cells.
        /// </summary>
        public Array2D<Cell> Cells { get; set; }

        /// <summary>
        /// The array of distances.
        /// </summary>
        private Array2D<int> Distances { get; set; }

        /// <summary>
        /// Initializes a new search.
        /// </summary>
        /// <param name="cells">An array of cells.</param>
        public CellDistanceSearch(Array2D<Cell> cells)
        {
            Cells = cells;
        }

        /// <summary>
        /// Returns an array of distances from the specified index to each cell.
        /// Values of -1 indicate that the index does not exist.
        /// </summary>
        /// <param name="index">The index for which distances will be calculated.</param>
        public Array2D<int> FindCellDistances(Vector2DInt index)
        {
            Distances = new Array2D<int>(Cells.Rows, Cells.Columns);
            Distances.Fill(-1);
            SearchCellDistances(index, 0);
            return Distances;
        }

        /// <summary>
        /// Performs a recursive crawl of the template cells to determine the distance to an index.
        /// </summary>
        /// <param name="index">The current index.</param>
        /// <param name="distance">The distance to the current index.</param>
        private void SearchCellDistances(Vector2DInt index, int distance)
        {
            if (Cells.GetOrDefault(index.X, index.Y) == null)
                return;
            if (Distances[index.X, index.Y] >= 0 && Distances[index.X, index.Y] <= distance)
                return;

            Distances[index.X, index.Y] = distance;
            SearchCellDistances(new Vector2DInt(index.X - 1, index.Y), distance + 1);
            SearchCellDistances(new Vector2DInt(index.X, index.Y - 1), distance + 1);
            SearchCellDistances(new Vector2DInt(index.X, index.Y + 1), distance + 1);
            SearchCellDistances(new Vector2DInt(index.X + 1, index.Y), distance + 1);
        }
    }
}
