﻿using SixLabors.ImageSharp;
using System.Collections.Generic;
using System.Reflection;

namespace MPewsey.ManiaMap.Drawing
{
    public static class MapTiles
    {
        /// <summary>
        /// Loads the map tile from resources at the specified path.
        /// </summary>
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
            const string path = "ManiaMap.Drawing.MapTiles.Default.";

            return new Dictionary<string, Image>
            {
                { "SouthDoor", LoadTile(path + "SouthDoor.png") },
                { "NorthDoor", LoadTile(path + "NorthDoor.png") },
                { "WestDoor", LoadTile(path + "WestDoor.png") },
                { "EastDoor", LoadTile(path + "EastDoor.png") },
                { "SouthWall", LoadTile(path + "SouthWall.png") },
                { "NorthWall", LoadTile(path + "NorthWall.png") },
                { "WestWall", LoadTile(path + "WestWall.png") },
                { "EastWall", LoadTile(path + "EastWall.png") },
                { "Grid", LoadTile(path + "Grid.png") },
            };
        }
    }
}
