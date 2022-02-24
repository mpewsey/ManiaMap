namespace MPewsey.ManiaMap.Samples
{
    public static class TemplateLibrary
    {
        public static class Miscellaneous
        {
            public static RoomTemplate SquareTemplate()
            {
                var o = Cell.New;
                var l = Cell.New.SetDoors("W", Door.TwoWay);
                var t = Cell.New.SetDoors("N", Door.TwoWay);
                var r = Cell.New.SetDoors("E", Door.TwoWay);
                var b = Cell.New.SetDoors("S", Door.TwoWay);

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
                var o = Cell.New;
                var l = Cell.New.SetDoors("W", Door.TwoWay);
                var t = Cell.New.SetDoors("N", Door.TwoWay);
                var r = Cell.New.SetDoors("E", Door.TwoWay);
                var b = Cell.New.SetDoors("S", Door.TwoWay);

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
                var a = Cell.New.SetDoors("TB", Door.TwoWay);
                var l = Cell.New.SetDoors("W", Door.TwoWay);
                var t = Cell.New.SetDoors("N", Door.TwoWay);
                var r = Cell.New.SetDoors("E", Door.TwoWay);
                var b = Cell.New.SetDoors("S", Door.TwoWay);

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
                var o = Cell.New.SetDoors("TB", Door.TwoWay);
                var a = Cell.New.SetDoors("WNTB", Door.TwoWay);
                var b = Cell.New.SetDoors("NTB", Door.TwoWay);
                var c = Cell.New.SetDoors("NETB", Door.TwoWay);
                var d = Cell.New.SetDoors("WTB", Door.TwoWay);
                var e = Cell.New.SetDoors("ETB", Door.TwoWay);
                var f = Cell.New.SetDoors("WSTB", Door.TwoWay);
                var g = Cell.New.SetDoors("STB", Door.TwoWay);
                var h = Cell.New.SetDoors("SETB", Door.TwoWay);

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
                var o = Cell.New;
                var a = Cell.New.SetDoors("WNETB", Door.TwoWay);
                var b = Cell.New.SetDoors("SENTB", Door.TwoWay);
                var c = Cell.New.SetDoors("SWTB", Door.TwoWay);

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
