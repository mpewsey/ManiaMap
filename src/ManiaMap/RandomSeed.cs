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
        public Random Random { get; }

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
        /// Shuffles the specified list in place.
        /// </summary>
        /// <param name="list">The list to shuffle.</param>
        public void Shuffle<T>(List<T> list)
        {
            for (int i = 0; i < list.Count - 1; i++)
            {
                var j = Random.Next(i, list.Count);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>
        /// Draws a random weighted index from an array.
        /// </summary>
        /// <param name="weights">An array of weights.</param>
        public int DrawWeightedIndex(double[] weights)
        {
            if (weights.Length > 0)
            {
                var totals = CumSum(weights);
                var value = totals[totals.Length - 1] * Random.NextDouble();

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
        /// Returns the cumulative sum of the array.
        /// </summary>
        /// <param name="values">An array of values.</param>
        public static double[] CumSum(double[] values)
        {
            double total = 0;
            var totals = new double[values.Length];
            
            for (int i = 0; i < values.Length; i++)
            {
                total += values[i];
                totals[i] = total;
            }

            return totals;
        }
    }
}
