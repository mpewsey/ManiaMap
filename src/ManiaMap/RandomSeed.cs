using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for associating a random number generator with an initial seed.
    /// </summary>
    public class RandomSeed
    {
        /// <summary>
        /// The random seed.
        /// </summary>
        public int Seed { get; }

        /// <summary>
        /// The random number generator.
        /// </summary>
        private Random Random { get; }

        /// <summary>
        /// Initializes a new random seed.
        /// </summary>
        /// <param name="seed">The random seed.</param>
        public RandomSeed(int seed)
        {
            Seed = seed;
            Random = new Random(seed);
        }

        /// <summary>
        /// Returns a random integer on the interval [0, `int.MaxValue`).
        /// </summary>
        public int Next()
        {
            return Random.Next();
        }

        /// <summary>
        /// Returns a random integer on the interval [`minValue`, `maxValue`).
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public int Next(int minValue, int maxValue)
        {
            return Random.Next(minValue, maxValue);
        }

        /// <summary>
        /// Returns a random double in the interval [0, 1).
        /// </summary>
        public double NextDouble()
        {
            return Random.NextDouble();
        }

        /// <summary>
        /// Shuffles the specified list in place.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        public void Shuffle<T>(IList<T> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                var j = Next(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>
        /// Returns a new shuffled copy of the collection.
        /// </summary>
        /// <param name="list">The collection</param>
        public List<T> Shuffled<T>(IEnumerable<T> collection)
        {
            var copy = new List<T>(collection);
            Shuffle(copy);
            return copy;
        }

        /// <summary>
        /// Draws a random weighted index from an array.
        /// </summary>
        /// <param name="weights">A list of weights.</param>
        public int DrawWeightedIndex(IList<double> weights)
        {
            if (weights.Count > 0)
            {
                var totals = CumSum(weights);
                var value = totals[totals.Length - 1] * NextDouble();

                for (int i = 0; i < totals.Length; i++)
                {
                    if (value <= totals[i] && totals[i] > 0)
                    {
                        return i;
                    }
                }
            }

            return -1;
        }

        /// <summary>
        /// Returns the cumulative sums of the list.
        /// </summary>
        /// <param name="values">An list of values.</param>
        public static double[] CumSum(IList<double> values)
        {
            double total = 0;
            var totals = new double[values.Count];

            for (int i = 0; i < totals.Length; i++)
            {
                total += values[i];
                totals[i] = total;
            }

            return totals;
        }
    }
}
