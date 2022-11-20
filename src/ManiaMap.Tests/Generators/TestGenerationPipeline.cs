using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Serialization;
using System;

namespace MPewsey.ManiaMap.Generators.Tests
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
        public void TestLoadXmlString()
        {
            var results = Samples.BigLayoutSample.Generate(12345);
            Assert.IsTrue(results.Success);
            Assert.IsTrue(results.Outputs.ContainsKey("Layout"));
            var layout = (Layout)results.Outputs["Layout"];

            var xml = XmlSerialization.GetXmlString(layout);
            var copy = XmlSerialization.LoadXmlString<Layout>(xml);
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
            XmlSerialization.SaveXml(path, layout);
            var copy = XmlSerialization.LoadXml<Layout>(path);
            Assert.AreEqual(layout.Id, copy.Id);
        }

        [TestMethod]
        public void TestSaveAndLoadEncryptedXml()
        {
            var results = Samples.BigLayoutSample.Generate(12345);
            Assert.IsTrue(results.Success);
            Assert.IsTrue(results.Outputs.ContainsKey("Layout"));
            var layout = (Layout)results.Outputs["Layout"];
            var path = "BigLayout.sav";

            var key = new byte[32];
            var random = new Random(12345);
            random.NextBytes(key);

            XmlSerialization.SaveEncryptedXml(path, layout, key);
            Console.WriteLine(Cryptography.DecryptTextFile(path, key).Replace("><", ">\n<"));
            var copy = XmlSerialization.LoadEncryptedXml<Layout>(path, key);
            Assert.AreEqual(layout.Id, copy.Id);
        }
    }
}