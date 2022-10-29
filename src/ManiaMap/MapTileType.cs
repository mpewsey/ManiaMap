using System;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Type names corresponding to map tiles.
    /// </summary>
    public static class MapTileType
    {
        public static string None { get; } = null;
        public static string Grid { get; } = "Grid";
        public static string NorthDoor { get; } = "NorthDoor";
        public static string SouthDoor { get; } = "SouthDoor";
        public static string EastDoor { get; } = "EastDoor";
        public static string WestDoor { get; } = "WestDoor";
        public static string TopDoor { get; } = "TopDoor";
        public static string BottomDoor { get; } = "BottomDoor";
        public static string NorthWall { get; } = "NorthWall";
        public static string SouthWall { get; } = "SouthWall";
        public static string EastWall { get; } = "EastWall";
        public static string WestWall { get; } = "WestWall";
        public static string SavePoint { get; } = "SavePoint";

        /// <summary>
        /// Returns the door tile type corresponding to the direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <exception cref="ArgumentException">Raised if the direction is not handled.</exception>
        public static string GetDoorTileType(DoorDirection direction)
        {
            switch (direction)
            {
                case DoorDirection.North:
                    return NorthDoor;
                case DoorDirection.South:
                    return SouthDoor;
                case DoorDirection.East:
                    return EastDoor;
                case DoorDirection.West:
                    return WestDoor;
                case DoorDirection.Top:
                    return TopDoor;
                case DoorDirection.Bottom:
                    return BottomDoor;
                default:
                    throw new ArgumentException($"Unhandled direction: {direction}.");
            }
        }

        /// <summary>
        /// Returns the wall tile type corresponding to the direction.
        /// </summary>
        /// <param name="direction">The direction.</param>
        /// <exception cref="ArgumentException">Raised if the direction is not handled.</exception>
        public static string GetWallTileType(DoorDirection direction)
        {
            switch (direction)
            {
                case DoorDirection.North:
                    return NorthWall;
                case DoorDirection.South:
                    return SouthWall;
                case DoorDirection.East:
                    return EastWall;
                case DoorDirection.West:
                    return WestWall;
                case DoorDirection.Top:
                case DoorDirection.Bottom:
                    return None;
                default:
                    throw new ArgumentException($"Unhandled direction: {direction}.");
            }
        }
    }
}
