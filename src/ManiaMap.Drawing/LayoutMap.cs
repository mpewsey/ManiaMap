using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System.Collections.Generic;
using System.IO;

namespace MPewsey.ManiaMap.Drawing
{
    /// <summary>
    /// A class for generating maps from a layout and layout state.
    /// </summary>
    public class LayoutMap
    {
        /// <summary>
        /// The room layout.
        /// </summary>
        public Layout Layout { get; set; }

        /// <summary>
        /// The size of the map tiles used in (x, y) coordinates.
        /// </summary>
        public Point TileSize { get; set; } = new Point(16, 16);

        /// <summary>
        /// The padding to include around the layout when the map is drawn.
        /// </summary>
        public Padding Padding { get; set; } = new Padding(1);

        /// <summary>
        /// The map background color.
        /// </summary>
        public Color BackgroundColor { get; set; } = Color.Black;

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
        /// The layout state, used to determine which tiles are visible when drawing a map.
        /// If this value is null, all tiles will be drawn.
        /// </summary>
        public LayoutState LayoutState { get; set; }

        /// <summary>
        /// A dictionary of room door positions by room ID.
        /// </summary>
        private Dictionary<Uid, List<DoorPosition>> RoomDoors { get; set; }

        /// <summary>
        /// The bounds of the layout.
        /// </summary>
        private System.Drawing.Rectangle LayoutBounds { get; set; }

        /// <summary>
        /// Initializes a layout map.
        /// </summary>
        /// <param name="layout">The layout.</param>
        /// <param name="layoutState">The layout state.</param>
        /// <param name="tileSize">The tile size. If null, the default property value will be used.</param>
        /// <param name="padding">The padding around the layout. If null, the default property value will be used.</param>
        /// <param name="tiles">A dictionary of map tiles to use. If null, the default tiles will be used.</param>
        /// <param name="backgroundColor">The background color. If null, the default property value will be used.</param>
        public LayoutMap(Layout layout, LayoutState layoutState = null,
            Point? tileSize = null, Padding? padding = null,
            Dictionary<string, Image> tiles = null, Color? backgroundColor = null)
        {
            Layout = layout;
            LayoutState = layoutState;
            TileSize = tileSize ?? TileSize;
            Padding = padding ?? Padding;
            BackgroundColor = backgroundColor ?? BackgroundColor;
            Tiles = tiles ?? MapTiles.GetDefaultTiles();
        }

        public override string ToString()
        {
            return $"LayoutMap(Layout = {Layout})";
        }

        /// <summary>
        /// Renders a map of the layout and saves it to the designated file path.
        /// </summary>
        /// <param name="path">The file path to which the image will be saved.</param>
        /// <param name="z">The z (layer) value used to render the layout.</param>
        public void SaveImage(string path, int z = 0)
        {
            var map = CreateImage(z);
            map.Save(path);
        }

        /// <summary>
        /// Renders map images of all layout layers and saves them to the designated file path.
        /// The z (layer) values are added into the file paths before the file extension.
        /// </summary>
        /// <param name="path">The file path.</param>
        public void SaveImages(string path)
        {
            var ext = Path.GetExtension(path);
            var name = Path.ChangeExtension(path, null);
            var maps = CreateImages();

            foreach (var pair in maps)
            {
                pair.Value.Save($"{name}_Z={pair.Key}{ext}");
            }
        }

        /// <summary>
        /// Returns true if the door exists for the room.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <param name="x">The local x value of the door.</param>
        /// <param name="y">The local y value of the door.</param>
        /// <param name="direction">The direction of the door.</param>
        private bool DoorExists(Room room, int x, int y, DoorDirection direction)
        {
            if (RoomDoors.TryGetValue(room.Id, out var doors))
            {
                foreach (var door in doors)
                {
                    if (door.Matches(x, y, direction))
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Returns a rendered map of the layout.
        /// </summary>
        /// <param name="z">The z (layer) value used to render the layout.</param>
        public Image CreateImage(int z = 0)
        {
            LayoutBounds = Layout.Bounds();
            RoomDoors = Layout.RoomDoors();
            var width = TileSize.X * (Padding.Left + Padding.Right + LayoutBounds.Width);
            var height = TileSize.Y * (Padding.Top + Padding.Bottom + LayoutBounds.Height);
            var map = new Image<Rgba32>(width, height);
            map.Mutate(x => DrawMap(x, z));
            return map;
        }

        /// <summary>
        /// Returns a dictionary of map layer images by z (layer) value.
        /// </summary>
        public Dictionary<int, Image> CreateImages()
        {
            LayoutBounds = Layout.Bounds();
            RoomDoors = Layout.RoomDoors();
            var width = TileSize.X * (Padding.Left + Padding.Right + LayoutBounds.Width);
            var height = TileSize.Y * (Padding.Top + Padding.Bottom + LayoutBounds.Height);
            var dict = new Dictionary<int, Image>();

            foreach (var room in Layout.Rooms.Values)
            {
                if (!dict.ContainsKey(room.Z))
                {
                    var map = new Image<Rgba32>(width, height);
                    map.Mutate(x => DrawMap(x, room.Z));
                    dict.Add(room.Z, map);
                }
            }

            return dict;
        }

        /// <summary>
        /// Draws the layout map onto the image context.
        /// </summary>
        /// <param name="image">The image context.</param>
        /// <param name="z">The z (layer) value used to render the layout.</param>
        private void DrawMap(IImageProcessingContext image, int z)
        {
            image.BackgroundColor(BackgroundColor);
            DrawGrid(image);
            DrawMapTiles(image, z);
        }

        /// <summary>
        /// Draws the grid tiles onto the image context.
        /// </summary>
        /// <param name="image">The image context.</param>
        private void DrawGrid(IImageProcessingContext image)
        {
            if (Tiles.TryGetValue("Grid", out Image gridTile))
            {
                var (width, height) = image.GetCurrentSize();

                for (int x = 0; x < width; x += TileSize.X)
                {
                    for (int y = 0; y < height; y += TileSize.Y)
                    {
                        image.DrawImage(gridTile, new Point(x, y), 1);
                    }
                }
            }
        }

        /// <summary>
        /// Draws the room map tiles onto the image context.
        /// </summary>
        /// <param name="image">The image context.</param>
        /// <param name="z">The z (layer) value used to render the layout.</param>
        private void DrawMapTiles(IImageProcessingContext image, int z)
        {
            var cellTile = new Image<Rgba32>(TileSize.X, TileSize.Y);

            foreach (var room in Layout.Rooms.Values)
            {
                // If room Z (layer) value is not equal, go to next room.
                if (room.Z != z)
                    continue;

                var roomState = LayoutState?.RoomStates[room.Id];
                var cells = room.Template.Cells;
                var x0 = (room.Y - LayoutBounds.X + Padding.Left) * TileSize.X;
                var y0 = (room.X - LayoutBounds.Y + Padding.Top) * TileSize.Y;
                cellTile.Mutate(x => x.BackgroundColor(ConvertColor(room.Color)));

                for (int i = 0; i < cells.Rows; i++)
                {
                    for (int j = 0; j < cells.Columns; j++)
                    {
                        var cell = cells[i, j];

                        // If cell it empty, go to next cell.
                        if (cell == null)
                            continue;

                        // If room state is defined and is not visible, go to next cell.
                        if (roomState != null && !roomState.Visibility.GetOrDefault(i, j))
                            continue;

                        // Calculate draw position
                        var x = TileSize.X * j + x0;
                        var y = TileSize.Y * i + y0;
                        var point = new Point(x, y);

                        // Get adjacent cells
                        var north = cells.GetOrDefault(i - 1, j);
                        var south = cells.GetOrDefault(i + 1, j);
                        var west = cells.GetOrDefault(i, j - 1);
                        var east = cells.GetOrDefault(i, j + 1);

                        // Get the wall or door tiles
                        var topTile = GetTile(room, i, j, DoorDirection.Top, cell.TopDoor, null, "TopDoor", null);
                        var bottomTile = GetTile(room, i, j, DoorDirection.Bottom, cell.BottomDoor, null, "BottomDoor", null);
                        var northTile = GetTile(room, i, j, DoorDirection.North, cell.NorthDoor, north, "NorthDoor", "NorthWall");
                        var southTile = GetTile(room, i, j, DoorDirection.South, cell.SouthDoor, south, "SouthDoor", "SouthWall");
                        var westTile = GetTile(room, i, j, DoorDirection.West, cell.WestDoor, west, "WestDoor", "WestWall");
                        var eastTile = GetTile(room, i, j, DoorDirection.East, cell.EastDoor, east, "EastDoor", "EastWall");

                        // Add cell background fill
                        image.DrawImage(cellTile, point, 1);

                        // Superimpose applicable map tiles
                        if (northTile != null)
                            image.DrawImage(northTile, point, 1);
                        if (southTile != null)
                            image.DrawImage(southTile, point, 1);
                        if (westTile != null)
                            image.DrawImage(westTile, point, 1);
                        if (eastTile != null)
                            image.DrawImage(eastTile, point, 1);
                        if (topTile != null)
                            image.DrawImage(topTile, point, 1);
                        if (bottomTile != null)
                            image.DrawImage(bottomTile, point, 1);
                    }
                }
            }
        }

        /// <summary>
        /// Converts a `System.Drawing` color to an ImageSharp color.
        /// </summary>
        /// <param name="color">The `System.Drawing` color.</param>
        private static Color ConvertColor(System.Drawing.Color color)
        {
            return Color.FromRgba(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Returns the map tile corresponding to the wall or door location.
        /// Returns null if the tile has neither a wall or door.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <param name="x">The local x coordinate.</param>
        /// <param name="y">The local y coordinate.</param>
        /// <param name="direction">The door direction.</param>
        /// <param name="door">The door.</param>
        /// <param name="neighbor">The neighbor cell in the door direction. The neighbor can be null.</param>
        /// <param name="doorName">The door map tile name.</param>
        /// <param name="wallName">The wall map tile name. If null, the wall tile will not be used.</param>
        /// <returns></returns>
        private Image GetTile(Room room, int x, int y, DoorDirection direction,
            Door door, Cell neighbor, string doorName, string wallName)
        {
            if (door != null && door.Type != DoorType.None && DoorExists(room, x, y, direction))
                return Tiles[doorName];
            if (neighbor == null && wallName != null)
                return Tiles[wallName];
            return null;
        }
    }
}
