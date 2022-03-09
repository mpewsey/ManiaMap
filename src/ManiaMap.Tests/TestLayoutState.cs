using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayoutState
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "ManiaMapLayoutState.xml";
            var layout = Samples.ManiaMapSample.ManiaMapLayout();
            var layoutState = new LayoutState(layout);
            layoutState.SaveXml(path);
            var copy = LayoutState.LoadXml(path);
            Assert.AreEqual(layoutState.Id, copy.Id);
        }
    }
}