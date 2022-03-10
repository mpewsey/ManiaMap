﻿namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A configuration where two `RoomTemplate` can be joined.
    /// </summary>
    public class Configuration
    {
        /// <summary>
        /// The to template position relative to the from template.
        /// </summary>
        public Vector2DInt Position { get; }

        /// <summary>
        /// The connected from door position.
        /// </summary>
        public DoorPosition FromDoor { get; }

        /// <summary>
        /// The connected to door position.
        /// </summary>
        public DoorPosition ToDoor { get; }

        /// <summary>
        /// The edge direction corresponding to the doors.
        /// </summary>
        public EdgeDirection EdgeDirection { get; }

        /// <summary>
        /// Initializes a configuration from offset and door positions.
        /// </summary>
        /// <param name="position">The to template position relative to the from template.</param>
        /// <param name="from">The from door position.</param>
        /// <param name="to">The to door position.</param>
        public Configuration(Vector2DInt position, DoorPosition from, DoorPosition to)
        {
            Position = position;
            FromDoor = from;
            ToDoor = to;
            EdgeDirection = Door.GetEdgeDirection(from.Door.Type, to.Door.Type);
        }

        public override string ToString()
        {
            return $"Configuration(Position = {Position}, FromDoor = {FromDoor}, ToDoor = {ToDoor})";
        }

        /// <summary>
        /// Returns true if the parameters match the configuration.
        /// </summary>
        /// <param name="z">The z offset between the templates.</param>
        public bool Matches(int z)
        {
            if (z > 0)
            {
                return FromDoor.Direction == DoorDirection.Top
                    && ToDoor.Direction == DoorDirection.Bottom;
            }

            if (z < 0)
            {
                return FromDoor.Direction == DoorDirection.Bottom
                    && ToDoor.Direction == DoorDirection.Top;
            }

            return true;
        }

        /// <summary>
        /// Returns true if the parameters match the configuration.
        /// </summary>
        /// <param name="z">The z offset between the templates.</param>
        /// <param name="code">The door code.</param>
        /// <param name="direction">The edge direction.</param>
        public bool Matches(int z, int code, EdgeDirection direction)
        {
            return EdgeDirection == direction
                && FromDoor.Door.Code == code
                && ToDoor.Door.Code == code
                && Matches(z);
        }

        /// <summary>
        /// Returns true if the parameters match the configuration.
        /// </summary>
        /// <param name="x">The x offset between the templates.</param>
        /// <param name="y">The y offset between the templates.</param>
        /// <param name="z">The z offset between the templates.</param>
        /// <param name="code">The door code.</param>
        /// <param name="direction">The edge direction.</param>
        public bool Matches(int x, int y, int z, int code, EdgeDirection direction)
        {
            return Position.X == x
                && Position.Y == y
                && Matches(z, code, direction);
        }
    }
}