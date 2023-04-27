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

        private float _weight = 1;
        /// <summary>
        /// The base draw weight.
        /// </summary>
        public float Weight { get => _weight; set => _weight = Math.Max(value, 0); }

        [OnDeserializing]
        private void OnDeserializing(StreamingContext context)
        {
            Weight = 1;
        }

        /// <summary>
        /// Initializes a new collectable spot.
        /// </summary>
        /// <param name="position">The local cell position within the room.</param>
        /// <param name="group">The collectable group name.</param>
        /// <param name="weight">The base draw weight.</param>
        public CollectableSpot(Vector2DInt position, string group, float weight)
        {
            Position = position;
            Group = group;
            Weight = weight;
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
            return $"CollectableSpot(Position = {Position}, Group = {Group}, Weight = {Weight})";
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
        /// <param name="template">The room template.</param>
        public void Validate(RoomTemplate template)
        {
            ValidatePosition(template);
            ValidateGroupName();
        }

        /// <summary>
        /// Checks that the position is within the bounds of the template and raises an exception otherwise.
        /// </summary>
        /// <param name="template">The room template.</param>
        /// <exception cref="IndexOutOfRangeException">Raised if the position is outside the bounds of the template.</exception>
        private void ValidatePosition(RoomTemplate template)
        {
            if (!template.Cells.IndexExists(Position.X, Position.Y))
                throw new IndexOutOfRangeException($"Position out of range: {this}.");
        }

        /// <summary>
        /// Checks that the group name is valid and raises an exception otherwise.
        /// </summary>
        /// <exception cref="InvalidNameException">Raised if the group name is null or whitespace.</exception>
        private void ValidateGroupName()
        {
            if (string.IsNullOrWhiteSpace(Group))
                throw new InvalidNameException($"Group name is null or whitespace: {this}.");
        }

        /// <summary>
        /// Returns true if the collectable spot's position and group name are valid.
        /// </summary>
        /// <param name="template">The room template.</param>
        public bool IsValid(RoomTemplate template)
        {
            return template.Cells.IndexExists(Position.X, Position.Y)
                && !string.IsNullOrWhiteSpace(Group);
        }
    }
}
