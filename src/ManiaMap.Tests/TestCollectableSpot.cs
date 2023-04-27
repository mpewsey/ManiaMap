using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Mathematics;

namespace MPewsey.ManiaMap.Generators.Tests
{
    [TestClass]
    public class TestCollectableSpot
    {
        [TestMethod]
        public void TestToString()
        {
            var spot = new CollectableSpot(new Vector2DInt(2, 3), "Default", 1);
            Assert.IsTrue(spot.ToString().StartsWith("CollectableSpot("));
        }
    }
}