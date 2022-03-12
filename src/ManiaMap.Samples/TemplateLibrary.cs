namespace MPewsey.ManiaMap.Samples
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

        /// <summary>
        /// Contains a collection of square `RoomTemplate`.
        /// </summary>
        public static class Squares
        {
            /// <summary>
            /// Returns a 1x1 square template with doors in all planar directions.
            /// </summary>
            public static RoomTemplate Square1x1Template()
            {
                var a = Cell.New.SetDoors("NWSE", Door.TwoWay);

                var cells = new Cell[,]
                {
                    { a },
                };

                return new RoomTemplate(100, "Square1x1", cells);
            }

            /// <summary>
            /// Returns a 2x2 square template with doors in all planar directions.
            /// </summary>
            public static RoomTemplate Square2x2Template()
            {
                var a = Cell.New.SetDoors("NW", Door.TwoWay);
                var b = Cell.New.SetDoors("NE", Door.TwoWay);
                var c = Cell.New.SetDoors("SW", Door.TwoWay);
                var d = Cell.New.SetDoors("SE", Door.TwoWay);

                var cells = new Cell[,]
                {
                    { a, b },
                    { c, d },
                };

                return new RoomTemplate(101, "Square2x2", cells);
            }

            /// <summary>
            /// Returns a 3x3 square template with doors in all planar directions.
            /// </summary>
            public static RoomTemplate Square3x3Template()
            {
                var o = Cell.New;
                var a = Cell.New.SetDoors("NW", Door.TwoWay);
                var b = Cell.New.SetDoors("N", Door.TwoWay);
                var c = Cell.New.SetDoors("NE", Door.TwoWay);
                var d = Cell.New.SetDoors("W", Door.TwoWay);
                var e = Cell.New.SetDoors("E", Door.TwoWay);
                var f = Cell.New.SetDoors("SW", Door.TwoWay);
                var g = Cell.New.SetDoors("S", Door.TwoWay);
                var h = Cell.New.SetDoors("SE", Door.TwoWay);

                var cells = new Cell[,]
                {
                    { a, b, c },
                    { d, o, e },
                    { f, g, h },
                };

                return new RoomTemplate(102, "Square3x3", cells);
            }
        }

        /// <summary>
        /// Contains a collection of rectangular `RoomTemplate`.
        /// </summary>
        public static class Rectangles
        {
            /// <summary>
            /// Returns a 1x2 rectangular template with doors in all planar directions.
            /// </summary>
            public static RoomTemplate Rectangle1x2Template()
            {
                var a = Cell.New.SetDoors("NWS", Door.TwoWay);
                var b = Cell.New.SetDoors("NES", Door.TwoWay);

                var cells = new Cell[,]
                {
                    { a, b },
                };

                return new RoomTemplate(200, "Rectangle1x2", cells);
            }

            /// <summary>
            /// Returns a 1x3 rectangular template with doors in all planar directions.
            /// </summary>
            public static RoomTemplate Rectangle1x3Template()
            {
                var o = Cell.New;
                var a = Cell.New.SetDoors("NWS", Door.TwoWay);
                var b = Cell.New.SetDoors("NES", Door.TwoWay);

                var cells = new Cell[,]
                {
                    { a, o, b },
                };

                return new RoomTemplate(201, "Rectangle1x3", cells);
            }

            /// <summary>
            /// Returns a 1x4 rectangular template with doors in all planar directions.
            /// </summary>
            public static RoomTemplate Rectangle1x4Template()
            {
                var o = Cell.New;
                var a = Cell.New.SetDoors("NWS", Door.TwoWay);
                var b = Cell.New.SetDoors("NES", Door.TwoWay);

                var cells = new Cell[,]
                {
                    { a, o, o, b },
                };

                return new RoomTemplate(202, "Rectangle1x4", cells);
            }

            /// <summary>
            /// Returns a 2x3 rectangular template with doors in all planar directions.
            /// </summary>
            public static RoomTemplate Rectangle2x3Template()
            {
                var a = Cell.New.SetDoors("NW", Door.TwoWay);
                var b = Cell.New.SetDoors("N", Door.TwoWay);
                var c = Cell.New.SetDoors("NE", Door.TwoWay);
                var d = Cell.New.SetDoors("SW", Door.TwoWay);
                var e = Cell.New.SetDoors("S", Door.TwoWay);
                var f = Cell.New.SetDoors("SE", Door.TwoWay);

                var cells = new Cell[,]
                {
                    { a, b, c },
                    { d, e, f },
                };

                return new RoomTemplate(203, "Rectangle2x3", cells);
            }

            /// <summary>
            /// Returns a 2x4 rectangular template with doors in all planar directions.
            /// </summary>
            public static RoomTemplate Rectangle2x4Template()
            {
                var a = Cell.New.SetDoors("NW", Door.TwoWay);
                var b = Cell.New.SetDoors("N", Door.TwoWay);
                var c = Cell.New.SetDoors("NE", Door.TwoWay);
                var d = Cell.New.SetDoors("SW", Door.TwoWay);
                var e = Cell.New.SetDoors("S", Door.TwoWay);
                var f = Cell.New.SetDoors("SE", Door.TwoWay);

                var cells = new Cell[,]
                {
                    { a, b, b, c },
                    { d, e, e, f },
                };

                return new RoomTemplate(204, "Rectangle2x4", cells);
            }
        }

        /// <summary>
        /// Contains a collection of angle `RoomTemplate`.
        /// </summary>
        public static class Angles
        {
            /// <summary>
            /// Returns a 3x4 angle template with doors at the tips and corner in all planar
            /// directions.
            /// </summary>
            public static RoomTemplate Angle3x4()
            {
                var x = Cell.Empty;
                var o = Cell.New;
                var a = Cell.New.SetDoors("NW", Door.TwoWay);
                var b = Cell.New.SetDoors("NES", Door.TwoWay);
                var c = Cell.New.SetDoors("WSE", Door.TwoWay);

                var cells = new Cell[,]
                {
                    { a, o, o, b },
                    { o, x, x, x },
                    { c, x, x, x },
                };

                return new RoomTemplate(300, "Angle3x4", cells);
            }
        }
    }
}
