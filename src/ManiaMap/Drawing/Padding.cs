using System;

namespace MPewsey.ManiaMap.Drawing
{
    /// <summary>
    /// A structure for specifying the padding around an object.
    /// </summary>
    public struct Padding : IEquatable<Padding>
    {
        /// <summary>
        /// The padding from the top.
        /// </summary>
        public int Top { get; }

        /// <summary>
        /// The padding from the bottom.
        /// </summary>
        public int Bottom { get; }

        /// <summary>
        /// The padding from the left.
        /// </summary>
        public int Left { get; }

        /// <summary>
        /// The padding from the right.
        /// </summary>
        public int Right { get; }

        /// <summary>
        /// Initializes equal directional padding.
        /// </summary>
        /// <param name="padding">The padding in all directions.</param>
        public Padding(int padding)
        {
            Top = padding;
            Bottom = padding;
            Left = padding;
            Right = padding;
        }

        /// <summary>
        /// Initializes padding in each direction.
        /// </summary>
        /// <param name="left">The padding to the left.</param>
        /// <param name="top">The padding to the top.</param>
        /// <param name="right">The padding to the right.</param>
        /// <param name="bottom">The padding to the bottom.</param>
        public Padding(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        public override string ToString()
        {
            return $"Padding(Left = {Left}, Top = {Top}, Right = {Right}, Bottom = {Bottom})";
        }

        public override bool Equals(object obj)
        {
            return obj is Padding padding && Equals(padding);
        }

        public bool Equals(Padding other)
        {
            return Top == other.Top &&
                   Bottom == other.Bottom &&
                   Left == other.Left &&
                   Right == other.Right;
        }

        public override int GetHashCode()
        {
            int hashCode = 920856443;
            hashCode = hashCode * -1521134295 + Top.GetHashCode();
            hashCode = hashCode * -1521134295 + Bottom.GetHashCode();
            hashCode = hashCode * -1521134295 + Left.GetHashCode();
            hashCode = hashCode * -1521134295 + Right.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Padding left, Padding right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Padding left, Padding right)
        {
            return !(left == right);
        }
    }
}
