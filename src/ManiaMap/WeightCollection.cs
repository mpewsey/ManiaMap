using System;
using System.Collections.Generic;

namespace MPewsey.ManiaMap
{
    /// <summary>
    /// A collection of draw weights that can be randomly drawn from.
    /// </summary>
    public class WeightCollection
    {
        /// <summary>
        /// A list of draw weights.
        /// </summary>
        private List<double> Weights { get; } = new List<double>();

        /// <summary>
        /// The total draw weight in the collection.
        /// </summary>
        public double TotalWeight { get => _totalWeight; private set => _totalWeight = Math.Max(value, 0); }
        private double _totalWeight;

        /// <summary>
        /// The number of weights in the collection.
        /// </summary>
        public int WeightCount => Weights.Count;

        public override string ToString()
        {
            return $"WeightCollection(Weights.Count = {Weights.Count})";
        }

        /// <summary>
        /// Adds a weight to the collection.
        /// </summary>
        /// <param name="value">The weight.</param>
        /// <exception cref="ArgumentException">Raised if the weight is negative.</exception>
        public void AddWeight(double value)
        {
            if (value < 0)
                throw new ArgumentException($"Value cannot be negative: {value}.");

            Weights.Add(value);
            TotalWeight += value;
        }

        /// <summary>
        /// Returns the weight at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        public double GetWeight(int index)
        {
            return Weights[index];
        }

        /// <summary>
        /// Sets the weight at the specified index.
        /// </summary>
        /// <param name="index">The index.</param>
        /// <param name="value">The weight.</param>
        /// <exception cref="ArgumentException">Raised if the weight is negative.</exception>
        public void SetWeight(int index, double value)
        {
            if (value < 0)
                throw new ArgumentException($"Value cannot be negative: {value}.");

            TotalWeight -= Weights[index];
            TotalWeight += value;
            Weights[index] = value;
        }

        /// <summary>
        /// Removes the weight at the specified index.
        /// </summary>
        /// <param name="index">The index to remove.</param>
        public void RemoveWeightAt(int index)
        {
            TotalWeight -= Weights[index];
            Weights.RemoveAt(index);
        }

        /// <summary>
        /// Draws a random index from the collection based on weight
        /// and removes it from the collection. Returns -1 if an index cannot be drawn.
        /// </summary>
        /// <param name="value">A random value between 0 and 1.</param>
        public int PopRandomIndex(double value)
        {
            var index = DrawRandomIndex(value);

            if (index >= 0)
                RemoveWeightAt(index);

            return index;
        }

        /// <summary>
        /// Draws a random index from the collection based on weight.
        /// Returns -1 if an index cannot be drawn.
        /// </summary>
        /// <param name="value">A random value between 0 and 1.</param>
        public int DrawRandomIndex(double value)
        {
            double total = 0;
            var weight = TotalWeight * Math.Min(Math.Max(value, 0), 1);

            for (int i = 0; i < Weights.Count; i++)
            {
                total += Weights[i];

                if (weight <= total && total > 0)
                    return i;
            }

            if (total > 0)
                return Weights.Count - 1;

            return -1;
        }
    }
}
