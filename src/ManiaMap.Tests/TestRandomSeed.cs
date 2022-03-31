using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestRandomSeed
    {
        [TestMethod]
        public void TestCumSum()
        {
            var values = new double[] { 1, 2, 3 };
            var result = RandomSeed.CumSum(values);
            var expected = new double[] { 1, 3, 6 };
            CollectionAssert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestDrawWeightedIndex()
        {
            var seed = new RandomSeed(12345);
            var values = new double[] { 0, 1, 0 };
            Assert.AreEqual(1, seed.DrawWeightedIndex(values));
            Assert.AreEqual(-1, seed.DrawWeightedIndex(Array.Empty<double>()));
        }
    }
}