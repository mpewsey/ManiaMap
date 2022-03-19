using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestWeightCollection
    {
        [TestMethod]
        public void TestAddWeight()
        {
            var weights = new WeightCollection();
            weights.AddWeight(10);
            Assert.AreEqual(1, weights.WeightCount);
            Assert.AreEqual(10, weights.TotalWeight);
            Assert.AreEqual(10, weights.GetWeight(0));
        }

        [TestMethod]
        public void TestToString()
        {
            var weights = new WeightCollection();
            weights.AddWeight(10);
            Assert.AreEqual("WeightCollection(Weights.Count = 1)", weights.ToString());
        }

        [TestMethod]
        public void TestRemoveWeightAt()
        {
            var weights = new WeightCollection();
            weights.AddWeight(10);
            weights.RemoveWeightAt(0);
            Assert.AreEqual(0, weights.WeightCount);
            Assert.AreEqual(0, weights.TotalWeight);
        }

        [TestMethod]
        public void TestEmptyPopRandomIndex()
        {
            var weights = new WeightCollection();
            var random = new Random(12345);
            var index = weights.PopRandomIndex(random.NextDouble());
            Assert.AreEqual(-1, index);
        }

        [TestMethod]
        public void TestPopRandomIndex()
        {
            var weights = new WeightCollection();
            weights.AddWeight(0);
            weights.AddWeight(10);
            var random = new Random(12345);
            var index = weights.PopRandomIndex(random.NextDouble());
            Assert.AreEqual(1, index);
            Assert.AreEqual(1, weights.WeightCount);
        }

        [TestMethod]
        public void TestSetWeight()
        {
            var weights = new WeightCollection();
            weights.AddWeight(10);
            weights.SetWeight(0, 30);
            Assert.AreEqual(30, weights.GetWeight(0));
            Assert.AreEqual(30, weights.TotalWeight);
        }

        [TestMethod]
        public void TestGetWeight()
        {
            var weights = new WeightCollection();
            weights.AddWeight(10);
            Assert.AreEqual(10, weights.GetWeight(0));
        }
    }
}