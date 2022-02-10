using System;
using System.Collections.Generic;
using System.Drawing;
using System.Reflection;
using System.Text;

namespace ManiaMap
{
    public static class MapTiles
    {
        /// <summary>
        /// Loads the map tile from resources at the specified path.
        /// </summary>
        private static Bitmap LoadTile(string path)
        {
            var assembly = Assembly.GetExecutingAssembly();

            using (var stream = assembly.GetManifestResourceStream(path))
            {
                return new Bitmap(stream);
            }
        }

        /// <summary>
        /// Returns a dictionary of default map tiles.
        /// </summary>
        public static Dictionary<string, Bitmap> GetDefaultTiles()
        {
            const string path = "ManiaMap.MapTiles.Default.";
            
            return new Dictionary<string, Bitmap>
            {
                { "BottomDoor", LoadTile(path + "BottomDoor.png") },
                { "TopDoor", LoadTile(path + "TopDoor.png") },
                { "LeftDoor", LoadTile(path + "LeftDoor.png") },
                { "RightDoor", LoadTile(path + "RightDoor.png") },
                { "BottomWall", LoadTile(path + "BottomWall.png") },
                { "TopWall", LoadTile(path + "TopWall.png") },
                { "LeftWall", LoadTile(path + "LeftWall.png") },
                { "RightWall", LoadTile(path + "RightWall.png") },
                { "Grid", LoadTile(path + "Grid.png") },
            };
        }
    }
}
