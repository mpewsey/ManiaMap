using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A structure containing a from and to RoomTemplate.
    /// </summary>
    public struct TemplatePair : IEquatable<TemplatePair>
    {
        /// <summary>
        /// The from room template.
        /// </summary>
        public RoomTemplate FromTemplate { get; }

        /// <summary>
        /// The to room template.
        /// </summary>
        public RoomTemplate ToTemplate { get; }

        public TemplatePair(RoomTemplate from, RoomTemplate to)
        {
            FromTemplate = from;
            ToTemplate = to;
        }

        public override string ToString()
        {
            return $"TemplatePair(FromTemplate = {FromTemplate}, ToTemplate = {ToTemplate})";
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
            int hashCode = -2144273727;
            hashCode = hashCode * -1521134295 + EqualityComparer<RoomTemplate>.Default.GetHashCode(FromTemplate);
            hashCode = hashCode * -1521134295 + EqualityComparer<RoomTemplate>.Default.GetHashCode(ToTemplate);
            return hashCode;
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
