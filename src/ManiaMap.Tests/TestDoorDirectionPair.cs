using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestDoorDirectionPair
    {
        [TestMethod]
        public void TestToString()
        {
            var obj = new DoorDirectionPair(DoorDirection.West, Door.TwoWay);
            Assert.IsTrue(obj.ToString().StartsWith("DoorDirectionPair("));
        }
    }
}