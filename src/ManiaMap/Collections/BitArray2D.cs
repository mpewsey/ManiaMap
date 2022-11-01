using System;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;
using System.Text;

namespace MPewsey.ManiaMap.Collections
{
    /// <summary>
    /// An 2D array of bits.
    /// </summary>
    [DataContract(Namespace = Serialization.Namespace)]
    public class BitArray2D
    {
        /// <summary>
        /// The number of bits in each chunk.
        /// </summary>
        public const int ChunkSize = 32;

        /// <summary>
        /// The number of rows in the array.
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public int Rows { get; private set; }

        /// <summary>
        /// The number of columns in the array.
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public int Columns { get; private set; }

        /// <summary>
        /// A flat array of data chunks.
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        public int[] Array { get; private set; } = System.Array.Empty<int>();

        /// <summary>
        /// Accesses the bit at the specified index.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        /// <exception cref="IndexOutOfRangeException">Raised if the index is out of bounds.</exception>
        public bool this[int row, int column]
        {
            get
            {
                if (!IndexExists(row, column))
                    throw new IndexOutOfRangeException($"Index out of range: ({row}, {column}).");

                var index = Index(row, column);
                return (Array[index.X] & 1 << index.Y) != 0;
            }
            set
            {
                if (!IndexExists(row, column))
                    throw new IndexOutOfRangeException($"Index out of range: ({row}, {column}).");

                var index = Index(row, column);

                if (value)
                    Array[index.X] |= 1 << index.Y;
                else
                    Array[index.X] &= ~(1 << index.Y);
            }
        }

        /// <summary>
        /// Initializes an empty array.
        /// </summary>
        public BitArray2D()
        {

        }

        /// <summary>
        /// Initializes an array by size.
        /// </summary>
        /// <param name="rows">The number of rows in the array.</param>
        /// <param name="columns">The number of columns in the array.</param>
        /// <exception cref="ArgumentException">Raised if either the input rows or columns are negative.</exception>
        public BitArray2D(int rows, int columns)
        {
            if (rows < 0)
                throw new ArgumentException($"Rows cannot be negative: {rows}.");
            if (columns < 0)
                throw new ArgumentException($"Columns cannot be negative: {columns}.");

            if (rows > 0 && columns > 0)
            {
                Rows = rows;
                Columns = columns;
                Array = new int[(int)Math.Ceiling(rows * (double)columns / ChunkSize)];
            }
        }

        public override string ToString()
        {
            return $"BitArray2D(Rows = {Rows}, Columns = {Columns})";
        }

        /// <summary>
        /// Returns a string of all array elements.
        /// </summary>
        public string ToArrayString()
        {
            var size = 2 + ChunkSize * Array.Length + 4 * Rows;
            var builder = new StringBuilder(size);
            builder.Append('[');

            for (int i = 0; i < Rows; i++)
            {
                builder.Append('[');

                for (int j = 0; j < Columns; j++)
                {
                    if (this[i, j])
                        builder.Append('1');
                    else
                        builder.Append('0');
                }

                builder.Append(']');

                if (i < Rows - 1)
                    builder.Append("\n ");
            }

            builder.Append(']');
            return builder.ToString();
        }

        /// <summary>
        /// Clears all active bits in the array.
        /// </summary>
        public void Clear()
        {
            System.Array.Clear(Array, 0, Array.Length);
        }

        /// <summary>
        /// Returns true if the index exists.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public bool IndexExists(int row, int column)
        {
            return (uint)row < Rows && (uint)column < Columns;
        }

        /// <summary>
        /// Returns a vector with the chunk index and position within the chunk
        /// for the specified row-column index.
        /// </summary>
        /// <param name="row">The row.</param>
        /// <param name="column">The column.</param>
        private Vector2DInt Index(int row, int column)
        {
            var index = row * Columns + column;
            return new Vector2DInt(index / ChunkSize, index % ChunkSize);
        }

        /// <summary>
        /// Returns the value at the specified index if it exists. If not,
        /// returns the fallback value.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        /// <param name="fallback">The fallback value.</param>
        public bool GetOrDefault(int row, int column, bool fallback = false)
        {
            if (IndexExists(row, column))
                return this[row, column];
            return fallback;
        }
    }
}
