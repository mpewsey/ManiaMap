namespace MPewsey.ManiaMap.Samples
{
    public static class TemplateLibrary
    {
        public static RoomTemplate SquareTemplate()
        {
            var o = new Cell();
            var l = new Cell { WestDoor = new Door(DoorType.TwoWay) };
            var t = new Cell { NorthDoor = new Door(DoorType.TwoWay) };
            var r = new Cell { EastDoor = new Door(DoorType.TwoWay) };
            var b = new Cell { SouthDoor = new Door(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { o, t, o },
                { l, o, r },
                { o, b, o },
            };

            return new RoomTemplate(1, "Square", cells);
        }

        public static RoomTemplate RingTemplate()
        {
            Cell x = null;
            var o = new Cell();
            var l = new Cell { WestDoor = new Door(DoorType.TwoWay) };
            var t = new Cell { NorthDoor = new Door(DoorType.TwoWay) };
            var r = new Cell { EastDoor = new Door(DoorType.TwoWay) };
            var b = new Cell { SouthDoor = new Door(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { o, t, o },
                { l, x, r },
                { o, b, o },
            };

            return new RoomTemplate(2, "Ring", cells);
        }

        public static RoomTemplate PlusTemplate()
        {
            Cell x = null;
            var a = new Cell { TopDoor = new Door(DoorType.TwoWay), BottomDoor = new Door(DoorType.TwoWay) };
            var l = new Cell { WestDoor = new Door(DoorType.TwoWay) };
            var t = new Cell { NorthDoor = new Door(DoorType.TwoWay) };
            var r = new Cell { EastDoor = new Door(DoorType.TwoWay) };
            var b = new Cell { SouthDoor = new Door(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { x, t, x },
                { l, a, r },
                { x, b, x },
            };

            return new RoomTemplate(3, "Plus", cells);
        }

        public static RoomTemplate HyperSquareTemplate()
        {
            var o = new Cell();
            var a = new Cell { WestDoor = new Door(DoorType.TwoWay), NorthDoor = new Door(DoorType.TwoWay) };
            var b = new Cell { NorthDoor = new Door(DoorType.TwoWay) };
            var c = new Cell { NorthDoor = new Door(DoorType.TwoWay), EastDoor = new Door(DoorType.TwoWay) };
            var d = new Cell { WestDoor = new Door(DoorType.TwoWay) };
            var e = new Cell { EastDoor = new Door(DoorType.TwoWay) };
            var f = new Cell { WestDoor = new Door(DoorType.TwoWay), SouthDoor = new Door(DoorType.TwoWay) };
            var g = new Cell { SouthDoor = new Door(DoorType.TwoWay) };
            var h = new Cell { SouthDoor = new Door(DoorType.TwoWay), EastDoor = new Door(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { a, b, c },
                { d, o, e },
                { f, g, h },
            };

            foreach (var cell in cells)
            {
                cell.TopDoor = new Door(DoorType.TwoWay);
                cell.BottomDoor = new Door(DoorType.TwoWay);
            }

            return new RoomTemplate(4, "HyperSquare", cells);
        }

        public static RoomTemplate LTemplate()
        {
            Cell x = null;
            var o = new Cell();
            var a = new Cell { WestDoor = new Door(DoorType.TwoWay), NorthDoor = new Door(DoorType.TwoWay), EastDoor = new Door(DoorType.TwoWay) };
            var b = new Cell { SouthDoor = new Door(DoorType.TwoWay), EastDoor = new Door(DoorType.TwoWay), NorthDoor = new Door(DoorType.TwoWay) };
            var c = new Cell { SouthDoor = new Door(DoorType.TwoWay), WestDoor = new Door(DoorType.TwoWay) };

            var cells = new Cell[,]
            {
                { a, x, x },
                { o, x, x },
                { o, x, x },
                { c, o, b },
            };

            foreach (var cell in cells)
            {
                if (cell != null && cell != o)
                {
                    cell.TopDoor = new Door(DoorType.TwoWay);
                    cell.BottomDoor = new Door(DoorType.TwoWay);
                }
            }

            return new RoomTemplate(5, "LTemplate", cells);
        }
    }
}
