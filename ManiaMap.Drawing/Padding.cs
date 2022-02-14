using System;

namespace MPewsey.ManiaMap.Drawing
{
    public struct Padding : IEquatable<Padding>
    {
        public int Top { get; }
        public int Bottom { get; }
        public int Left { get; }
        public int Right { get; }

        public Padding(int padding)
        {
            Top = padding;
            Bottom = padding;
            Left = padding;
            Right = padding;
        }

        public Padding(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
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
