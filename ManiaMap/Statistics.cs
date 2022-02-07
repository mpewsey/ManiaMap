using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public static class Statistics
    {
        /// <summary>
        /// Shuffles an array in place.
        /// </summary>
        public static void Shuffle<T>(T[] array, Random random)
        {
            for (int i = 0; i < array.Length; i++)
            {
                var j = random.Next(i, array.Length - 1);
                (array[i], array[j]) = (array[j], array[i]);
            }
        }

        /// <summary>
        /// Shuffles a list in place.
        /// </summary>
        public static void Shuffle<T>(List<T> list, Random random)
        {
            for (int i = 0; i < list.Count; i++)
            {
                var j = random.Next(i, list.Count - 1);
                (list[i], list[j]) = (list[j], list[i]);
            }
        }

        /// <summary>
        /// Returns a new shuffled array for an array.
        /// </summary>
        public static T[] Shuffled<T>(T[] array, Random random)
        {
            if (array.Length == 0)
                return Array.Empty<T>();
            
            var copy = new T[array.Length];
            array.CopyTo(copy, 0);
            Shuffle(copy, random);
            return copy;
        }

        /// <summary>
        /// Returns a new shuffled array for a list.
        /// </summary>
        public static T[] Shuffled<T>(List<T> list, Random random)
        {
            if (list.Count == 0)
                return Array.Empty<T>();
            
            var array = list.ToArray();
            Shuffle(array, random);
            return array;
        }
    }
}
