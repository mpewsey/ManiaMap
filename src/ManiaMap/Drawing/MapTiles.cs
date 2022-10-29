using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.Reflection;

namespace MPewsey.ManiaMap.Drawing
{
    /// <summary>
    /// This class contains methods for loading built-in map tiles.
    /// </summary>
    public static class MapTiles
    {
        /// <summary>
        /// Loads the map tile from resources at the specified path.
        /// </summary>
        /// <param name="path">The resource path.</param>
        private static Image LoadTile(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(path))
            {
                return Image.Load(stream);
            }
        }

        /// <summary>
        /// Returns a dictionary of default map tiles.
        /// </summary>
        public static Dictionary<string, Image> GetDefaultTiles()
        {
            return GetTiles("Default");
        }

        /// <summary>
        /// Returns a dictionary of built-in map tiles.
        /// </summary>
        /// <param name="folder">The name of the folder within the map tiles directory.</param>
        private static Dictionary<string, Image> GetTiles(string folder)
        {
            var path = "MPewsey.ManiaMap.Drawing.MapTiles." + folder + ".";

            return new Dictionary<string, Image>
            {
                { MapTileType.SouthDoor, LoadTile(path + "SouthDoor.png") },
                { MapTileType.NorthDoor, LoadTile(path + "NorthDoor.png") },
                { MapTileType.WestDoor, LoadTile(path + "WestDoor.png") },
                { MapTileType.EastDoor, LoadTile(path + "EastDoor.png") },
                { MapTileType.SouthWall, LoadTile(path + "SouthWall.png") },
                { MapTileType.NorthWall, LoadTile(path + "NorthWall.png") },
                { MapTileType.WestWall, LoadTile(path + "WestWall.png") },
                { MapTileType.EastWall, LoadTile(path + "EastWall.png") },
                { MapTileType.TopDoor, LoadTile(path + "TopDoor.png") },
                { MapTileType.BottomDoor, LoadTile(path + "BottomDoor.png") },
                { MapTileType.Grid, LoadTile(path + "Grid.png") },
                { MapTileType.SavePoint, LoadTile(path + "SavePoint.png") },
            };
        }
    }
}
