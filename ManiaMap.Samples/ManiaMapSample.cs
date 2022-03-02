namespace MPewsey.ManiaMap.Samples
{
    public static class ManiaMapSample
    {
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

        public static Layout ManiaMapLayout()
        {
            var graph = ManiaMapLayoutGraph();
            var templateGroups = LetterTemplateGroups();
            var generator = new LayoutGenerator(12345, graph, templateGroups);
            return generator.GenerateLayout(1);
        }
    }
}
