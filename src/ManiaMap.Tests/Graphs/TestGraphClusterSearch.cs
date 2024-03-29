﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Random;
using MPewsey.ManiaMap.Generators;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Graphs.Tests
{
    [TestClass]
    public class TestGraphClusterSearch
    {
        [TestMethod]
        public void TestFindClusterOfGeekGraph()
        {
            var graph = Samples.GraphLibrary.GeekGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator();
            var random = new RandomSeed(123456);
            var layout = generator.Generate(1, graph, templateGroups, random);

            var result = layout.FindCluster(new Uid(5), 2).ToList();
            var expected = new List<int>() { 5, 3, 2, 4, 6, 9, 10 };
            var values = expected.Select(x => new Uid(x)).ToList();
            CollectionAssert.AreEquivalent(values, result);
        }

        [TestMethod]
        public void TestFindClustersOfGeekGraph()
        {
            var graph = Samples.GraphLibrary.GeekGraph();

            var templateGroups = new TemplateGroups();
            templateGroups.Add("Default", Samples.TemplateLibrary.Miscellaneous.HyperSquareTemplate());

            var generator = new LayoutGenerator();
            var random = new RandomSeed(123456);
            var layout = generator.Generate(1, graph, templateGroups, random);

            var results = layout.FindClusters(2);
            var result = results[new Uid(5)].ToList();
            var expected = new List<int>() { 5, 3, 2, 4, 6, 9, 10 };
            var values = expected.Select(x => new Uid(x)).ToList();
            CollectionAssert.AreEquivalent(values, result);
        }
    }
}