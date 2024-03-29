﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.Common.Serialization;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestColor4
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "Color4.xml";
            var color = new Color4(1, 2, 3, 4);
            XmlSerialization.SaveXml(path, color);
            var copy = XmlSerialization.LoadXml<Color4>(path);
            Assert.AreEqual(color, copy);
        }

        [TestMethod]
        public void TestToString()
        {
            var color = new Color4(1, 2, 3, 4);
            var expected = "Color32(R = 1, G = 2, B = 3, A = 4)";
            Assert.AreEqual(expected, color.ToString());
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var color1 = new Color4(1, 2, 3, 4);
            var color2 = new Color4(2, 3, 4, 5);
            var color3 = new Color4(1, 2, 3, 4);
            Assert.AreEqual(color1.GetHashCode(), color3.GetHashCode());
            Assert.AreNotEqual(color1.GetHashCode(), color2.GetHashCode());
        }

        [TestMethod]
        public void TestEquals()
        {
            var color1 = new Color4(1, 2, 3, 4);
            var color2 = new Color4(2, 3, 4, 5);
            var color3 = new Color4(1, 2, 3, 4);
            Assert.IsTrue(color1 == color3);
            Assert.IsFalse(color1 == color2);
            Assert.IsFalse(color1 != color3);
            Assert.IsTrue(color1 != color2);
            Assert.IsTrue(color1.Equals(color3));
            Assert.IsFalse(color1.Equals("Object"));
        }
    }
}