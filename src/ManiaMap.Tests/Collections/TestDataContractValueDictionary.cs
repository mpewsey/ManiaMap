using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Serialization;
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

            XmlSerialization.SaveXml(path, dict);
            var copy = XmlSerialization.LoadXml<DataContractValueDictionary<int, LayoutNode>>(path);
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