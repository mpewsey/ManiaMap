using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;

namespace MPewsey.ManiaMap.Collections.Tests
{
    [TestClass]
    public class TestDataContractValueDictionary
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "DataContractValueDictionary.xml";
            var dict = new DataContractValueDictionary<int, LayoutNode>
            {
                { 1, new LayoutNode(1) },
                { 3, new LayoutNode(3) },
            };

            Serialization.SaveXml(path, dict);
            var copy = Serialization.LoadXml<DataContractValueDictionary<int, LayoutNode>>(path);
            CollectionAssert.AreEquivalent(dict.Keys.ToList(), copy.Keys.ToList());
            CollectionAssert.AreEquivalent(dict.Values.Select(x => x.Id).ToList(), copy.Values.Select(x => x.Id).ToList());
        }

        [TestMethod]
        public void TestInitializers()
        {
            var dict = new DataContractValueDictionary<int, LayoutNode>(10);
            dict = new DataContractValueDictionary<int, LayoutNode>(dict);
            Assert.AreEqual(0, dict.Count);
        }
    }
}