using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public class RoomTemplate
    {
        public int Id { get; }
        public Array2D<Cell> Cells { get; }

        public RoomTemplate(int id, Array2D<Cell> cells)
        {
            Id = id;
            Cells = cells;
            NumberDoors();
        }

        public override string ToString()
        {
            return $"RoomTemplate(Id = {Id})";
        }

        private void NumberDoors()
        {
            for (int i = 0; i < Cells.Array.Length; i++)
            {
                Cells.Array[i]?.NumberDoors(i);
            }
        }

        public bool Intersects(RoomTemplate other, int dx, int dy)
        {
            var iStart = Math.Max(0, dx);
            var iStop = Math.Min(Cells.Rows, other.Cells.Rows + dx);
            var jStart = Math.Max(0, dy);
            var jStop = Math.Min(Cells.Columns, other.Cells.Columns + dy);

            if (jStart < jStop)
            {
                for (int i = iStart; i < iStop; i++)
                {
                    for (int j = jStart; j < jStop; j++)
                    {
                        if (Cells[i, j] != null && other.Cells[i - dx, j - dy] != null)
                        {
                            return true;
                        }
                    }
                }
            }

            return false;
        }

        public List<DoorPair> AlignedDoors(RoomTemplate other, int dx, int dy)
        {
            var result = new List<DoorPair>();
            var iStart = Math.Max(0, dx - 1);
            var iStop = Math.Min(Cells.Rows, other.Cells.Rows + dx + 1);
            var jStart = Math.Max(0, dy - 1);
            var jStop = Math.Min(Cells.Columns, other.Cells.Columns + dy + 1);

            if (jStart < jStop)
            {
                for (int i = iStart; i < iStop; i++)
                {
                    for (int j = jStart; j < jStop; j++)
                    {
                        var cell = Cells[i, j];
                        
                        if (cell != null)
                        {
                            var x = i - dx;
                            var y = j - dy;
                            var top = other.Cells.GetOrDefault(x - 1, y);
                            var bottom = other.Cells.GetOrDefault(x + 1, y);
                            var left = other.Cells.GetOrDefault(x, y - 1);
                            var right = other.Cells.GetOrDefault(x, y + 1);

                            if (cell.LeftDoorAligns(left))
                                result.Add(new(cell.LeftDoor, left.RightDoor));
                            if (cell.TopDoorAligns(top))
                                result.Add(new(cell.TopDoor, top.BottomDoor));
                            if (cell.RightDoorAligns(right))
                                result.Add(new(cell.RightDoor, right.LeftDoor));
                            if (cell.BottomDoorAligns(bottom))
                                result.Add(new(cell.BottomDoor, bottom.TopDoor));
                        }
                    }
                }
            }

            return result;
        }
    }
}
