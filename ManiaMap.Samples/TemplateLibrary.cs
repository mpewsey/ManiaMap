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

        public static class RightTriangles
        {
            public static RoomTemplate Small()
            {
                var x = Cell.Empty;
                var a = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var c = new Cell { WestDoor = Door.TwoWay, SouthDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { a, x },
                    { c, b },
                };

                return new RoomTemplate(100, "SmallRightTriangle", cells);
            }

            public static RoomTemplate Medium()
            {
                var x = Cell.Empty;
                var a = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var c = new Cell { WestDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var d = new Cell { WestDoor = Door.TwoWay };
                var e = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var f = new Cell { SouthDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { a, x, x },
                    { d, e, x },
                    { c, f, b },
                };

                return new RoomTemplate(101, "MediumRightTriangle", cells);
            }

            public static RoomTemplate Large()
            {
                var x = Cell.Empty;
                var o = new Cell();
                var a = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var c = new Cell { WestDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var d = new Cell { WestDoor = Door.TwoWay };
                var e = new Cell { WestDoor = Door.TwoWay };
                var f = new Cell { SouthDoor = Door.TwoWay };
                var g = new Cell { SouthDoor = Door.TwoWay };
                var h = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var i = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { a, x, x, x },
                    { d, h, x, x },
                    { e, o, i, x },
                    { c, f, g, b },
                };

                return new RoomTemplate(102, "LargeRightTriangle", cells);
            }
        }

        public static class Pyramids
        {
            public static RoomTemplate Small()
            {
                var x = Cell.Empty;
                var a = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var c = new Cell { EastDoor = Door.TwoWay, NorthDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var d = new Cell { SouthDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { x, a, x },
                    { b, d, c },
                };

                return new RoomTemplate(200, "SmallPyramid", cells);
            }

            public static RoomTemplate Medium()
            {
                var x = Cell.Empty;
                var o = new Cell();
                var a = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var c = new Cell { EastDoor = Door.TwoWay, NorthDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var d = new Cell { SouthDoor = Door.TwoWay };
                var e = new Cell { SouthDoor = Door.TwoWay };
                var f = new Cell { SouthDoor = Door.TwoWay };
                var g = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay };
                var h = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { x, x, a, x, x },
                    { x, g, o, h, x },
                    { b, e, d, f, c },
                };

                return new RoomTemplate(201, "MediumPyramid", cells);
            }

            public static RoomTemplate Large()
            {
                var x = Cell.Empty;
                var o = new Cell();
                var a = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var c = new Cell { EastDoor = Door.TwoWay, NorthDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var d = new Cell { SouthDoor = Door.TwoWay };
                var e = new Cell { SouthDoor = Door.TwoWay };
                var f = new Cell { SouthDoor = Door.TwoWay };
                var g = new Cell { SouthDoor = Door.TwoWay };
                var h = new Cell { SouthDoor = Door.TwoWay };
                var i = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay };
                var j = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay };
                var k = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var l = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { x, x, x, a, x, x, x },
                    { x, x, j, o, k, x, x },
                    { x, i, o, o, o, l, x },
                    { b, e, f, d, g, h, c },
                };

                return new RoomTemplate(202, "LargePyramid", cells);
            }
        }

        public static class SquareNotches
        {
            public static RoomTemplate Small()
            {
                var x = Cell.Empty;
                var o = new Cell();
                var a = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { SouthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { x, a },
                    { o, o },
                    { o, o },
                    { b, x },
                };

                return new RoomTemplate(300, "SmallSquareNotch", cells);
            }

            public static RoomTemplate Medium()
            {
                var x = Cell.Empty;
                var o = new Cell();
                var a = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { SouthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { x, a, x },
                    { o, o, o },
                    { o, o, o },
                    { o, o, o },
                    { x, b, x },
                };

                return new RoomTemplate(301, "MediumSquareNotch", cells);
            }

            public static RoomTemplate Large()
            {
                var x = Cell.Empty;
                var o = new Cell();
                var a = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { SouthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { x, x, a, x },
                    { o, o, o, o },
                    { o, o, o, o },
                    { o, o, o, o },
                    { o, o, o, o },
                    { x, b, x, x },
                };

                return new RoomTemplate(302, "LargeSquareNotch", cells);
            }
        }

        public static class Diamonds
        {
            public static RoomTemplate Small()
            {
                var x = Cell.Empty;
                var o = new Cell();
                var a = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var c = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var d = new Cell { SouthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { x, a, x },
                    { b, o, c },
                    { x, d, x },
                };

                return new RoomTemplate(400, "SmallDiamond", cells);
            }

            public static RoomTemplate Medium()
            {
                var x = Cell.Empty;
                var o = new Cell();
                var a = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var c = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var d = new Cell { SouthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { x, x, a, x, x },
                    { x, o, o, o, x },
                    { b, o, o, o, c },
                    { x, o, o, o, x },
                    { x, x, d, x, x },
                };

                return new RoomTemplate(401, "MediumDiamond", cells);
            }

            public static RoomTemplate Large()
            {
                var x = Cell.Empty;
                var o = new Cell();
                var a = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var c = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var d = new Cell { SouthDoor = Door.TwoWay, WestDoor = Door.TwoWay, EastDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { x, x, x, a, x, x, x },
                    { x, x, o, o, o, x, x },
                    { x, o, o, o, o, o, x },
                    { b, o, o, o, o, o, c },
                    { x, o, o, o, o, o, x },
                    { x, x, o, o, o, x, x },
                    { x, x, x, d, x, x, x },
                };

                return new RoomTemplate(402, "LargeDiamond", cells);
            }
        }

        public static class Angles
        {
            public static RoomTemplate Small()
            {
                var x = Cell.Empty;
                var o = new Cell();
                var a = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { SouthDoor = Door.TwoWay, EastDoor = Door.TwoWay, NorthDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { a, x },
                    { o, x },
                    { o, b },
                };

                return new RoomTemplate(500, "SmallAngle", cells);
            }

            public static RoomTemplate Medium()
            {
                var x = Cell.Empty;
                var o = new Cell();
                var a = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { SouthDoor = Door.TwoWay, EastDoor = Door.TwoWay, NorthDoor = Door.TwoWay };
                var c = new Cell { WestDoor = Door.TwoWay, SouthDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { a, x, x },
                    { o, x, x },
                    { o, x, x },
                    { c, o, b },
                };

                return new RoomTemplate(501, "MediumAngle", cells);
            }

            public static RoomTemplate Large()
            {
                var x = Cell.Empty;
                var o = new Cell();
                var a = new Cell { WestDoor = Door.TwoWay, NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var b = new Cell { SouthDoor = Door.TwoWay, EastDoor = Door.TwoWay, NorthDoor = Door.TwoWay };
                var c = new Cell { WestDoor = Door.TwoWay, SouthDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { a, x, x, x },
                    { o, x, x, x },
                    { o, x, x, x },
                    { o, x, x, x },
                    { c, o, o, b },
                };

                return new RoomTemplate(502, "MediumAngle", cells);
            }
        }

        public static class Squares
        {
            public static RoomTemplate Small()
            {
                var a = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay };
                var b = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var c = new Cell { SouthDoor = Door.TwoWay, WestDoor = Door.TwoWay };
                var d = new Cell { SouthDoor = Door.TwoWay, EastDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { a, b },
                    { c, d },
                };

                return new RoomTemplate(600, "SmallSquare", cells);
            }

            public static RoomTemplate Medium()
            {
                var o = new Cell();
                var a = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay };
                var b = new Cell { NorthDoor = Door.TwoWay };
                var c = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var d = new Cell { WestDoor = Door.TwoWay };
                var e = new Cell { EastDoor = Door.TwoWay };
                var f = new Cell { SouthDoor = Door.TwoWay, WestDoor = Door.TwoWay };
                var g = new Cell { SouthDoor = Door.TwoWay };
                var h = new Cell { SouthDoor = Door.TwoWay, EastDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { a, b, c },
                    { d, o, e },
                    { f, g, h },
                };

                return new RoomTemplate(601, "MediumSquare", cells);
            }

            public static RoomTemplate Large()
            {
                var o = new Cell();
                var a = new Cell { NorthDoor = Door.TwoWay, WestDoor = Door.TwoWay };
                var b = new Cell { NorthDoor = Door.TwoWay };
                var c = new Cell { NorthDoor = Door.TwoWay };
                var d = new Cell { NorthDoor = Door.TwoWay, EastDoor = Door.TwoWay };
                var e = new Cell { WestDoor = Door.TwoWay };
                var f = new Cell { EastDoor = Door.TwoWay };
                var g = new Cell { WestDoor = Door.TwoWay };
                var h = new Cell { EastDoor = Door.TwoWay };
                var i = new Cell { WestDoor = Door.TwoWay, SouthDoor = Door.TwoWay };
                var j = new Cell { SouthDoor = Door.TwoWay };
                var k = new Cell { SouthDoor = Door.TwoWay };
                var l = new Cell { SouthDoor = Door.TwoWay, EastDoor = Door.TwoWay };

                var cells = new Cell[,]
                {
                    { a, b, c, d },
                    { e, o, o, f },
                    { g, o, o, h },
                    { i, j, k, l },
                };

                return new RoomTemplate(602, "LargeSquare", cells);
            }
        }
    }
}
