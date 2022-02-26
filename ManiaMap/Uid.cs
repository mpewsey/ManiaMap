using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public struct Uid : IEquatable<Uid>
    {
        [DataMember(Order = 1)]
        public int Value1 { get; private set; }

        [DataMember(Order = 2)]
        public int Value2 { get; private set; }

        [DataMember(Order = 3)]
        public int Value3 { get; private set; }

        public Uid(int value1)
        {
            Value1 = value1;
            Value2 = 0;
            Value3 = 0;
        }

        public Uid(int value1, int value2)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = 0;
        }

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
