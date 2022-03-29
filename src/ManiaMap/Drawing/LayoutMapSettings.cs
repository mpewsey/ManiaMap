using SixLabors.ImageSharp;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Drawing
{
    /// <summary>
    /// A container for storing `LayoutMap` settings.
    /// </summary>
    public class LayoutMapSettings
    {
        /// <summary>
        /// The size of the map tiles used in (x, y) coordinates.
        /// </summary>
        public Vector2DInt TileSize { get; set; } = new Vector2DInt(16, 16);

        /// <summary>
        /// The padding to include around the layout when the map is drawn.
        /// </summary>
        public Padding Padding { get; set; } = new Padding(1);

        /// <summary>
        /// The map background color.
        /// </summary>
        public System.Drawing.Color BackgroundColor { get; set; } = System.Drawing.Color.Black;

        /// <summary>
        /// A dictionary of map tiles by name. The applicable tiles are superimposed at the cell location
        /// when the map is drawn. The following names are used internally:
        /// 
        /// * "NorthDoor", "TopDoor", etc. - Tiles used when there is a door in that direction.
        /// * "NorthWall", "TopWall", etc. - Tiles used when there is a wall in that direction.
        /// * "Grid" (Optional) - If specified, used to fill the background before any tiles are drawn.
        /// </summary>
        public Dictionary<string, Image> Tiles { get; set; }

        /// <summary>
        /// Initializes the settings.
        /// </summary>
        /// <param name="tileSize">The tile size. If null, the default property value will be used.</param>
        /// <param name="padding">The padding around the layout. If null, the default property value will be used.</param>
        /// <param name="tiles">A dictionary of map tiles to use. If null, the default tiles will be used.</param>
        /// <param name="backgroundColor">The background color. If null, the default property value will be used.</param>
        public LayoutMapSettings(Padding? padding = null, System.Drawing.Color? backgroundColor = null,
            Vector2DInt? tileSize = null, Dictionary<string, Image> tiles = null)
        {
            TileSize = tileSize ?? TileSize;
            Padding = padding ?? Padding;
            BackgroundColor = backgroundColor ?? BackgroundColor;
            Tiles = tiles ?? MapTiles.GetDefaultTiles();
        }
    }
}
