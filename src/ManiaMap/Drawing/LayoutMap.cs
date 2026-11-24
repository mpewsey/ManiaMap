using MPewsey.Common.Mathematics;
using SkiaSharp;
using System.Collections.Generic;
using System.IO;

namespace MPewsey.ManiaMap.Drawing
{
    /// <summary>
    /// A class for generating maps from a Layout and LayoutState.
    /// </summary>
    public class LayoutMap
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
        public Color4 BackgroundColor { get; set; } = new Color4(0, 0, 0, 255);

        /// <summary>
        /// The room color if visible.
        /// </summary>
        public Color4 RoomColor { get; set; } = new Color4(75, 75, 75, 255);

        /// <summary>
        /// An option controlling which doors will be drawn.
        /// </summary>
        public DoorDrawMode DoorDrawMode { get; set; } = DoorDrawMode.AllDoors;

        /// <summary>
        /// A dictionary of map tiles by name. The applicable tiles are superimposed at the cell location
        /// when the map is drawn. The following names are used internally:
        /// 
        /// * "NorthDoor", "TopDoor", etc. - Tiles used when there is a door in that direction.
        /// * "NorthWall", "TopWall", etc. - Tiles used when there is a wall in that direction.
        /// * "Grid" (Optional) - If specified, used to fill the background before any tiles are drawn.
        /// 
        /// Features assigned to cells will also be checked against this dictionary and drawn
        /// if the tile exists.
        /// </summary>
        public Dictionary<string, SKImage> Tiles { get; set; }

        /// <summary>
        /// The room layout.
        /// </summary>
        private Layout Layout { get; set; }

        /// <summary>
        /// The layout state, used to determine which tiles are visible when drawing a map.
        /// If this value is null, all tiles will be drawn.
        /// </summary>
        private LayoutState LayoutState { get; set; }

        /// <summary>
        /// A dictionary of room door positions by room ID.
        /// </summary>
        private Dictionary<Uid, List<DoorPosition>> RoomDoors { get; set; }

        /// <summary>
        /// The bounds of the layout.
        /// </summary>
        private RectangleInt LayoutBounds { get; set; }

        /// <summary>
        /// Initializes the settings.
        /// </summary>
        /// <param name="padding">The padding around the layout. If null, the default property value will be used.</param>
        /// <param name="backgroundColor">The background color. If null, the default property value will be used.</param>
        /// <param name="doorDrawMode">An option controlling which doors will be drawn.</param>
        /// <param name="roomColor">The room color if visible. If null, the default property value will be used.</param>
        /// <param name="tileSize">The tile size. If null, the default property value will be used.</param>
        /// <param name="tiles">A dictionary of map tiles to use. If null, the default tiles will be used.</param>
        public LayoutMap(Padding? padding = null, Color4? backgroundColor = null, Color4? roomColor = null,
            DoorDrawMode doorDrawMode = DoorDrawMode.AllDoors, Vector2DInt? tileSize = null, Dictionary<string, SKImage> tiles = null)
        {
            DoorDrawMode = doorDrawMode;
            TileSize = tileSize ?? TileSize;
            Padding = padding ?? Padding;
            BackgroundColor = backgroundColor ?? BackgroundColor;
            RoomColor = roomColor ?? RoomColor;
            Tiles = tiles ?? MapTiles.GetDefaultTiles();
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return $"LayoutMap(TileSize = {TileSize}, Padding = {Padding})";
        }

        /// <summary>
        /// Returns the image format corresponding to the specified file extension.
        /// Supports PNG, JPEG, and WebP.
        /// Returns PNG if no supported matching extension is found.
        /// </summary>
        /// <param name="extension">The file extension.</param>
        public static SKEncodedImageFormat GetImageFormat(string extension)
        {
            switch (extension.ToLowerInvariant())
            {
                case ".jpg":
                case ".jpeg":
                    return SKEncodedImageFormat.Jpeg;
                case ".webp":
                    return SKEncodedImageFormat.Webp;
                case ".png":
                default:
                    return SKEncodedImageFormat.Png;
            }
        }

        /// <summary>
        /// Renders map images of all layout layers and saves them to the designated file path.
        /// The z (layer) values are added into the file paths before the file extension.
        /// </summary>
        /// <param name="path">The file path.</param>
        /// <param name="layout">The room layout.</param>
        /// <param name="state">The room layout state.</param>
        public void SaveImages(string path, Layout layout, LayoutState state = null)
        {
            var extension = Path.GetExtension(path);
            var name = Path.ChangeExtension(path, null);
            var maps = CreateImages(layout, state);
            var format = GetImageFormat(extension);

            foreach (var pair in maps)
            {
                using (var data = pair.Value.Encode(format, 100))
                using (var stream = File.OpenWrite($"{name}_Z={pair.Key}{extension}"))
                {
                    data.SaveTo(stream);
                }
            }
        }

        /// <summary>
        /// Returns true if the door exists for the room.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <param name="position">The local position of the door.</param>
        /// <param name="direction">The direction of the door.</param>
        private bool DoorExists(Room room, Vector2DInt position, DoorDirection direction)
        {
            if (RoomDoors.TryGetValue(room.Id, out var doors))
            {
                foreach (var door in doors)
                {
                    if (door.Matches(position, direction))
                        return true;
                }
            }

            return false;
        }

        /// <summary>
        /// Initializes the object's buffers.
        /// </summary>
        /// <param name="layout">The room layout.</param>
        /// <param name="state">The room layout state.</param>
        private void Initialize(Layout layout, LayoutState state)
        {
            Layout = layout;
            LayoutState = state;
            LayoutBounds = Layout.GetBounds();
            RoomDoors = Layout.GetRoomDoors();
        }

        /// <summary>
        /// Returns a dictionary of map layer images by z (layer) value.
        /// </summary>
        /// <param name="layout">The room layout.</param>
        /// <param name="state">The room layout state.</param>
        public Dictionary<int, SKBitmap> CreateImages(Layout layout, LayoutState state = null)
        {
            Initialize(layout, state);

            var width = TileSize.X * (Padding.Left + Padding.Right + LayoutBounds.Width);
            var height = TileSize.Y * (Padding.Top + Padding.Bottom + LayoutBounds.Height);
            var dict = new Dictionary<int, SKBitmap>();

            foreach (var room in Layout.Rooms.Values)
            {
                if (!dict.ContainsKey(room.Position.Z))
                {
                    var map = new SKBitmap(width, height);
                    DrawMap(map, room.Position.Z);
                    dict.Add(room.Position.Z, map);
                }
            }

            return dict;
        }

        /// <summary>
        /// Draws the layout map onto the image context.
        /// </summary>
        /// <param name="image">The image context.</param>
        /// <param name="z">The z (layer) value used to render the layout.</param>
        private void DrawMap(SKBitmap image, int z)
        {
            using (var canvas = new SKCanvas(image))
            {
                canvas.Clear(ConvertColor(BackgroundColor));
                DrawGrid(canvas, image.Width, image.Height);
                DrawMapTiles(canvas, z);
            }
        }

        /// <summary>
        /// Draws the grid tiles onto the image context.
        /// </summary>
        /// <param name="image">The image context.</param>
        /// <param name="width">The width of the image.</param>
        /// <param name="height">The height of the image.</param>
        private void DrawGrid(SKCanvas image, int width, int height)
        {
            if (Tiles.TryGetValue(MapTileType.Grid, out var gridTile))
            {
                var sampling = SKSamplingOptions.Default;

                for (int x = 0; x < width; x += TileSize.X)
                {
                    for (int y = 0; y < height; y += TileSize.Y)
                    {
                        image.DrawImage(gridTile, new SKPoint(x, y), sampling);
                    }
                }
            }
        }

        private SKImage CreateSolidTile(SKColor color)
        {
            var bitmap = new SKBitmap(TileSize.X, TileSize.Y);

            using (var canvas = new SKCanvas(bitmap))
            {
                canvas.Clear(color);
            }

            return SKImage.FromBitmap(bitmap);
        }

        /// <summary>
        /// Draws the room map tiles onto the image context.
        /// </summary>
        /// <param name="image">The image context.</param>
        /// <param name="z">The z (layer) value used to render the layout.</param>
        private void DrawMapTiles(SKCanvas image, int z)
        {
            var sampling = SKSamplingOptions.Default;
            var roomTile = CreateSolidTile(ConvertColor(RoomColor));

            foreach (var room in Layout.Rooms.Values)
            {
                // If room Z (layer) value is not equal, go to next room.
                if (room.Position.Z != z)
                    continue;

                var roomState = LayoutState?.RoomStates[room.Id];
                var cells = room.Template.Cells;
                var x0 = (room.Position.Y - LayoutBounds.X + Padding.Left) * TileSize.X;
                var y0 = (room.Position.X - LayoutBounds.Y + Padding.Top) * TileSize.Y;
                var cellTile = CreateSolidTile(ConvertColor(room.Color));

                for (int i = 0; i < cells.Rows; i++)
                {
                    for (int j = 0; j < cells.Columns; j++)
                    {
                        var cell = cells[i, j];

                        // If cell is empty, go to next cell.
                        if (cell == null)
                            continue;

                        var position = new Vector2DInt(i, j);
                        var isCompletelyVisible = roomState == null || roomState.CellIsVisible(position);

                        // If room state is defined and is not visible, go to next cell.
                        if (!isCompletelyVisible && !roomState.IsVisible)
                            continue;

                        // Calculate draw position
                        var x = TileSize.X * j + x0;
                        var y = TileSize.Y * i + y0;
                        var point = new SKPoint(x, y);

                        // Get adjacent cells
                        var north = cells.GetOrDefault(i - 1, j);
                        var south = cells.GetOrDefault(i + 1, j);
                        var west = cells.GetOrDefault(i, j - 1);
                        var east = cells.GetOrDefault(i, j + 1);

                        // Get the wall or door tiles
                        var topTile = GetTile(room, cell, null, position, DoorDirection.Top);
                        var bottomTile = GetTile(room, cell, null, position, DoorDirection.Bottom);
                        var northTile = GetTile(room, cell, north, position, DoorDirection.North);
                        var southTile = GetTile(room, cell, south, position, DoorDirection.South);
                        var westTile = GetTile(room, cell, west, position, DoorDirection.West);
                        var eastTile = GetTile(room, cell, east, position, DoorDirection.East);

                        // Add cell background fill
                        var tile = isCompletelyVisible ? cellTile : roomTile;
                        image.DrawImage(tile, point, sampling);

                        // Superimpose applicable map tiles
                        if (northTile != null)
                            image.DrawImage(northTile, point, sampling);
                        if (southTile != null)
                            image.DrawImage(southTile, point, sampling);
                        if (westTile != null)
                            image.DrawImage(westTile, point, sampling);
                        if (eastTile != null)
                            image.DrawImage(eastTile, point, sampling);
                        if (topTile != null)
                            image.DrawImage(topTile, point, sampling);
                        if (bottomTile != null)
                            image.DrawImage(bottomTile, point, sampling);

                        // Draw features only if the cell is completely visible
                        if (isCompletelyVisible)
                            DrawFeatureTiles(image, cell, point);
                    }
                }
            }
        }

        /// <summary>
        /// Draws any feature map tiles assigned to the cell.
        /// </summary>
        /// <param name="image">The image context.</param>
        /// <param name="cell">The cell.</param>
        /// <param name="point">The tile position.</param>
        private void DrawFeatureTiles(SKCanvas image, Cell cell, SKPoint point)
        {
            var sampling = SKSamplingOptions.Default;

            foreach (var tileName in cell.Features)
            {
                if (Tiles.TryGetValue(tileName, out var tile))
                {
                    image.DrawImage(tile, point, sampling);
                }
            }
        }

        /// <summary>
        /// Converts a color to an SkiaSharp color.
        /// </summary>
        /// <param name="color">The color.</param>
        private static SKColor ConvertColor(Color4 color)
        {
            return new SKColor(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Returns the map tile corresponding to the wall or door location.
        /// Returns null if the tile has neither a wall or door.
        /// </summary>
        /// <param name="room">The room.</param>
        /// <param name="cell">The cell.</param>
        /// <param name="neighbor">The neighbor cell in the door direction. The neighbor can be null.</param>
        /// <param name="position">The local coordinate.</param>
        /// <param name="direction">The door direction.</param>
        private SKImage GetTile(Room room, Cell cell, Cell neighbor, Vector2DInt position, DoorDirection direction)
        {
            if (Door.ShowDoor(DoorDrawMode, direction) && cell.GetDoor(direction) != null && DoorExists(room, position, direction))
                return GetMapTile(MapTileType.GetDoorTileType(direction));

            if (neighbor == null)
                return GetMapTile(MapTileType.GetWallTileType(direction));

            return null;
        }

        /// <summary>
        /// Returns the map tile for the specified tile name if it exists in the tiles dictionary.
        /// Returns null if the name does not exist.
        /// </summary>
        /// <param name="name">The tile name.</param>
        private SKImage GetMapTile(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            if (Tiles.TryGetValue(name, out var tile))
                return tile;
            return null;
        }
    }
}
