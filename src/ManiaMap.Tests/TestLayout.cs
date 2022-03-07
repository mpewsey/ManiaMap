using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestLayout
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "ManiaMapLayout.xml";
            var layout = Samples.ManiaMapSample.ManiaMapLayout();
            layout.SaveXml(path);
            var copy = Layout.LoadXml(path);
            Assert.AreEqual(layout.Seed, copy.Seed);
        }
    }
}