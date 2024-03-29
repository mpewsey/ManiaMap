﻿using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A structure containing a collectable group and ID.
    /// </summary>
    public struct Collectable : IEquatable<Collectable>
    {
        /// <summary>
        /// The ID.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// The group name.
        /// </summary>
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

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"Collectable(Id = {Id}, Group = {Group})";
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            return obj is Collectable collectable && Equals(collectable);
        }

        /// <inheritdoc/>
        public bool Equals(Collectable other)
        {
            return Id == other.Id
                && Group == other.Group;
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            int hashCode = 84831244;
            hashCode = hashCode * -1521134295 + Id.GetHashCode();
            hashCode = hashCode * -1521134295 + EqualityComparer<string>.Default.GetHashCode(Group);
            return hashCode;
        }

        /// <inheritdoc/>
        public static bool operator ==(Collectable left, Collectable right)
        {
            return left.Equals(right);
        }

        /// <inheritdoc/>
        public static bool operator !=(Collectable left, Collectable right)
        {
            return !(left == right);
        }
    }
}
