﻿using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A 32 bit color.
    /// </summary>
    [DataContract]
    public struct Color32 : IEquatable<Color32>
    {
        /// <summary>
        /// The red channel.
        /// </summary>
        [DataMember(Order = 1)]
        public byte R { get; private set; }

        /// <summary>
        /// The green channel.
        /// </summary>
        [DataMember(Order = 2)]
        public byte G { get; private set; }

        /// <summary>
        /// The blue channel.
        /// </summary>
        [DataMember(Order = 3)]
        public byte B { get; private set; }

        /// <summary>
        /// The alpha channel.
        /// </summary>
        [DataMember(Order = 4)]
        public byte A { get; private set; }

        /// <summary>
        /// Initializes a new color.
        /// </summary>
        /// <param name="r">The red channel.</param>
        /// <param name="g">The green channel.</param>
        /// <param name="b">The blue channel.</param>
        /// <param name="a">The alpha channel.</param>
        public Color32(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        public override string ToString()
        {
            return $"Color32(R = {R}, G = {G}, B = {B}, A = {A})";
        }

        public override bool Equals(object obj)
        {
            return obj is Color32 color && Equals(color);
        }

        public bool Equals(Color32 other)
        {
            return R == other.R &&
                   G == other.G &&
                   B == other.B &&
                   A == other.A;
        }

        public override int GetHashCode()
        {
            int hashCode = 1960784236;
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Color32 left, Color32 right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Color32 left, Color32 right)
        {
            return !(left == right);
        }
    }
}
