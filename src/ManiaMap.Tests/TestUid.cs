using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Serialization;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestUid
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "Uid.xml";
            var id = new Uid(1, 2, 3);
            XmlSerialization.SaveXml(path, id);
            var copy = XmlSerialization.LoadXml<Uid>(path);
            Assert.AreEqual(id, copy);
        }

        [TestMethod]
        public void TestEqualsOperator()
        {
            var x = new Uid(1, 2, 3);
            var y = new Uid(1, 2, 3);
            Assert.IsTrue(x == y);
            Assert.IsFalse(x.Equals(null));
        }

        [TestMethod]
        public void TestDoesNotEqualOperator()
        {
            var x = new Uid(1, 2, 3);
            var y = new Uid(1, 2, 4);
            Assert.IsTrue(x != y);
        }

        [TestMethod]
        public void TestInitializers()
        {
            var x = new Uid(1);
            Assert.AreEqual(1, x.A);
            Assert.AreEqual(0, x.B);
            Assert.AreEqual(0, x.C);

            var y = new Uid(1, 2);
            Assert.AreEqual(1, y.A);
            Assert.AreEqual(2, y.B);
            Assert.AreEqual(0, y.C);

            var z = new Uid(1, 2, 3);
            Assert.AreEqual(1, z.A);
            Assert.AreEqual(2, z.B);
            Assert.AreEqual(3, z.C);
        }

        [TestMethod]
        public void TestComparison()
        {
            var x1 = new Uid(1);
            var y1 = new Uid(2);
            Assert.AreEqual(-1, x1.CompareTo(y1));

            var x2 = new Uid(1);
            var y2 = new Uid(1);
            Assert.AreEqual(0, x2.CompareTo(y2));

            var x3 = new Uid(1, 1);
            var y3 = new Uid(1, 2);
            Assert.AreEqual(-1, x3.CompareTo(y3));
        }
    }
}