using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A 2D vector with integer values.
    /// </summary>
    [DataContract]
    public struct Vector2DInt : IEquatable<Vector2DInt>
    {
        /// <summary>
        /// Returns a zero vector.
        /// </summary>
        public static Vector2DInt Zero => new Vector2DInt();

        /// <summary>
        /// The X value.
        /// </summary>
        [DataMember(Order = 0)]
        public int X { get; private set; }

        /// <summary>
        /// The Y value.
        /// </summary>
        [DataMember(Order = 1)]
        public int Y { get; private set; }

        /// <summary>
        /// Initializes a new vector.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        public Vector2DInt(int x, int y)
        {
            X = x;
            Y = y;
        }

        public override string ToString()
        {
            return $"Vector2DInt({X}, {Y})";
        }

        public override bool Equals(object obj)
        {
            return obj is Vector2DInt vector && Equals(vector);
        }

        public bool Equals(Vector2DInt other)
        {
            return X == other.X &&
                   Y == other.Y;
        }

        public override int GetHashCode()
        {
            int hashCode = 1861411795;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Vector2DInt left, Vector2DInt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector2DInt left, Vector2DInt right)
        {
            return !(left == right);
        }

        public static Vector2DInt operator +(Vector2DInt left, Vector2DInt right)
        {
            return new Vector2DInt(left.X + right.X, left.Y + right.Y);
        }

        public static Vector2DInt operator -(Vector2DInt left, Vector2DInt right)
        {
            return new Vector2DInt(left.X - right.X, left.Y - right.Y);
        }
    }
}
