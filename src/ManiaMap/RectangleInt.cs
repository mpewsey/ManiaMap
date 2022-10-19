using System;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A rectangle consisting of position and size integers.
    /// </summary>
    public struct RectangleInt : IEquatable<RectangleInt>
    {
        /// <summary>
        /// The x position.
        /// </summary>
        public int X { get; }

        /// <summary>
        /// The y position.
        /// </summary>
        public int Y { get; }

        /// <summary>
        /// The width.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// The height.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Initializes a new rectangle.
        /// </summary>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public RectangleInt(int x, int y, int width, int height)
        {
            X = x;
            Y = y;
            Width = width;
            Height = height;
        }

        public override string ToString()
        {
            return $"RectangleInt(X = {X}, Y = {Y}, Width = {Width}, Height = {Height})";
        }

        public override bool Equals(object obj)
        {
            return obj is RectangleInt rect && Equals(rect);
        }

        public bool Equals(RectangleInt other)
        {
            return X == other.X &&
                   Y == other.Y &&
                   Width == other.Width &&
                   Height == other.Height;
        }

        public override int GetHashCode()
        {
            int hashCode = 466501756;
            hashCode = hashCode * -1521134295 + X.GetHashCode();
            hashCode = hashCode * -1521134295 + Y.GetHashCode();
            hashCode = hashCode * -1521134295 + Width.GetHashCode();
            hashCode = hashCode * -1521134295 + Height.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(RectangleInt left, RectangleInt right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RectangleInt left, RectangleInt right)
        {
            return !(left == right);
        }
    }
}
