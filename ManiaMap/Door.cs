using System;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    [DataContract(IsReference = true)]
    public class Door
    {
        [DataMember(Order = 1)]
        public int X { get; private set; }

        [DataMember(Order = 2)]
        public int Y { get; private set; }

        [DataMember(Order = 3)]
        public DoorDirection Direction { get; private set; }

        [DataMember(Order = 4)]
        public DoorType Type { get; set; }

        public Door(DoorType type = DoorType.TwoWay)
        {
            Type = type;
        }

        public override string ToString()
        {
            return $"Door(X = {X}, Y = {Y}, Direction = {Direction}, Type = {Type})";
        }

        /// <summary>
        /// Returns a new door with the same door type.
        /// </summary>
        public Door CopyType()
        {
            return new Door(Type);
        }

        /// <summary>
        /// Returns true if the door matches the specified properties.
        /// </summary>
        public bool Matches(int x, int y, DoorDirection direction)
        {
            return X == x && Y == y && Direction == direction;
        }

        /// <summary>
        /// Sets the properties of the door.
        /// </summary>
        public void SetProperties(int x, int y, DoorDirection direction)
        {
            X = x;
            Y = y;
            Direction = direction;
        }

        /// <summary>
        /// Returns true if the door types are compatible.
        /// </summary>
        public static bool DoorTypesAligns(DoorType from, DoorType to)
        {
            switch (from)
            {
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

        /// <summary>
        /// Returns the reverse of the specified direction.
        /// </summary>
        public static EdgeDirection ReverseEdgeDirection(EdgeDirection direction)
        {
            switch (direction)
            {
                case EdgeDirection.Both:
                    return EdgeDirection.Both;
                case EdgeDirection.ForwardFlexible:
                    return EdgeDirection.ReverseFlexible;
                case EdgeDirection.ForwardFixed:
                    return EdgeDirection.ReverseFixed;
                case EdgeDirection.ReverseFlexible:
                    return EdgeDirection.ForwardFlexible;
                case EdgeDirection.ReverseFixed:
                    return EdgeDirection.ForwardFixed;
                default:
                    throw new ArgumentException($"Unhandled Edge Direction: {direction}.");
            }
        }
    }
}
