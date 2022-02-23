namespace MPewsey.ManiaMap.Samples
{
    public static class TemplateLibrary
    {
        public static class Miscellaneous
        {
            public static RoomTemplate SquareTemplate()
            {
                var o = new Cell();
                var l = new Cell { WestDoor = Door.TwoWay };
                var t = new Cell { NorthDoor = Door.TwoWay };
                var r = new Cell { EastDoor = Door.TwoWay };
                var b = new Cell { SouthDoor = Door.TwoWay };

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
                var x = Cell.Empty;
                var o = new Cell();
                var l = new Cell { WestDoor = Door.TwoWay };
                var t = new Cell { NorthDoor = Door.TwoWay };
                var r = new Cell { EastDoor = Door.TwoWay };
                var b = new Cell { SouthDoor = Door.TwoWay };

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
                var x = Cell.Empty;
                var a = new Cell { TopDoor = Door.TwoWay, BottomDoor = Door.TwoWay };
                var l = new Cell { WestDoor = Door.TwoWay };
                var t = new Cell { NorthDoor = Door.TwoWay };
                var r = new Cell { EastDoor = Door.TwoWay };
                var b = new Cell { SouthDoor = Door.TwoWay };

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
                var o = new Cell { TopDoor = Door.TwoWay, BottomDoor = Door.TwoWay };
                var a = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, TopDoor = Door.TwoWay, BottomDoor = Door.TwoWay };
                var b = new Cell { NorthDoor = Door.TwoWay, TopDoor = Door.TwoWay, BottomDoor = Door.TwoWay };
                var c = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay, TopDoor = Door.TwoWay, BottomDoor = Door.TwoWay };
                var d = new Cell { WestDoor = Door.TwoWay, TopDoor = Door.TwoWay, BottomDoor = Door.TwoWay };
                var e = new Cell { EastDoor = Door.TwoWay, TopDoor = Door.TwoWay, BottomDoor = Door.TwoWay };
                var f = new Cell { WestDoor = Door.TwoWay, SouthDoor = Door.TwoWay, TopDoor = Door.TwoWay, BottomDoor = Door.TwoWay };
                var g = new Cell { SouthDoor = Door.TwoWay, TopDoor = Door.TwoWay, BottomDoor = Door.TwoWay };
                var h = new Cell { SouthDoor = Door.TwoWay, EastDoor = Door.TwoWay, TopDoor = Door.TwoWay, BottomDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { a, b, c },
                    { d, o, e },
                    { f, g, h },
                };

                return new RoomTemplate(4, "HyperSquare", cells);
            }

            public static RoomTemplate LTemplate()
            {
                var x = Cell.Empty;
                var o = new Cell();
                var a = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay, TopDoor = Door.TwoWay, BottomDoor = Door.TwoWay };
                var b = new Cell { SouthDoor = Door.TwoWay, EastDoor = Door.TwoWay, NorthDoor = Door.TwoWay, TopDoor = Door.TwoWay, BottomDoor = Door.TwoWay };
                var c = new Cell { SouthDoor = Door.TwoWay, WestDoor = Door.TwoWay, TopDoor = Door.TwoWay, BottomDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { a, x, x },
                    { o, x, x },
                    { o, x, x },
                    { c, o, b },
                };

                return new RoomTemplate(5, "LTemplate", cells);
            }
        }
    }
}
