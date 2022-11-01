using System;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// Type names corresponding to map tiles.
    /// </summary>
    public static class MapTileType
    {
        /// <summary>
        /// Returns null, the value of no tile.
        /// </summary>
        public static string None { get; } = null;

        /// <summary>
        /// The name of the grid tile.
        /// </summary>
        public static string Grid { get; } = "Grid";

        /// <summary>
        /// The name of the north door tile.
        /// </summary>
        public static string NorthDoor { get; } = "NorthDoor";

        /// <summary>
        /// The name of the south door tile.
        /// </summary>
        public static string SouthDoor { get; } = "SouthDoor";

        /// <summary>
        /// The name of the east door tile.
        /// </summary>
        public static string EastDoor { get; } = "EastDoor";

        /// <summary>
        /// The name of the west door tile.
        /// </summary>
        public static string WestDoor { get; } = "WestDoor";

        /// <summary>
        /// The name of the top door tile.
        /// </summary>
        public static string TopDoor { get; } = "TopDoor";

        /// <summary>
        /// The name of the bottom door tile.
        /// </summary>
        public static string BottomDoor { get; } = "BottomDoor";

        /// <summary>
        /// The name of the north wall tile.
        /// </summary>
        public static string NorthWall { get; } = "NorthWall";

        /// <summary>
        /// The name of the south wall tile.
        /// </summary>
        public static string SouthWall { get; } = "SouthWall";

        /// <summary>
        /// The name of the east wall tile.
        /// </summary>
        public static string EastWall { get; } = "EastWall";

        /// <summary>
        /// The name of the west wall tile.
        /// </summary>
        public static string WestWall { get; } = "WestWall";

        /// <summary>
        /// The name of the save point tile.
        /// </summary>
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
