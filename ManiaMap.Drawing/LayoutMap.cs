using SixLabors.ImageSharp;
using SixLabors.ImageSharp.PixelFormats;
using SixLabors.ImageSharp.Processing;
using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Drawing
{
    public class LayoutMap
    {
        public Layout Layout { get; set; }
        public Point TileSize { get; set; }
        public Padding Padding { get; set; }
        public Color BackgroundColor { get; set; }
        public Dictionary<string, Image> Tiles { get; set; }
        private HashSet<RoomDoorPair> RoomDoors { get; } = new HashSet<RoomDoorPair>();

        public LayoutMap(Layout layout, Point? tileSize = null, Padding? padding = null,
            Dictionary<string, Image> tiles = null, Color? backgroundColor = null)
        {
            Layout = layout;
            TileSize = tileSize ?? new Point(16, 16);
            Padding = padding ?? new Padding(1);
            BackgroundColor = backgroundColor ?? Color.Black;
            Tiles = tiles ?? MapTiles.GetDefaultTiles();
        }

        public override string ToString()
        {
            return $"LayoutMap(Layout = {Layout})";
        }

        /// <summary>
        /// Returns the rectangular bounds for the layout.
        /// </summary>
        private Rectangle LayoutBounds()
        {
            if (Layout.Rooms.Count == 0)
                return new Rectangle();

            var minX = int.MaxValue;
            var minY = int.MaxValue;
            var maxX = int.MinValue;
            var maxY = int.MinValue;

            foreach (var room in Layout.Rooms.Values)
            {
                minX = Math.Min(minX, room.X);
                minY = Math.Min(minY, room.Y);
                maxX = Math.Max(maxX, room.X + room.Template.Cells.Rows);
                maxY = Math.Max(maxY, room.Y + room.Template.Cells.Columns);
            }

            return new Rectangle(minY, minX, maxY - minY, maxX - minX);
        }

        /// <summary>
        /// Adds the room door pairs for the layout to the room door set.
        /// </summary>
        private void MarkRoomDoors()
        {
            RoomDoors.Clear();

            foreach (var connection in Layout.DoorConnections)
            {
                RoomDoors.Add(new RoomDoorPair(connection.FromRoom, connection.FromDoor));
                RoomDoors.Add(new RoomDoorPair(connection.ToRoom, connection.ToDoor));
            }
        }

        /// <summary>
        /// Renders a map of the layout and saves it to the designated file path.
        /// </summary>
        public void SaveImage(string path, int z = 0)
        {
            var map = CreateImage(z);
            map.Save(path);
        }

        /// <summary>
        /// Returns a rendered map of the layout.
        /// </summary>
        public Image CreateImage(int z = 0)
        {
            MarkRoomDoors();
            var bounds = LayoutBounds();
            var width = TileSize.X * (Padding.Left + Padding.Right + bounds.Width);
            var height = TileSize.Y * (Padding.Top + Padding.Bottom + bounds.Height);
            var map = new Image<Rgba32>(width, height);

            map.Mutate(image =>
            {
                // Add background fill
                image.BackgroundColor(BackgroundColor);

                // Draw grid if tile exists
                if (Tiles.TryGetValue("Grid", out Image gridTile))
                {
                    for (int x = 0; x < width; x += TileSize.X)
                    {
                        for (int y = 0; y < height; y += TileSize.Y)
                        {
                            image.DrawImage(gridTile, new Point(x, y), 1);
                        }
                    }
                }

                // Draw map tiles
                var cellTile = new Image<Rgba32>(TileSize.X, TileSize.Y);

                foreach (var room in Layout.Rooms.Values)
                {
                    if (room.Z != z)
                        continue;

                    var cells = room.Template.Cells;
                    var x0 = (room.Y - bounds.X + Padding.Left) * TileSize.X;
                    var y0 = (room.X - bounds.Y + Padding.Top) * TileSize.Y;
                    cellTile.Mutate(x => x.BackgroundColor(ConvertColor(room.Color)));

                    for (int i = 0; i < cells.Rows; i++)
                    {
                        for (int j = 0; j < cells.Columns; j++)
                        {
                            var cell = cells[i, j];

                            if (cell != null)
                            {
                                var x = TileSize.X * j + x0;
                                var y = TileSize.Y * i + y0;
                                var point = new Point(x, y);

                                var north = cells.GetOrDefault(i - 1, j);
                                var south = cells.GetOrDefault(i + 1, j);
                                var west = cells.GetOrDefault(i, j - 1);
                                var east = cells.GetOrDefault(i, j + 1);

                                var topTile = GetTile(room, cell.TopDoor, "TopDoor");
                                var bottomTile = GetTile(room, cell.BottomDoor, "BottomDoor");
                                var northTile = GetTile(room, cell.NorthDoor, north, "NorthDoor", "NorthWall");
                                var southTile = GetTile(room, cell.SouthDoor, south, "SouthDoor", "SouthWall");
                                var westTile = GetTile(room, cell.WestDoor, west, "WestDoor", "WestWall");
                                var eastTile = GetTile(room, cell.EastDoor, east, "EastDoor", "EastWall");

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
            });

            return map;
        }

        /// <summary>
        /// Converts a System.Drawing color to an ImageSharp color.
        /// </summary>
        private static Color ConvertColor(System.Drawing.Color color)
        {
            return Color.FromRgba(color.R, color.G, color.B, color.A);
        }

        /// <summary>
        /// Returns the map tile corresponding to the door location.
        /// Returns null if the door does not exist.
        /// </summary>
        private Image GetTile(Room room, Door door, string doorName)
        {
            if (door != null && RoomDoors.Contains(new RoomDoorPair(room, door)))
                return Tiles[doorName];
            return null;
        }

        /// <summary>
        /// Returns the map tile corresponding to the wall or door location.
        /// Returns null if the tile has neither a wall or door.
        /// </summary>
        private Image GetTile(Room room, Door door, Cell neighbor, string doorName, string wallName)
        {
            if (door != null && RoomDoors.Contains(new RoomDoorPair(room, door)))
                return Tiles[doorName];
            if (neighbor == null)
                return Tiles[wallName];
            return null;
        }
    }
}
