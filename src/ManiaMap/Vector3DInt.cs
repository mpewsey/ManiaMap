using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A 3D vector with integer values.
    /// </summary>
    [DataContract]
    public struct Vector3DInt : IEquatable<Vector3DInt>
    {
        /// <summary>
        /// Returns a zero vector.
        /// </summary>
        public static Vector3DInt Zero => new Vector3DInt();

        /// <summary>
        /// The x value.
        /// </summary>
        [DataMember(Order = 0)]
        public int X { get; private set; }

        /// <summary>
        /// The y value.
        /// </summary>
        [DataMember(Order = 1)]
        public int Y { get; private set; }

        /// <summary>
        /// The z value.
        /// </summary>
        [DataMember(Order = 2)]
        public int Z { get; private set; }

        /// <summary>
        /// Initializes a new vector.
        /// </summary>
        /// <param name="x">The x value.</param>
        /// <param name="y">The y value.</param>
        /// <param name="z">The z value.</param>
        public Vector3DInt(int x, int y, int z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public static implicit operator Vector2DInt(Vector3DInt v) => new Vector2DInt(v.X, v.Y);

        public override string ToString()
        {
            return $"Vector3DInt({X}, {Y}, {Z})";
        }

        public override bool Equals(object obj)
        {
            return obj is Vector3DInt vector && Equals(vector);
        }

        public bool Equals(Vector3DInt other)
        {
            return X == other.X &&
                   Y == other.Y &&
                   Z == other.Z;
        }

        public override int GetHashCode()
        {
            int hashCode = -307843816;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Z.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Vector3DInt left, Vector3DInt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Vector3DInt left, Vector3DInt right)
        {
            return !(left == right);
        }

        public static Vector3DInt operator +(Vector3DInt left, Vector3DInt right)
        {
            return new Vector3DInt(left.X + right.X, left.Y + right.Y, left.Z + right.Z);
        }

        public static Vector3DInt operator -(Vector3DInt left, Vector3DInt right)
        {
            return new Vector3DInt(left.X - right.X, left.Y - right.Y, left.Z - right.Z);
        }
    }
}
