using MPewsey.ManiaMap.Serialization;
using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A class for performing pseudo-random number generation.
    /// 
    /// References
    /// ----------
    /// * [1] Rossetta Code. Subtractive generator. Retrieved April 12, 2022, from https://rosettacode.org/wiki/Subtractive_generator.
    /// * [2] Microsoft Corporation. Reference Source .NET Framework 4.8. Retrieved April 12, 2022, from https://referencesource.microsoft.com/#mscorlib/system/random.cs,bb77e610694e64ca.
    /// </summary>
    [DataContract(Namespace = XmlSerialization.Namespace)]
    public class RandomSeed
    {
        /// <summary>
        /// The random seed.
        /// </summary>
        [DataMember(Order = 1, IsRequired = true)]
        public int Seed { get; private set; }

        /// <summary>
        /// The first position of the randomizer.
        /// </summary>
        [DataMember(Order = 2, IsRequired = true)]
        private int Position1 { get; set; }

        /// <summary>
        /// The second position of the randomizer.
        /// </summary>
        [DataMember(Order = 3, IsRequired = true)]
        private int Position2 { get; set; }

        /// <summary>
        /// An array of previous seeds.
        /// </summary>
        [DataMember(Order = 4, IsRequired = true)]
        private int[] Seeds { get; set; } = new int[56];

        /// <summary>
        /// Initializes a new random seed based on the current system ticks.
        /// </summary>
        public RandomSeed()
        {
            SetSeed(Environment.TickCount);
        }

        /// <summary>
        /// Initializes a new random seed.
        /// </summary>
        /// <param name="seed">The random seed.</param>
        public RandomSeed(int seed)
        {
            SetSeed(seed);
        }

        /// <summary>
        /// Initializes a copy of a random seed.
        /// </summary>
        /// <param name="other">The random seed to be copied.</param>
        private RandomSeed(RandomSeed other)
        {
            Seed = other.Seed;
            Position1 = other.Position1;
            Position2 = other.Position2;
            Seeds = new int[other.Seeds.Length];
            Array.Copy(other.Seeds, Seeds, other.Seeds.Length);
        }

        public override string ToString()
        {
            return $"RandomSeed(Seed = {Seed})";
        }

        /// <summary>
        /// Returns a copy of the object.
        /// </summary>
        public RandomSeed Copy()
        {
            return new RandomSeed(this);
        }

        /// <summary>
        /// Returns the positive modulo of a value with respect to int.MaxValue.
        /// </summary>
        /// <param name="value">The value.</param>
        private static int Mod(int value)
        {
            if (value < 0)
                return value + int.MaxValue;
            return value;
        }

        /// <summary>
        /// Wraps an index if it exceeds the top array bounds.
        /// </summary>
        /// <param name="value">The index.</param>
        private static int WrapIndex(int value)
        {
            if (value > 55)
                return 1;
            return value;
        }

        /// <summary>
        /// Sets the random seed and initializes the randomizer.
        /// </summary>
        /// <param name="seed">The random seed.</param>
        public void SetSeed(int seed)
        {
            Seed = seed;
            Position1 = 0;
            Position2 = 21;
            var mj = seed == int.MinValue ? int.MaxValue : Math.Abs(seed);
            mj = 161803398 - mj;
            Seeds[55] = mj;
            var mk = 1;

            // Populate random seed array.
            for (int i = 1; i < 55; i++)
            {
                var index = (21 * i) % 55;
                Seeds[index] = mk;
                mk = Mod(mj - mk);
                mj = Seeds[index];
            }

            // Generate random numbers to reduce seed bias.
            for (int k = 1; k < 5; k++)
            {
                for (int i = 1; i < 56; i++)
                {
                    Seeds[i] = Mod(Seeds[i] - Seeds[1 + (i + 30) % 55]);
                }
            }
        }

        /// <summary>
        /// Returns a random integer on the interval [0, int.MaxValue).
        /// </summary>
        /// <returns></returns>
        public int Next()
        {
            var i = WrapIndex(Position1 + 1);
            var j = WrapIndex(Position2 + 1);
            var next = Mod(Seeds[i] - Seeds[j]);

            if (next == int.MaxValue)
                next--;

            Seeds[i] = next;
            Position1 = i;
            Position2 = j;
            return next;
        }

        /// <summary>
        /// Returns a random value on the interval [0, maxValue).
        /// </summary>
        /// <param name="maxValue">The maximum value.</param>
        public int Next(int maxValue)
        {
            return (int)(NextDouble() * maxValue);
        }

        /// <summary>
        /// Returns a random value on the interval [minValue, maxValue).
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public int Next(int minValue, int maxValue)
        {
            var delta = maxValue - (long)minValue;
            var t = delta <= int.MaxValue ? NextDouble() : NextLargeDouble();
            return (int)((long)(t * delta) + minValue);
        }

        /// <summary>
        /// Returns a random double on the interval [0, 1).
        /// </summary>
        public double NextDouble()
        {
            return Next() / (double)int.MaxValue;
        }

        /// <summary>
        /// Returns a random double on the interval [0, maxValue).
        /// </summary>
        /// <param name="maxValue">The maximum value.</param>
        public double NextDouble(double maxValue)
        {
            return NextDouble() * maxValue;
        }

        /// <summary>
        /// Returns a random double on the interval [minValue, maxValue).
        /// </summary>
        /// <param name="minValue">The minimum value.</param>
        /// <param name="maxValue">The maximum value.</param>
        public double NextDouble(double minValue, double maxValue)
        {
            var x = NextDouble();
            return x * maxValue + (1 - x) * minValue;
        }

        /// <summary>
        /// Returns a random high resolution double on the interval [0, 1).
        /// </summary>
        private double NextLargeDouble()
        {
            var next = Next();

            if ((Next() % 2) == 0)
                next = -next;

            double result = next;
            result += int.MaxValue - 1;
            result /= 2.0 * int.MaxValue - 1;
            return result;
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
        /// Draws a random weighted index from a list.
        /// </summary>
        /// <param name="weights">A list of weights.</param>
        public int DrawWeightedIndex(IList<double> weights)
        {
            if (weights.Count > 0)
            {
                var totals = CumSum(weights);
                var value = NextDouble(totals[totals.Length - 1]);

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
            if (values.Count == 0)
                return Array.Empty<double>();

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
