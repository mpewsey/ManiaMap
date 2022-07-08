using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestGenerationPipeline
    {
        [DataTestMethod]
        [DataRow(12345)]
        [DataRow(12355)]
        [DataRow(12365)]
        [DataRow(12375)]
        [DataRow(12385)]
        [DataRow(123456789)]
        public void TestDefaultPipelineGeneration(int seed)
        {
            var results = Samples.BigLayoutSample.Generate(seed);
            Assert.IsTrue(results.Success);
            Assert.IsTrue(results.Outputs.ContainsKey("Layout"));
        }

        [TestMethod]
        public void TestGetPrettyXmlString()
        {
            var results = Samples.BigLayoutSample.Generate(12345);
            Assert.IsTrue(results.Success);
            Assert.IsTrue(results.Outputs.ContainsKey("Layout"));
            var layout = (Layout)results.Outputs["Layout"];
            var path = "BigLayoutPrettyPrint.xml";
            Serialization.SavePrettyXml(path, layout);

            var text1 = File.ReadAllText(path);
            var text2 = Serialization.GetPrettyXmlString(layout);
            Assert.AreEqual(text1, text2);
        }

        [TestMethod]
        public void TestLoadXmlString()
        {
            var results = Samples.BigLayoutSample.Generate(12345);
            Assert.IsTrue(results.Success);
            Assert.IsTrue(results.Outputs.ContainsKey("Layout"));
            var layout = (Layout)results.Outputs["Layout"];

            var xml = Serialization.GetPrettyXmlString(layout);
            var copy = Serialization.LoadXmlString<Layout>(xml);
            Assert.AreEqual(layout.Id, copy.Id);
        }

        [TestMethod]
        public void TestSaveAndLoadPrettyXml()
        {
            var results = Samples.BigLayoutSample.Generate(12345);
            Assert.IsTrue(results.Success);
            Assert.IsTrue(results.Outputs.ContainsKey("Layout"));
            var layout = (Layout)results.Outputs["Layout"];
            var path = "BigLayoutPrettyPrint.xml";
            Serialization.SavePrettyXml(path, layout);
            var copy = Serialization.LoadXml<Layout>(path);
            Assert.AreEqual(layout.Id, copy.Id);
        }

        [TestMethod]
        public void TestSaveAndLoadXml()
        {
            var results = Samples.BigLayoutSample.Generate(12345);
            Assert.IsTrue(results.Success);
            Assert.IsTrue(results.Outputs.ContainsKey("Layout"));
            var layout = (Layout)results.Outputs["Layout"];
            var path = "BigLayout.xml";
            Serialization.SaveXml(path, layout);
            var copy = Serialization.LoadXml<Layout>(path);
            Assert.AreEqual(layout.Id, copy.Id);
        }
    }
}