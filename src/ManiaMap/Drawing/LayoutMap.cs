using MPewsey.Common.Mathematics;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
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
        public Dictionary<string, Image> Tiles { get; set; }

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
            DoorDrawMode doorDrawMode = DoorDrawMode.AllDoors, Vector2DInt? tileSize = null, Dictionary<string, Image> tiles = null)
        {
            DoorDrawMode = doorDrawMode;
            TileSize = tileSize ?? TileSize;
            Padding = padding ?? Padding;
            BackgroundColor = backgroundColor ?? BackgroundColor;
            RoomColor = roomColor ?? RoomColor;
            Tiles = tiles ?? MapTiles.GetDefaultTiles();
        }

        public override string ToString()
        {
            return $"LayoutMap(TileSize = {TileSize}, Padding = {Padding})";
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
            var ext = Path.GetExtension(path);
            var name = Path.ChangeExtension(path, null);
            var maps = CreateImages(layout, state);

            foreach (var pair in maps)
            {
                pair.Value.Save($"{name}_Z={pair.Key}{ext}");
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
        public Dictionary<int, Image> CreateImages(Layout layout, LayoutState state = null)
        {
            Initialize(layout, state);

            var width = TileSize.X * (Padding.Left + Padding.Right + LayoutBounds.Width);
            var height = TileSize.Y * (Padding.Top + Padding.Bottom + LayoutBounds.Height);
            var dict = new Dictionary<int, Image>();

            foreach (var room in Layout.Rooms.Values)
            {
                if (!dict.ContainsKey(room.Position.Z))
                {
                    var map = new Image<Rgba32>(width, height);
                    map.Mutate(x => DrawMap(x, room.Position.Z));
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
        private void DrawMap(IImageProcessingContext image, int z)
        {
            image.BackgroundColor(ConvertColor(BackgroundColor));
            DrawGrid(image);
            DrawMapTiles(image, z);
        }

        /// <summary>
        /// Draws the grid tiles onto the image context.
        /// </summary>
        /// <param name="image">The image context.</param>
        private void DrawGrid(IImageProcessingContext image)
        {
            if (Tiles.TryGetValue(MapTileType.Grid, out Image gridTile))
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
            var roomTile = new Image<Rgba32>(TileSize.X, TileSize.Y);
            roomTile.Mutate(x => x.BackgroundColor(ConvertColor(RoomColor)));

            foreach (var room in Layout.Rooms.Values)
            {
                // If room Z (layer) value is not equal, go to next room.
                if (room.Position.Z != z)
                    continue;

                var roomState = LayoutState?.RoomStates[room.Id];
                var cells = room.Template.Cells;
                var x0 = (room.Position.Y - LayoutBounds.X + Padding.Left) * TileSize.X;
                var y0 = (room.Position.X - LayoutBounds.Y + Padding.Top) * TileSize.Y;
                var cellTile = new Image<Rgba32>(TileSize.X, TileSize.Y);
                cellTile.Mutate(x => x.BackgroundColor(ConvertColor(room.Color)));

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
                        var point = new Point(x, y);

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
                        image.DrawImage(tile, point, 1);

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
        private void DrawFeatureTiles(IImageProcessingContext image, Cell cell, Point point)
        {
            foreach (var tileName in cell.Features)
            {
                if (Tiles.TryGetValue(tileName, out Image tile))
                {
                    image.DrawImage(tile, point, 1);
                }
            }
        }

        /// <summary>
        /// Converts a color to an ImageSharp color.
        /// </summary>
        /// <param name="color">The color.</param>
        private static Color ConvertColor(Color4 color)
        {
            return Color.FromRgba(color.R, color.G, color.B, color.A);
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
        private Image GetTile(Room room, Cell cell, Cell neighbor, Vector2DInt position, DoorDirection direction)
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
        private Image GetMapTile(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                return null;
            if (Tiles.TryGetValue(name, out Image tile))
                return tile;
            return null;
        }
    }
}
