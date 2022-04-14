using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Represents a possible door in a RoomTemplate.
    /// </summary>
    [DataContract]
    public class Door
    {
        /// <summary>
        /// Returns a new two-way door.
        /// </summary>
        public static Door TwoWay => new Door(DoorType.TwoWay);

        /// <summary>
        /// Returns a new two-way exit door.
        /// </summary>
        public static Door TwoWayExit => new Door(DoorType.TwoWayExit);

        /// <summary>
        /// Returns a new two-way entrance door.
        /// </summary>
        public static Door TwoWayEntrance => new Door(DoorType.TwoWayEntrance);

        /// <summary>
        /// Returns a new one-way exit door.
        /// </summary>
        public static Door OneWayExit => new Door(DoorType.OneWayExit);

        /// <summary>
        /// Returns a new one-way entrance door.
        /// </summary>
        public static Door OneWayEntrance => new Door(DoorType.OneWayEntrance);

        /// <summary>
        /// The door type.
        /// </summary>
        [DataMember(Order = 1)]
        public DoorType Type { get; set; }

        /// <summary>
        /// The door code required for matching two doors.
        /// </summary>
        [DataMember(Order = 2)]
        public int Code { get; set; }

        /// <summary>
        /// Initializes a new door.
        /// </summary>
        public Door(DoorType type = DoorType.None, int code = 0)
        {
            Type = type;
            Code = code;
        }

        /// <summary>
        /// Initializes a copy of a door.
        /// </summary>
        /// <param name="other">The door to be copied.</param>
        private Door(Door other)
        {
            Type = other.Type;
            Code = other.Code;
        }

        public override string ToString()
        {
            return $"Door(Type = {Type}, Code = {Code})";
        }

        /// <summary>
        /// Returns a copy of the door.
        /// </summary>
        public Door Copy()
        {
            return new Door(this);
        }

        /// <summary>
        /// Sets the door code and returns the door.
        /// </summary>
        public Door SetCode(int code)
        {
            Code = code;
            return this;
        }

        /// <summary>
        /// Returns true if the two doors have matching values.
        /// </summary>
        public static bool ValuesAreEqual(Door x, Door y)
        {
            if (x == y)
                return true;

            if (x == null || y == null)
                return false;

            return x.ValuesAreEqual(y);
        }

        /// <summary>
        /// Returns true if the values of the two doors match.
        /// </summary>
        public bool ValuesAreEqual(Door other)
        {
            return Code == other.Code && Type == other.Type;
        }

        /// <summary>
        /// Returns true if the door code and type aligns with the other door.
        /// </summary>
        public bool Aligns(Door other)
        {
            return Code == other.Code && DoorTypesAlign(Type, other.Type);
        }

        /// <summary>
        /// Returns true if the door types are compatible.
        /// </summary>
        /// <param name="from">The from door type.</param>
        /// <param name="to">The to door type.</param>
        /// <exception cref="ArgumentException">Raised if an unhandled door type is submitted.</exception>
        public static bool DoorTypesAlign(DoorType from, DoorType to)
        {
            if (to == DoorType.None)
                return false;

            switch (from)
            {
                case DoorType.None:
                    return false;
                case DoorType.TwoWay:
                    return true;
                case DoorType.TwoWayExit:
                case DoorType.OneWayExit:
                    return to != DoorType.TwoWayExit && to != DoorType.OneWayExit;
                case DoorType.TwoWayEntrance:
                case DoorType.OneWayEntrance:
                    return to != DoorType.TwoWayEntrance && to != DoorType.OneWayEntrance;
                default:
                    throw new ArgumentException($"Unhandled Door Type: {from}.");
            }
        }

        /// <summary>
        /// Returns the edge direction corresponding to the door type combination.
        /// </summary>
        /// <param name="from">The from door type.</param>
        /// <param name="to">Tne to door type.</param>
        /// <exception cref="ArgumentException">Raised is the door types are not compatible.</exception>
        public static EdgeDirection GetEdgeDirection(DoorType from, DoorType to)
        {
            switch (from)
            {
                case DoorType.TwoWay:
                    return TwoWayEdgeDirection(to);
                case DoorType.TwoWayExit:
                    return TwoWayExitEdgeDirection(to);
                case DoorType.TwoWayEntrance:
                    return TwoWayEntranceEdgeDirection(to);
                case DoorType.OneWayExit:
                    return OneWayExitEdgeDirection(to);
                case DoorType.OneWayEntrance:
                    return OneWayEntranceEdgeDirection(to);
                default:
                    throw new ArgumentException($"Unhandled Door Type: {from}.");
            }
        }

        private static EdgeDirection TwoWayEdgeDirection(DoorType to)
        {
            switch (to)
            {
                case DoorType.TwoWay:
                    return EdgeDirection.Both;
                case DoorType.TwoWayExit:
                    return EdgeDirection.ReverseFlexible;
                case DoorType.TwoWayEntrance:
                    return EdgeDirection.ForwardFlexible;
                case DoorType.OneWayExit:
                    return EdgeDirection.ReverseFixed;
                case DoorType.OneWayEntrance:
                    return EdgeDirection.ForwardFixed;
                default:
                    throw new ArgumentException($"Unhandled Door Type: {to}.");
            }
        }

        private static EdgeDirection TwoWayExitEdgeDirection(DoorType to)
        {
            switch (to)
            {
                case DoorType.TwoWay:
                case DoorType.TwoWayEntrance:
                    return EdgeDirection.ForwardFlexible;
                case DoorType.OneWayEntrance:
                    return EdgeDirection.ForwardFixed;
                case DoorType.TwoWayExit:
                case DoorType.OneWayExit:
                    throw new ArgumentException($"Invalid Door Type combination: ({DoorType.TwoWayExit}, {to}).");
                default:
                    throw new ArgumentException($"Unhandled Door Type: {to}.");
            }
        }

        private static EdgeDirection TwoWayEntranceEdgeDirection(DoorType to)
        {
            switch (to)
            {
                case DoorType.TwoWay:
                case DoorType.TwoWayExit:
                    return EdgeDirection.ReverseFlexible;
                case DoorType.OneWayExit:
                    return EdgeDirection.ReverseFixed;
                case DoorType.TwoWayEntrance:
                case DoorType.OneWayEntrance:
                    throw new ArgumentException($"Invalid Door Type combination: ({DoorType.TwoWayEntrance}, {to}).");
                default:
                    throw new ArgumentException($"Unhandled Door Type: {to}.");
            }
        }

        private static EdgeDirection OneWayExitEdgeDirection(DoorType to)
        {
            switch (to)
            {
                case DoorType.TwoWay:
                case DoorType.TwoWayEntrance:
                case DoorType.OneWayEntrance:
                    return EdgeDirection.ForwardFixed;
                case DoorType.TwoWayExit:
                case DoorType.OneWayExit:
                    throw new ArgumentException($"Invalid Door Type combination: ({DoorType.OneWayExit}, {to}).");
                default:
                    throw new ArgumentException($"Unhandled Door Type: {to}.");
            }
        }

        private static EdgeDirection OneWayEntranceEdgeDirection(DoorType to)
        {
            switch (to)
            {
                case DoorType.TwoWay:
                case DoorType.TwoWayExit:
                case DoorType.OneWayExit:
                    return EdgeDirection.ReverseFixed;
                case DoorType.TwoWayEntrance:
                case DoorType.OneWayEntrance:
                    throw new ArgumentException($"Invalid Door Type combination: ({DoorType.OneWayEntrance}, {to}).");
                default:
                    throw new ArgumentException($"Unhandled Door Type: {to}.");
            }
        }
    }
}
