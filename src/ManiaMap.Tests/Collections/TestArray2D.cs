using Microsoft.VisualStudio.TestTools.UnitTesting;
using MPewsey.ManiaMap.Serialization;
using System;

namespace MPewsey.ManiaMap.Collections.Tests
{
    [TestClass]
    public class TestArray2D
    {
        [TestMethod]
        public void TestSaveAndLoad()
        {
            var path = "Array2D.xml";
            Array2D<int> array = new int[,]
            {
                { 1, 2, 3 },
                { 4, 5, 6 },
                { 7, 8, 9 },
                { 10, 11, 12 },
            };

            XmlSerialization.SaveXml(path, array);
            var copy = XmlSerialization.LoadXml<Array2D<int>>(path);
            Assert.AreEqual(array.Rows, copy.Rows);
            Assert.AreEqual(array.Columns, copy.Columns);
            CollectionAssert.AreEqual(array.Array, copy.Array);
        }

        [TestMethod]
        public void TestEmptyInitializer()
        {
            var array = new Array2D<int>();
            Assert.AreEqual(0, array.Rows);
            Assert.AreEqual(0, array.Columns);
            Assert.AreEqual(0, array.Array.Length);
        }

        [TestMethod]
        public void TestClear()
        {
            var array = new Array2D<int>(2, 3);

            for (int i = 0; i < array.Rows; i++)
            {
                for (int j = 0; j < array.Columns; j++)
                {
                    array[i, j] = i + j;
                }
            }

            array.Clear();

            for (int i = 0; i < array.Rows; i++)
            {
                for (int j = 0; j < array.Columns; j++)
                {
                    Assert.AreEqual(0, array[i, j]);
                }
            }
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
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
            };

            Array2D<int> expected = new int[,]
            {
                { 9, 5, 1 },
                { 10, 6, 2 },
                { 11, 7, 3 },
                { 12, 8, 4 },
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
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
            };

            Array2D<int> expected = new int[,]
            {
                { 12, 11, 10, 9 },
                { 8, 7, 6, 5 },
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
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
            };

            Array2D<int> expected = new int[,]
            {
                { 4, 8, 12 },
                { 3, 7, 11 },
                { 2, 6, 10 },
                { 1, 5, 9 },
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
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
            };

            Array2D<int> expected = new int[,]
            {
                { 9, 10, 11, 12 },
                { 5, 6, 7, 8 },
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
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
            };

            Array2D<int> expected = new int[,]
            {
                { 4, 3, 2, 1 },
                { 8, 7, 6, 5 },
                { 12, 11, 10, 9 },
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

        [TestMethod]
        public void TestInverseIndex()
        {
            int k = 0;
            var array = new Array2D<bool>(4, 5);

            for (int i = 0; i < array.Rows; i++)
            {
                for (int j = 0; j < array.Columns; j++)
                {
                    var index = array.InverseIndex(k++);
                    Assert.AreEqual(new Vector2DInt(i, j), index);
                }
            }
        }

        [TestMethod]
        public void TestInverseIndexOutOfRange()
        {
            var array = new Array2D<bool>(4, 5);
            Assert.ThrowsException<IndexOutOfRangeException>(() => array.InverseIndex(-1));
            Assert.ThrowsException<IndexOutOfRangeException>(() => array.InverseIndex(array.Array.Length));
        }

        [TestMethod]
        public void TestFindIndex()
        {
            var data = new int[,]
            {
                { 1, 2, 3, 4 },
                { 5, 6, 7, 8 },
                { 9, 10, 11, 12 },
            };

            var array = new Array2D<int>(data);

            for (int i = 0; i < array.Rows; i++)
            {
                for (int j = 0; j < array.Columns; j++)
                {
                    var value = array[i, j];
                    var expected = new Vector2DInt(i, j);
                    var result = array.FindIndex(x => x == value);
                    Assert.AreEqual(expected, result);
                }
            }

            Assert.AreEqual(new Vector2DInt(-1, -1), array.FindIndex(x => x == -1));
        }

        [TestMethod]
        public void TestValuesAreEqual()
        {
            Array2D<int> array1 = new int[,]
            {
                { 1, 2, 3, 4, 5 },
                { 6, 7, 8, 9, 10 },
                { 11, 12, 13, 14, 15 },
            };

            Array2D<int> array2 = new int[,]
            {
                { 1, 2, 3, 4, 5 },
                { 6, 7, 8, 9, 10 },
                { 11, 12, 13, 14, 15 },
            };

            Array2D<int> array3 = new int[,]
            {
                { 1, 2, 3, 4, 5 },
                { 6, 7, 8, 9, 10 },
                { 11, 12, 13, 14, 100 },
            };

            Assert.IsTrue(Array2D<int>.ValuesAreEqual(array1, array2));
            Assert.IsTrue(Array2D<int>.ValuesAreEqual(array1, array1));
            Assert.IsTrue(array1.ValuesAreEqual(array2));
            Assert.IsFalse(Array2D<int>.ValuesAreEqual(array1, null));
            Assert.IsFalse(Array2D<int>.ValuesAreEqual(array1, array3));
            Assert.IsFalse(array1.ValuesAreEqual(array3));
        }
    }
}