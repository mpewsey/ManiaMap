namespace MPewsey.ManiaMap.Samples
{
    /// <summary>
    /// Contains methods for the Mania Map sample.
    /// </summary>
    public static class ManiaMapSample
    {
        /// <summary>
        /// Returns a template for the first letter M in Mania Map.
        /// </summary>
        public static RoomTemplate LetterM1Template()
        {
            var x = Cell.Empty;
            var o = Cell.New;
            var a = Cell.New.SetDoors("E", Door.TwoWay);

            var cells = new Cell[,]
            {
                { o, x, x, x, o },
                { o, o, o, o, o },
                { o, x, o, x, o },
                { o, x, o, x, o },
                { o, x, x, x, a },
            };

            return new RoomTemplate(1, "M", cells);
        }

        /// <summary>
        /// Returns a template for the second letter M in Mania Map.
        /// </summary>
        public static RoomTemplate LetterM2Template()
        {
            var x = Cell.Empty;
            var o = Cell.New;
            var a = Cell.New.SetDoors("E", Door.TwoWay);
            var b = Cell.New.SetDoors("W", Door.TwoWay);

            var cells = new Cell[,]
            {
                { o, x, x, x, o },
                { o, o, o, o, o },
                { o, x, o, x, o },
                { o, x, o, x, o },
                { b, x, x, x, a },
            };

            return new RoomTemplate(1, "M", cells);
        }

        /// <summary>
        /// Returns a template for the letter A in Mania Map.
        /// </summary>
        public static RoomTemplate LetterATemplate()
        {
            var x = Cell.Empty;
            var o = Cell.New;
            var a = Cell.New.SetDoors("E", Door.TwoWay);
            var b = Cell.New.SetDoors("W", Door.TwoWay);

            var cells = new Cell[,]
            {
                { o, o, o },
                { x, x, o },
                { o, o, o },
                { o, x, o },
                { b, o, a },
            };

            return new RoomTemplate(2, "A", cells);
        }

        /// <summary>
        /// Returns a template for the leter N in Mania Map.
        /// </summary>
        public static RoomTemplate LetterNTemplate()
        {
            var x = Cell.Empty;
            var o = Cell.New;
            var a = Cell.New.SetDoors("E", Door.TwoWay);
            var b = Cell.New.SetDoors("W", Door.TwoWay);

            var cells = new Cell[,]
            {
                { x, x, x },
                { o, o, o },
                { o, x, o },
                { o, x, o },
                { b, x, a },
            };

            return new RoomTemplate(3, "N", cells);
        }

        /// <summary>
        /// Returns a template for the letter I in Mania Map.
        /// </summary>
        public static RoomTemplate LetterITemplate()
        {
            var x = Cell.Empty;
            var o = Cell.New;
            var a = Cell.New.SetDoors("E", Door.TwoWay);
            var b = Cell.New.SetDoors("W", Door.TwoWay);

            var cells = new Cell[,]
            {
                { o, o, o },
                { x, o, x },
                { x, o, x },
                { x, o, x },
                { b, o, a },
            };

            return new RoomTemplate(4, "I", cells);
        }

        /// <summary>
        /// Returns a template for the letter P in Mania Map.
        /// </summary>
        public static RoomTemplate LetterPTemplate()
        {
            var x = Cell.Empty;
            var o = Cell.New;
            var a = Cell.New.SetDoors("W", Door.TwoWay);

            var cells = new Cell[,]
            {
                { o, o, o },
                { o, x, o },
                { o, o, o },
                { o, x, x },
                { a, x, x },
            };

            return new RoomTemplate(5, "P", cells);
        }

        /// <summary>
        /// Returns the template groups for the letters in Mania Map.
        /// </summary>
        public static TemplateGroups LetterTemplateGroups()
        {
            var templateGroups = new TemplateGroups();
            templateGroups.Add("M1", LetterM1Template());
            templateGroups.Add("M2", LetterM2Template());
            templateGroups.Add("A", LetterATemplate());
            templateGroups.Add("N", LetterNTemplate());
            templateGroups.Add("I", LetterITemplate());
            templateGroups.Add("P", LetterPTemplate());
            return templateGroups;
        }

        /// <summary>
        /// Returns a layout graph for the Mania Map layout.
        /// </summary>
        public static LayoutGraph ManiaMapLayoutGraph()
        {
            var graph = new LayoutGraph(1, "ManiaMap");

            graph.AddNode(0).TemplateGroups.Add("M1");
            graph.AddNode(1).TemplateGroups.Add("A");
            graph.AddNode(2).TemplateGroups.Add("N");
            graph.AddNode(3).TemplateGroups.Add("I");
            graph.AddNode(4).TemplateGroups.Add("A");
            graph.AddNode(5).TemplateGroups.Add("M2");
            graph.AddNode(6).TemplateGroups.Add("A");
            graph.AddNode(7).TemplateGroups.Add("P");

            graph.AddEdge(0, 1);
            graph.AddEdge(1, 2);
            graph.AddEdge(2, 3);
            graph.AddEdge(3, 4);
            graph.AddEdge(4, 5);
            graph.AddEdge(5, 6);
            graph.AddEdge(6, 7);

            return graph;
        }

        /// <summary>
        /// Generates the Mania Map layout and returns it.
        /// </summary>
        public static Layout ManiaMapLayout()
        {
            var graph = ManiaMapLayoutGraph();
            var templateGroups = LetterTemplateGroups();
            var generator = new LayoutGenerator(12345, graph, templateGroups);
            return generator.GenerateLayout(1);
        }
    }
}
