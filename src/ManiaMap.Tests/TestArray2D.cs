﻿using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace MPewsey.ManiaMap.Tests
{
    [TestClass]
    public class TestArray2D
    {
        [TestMethod]
        public void TestEmptyInitializer()
        {
            var array = new Array2D<int>();
            Assert.AreEqual(0, array.Rows);
            Assert.AreEqual(0, array.Columns);
            Assert.AreEqual(0, array.Array.Length);
        }

        [TestMethod]
        public void TestToString()
        {
            var result = new Array2D<int>(1, 2).ToString();
            var expected = $"Array2D<{typeof(int)}>(Rows = 1, Columns = 2)";
            Assert.AreEqual(expected, result);
        }

        [TestMethod]
        public void TestInitializeNegativeRow()
        {
            Assert.ThrowsException<ArgumentException>(() => new Array2D<int>(-1, 1));
        }

        [TestMethod]
        public void TestInitializeNegativeColumn()
        {
            Assert.ThrowsException<ArgumentException>(() => new Array2D<int>(1, -1));
        }

        [TestMethod]
        public void TestGetOutOfBoundsIndex()
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() => new Array2D<int>()[-1, -1]);
        }

        [TestMethod]
        public void TestSetOutOfBoundsIndex()
        {
            Assert.ThrowsException<IndexOutOfRangeException>(() => new Array2D<int>()[-1, -1] = 1);
        }

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