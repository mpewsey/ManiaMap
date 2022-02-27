using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract]
    public class Door
    {
        public static Door TwoWay { get => new Door { Type = DoorType.TwoWay }; }
        public static Door TwoWayExit { get => new Door { Type = DoorType.TwoWayExit }; }
        public static Door TwoWayEntrance { get => new Door { Type = DoorType.TwoWayEntrance }; }
        public static Door OneWayExit { get => new Door { Type = DoorType.OneWayExit }; }
        public static Door OneWayEntrance { get => new Door { Type = DoorType.OneWayEntrance }; }

        [DataMember(Order = 1)]
        public DoorType Type { get; set; }

        [DataMember(Order = 2)]
        public int Code { get; set; }

        public override string ToString()
        {
            return $"Door(Type = {Type}, Code = {Code})";
        }

        /// <summary>
        /// Returns a copy of the door.
        /// </summary>
        public Door Copy()
        {
            return new Door { Type = Type, Code = Code };
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
        /// Returns true if the values of the two doors match.
        /// </summary>
        public bool Matches(Door other)
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
