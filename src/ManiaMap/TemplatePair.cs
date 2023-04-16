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

        /// <summary>
        /// Initializes a new template pair.
        /// </summary>
        /// <param name="from">The from template.</param>
        /// <param name="to">The to template.</param>
        public TemplatePair(RoomTemplate from, RoomTemplate to)
        {
            FromTemplate = from;
            ToTemplate = to;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"TemplatePair(FromTemplate = {FromTemplate}, ToTemplate = {ToTemplate})";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is TemplatePair pair && Equals(pair);
        }

        /// <inheritdoc/>
        public bool Equals(TemplatePair other)
        {
            return EqualityComparer<RoomTemplate>.Default.Equals(FromTemplate, other.FromTemplate) &&
                   EqualityComparer<RoomTemplate>.Default.Equals(ToTemplate, other.ToTemplate);
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = -2144273727;
            hashCode = hashCode * -1521134295 + EqualityComparer<RoomTemplate>.Default.GetHashCode(FromTemplate);
            hashCode = hashCode * -1521134295 + EqualityComparer<RoomTemplate>.Default.GetHashCode(ToTemplate);
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(TemplatePair left, TemplatePair right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(TemplatePair left, TemplatePair right)
        {
            return !(left == right);
        }
    }
}
