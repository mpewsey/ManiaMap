using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;

namespace MPewsey.ManiaMap
{
    public class LayoutMap
    {
        private Layout Layout { get; }
        private int TileWidth { get; }
        private int TileHeight { get; }
        private int Padding { get; }
        private Color BackgroundColor { get; } = Color.Black;
        private Color CellBackgroundColor { get; } = Color.MidnightBlue;
        private Dictionary<string, Bitmap> Tiles { get; }
        private HashSet<RoomDoorPair> RoomDoors { get; } = new HashSet<RoomDoorPair>();
        
        public LayoutMap(Layout layout, int tileWidth = 16, int tileHeight = 16,
            int padding = 1, Dictionary<string, Bitmap> tiles = null)
        {
            Layout = layout;
            TileWidth = tileWidth;
            TileHeight = tileHeight;
            Padding = padding;
            Tiles = tiles ?? MapTiles.GetDefaultTiles();
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
                minY = Math.Min(maxY, room.Y);
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
        public void SavePng(string path)
        {
            var map = CreateImage();
            map.Save(path, ImageFormat.Png);
        }

        /// <summary>
        /// Returns a rendered map of the layout.
        /// </summary>
        public Bitmap CreateImage()
        {
            MarkRoomDoors();
            var bounds = LayoutBounds();
            var width = TileWidth * (2 * Padding + bounds.Width);
            var height = TileHeight * (2 * Padding + bounds.Height);
            var map = new Bitmap(width, height);
            var graphic = Graphics.FromImage(map);

            // Add background fill
            var backgroundBrush = new SolidBrush(BackgroundColor);
            graphic.FillRectangle(backgroundBrush, 0, 0, width, height);

            // Draw grid if tile exists
            if (Tiles.TryGetValue("Grid", out Bitmap gridTile))
            {
                for (int x = 0; x < width; x += TileWidth)
                {
                    for (int y = 0; y < height; y += TileHeight)
                    {
                        graphic.DrawImage(gridTile, x, y);
                    }
                }
            }

            // Draw map tiles
            var cellBrush = new SolidBrush(CellBackgroundColor);

            foreach (var room in Layout.Rooms.Values)
            {
                var cells = room.Template.Cells;
                var x0 = (room.Y - bounds.X + Padding) * TileWidth;
                var y0 = (room.X - bounds.Y + Padding) * TileHeight;
                
                for (int i = 0; i < cells.Rows; i++)
                {
                    for (int j = 0; j < cells.Columns; j++)
                    {
                        var cell = cells[i, j];

                        if (cell != null)
                        {
                            var x = TileWidth * j + x0;
                            var y = TileHeight * i + y0;
                            
                            var top = cells.GetOrDefault(i - 1, j);
                            var bottom = cells.GetOrDefault(i + 1, j);
                            var left = cells.GetOrDefault(i, j - 1);
                            var right = cells.GetOrDefault(i, j + 1);

                            var topTile = GetTile(room, cell.TopDoor, top, "TopDoor", "TopWall");
                            var bottomTile = GetTile(room, cell.BottomDoor, bottom, "BottomDoor", "BottomWall");
                            var leftTile = GetTile(room, cell.LeftDoor, left, "LeftDoor", "LeftWall");
                            var rightTile = GetTile(room, cell.RightDoor, right, "RightDoor", "RightWall");

                            // Add cell background fill
                            graphic.FillRectangle(cellBrush, x, y, TileWidth, TileHeight);

                            // Superimpose applicable map tiles
                            if (topTile != null)
                                graphic.DrawImage(topTile, x, y);
                            if (bottomTile != null)
                                graphic.DrawImage(bottomTile, x, y);
                            if (leftTile != null)
                                graphic.DrawImage(leftTile, x, y);
                            if (rightTile != null)
                                graphic.DrawImage(rightTile, x, y);
                        }
                    }
                }
            }

            return map;
        }

        /// <summary>
        /// Returns the map tile corresponding to the wall or door location.
        /// Returns null if the tile has neither a wall or door.
        /// </summary>
        private Bitmap GetTile(Room room, Door door, Cell neighbor, string doorName, string wallName)
        {
            if (door != null && RoomDoors.Contains(new RoomDoorPair(room, door)))
                return Tiles[doorName];
            if (neighbor == null)
                return Tiles[wallName];
            return null;
        }
    }
}
