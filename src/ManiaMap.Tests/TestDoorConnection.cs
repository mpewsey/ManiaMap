using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestDoorConnection
    {
        [TestMethod]
        public void TestEdgeDirection()
        {
            var results = Samples.BigLayoutSample.Generate(12345);
            Assert.IsTrue(results.Success);
            var layout = (Layout)results.Outputs["Layout"];

            foreach (var connection in layout.DoorConnections.Values)
            {
                Assert.AreEqual(EdgeDirection.Both, connection.EdgeDirection);
            }
        }
    }
}
