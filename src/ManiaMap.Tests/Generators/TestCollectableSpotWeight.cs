using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Mathematics;

namespace MPewsey.ManiaMap.Generators.Tests
{
    [TestClass]
    public class TestCollectableSpotWeight
    {
        [TestMethod]
        public void TestToString()
        {
            var spot = new CollectableSpotWeight(new Uid(1), 1, new CollectableSpot(new Vector2DInt(2, 3), "Default", 1));
            Assert.IsTrue(spot.ToString().StartsWith("CollectableSpotWeight("));
        }
    }
}