using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutState
    {
        [TestMethod]
        public void TestSaveAndLoadXml()
        {
            var path = "ManiaMapLayoutState.xml";
            var layout = Samples.ManiaMapSample.ManiaMapLayout();
            var layoutState = new LayoutState(layout);
            Serialization.SaveXml(path, layoutState);
            var copy = Serialization.LoadXml<LayoutState>(path);
            Assert.AreEqual(layoutState.Id, copy.Id);
        }
    }
}