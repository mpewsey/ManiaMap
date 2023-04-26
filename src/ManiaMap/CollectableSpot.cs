using MPewsey.Common.Collections;
using MPewsey.Common.Mathematics;
using MPewsey.ManiaMap.Exceptions;
using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A room collectable spot.
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class CollectableSpot
    {
        /// <summary>
        /// The local cell position within the room.
        /// </summary>
        [DataMember(Order = 1)]
        public Vector2DInt Position { get; set; }

        /// <summary>
        /// The collectable group name.
        /// </summary>
        [DataMember(Order = 2)]
        public string Group { get; set; }

        /// <summary>
        /// Initializes a new collectable spot.
        /// </summary>
        /// <param name="position">The local cell position within the room.</param>
        /// <param name="group">The collectable group name.</param>
        public CollectableSpot(Vector2DInt position, string group)
        {
            Position = position;
            Group = group;
        }

        /// <summary>
        /// Returns a copy of a collectable spot.
        /// </summary>
        /// <param name="other">The collectable spot to copy.</param>
        public CollectableSpot(CollectableSpot other)
        {
            Position = other.Position;
            Group = other.Group;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"CollectableSpot(Position = {Position}, Group = {Group})";
        }

        /// <summary>
        /// Returns a copy of the collectable spot.
        /// </summary>
        public CollectableSpot Copy()
        {
            return new CollectableSpot(this);
        }

        /// <summary>
        /// Returns true if the values of the two collectable spots are equal.
        /// </summary>
        /// <param name="value1">The first spot.</param>
        /// <param name="value2">The second spot.</param>
        public static bool ValuesAreEqual(CollectableSpot value1, CollectableSpot value2)
        {
            if (value1 == value2)
                return true;

            if (value1 == null || value2 == null)
                return false;

            return value1.ValuesAreEqual(value2);
        }

        /// <summary>
        /// Returns true if the values of the collectable spot are equal to the specified.
        /// </summary>
        /// <param name="other">The other spot.</param>
        public bool ValuesAreEqual(CollectableSpot other)
        {
            return Position == other.Position
                && Group == other.Group;
        }

        /// <summary>
        /// Checks that the collectable spot is valid and raises exceptions otherwise.
        /// </summary>
        /// <param name="cells">The array of cells.</param>
        /// <exception cref="IndexOutOfRangeException">Raised if the collectable spot position is outside of the cells bounding box.</exception>
        /// <exception cref="InvalidNameException">Raised if the collectable spot name is null or whitespace.</exception>
        public void Validate(Array2D<Cell> cells)
        {
            if (!cells.IndexExists(Position.X, Position.Y))
                throw new IndexOutOfRangeException($"Position of of range: {this}.");
            if (string.IsNullOrWhiteSpace(Group))
                throw new InvalidNameException($"Group name is null or whitespace: {this}.");
        }

        /// <summary>
        /// Returns true if the collectable spot's position and group name are valid.
        /// </summary>
        /// <param name="cells">The array of cells.</param>
        public bool IsValid(Array2D<Cell> cells)
        {
            return cells.IndexExists(Position.X, Position.Y)
                && !string.IsNullOrWhiteSpace(Group);
        }
    }
}
