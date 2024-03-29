﻿using MPewsey.Common.Mathematics;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class referencing a Door and its local position within a Room.
    /// </summary>
    [DataContract(Namespace = Constants.DataContractNamespace)]
    public class DoorPosition
    {
        /// <summary>
        /// The local door position.
        /// </summary>
        [DataMember(Order = 1)]
        public Vector2DInt Position { get; private set; }

        /// <summary>
        /// The door direction.
        /// </summary>
        [DataMember(Order = 3)]
        public DoorDirection Direction { get; private set; }

        /// <summary>
        /// The referenced door.
        /// </summary>
        [DataMember(Order = 4)]
        public Door Door { get; set; }

        /// <summary>
        /// Initializes an object.
        /// </summary>
        /// <param name="position">The local position.</param>
        /// <param name="direction">The door direction.</param>
        /// <param name="door">The referenced door.</param>
        public DoorPosition(Vector2DInt position, DoorDirection direction, Door door)
        {
            Position = position;
            Direction = direction;
            Door = door;
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"DoorPosition(Position = {Position}, Direction = {Direction}, Door = {Door})";
        }

        /// <summary>
        /// Returns true if the door matches the specified properties.
        /// </summary>
        /// <param name="position">The local door position.</param>
        /// <param name="direction">The door direction.</param>
        public bool Matches(Vector2DInt position, DoorDirection direction)
        {
            return Position == position && Direction == direction;
        }
    }
}
