using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A structure containing a collectable group name and ID.
    /// </summary>
    public struct Collectable : IEquatable<Collectable>
    {
        /// <summary>
        /// The collectable group name.
        /// </summary>
        public string Group { get; }

        /// <summary>
        /// The collectable ID.
        /// </summary>
        public int Id { get; }

        /// <summary>
        /// Initializes a new collectable.
        /// </summary>
        /// <param name="group">The collectable name.</param>
        /// <param name="id">The collectable ID.</param>
        public Collectable(string group, int id)
        {
            Group = group;
            Id = id;
        }

        public override string ToString()
        {
            return $"Collectable(Group = {Group}, Id = {Id})";
        }

        public override bool Equals(object obj)
        {
            return obj is Collectable collectable && Equals(collectable);
        }

        public bool Equals(Collectable other)
        {
            return Group == other.Group &&
                   Id == other.Id;
        }

        public override int GetHashCode()
        {
            int hashCode = 84831244;
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Group);
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            return hashCode;
        }

        public static bool operator ==(Collectable left, Collectable right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(Collectable left, Collectable right)
        {
            return !(left == right);
        }
    }
}
