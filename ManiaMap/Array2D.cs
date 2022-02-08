using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;

namespace ManiaMap
{
    [DataContract]
    public class Array2D<T>
    {
        [DataMember(Order = 1)]
        public int Rows { get; private set; }

        [DataMember(Order = 2)]
        public int Columns { get; private set; }

        [DataMember(Order = 3)]
        public T[] Array { get; private set; } = System.Array.Empty<T>();

        public Array2D()
        {

        }

        public Array2D(int rows, int columns)
        {
            if (rows < 0)
                throw new Exception($"Rows cannot be negative: {rows}.");
            if (columns < 0)
                throw new Exception($"Columns cannot be negative: {columns}.");
            
            if (rows > 0 && columns > 0)
            {
                Rows = rows;
                Columns = columns;
                Array = new T[rows * columns];
            }
        }

        public Array2D(T[,] array)
        {
            Rows = array.GetLength(0);
            Columns = array.GetLength(1);
            Array = FlattenArray(array);
        }

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

        public static implicit operator Array2D<T>(T[,] array) => new Array2D<T>(array);

        public override string ToString()
        {
            return $"Array2D<{typeof(T)}>(Rows = {Rows}, Columns = {Columns})";
        }

        /// <summary>
        /// Returns true if the index exists.
        /// </summary>
        public bool IndexExists(int row, int column)
        {
            return (uint)row < Rows && (uint)column < Columns;
        }

        /// <summary>
        /// Returns the flat array index corresponding to the specified 2D index.
        /// </summary>
        private int Index(int row, int column)
        {
            return row * Columns + column;
        }

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
        public T GetOrDefault(int row, int column, T fallback = default)
        {
            if (IndexExists(row, column))
                return Array[Index(row, column)];
            return fallback;
        }
    }
}
