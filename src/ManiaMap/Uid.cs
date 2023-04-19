using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A structure for representing a unique ID consisting of 3 integers.
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public struct Uid : IEquatable<Uid>, IComparable<Uid>
    {
        /// <summary>
        /// The first ID value.
        /// </summary>
        [DataMember(Order = 1)]
        public int A { get; private set; }

        /// <summary>
        /// The second ID value.
        /// </summary>
        [DataMember(Order = 2)]
        public int B { get; private set; }

        /// <summary>
        /// The third ID value.
        /// </summary>
        [DataMember(Order = 3)]
        public int C { get; private set; }

        /// <summary>
        /// Initializes a unique ID from a single value.
        /// </summary>
        /// <param name="a">The first ID value.</param>
        public Uid(int a)
        {
            A = a;
            B = 0;
            C = 0;
        }

        /// <summary>
        /// Initializes a unique ID from two values.
        /// </summary>
        /// <param name="a">The first ID value.</param>
        /// <param name="b">The second ID value.</param>
        public Uid(int a, int b)
        {
            A = a;
            B = b;
            C = 0;
        }

        /// <summary>
        /// Initializes a unique ID from three values.
        /// </summary>
        /// <param name="a">The first ID value.</param>
        /// <param name="b">The second ID value.</param>
        /// <param name="c">The third ID value.</param>
        public Uid(int a, int b, int c)
        {
            A = a;
            B = b;
            C = c;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Uid({A}, {B}, {C})";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is Uid uid && Equals(uid);
        }

        /// <inheritdoc/>
        public bool Equals(Uid other)
        {
            return A == other.A &&
                   B == other.B &&
                   C == other.C;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 29335732;
            hashCode = hashCode * -1521134295 + A.GetHashCode();
            hashCode = hashCode * -1521134295 + B.GetHashCode();
            hashCode = hashCode * -1521134295 + C.GetHashCode();
            return hashCode;
        }

        /// <inheritdoc/>
        public int CompareTo(Uid other)
        {
            var comparison = A.CompareTo(other.A);

            if (comparison != 0)
                return comparison;

            comparison = B.CompareTo(other.B);

            if (comparison != 0)
                return comparison;

            return C.CompareTo(other.C);
        }

        /// <inheritdoc/>
        public static bool operator ==(Uid left, Uid right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(Uid left, Uid right)
        {
            return !(left == right);
        }
    }
}
