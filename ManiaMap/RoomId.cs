using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public struct RoomId : IEquatable<RoomId>
    {
        [DataMember(Order = 1)]
        public int Value1 { get; set; }

        [DataMember(Order = 2)]
        public int Value2 { get; set; }

        [DataMember(Order = 3)]
        public bool Value3 { get; set; }

        public RoomId(int value)
        {
            Value1 = value;
            Value2 = 0;
            Value3 = false;
        }

        public RoomId(int value1, int value2)
        {
            Value1 = value1;
            Value2 = value2;
            Value3 = true;
        }

        public static implicit operator RoomId(int value) => new RoomId(value);

        public override string ToString()
        {
            return $"RoomId(Value1 = {Value1}, Value2 = {Value2}, Value3 = {Value3})";
        }

        public override bool Equals(object obj)
        {
            return obj is RoomId id && Equals(id);
        }

        public bool Equals(RoomId other)
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

        public static bool operator ==(RoomId left, RoomId right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(RoomId left, RoomId right)
        {
            return !(left == right);
        }
    }
}
