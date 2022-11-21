using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;

namespace MPewsey.ManiaMap.Generators.Tests
{
    [TestClass]
    public class TestGenerationLogger
    {
        [TestMethod]
        public void TestLog()
        {
            GenerationLogger.RemoveAllListeners();
            var messages = new List<string>();
            var expected = new List<string> { "Message1", "Message2", "Message2" };

            // Add the listener.
            GenerationLogger.AddListener(messages.Add);

            foreach (var message in expected)
            {
                GenerationLogger.Log(message);
            }

            CollectionAssert.AreEqual(expected, messages);

            // Remove the listener.
            GenerationLogger.RemoveListener(messages.Add);

            foreach (var message in expected)
            {
                GenerationLogger.Log(message);
            }

            CollectionAssert.AreEqual(expected, messages);
        }
    }
}