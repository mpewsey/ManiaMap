using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestArray2D
    {
        [TestMethod]
        public void TestRotate90()
        {
            Array2D<int> array = new int[,]
            {
                { 1, 2, 3, 4 },
                { 0, 0, 0, 5 },
                { 0, 0, 0, 6 },
            };

            Array2D<int> expected = new int[,]
            {
                { 0, 0, 1 },
                { 0, 0, 2 },
                { 0, 0, 3 },
                { 6, 5, 4 },
            };

            var result = array.Rotated90();
            Console.WriteLine("Original");
            Console.WriteLine(array.ToArrayString());
            Console.WriteLine("\nExpected:");
            Console.WriteLine(expected.ToArrayString());
            Console.WriteLine("\nResult:");
            Console.WriteLine(result.ToArrayString());
            CollectionAssert.AreEqual(expected.Array, result.Array);
        }

        [TestMethod]
        public void TestRotate180()
        {
            Array2D<int> array = new int[,]
            {
                { 1, 2, 3, 4 },
                { 0, 0, 0, 5 },
                { 0, 0, 0, 6 },
            };

            Array2D<int> expected = new int[,]
            {
                { 6, 0, 0, 0 },
                { 5, 0, 0, 0 },
                { 4, 3, 2, 1 },
            };

            var result = array.Rotated180();
            Console.WriteLine("Original");
            Console.WriteLine(array.ToArrayString());
            Console.WriteLine("\nExpected:");
            Console.WriteLine(expected.ToArrayString());
            Console.WriteLine("\nResult:");
            Console.WriteLine(result.ToArrayString());
            CollectionAssert.AreEqual(expected.Array, result.Array);
        }

        [TestMethod]
        public void TestRotate270()
        {
            Array2D<int> array = new int[,]
            {
                { 1, 2, 3, 4 },
                { 0, 0, 0, 5 },
                { 0, 0, 0, 6 },
            };

            Array2D<int> expected = new int[,]
            {
                { 4, 5, 6 },
                { 3, 0, 0 },
                { 2, 0, 0 },
                { 1, 0, 0 },
            };

            var result = array.Rotated270();
            Console.WriteLine("Original");
            Console.WriteLine(array.ToArrayString());
            Console.WriteLine("\nExpected:");
            Console.WriteLine(expected.ToArrayString());
            Console.WriteLine("\nResult:");
            Console.WriteLine(result.ToArrayString());
            CollectionAssert.AreEqual(expected.Array, result.Array);
        }

        [TestMethod]
        public void TestMirroredVertically()
        {
            Array2D<int> array = new int[,]
            {
                { 1, 2, 3, 4 },
                { 0, 0, 0, 5 },
                { 0, 0, 0, 6 },
            };

            Array2D<int> expected = new int[,]
            {
                { 0, 0, 0, 6 },
                { 0, 0, 0, 5 },
                { 1, 2, 3, 4 },
            };

            var result = array.MirroredVertically();
            Console.WriteLine("Original");
            Console.WriteLine(array.ToArrayString());
            Console.WriteLine("\nExpected:");
            Console.WriteLine(expected.ToArrayString());
            Console.WriteLine("\nResult:");
            Console.WriteLine(result.ToArrayString());
            CollectionAssert.AreEqual(expected.Array, result.Array);
        }

        [TestMethod]
        public void TestMirroredHorizontally()
        {
            Array2D<int> array = new int[,]
            {
                { 1, 2, 3, 4 },
                { 0, 0, 0, 5 },
                { 0, 0, 0, 6 },
            };

            Array2D<int> expected = new int[,]
            {
                { 4, 3, 2, 1 },
                { 5, 0, 0, 0 },
                { 6, 0, 0, 0 },
            };

            var result = array.MirroredHorizontally();
            Console.WriteLine("Original");
            Console.WriteLine(array.ToArrayString());
            Console.WriteLine("\nExpected:");
            Console.WriteLine(expected.ToArrayString());
            Console.WriteLine("\nResult:");
            Console.WriteLine(result.ToArrayString());
            CollectionAssert.AreEqual(expected.Array, result.Array);
        }
    }
}