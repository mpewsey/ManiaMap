using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Mathematics;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestCollectableSpot
    {
        [TestMethod]
        public void TestToString()
        {
            var spot = new CollectableSpot(new Uid(1), new Vector2DInt(2, 3), 1, "Default");
            Assert.IsTrue(spot.ToString().StartsWith("CollectableSpot("));
        }
    }
}