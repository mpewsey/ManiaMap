using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ManiaMap
{
    public static class Randomizer
    {
        public static void Shuffle<T>(List<T> collection, Random random)
        {
            for (int i = 0; i < collection.Count; i++)
            {
                var j = random.Next(0, collection.Count - 1);
                (collection[i], collection[j]) = (collection[j], collection[i]);
            }
        }

        public static void Shuffle<T>(T[] collection, Random random)
        {
            for (int i = 0; i < collection.Length; i++)
            {
                var j = random.Next(0, collection.Length - 1);
                (collection[i], collection[j]) = (collection[j], collection[i]);
            }
        }
    }
}
