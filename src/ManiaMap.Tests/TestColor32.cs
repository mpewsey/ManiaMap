using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestColor32
    {
        [TestMethod]
        public void TestToString()
        {
            var color = new Color32(1, 2, 3, 4);
            var expected = "Color32(R = 1, G = 2, B = 3, A = 4)";
            Assert.AreEqual(expected, color.ToString());
        }

        [TestMethod]
        public void TestGetHashCode()
        {
            var color1 = new Color32(1, 2, 3, 4);
            var color2 = new Color32(2, 3, 4, 5);
            var color3 = new Color32(1, 2, 3, 4);
            Assert.AreEqual(color1.GetHashCode(), color3.GetHashCode());
            Assert.AreNotEqual(color1.GetHashCode(), color2.GetHashCode());
        }

        [TestMethod]
        public void TestEquals()
        {
            var color1 = new Color32(1, 2, 3, 4);
            var color2 = new Color32(2, 3, 4, 5);
            var color3 = new Color32(1, 2, 3, 4);
            Assert.IsTrue(color1 == color3);
            Assert.IsFalse(color1 == color2);
            Assert.IsFalse(color1 != color3);
            Assert.IsTrue(color1 != color2);
            Assert.IsTrue(color1.Equals(color3));
            Assert.IsFalse(color1.Equals("Object"));
        }
    }
}