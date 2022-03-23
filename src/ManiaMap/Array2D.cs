using System;
using System.Runtime.Serialization;
using System.Text;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A 2D array that can be serialized.
    /// </summary>
    [DataContract]
    public class Array2D<T>
    {
        /// <summary>
        /// The number of rows in the array.
        /// </summary>
        [DataMember(Order = 1)]
        public int Rows { get; private set; }

        /// <summary>
        /// The number of columns in the array.
        /// </summary>
        [DataMember(Order = 2)]
        public int Columns { get; private set; }

        /// <summary>
        /// The underlying flat array.
        /// </summary>
        [DataMember(Order = 3)]
        public T[] Array { get; private set; } = System.Array.Empty<T>();

        /// <summary>
        /// Initializes an empty array.
        /// </summary>
        public Array2D()
        {

        }

        /// <summary>
        /// Initializes an array by size.
        /// </summary>
        /// <param name="rows">The number of rows in the array.</param>
        /// <param name="columns">The number of columns in the array.</param>
        /// <exception cref="ArgumentException">Raised if either the input rows or columns are negative.</exception>
        public Array2D(int rows, int columns)
        {
            if (rows < 0)
                throw new ArgumentException($"Rows cannot be negative: {rows}.");
            if (columns < 0)
                throw new ArgumentException($"Columns cannot be negative: {columns}.");

            if (rows > 0 && columns > 0)
            {
                Rows = rows;
                Columns = columns;
                Array = new T[rows * columns];
            }
        }

        /// <summary>
        /// Initializes an array from a built-in 2D array.
        /// </summary>
        /// <param name="array">The built-in 2D array.</param>
        public Array2D(T[,] array)
        {
            Rows = array.GetLength(0);
            Columns = array.GetLength(1);
            Array = FlattenArray(array);
        }

        /// <summary>
        /// Accesses the array by 2D index.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        /// <exception cref="IndexOutOfRangeException">Raised if the index is out of range.</exception>
        public T this[int row, int column]
        {
            get
            {
                if (!IndexExists(row, column))
                    throw new IndexOutOfRangeException($"Index out of range: ({row}, {column}).");
                return Array[Index(row, column)];
            }
            set
            {
                if (!IndexExists(row, column))
                    throw new IndexOutOfRangeException($"Index out of range: ({row}, {column}).");
                Array[Index(row, column)] = value;
            }
        }

        /// <summary>
        /// Implicitly casts a built-in 2D array to an Array2D.
        /// </summary>
        /// <param name="array">The built-in 2D array.</param>
        public static implicit operator Array2D<T>(T[,] array) => new Array2D<T>(array);

        public override string ToString()
        {
            return $"Array2D<{typeof(T)}>(Rows = {Rows}, Columns = {Columns})";
        }

        /// <summary>
        /// Returns a string of all array elements.
        /// </summary>
        public string ToArrayString()
        {
            var size = 2 + 2 * Array.Length + 4 * Rows;
            var builder = new StringBuilder(size);
            builder.Append('[');

            for (int i = 0; i < Rows; i++)
            {
                builder.Append('[');

                for (int j = 0; j < Columns; j++)
                {
                    builder.Append(this[i, j]);

                    if (j < Columns - 1)
                        builder.Append(", ");
                }

                builder.Append(']');

                if (i < Rows - 1)
                    builder.Append("\n ");
            }

            builder.Append(']');
            return builder.ToString();
        }

        /// <summary>
        /// Sets all elements of the array to the value.
        /// </summary>
        /// <param name="value">The fill value.</param>
        public void Fill(T value)
        {
            for (int i = 0; i < Array.Length; i++)
            {
                Array[i] = value;
            }
        }

        /// <summary>
        /// Returns true if the index exists.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        public bool IndexExists(int row, int column)
        {
            return (uint)row < Rows && (uint)column < Columns;
        }

        /// <summary>
        /// Returns the flat array index corresponding to the specified 2D index.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        private int Index(int row, int column)
        {
            return row * Columns + column;
        }

        /// <summary>
        /// Returns a new flattened array from a built-in 2D array.
        /// </summary>
        /// <param name="array">The built-in 2D array.</param>
        private static T[] FlattenArray(T[,] array)
        {
            var result = new T[array.Length];
            int k = 0;

            for (int i = 0; i < array.GetLength(0); i++)
            {
                for (int j = 0; j < array.GetLength(1); j++)
                {
                    result[k++] = array[i, j];
                }
            }

            return result;
        }

        /// <summary>
        /// Returns the value at the specified index if it exists. If not,
        /// returns the fallback value.
        /// </summary>
        /// <param name="row">The row index.</param>
        /// <param name="column">The column index.</param>
        /// <param name="fallback">The fallback value.</param>
        public T GetOrDefault(int row, int column, T fallback = default)
        {
            if (IndexExists(row, column))
                return Array[Index(row, column)];
            return fallback;
        }

        /// <summary>
        /// Returns a new array rotated clockwise 90 degrees.
        /// </summary>
        public Array2D<T> Rotated90()
        {
            var rotation = new Array2D<T>(Columns, Rows);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    var k = rotation.Columns - 1 - i;
                    rotation[j, k] = this[i, j];
                }
            }

            return rotation;
        }

        /// <summary>
        /// Returns a new array rotated 180 degrees.
        /// </summary>
        public Array2D<T> Rotated180()
        {
            var rotation = new Array2D<T>(Rows, Columns);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    var k = rotation.Rows - 1 - i;
                    var l = rotation.Columns - 1 - j;
                    rotation[k, l] = this[i, j];
                }
            }

            return rotation;
        }

        /// <summary>
        /// Returns a new array rotated clockwise 270 degrees.
        /// </summary>
        public Array2D<T> Rotated270()
        {
            var rotation = new Array2D<T>(Columns, Rows);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    var k = rotation.Rows - 1 - j;
                    rotation[k, i] = this[i, j];
                }
            }

            return rotation;
        }

        /// <summary>
        /// Returns a new array mirrored horizontally, i.e. about the vertical axis.
        /// </summary>
        public Array2D<T> MirroredHorizontally()
        {
            var mirror = new Array2D<T>(Rows, Columns);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    var k = mirror.Columns - 1 - j;
                    mirror[i, k] = this[i, j];
                }
            }

            return mirror;
        }

        /// <summary>
        /// Returns a new array mirrored vertically, i.e. about the horizontal axis.
        /// </summary>
        public Array2D<T> MirroredVertically()
        {
            var mirror = new Array2D<T>(Rows, Columns);

            for (int i = 0; i < Rows; i++)
            {
                for (int j = 0; j < Columns; j++)
                {
                    var k = mirror.Rows - 1 - i;
                    mirror[k, j] = this[i, j];
                }
            }

            return mirror;
        }
    }
}
