using MPewsey.ManiaMap.Serialization;
using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A structure for representing a unique ID consisting of 3 integers.
    /// </summary>
    [DataContract(Namespace = XmlSerialization.Namespace)]
    public struct Uid : IEquatable<Uid>, IComparable<Uid>
    {
        /// <summary>
        /// The first ID value.
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public int Value1 { get; private set; }

        /// <summary>
        /// The second ID value.
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        public int Value2 { get; private set; }

        /// <summary>
        /// The third ID value.
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        public int Value3 { get; private set; }

        /// <summary>
        /// Initializes a unique ID from a single value.
        /// </summary>
        /// <param name="value1">The first ID value.</param>
        public Uid(int value1)
        {
            Value1 = value1;
            Value2 = 0;
            Value3 = 0;
        }

        /// <summary>
        /// Initializes a unique ID from two values.
        /// </summary>
        /// <param name="value1">The first ID value.</param>
        /// <param name="value2">The second ID value.</param>
        public Uid(int value1, int value2)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = 0;
        }

        /// <summary>
        /// Initializes a unique ID from three values.
        /// </summary>
        /// <param name="value1">The first ID value.</param>
        /// <param name="value2">The second ID value.</param>
        /// <param name="value3">The third ID value.</param>
        public Uid(int value1, int value2, int value3)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = value3;
        }

        public override string ToString()
        {
            return $"Uid({Value1}, {Value2}, {Value3})";
        }

        public override bool Equals(object obj)
        {
            return obj is Uid uid && Equals(uid);
        }

        public bool Equals(Uid other)
        {
            return Value1 == other.Value1 &&
                   Value2 == other.Value2 &&
                   Value3 == other.Value3;
        }

        public override int GetHashCode()
        {
            int hashCode = 29335732;
            hashCode = hashCode * -1521134295 + Value1.GetHashCode();
            hashCode = hashCode * -1521134295 + Value2.GetHashCode();
            hashCode = hashCode * -1521134295 + Value3.GetHashCode();
            return hashCode;
        }

        public int CompareTo(Uid other)
        {
            var comparison = Value1.CompareTo(other.Value1);

            if (comparison != 0)
                return comparison;

            comparison = Value2.CompareTo(other.Value2);

            if (comparison != 0)
                return comparison;

            return Value3.CompareTo(other.Value3);
        }

        public static bool operator ==(Uid left, Uid right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Uid left, Uid right)
        {
            return !(left == right);
        }
    }
}
