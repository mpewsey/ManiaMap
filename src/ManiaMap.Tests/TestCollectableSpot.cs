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
            var spot = new CollectableSpot(new Vector2DInt(2, 3), "Default", 1);
            Assert.IsTrue(spot.ToString().StartsWith("CollectableSpot("));
        }

        [TestMethod]
        public void TestValuesAreEqual()
        {
            var spot1 = new CollectableSpot(new Vector2DInt(2, 3), "Default", 1);
            var spot2 = new CollectableSpot(new Vector2DInt(2, 3), "Default", 1);

            Assert.IsTrue(CollectableSpot.ValuesAreEqual(null, null));
            Assert.IsTrue(CollectableSpot.ValuesAreEqual(spot1, spot1));
            Assert.IsTrue(CollectableSpot.ValuesAreEqual(spot2, spot2));
            Assert.IsFalse(CollectableSpot.ValuesAreEqual(spot1, null));
            Assert.IsFalse(CollectableSpot.ValuesAreEqual(null, spot1));
            Assert.IsTrue(CollectableSpot.ValuesAreEqual(spot1, spot2));
        }
    }
}