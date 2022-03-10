﻿namespace MPewsey.ManiaMap.Samples
{
    /// <summary>
    /// Contains collections of `RoomTemplate`.
    /// </summary>
    public static class TemplateLibrary
    {
        /// <summary>
        /// Contains a collection of miscellaneous `RoomTemplate`.
        /// </summary>
        public static class Miscellaneous
        {
            /// <summary>
            /// Returns a square template with doors at each wall's midpoint.
            /// </summary>
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

            /// <summary>
            /// Returns a ring template with a hole in the center and doors at each wall's midpoint.
            /// </summary>
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

            /// <summary>
            /// Returns a template in the shape of a "+" with doors at each point.
            /// </summary>
            /// <returns></returns>
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

            /// <summary>
            /// Returns a square template with doors in all directions.
            /// </summary>
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

            /// <summary>
            /// Returns an "L" template with doors at the ends and intersection of the "L".
            /// </summary>
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