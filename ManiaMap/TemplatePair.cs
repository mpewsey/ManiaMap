using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public struct TemplatePair : IEquatable<TemplatePair>
    {
        public RoomTemplate FromTemplate { get; }
        public RoomTemplate ToTemplate { get; }

        public TemplatePair(RoomTemplate from, RoomTemplate to)
        {
            FromTemplate = from;
            ToTemplate = to;
        }

        public override bool Equals(object obj)
        {
            return obj is TemplatePair pair && Equals(pair);
        }

        public bool Equals(TemplatePair other)
        {
            return EqualityComparer<RoomTemplate>.Default.Equals(FromTemplate, other.FromTemplate) &&
                   EqualityComparer<RoomTemplate>.Default.Equals(ToTemplate, other.ToTemplate);
        }

        public override int GetHashCode()
        {
            return HashCode.Combine(FromTemplate, ToTemplate);
        }

        public static bool operator ==(TemplatePair left, TemplatePair right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(TemplatePair left, TemplatePair right)
        {
            return !(left == right);
        }
    }
}
