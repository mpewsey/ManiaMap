using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;

namespace MPewsey.ManiaMap.Collections.Tests
{
    [TestClass]
    public class TestDataContractDictionary
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "DataContractDictionary.xml";
            var dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
                { 3, 4 },
            };

            Serialization.SaveXml(path, dict);
            var copy = Serialization.LoadXml<DataContractDictionary<int, int>>(path);
            CollectionAssert.AreEquivalent(dict.Keys.ToList(), copy.Keys.ToList());
            CollectionAssert.AreEquivalent(dict.Values.ToList(), copy.Values.ToList());
        }

        [TestMethod]
        public void TestIsReadOnly()
        {
            var dict = new DataContractDictionary<int, int>();
            Assert.IsFalse(dict.IsReadOnly);
        }

        [TestMethod]
        public void TestKeys()
        {
            var dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
                { 3, 4 },
            };

            var expected = new int[] { 1, 3 };
            CollectionAssert.AreEquivalent(expected, dict.Keys.ToList());
        }

        [TestMethod]
        public void TestValues()
        {
            var dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
                { 3, 4 },
            };

            var expected = new int[] { 2, 4 };
            CollectionAssert.AreEquivalent(expected, dict.Values.ToList());
        }

        [TestMethod]
        public void TestAdd()
        {
            var dict = new DataContractDictionary<int, int>();
            dict.Add(1, 2);
            Assert.AreEqual(2, dict[1]);
        }

        [TestMethod]
        public void TestAddKeyValue()
        {
            var dict = new DataContractDictionary<int, int>();
            dict.Add(new KeyValuePair<int, int>(1, 2));
            Assert.AreEqual(2, dict[1]);
        }

        [TestMethod]
        public void TestClear()
        {
            var dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
                { 3, 4 },
            };

            Assert.AreEqual(2, dict.Count);
            dict.Clear();
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void TestContainsKey()
        {
            var dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
                { 3, 4 },
            };

            Assert.IsTrue(dict.ContainsKey(1));
        }

        [TestMethod]
        public void TestContains()
        {
            var dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
                { 3, 4 },
            };

            Assert.IsTrue(dict.Contains(new KeyValuePair<int, int>(1, 2)));
        }

        [TestMethod]
        public void TestRemove()
        {
            var dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
                { 3, 4 },
            };

            Assert.IsTrue(dict.Remove(1));
            Assert.AreEqual(1, dict.Count);
        }

        [TestMethod]
        public void TestEnumerator()
        {
            var dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
                { 3, 4 },
            };

            foreach (var pair in dict)
            {
                Assert.IsTrue(dict.Contains(pair));
            }
        }
    }
}