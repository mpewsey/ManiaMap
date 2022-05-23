using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A structure containing a collectable group and ID.
    /// </summary>
    [DataContract]
    public struct Collectable : IEquatable<Collectable>
    {
        /// <summary>
        /// The ID.
        /// </summary>
        [DataMember(Order = 1)]
        public int Id { get; private set; }

        /// <summary>
        /// The group name.
        /// </summary>
        [DataMember(Order = 2)]
        public string Group { get; private set; }

        /// <summary>
        /// Initializes a new collectable.
        /// </summary>
        /// <param name="id">The ID.</param>
        /// <param name="group">The group name.</param>
        public Collectable(int id, string group)
        {
            Id = id;
            Group = group;
        }

        public override string ToString()
        {
            return $"Collectable(Id = {Id}, Group = {Group})";
        }

        public override bool Equals(object obj)
        {
            return obj is Collectable collectable && Equals(collectable);
        }

        public bool Equals(Collectable other)
        {
            return Id == other.Id
                && Group == other.Group;

        }

        public override int GetHashCode()
        {
            int hashCode = 84831244;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Group);
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
