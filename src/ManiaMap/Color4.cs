using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A 4 byte (32 bit) color.
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public struct Color4 : IEquatable<Color4>
    {
        /// <summary>
        /// The red channel.
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public byte R { get; private set; }

        /// <summary>
        /// The green channel.
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public byte G { get; private set; }

        /// <summary>
        /// The blue channel.
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        public byte B { get; private set; }

        /// <summary>
        /// The alpha channel.
        /// </summary>
        [DataMember(Order = 4, IsRequired = true)]
        public byte A { get; private set; }

        /// <summary>
        /// Initializes a new color.
        /// </summary>
        /// <param name="r">The red channel.</param>
        /// <param name="g">The green channel.</param>
        /// <param name="b">The blue channel.</param>
        /// <param name="a">The alpha channel.</param>
        public Color4(byte r, byte g, byte b, byte a)
        {
            R = r;
            G = g;
            B = b;
            A = a;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Color32(R = {R}, G = {G}, B = {B}, A = {A})";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is Color4 color && Equals(color);
        }

        /// <inheritdoc/>
        public bool Equals(Color4 other)
        {
            return R == other.R &&
                   G == other.G &&
                   B == other.B &&
                   A == other.A;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 1960784236;
            hashCode = hashCode * -1521134295 + R.GetHashCode();
            hashCode = hashCode * -1521134295 + G.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(Color4 left, Color4 right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(Color4 left, Color4 right)
        {
            return !(left == right);
        }
    }
}
