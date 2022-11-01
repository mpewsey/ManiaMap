using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections;
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
        public void TestInitializer()
        {
            var dict = new DataContractDictionary<int, int>(10);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void TestIsReadOnly()
        {
            var dict = new DataContractDictionary<int, int>();
            Assert.IsFalse(dict.IsReadOnly);
        }

        [TestMethod]
        public void TestCollectionKeys()
        {
            IDictionary dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
            };

            foreach (var value in dict.Keys)
            {
                Assert.AreEqual(1, value);
            }
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

        [TestMethod]
        public void TestObjectKey()
        {
            IDictionary dict = new DataContractDictionary<int, int>();
            dict[1] = 2;
            Assert.AreEqual(2, dict[1]);
        }

        [TestMethod]
        public void TestIsSynchronized()
        {
            var dict = new DataContractDictionary<int, int>();
            Assert.IsFalse(dict.IsSynchronized);
        }

        [TestMethod]
        public void TestSyncRoot()
        {
            var dict = new DataContractDictionary<int, int>();
            Assert.IsNotNull(dict.SyncRoot);
        }

        [TestMethod]
        public void TestIsFixedSize()
        {
            var dict = new DataContractDictionary<int, int>();
            Assert.IsFalse(dict.IsFixedSize);
        }

        [TestMethod]
        public void TestReadOnlyEnumerable()
        {
            IReadOnlyDictionary<int, int> dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
            };

            foreach (var pair in dict)
            {
                Assert.AreEqual(1, pair.Key);
                Assert.AreEqual(2, pair.Value);
            }
        }

        [TestMethod]
        public void TestReadOnlyKeys()
        {
            IReadOnlyDictionary<int, int> dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
            };

            foreach (var value in dict.Keys)
            {
                Assert.AreEqual(1, value);
            }
        }

        [TestMethod]
        public void TestReadOnlyValues()
        {
            IReadOnlyDictionary<int, int> dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
            };

            foreach (var value in dict.Values)
            {
                Assert.AreEqual(2, value);
            }
        }

        [TestMethod]
        public void TestDictionaryValues()
        {
            IDictionary dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
            };

            foreach (var value in dict.Values)
            {
                Assert.AreEqual(2, value);
            }
        }

        [TestMethod]
        public void TestDictionaryAdd()
        {
            IDictionary dict = new DataContractDictionary<int, int>();
            dict.Add(1, 2);
            Assert.AreEqual(1, dict.Count);
        }

        [TestMethod]
        public void TestDictionaryContains()
        {
            IDictionary dict = new DataContractDictionary<int, int>();
            dict.Add(1, 2);
            Assert.IsTrue(dict.Contains(1));
        }

        [TestMethod]
        public void TestCollectionCopyTo()
        {
            var dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
            };

            Array array = new KeyValuePair<int, int>[10];
            dict.CopyTo(array, 0);
        }

        [TestMethod]
        public void TestRemoveKeyValuePair()
        {
            var dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
            };

            Assert.IsTrue(dict.Remove(new KeyValuePair<int, int>(1, 2)));
        }

        [TestMethod]
        public void TestRemoveObject()
        {
            IDictionary dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
            };

            dict.Remove(1);
            Assert.AreEqual(0, dict.Count);
        }

        [TestMethod]
        public void TestGetEnumerator()
        {
            IEnumerable dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
            };

            foreach (var value in dict)
            {
                Assert.IsNotNull(value);
            }
        }

        [TestMethod]
        public void TestGetDictionaryEnumerator()
        {
            IDictionary dict = new DataContractDictionary<int, int>
            {
                { 1, 2 },
            };

            foreach (var value in dict)
            {
                Assert.IsNotNull(value);
            }
        }
    }
}