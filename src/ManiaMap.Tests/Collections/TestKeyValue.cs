using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace MPewsey.ManiaMap.Collections.Tests
{
    [TestClass]
    public class TestKeyValue
    {
        [TestMethod]
        public void TestToString()
        {
            var obj = new KeyValue<int, int>(1, 2);
            Assert.AreEqual("KeyValue(Key = 1, Value = 2)", obj.ToString());
        }
    }
}