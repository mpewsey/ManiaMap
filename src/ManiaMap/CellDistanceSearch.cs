namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Contains methods for calculating distances between cells.
    /// </summary>
    public static class CellDistanceSearch
    {
        /// <summary>
        /// Returns an array of distances from the specified index to each cell.
        /// Values of -1 indicate that the index does not exist.
        /// </summary>
        /// <param name="cells">An array of cells.</param>
        /// <param name="index">The index for which distances will be calculated.</param>
        public static Array2D<int> FindCellDistances(Array2D<Cell> cells, Vector2DInt index)
        {
            var distances = new Array2D<int>(cells.Rows, cells.Columns);
            distances.Fill(-1);
            SearchCellDistances(index, 0, cells, distances);
            return distances;
        }

        /// <summary>
        /// Performs a recursive crawl of the template cells to determine the distance to an index.
        /// </summary>
        /// <param name="index">The current index.</param>
        /// <param name="distance">The distance to the current index.</param>
        /// <param name="cells">An array of cells.</param>
        /// <param name="distances">The distances array.</param>
        private static void SearchCellDistances(Vector2DInt index, int distance,
            Array2D<Cell> cells, Array2D<int> distances)
        {
            if (cells.GetOrDefault(index.X, index.Y) == null)
                return;
            if (distances[index.X, index.Y] >= 0 && distances[index.X, index.Y] <= distance)
                return;

            distances[index.X, index.Y] = distance;
            SearchCellDistances(new Vector2DInt(index.X - 1, index.Y), distance + 1, cells, distances);
            SearchCellDistances(new Vector2DInt(index.X, index.Y - 1), distance + 1, cells, distances);
            SearchCellDistances(new Vector2DInt(index.X, index.Y + 1), distance + 1, cells, distances);
            SearchCellDistances(new Vector2DInt(index.X + 1, index.Y), distance + 1, cells, distances);
        }
    }
}
